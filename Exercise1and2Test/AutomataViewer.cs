using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Exercise1
{
    // this user controller is not part of the project
    public partial class AutomataViewer : UserControl
    {
        Dictionary<string,string> _automata;
        List<int> finalStates = new List<int>();
        int stateCount;
        Pen edge;
        Pen state;
        private bool isDragging = false;
        Point offset = new Point();
        Point startOffset = new Point();

        public AutomataViewer()
        {
            InitializeComponent();
            edge = new Pen(Color.Black, 1);
            edge.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            state = new Pen(Color.Black, 2);
        }

        public void Initialize(RegularExpression regEx)
        {
            _automata = new Dictionary<string, string>();
            finalStates.Clear();
            foreach (var st in regEx.ParserAutomata.States)
            {
                foreach (var to in st.Transitions)
                {
                    string key = st.ID + ":" + to.NextState.ID;
                    string val = "";
                    if(_automata.ContainsKey(key)){
                        val = _automata[key];
                        _automata.Remove(key);
                    }
                    if(val!="") val+=":";
                    val += to.MatchedChar;
                    _automata.Add(key,val);
                }
                if (st.IsFinal) finalStates.Add(st.ID);
            }
            stateCount = regEx.ParserAutomata.States.Length;
            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e){ }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            int statesDist = Math.Max(this.Width / (stateCount+1), 100);
            int stateSize = 40;

            g.TranslateTransform(statesDist - offset.X, Height / 2 - offset.Y);

            if (_automata!=null && _automata.Count != 0)
            {
                foreach (var keyIndx in _automata.Keys)
                {
                    if (!_automata.ContainsKey(keyIndx))
                        continue;
                    var pair = keyIndx.Split(':');
                    var startEl = int.Parse(pair[0]);
                    var endEl = int.Parse(pair[1]);
                    int curC = 1;
                    var trans =  _automata[keyIndx].Split(':');
                    foreach (var associationChar in trans)
                    {
                        int x = startEl * statesDist;
                        int height = (int)((curC + 1) / 2) * 20 * (startEl != endEl?Math.Abs(endEl - startEl):1);
                        if (curC % 2 == 1)
                            height = -height;
                        Point[] line;
                        var midPoint = (int)(statesDist * (startEl + (double)(endEl-startEl) / 2.0));
                        if (startEl != endEl)
                        {
                            height = (int)Math.Pow(Math.Abs(endEl - startEl) + curC - 2, 1.0) * 30;// *;
                            if (curC % 2 == 1)
                                height = -height + stateSize/2;

                            line = new Point[]{
                                new Point(x + (startEl<endEl?1:-1) * (int)(stateSize * 0.5), stateSize/2),
                                new Point(midPoint, height), 
                                new Point(endEl*statesDist + (startEl<endEl?-1:1) *  (int)(stateSize * 0.5), stateSize/2)
                            };
                        }
                        else
                        {
                            midPoint = x;
                            var offsetX = (int)(((double)((curC+1) / 2.0) / (double)(trans.Length+2) * 2.0) * (stateSize) / 2.0);
                            var offsetX2 = (int)(offsetX * (0.7 * ((curC + 1) / 2.0 + 1)));
                            var offsetY = (int)((curC % 2 == 1 ? 1 : -1) * (1.0 - Math.Pow((double)((curC + 1) / 2) / (double)(trans.Length + 2) * 2.0, 2) * stateSize / 2.0));
                            line = new Point[]{
                                new Point(x - offsetX,-offsetY),
                                new Point(x - offsetX2, (int)(height * 0.5)), 
                                new Point(x, height), 
                                new Point(x + offsetX2, (int)(height * 0.5)), 
                                new Point(x+ offsetX,-offsetY)
                            };
                        }
                        if (curC % 2 == 0 && startEl==endEl)
                            g.TranslateTransform(0, stateSize / 2);
                        else
                            g.TranslateTransform(0, -stateSize / 2);

                        g.DrawCurve(edge, line);

                        var str = associationChar.Replace(".", "{.}");
                        var sz = g.MeasureString(str, Font);
                        g.DrawString(str, Font, Brushes.Black, new RectangleF(midPoint - sz.Width / 2, height - (curC % 2 == 1 ? sz.Height: 0), midPoint + sz.Width, sz.Height));

                        if (curC % 2 == 0 && startEl == endEl)
                            g.TranslateTransform(0, -stateSize / 2);
                        else
                            g.TranslateTransform(0, stateSize / 2);
                        curC++;
                    }
                }
                for (int i = 0; i < stateCount; i++)
                {
                    g.DrawEllipse(Pens.Black, new Rectangle(statesDist * i - stateSize / 2, -stateSize / 2, stateSize, stateSize));
                    if (finalStates.Contains(i)) g.DrawEllipse(Pens.Black, new Rectangle(statesDist * i - stateSize / 2+2, -stateSize / 2+2, stateSize-4, stateSize-4));

                    var szTxt = g.MeasureString("S" + i, Font);
                    g.DrawString("S" + i, Font, Brushes.Black, new RectangleF(statesDist * i - szTxt.Width / 2, -szTxt.Height / 2, szTxt.Width, szTxt.Height));
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDragging)
            {
                offset.X = offset.X + (startOffset.X - e.X);
                offset.Y = offset.Y + (startOffset.Y - e.Y);
                startOffset.X = e.X;
                startOffset.Y = e.Y;
                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            startOffset.X = e.X;
            startOffset.Y = e.Y;
            isDragging = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isDragging = false;
        }
    }
}
