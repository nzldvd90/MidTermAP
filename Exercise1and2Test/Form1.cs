using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Exercise1
{
    public partial class frmTestExercise1 : Form
    {
        static string pigreco;
        public frmTestExercise1()
        {

            InitializeComponent();

            var rand = new Random();
            var piGreco = new StringBuilder(100000 + 3);
            piGreco.Append("3,");
            for (int i = 0; i < 100000; i++)
            {
                piGreco.Append(rand.Next(9));
            }
            pigreco = piGreco.ToString();

            lstLog.FullRowSelect = true;
            lstLog.AllowColumnReorder = true;
            ResizeColumns();
            this.Resize += new EventHandler((o, e) => { ResizeColumns(); });
            this.splitContainer1.SplitterMoved += new SplitterEventHandler((o, e) => { ResizeColumns(); });
        }

        private void ResizeColumns()
        {
            double theWidth = (this.lstLog.ClientSize.Width - 261);
            lstLog.Columns[0].Width = (int)(theWidth * 0.7);
            lstLog.Columns[1].Width = (int)(theWidth * 0.3);
            lstLog.Columns[2].Width = 120;
            lstLog.Columns[3].Width = 140;
        }

        private void ReportResult(string operation, string result, bool compiledMode, Stopwatch elapsedTimer)
        {
            var item = lstLog.Items.Add(operation);
            item.BackColor = Color.FromArgb(200, 250, 200);
            item.SubItems.Add(result);
            double ms1 = (double)elapsedTimer.ElapsedTicks / 10000.0;
            item.SubItems.Add(ms1.ToString("0.000") + " ms.");
            if (compiledMode)
                item.SubItems.Add("COMPILED");
            else
                item.SubItems.Add("INTERPRETED");
            item.SubItems[2].Font = new Font(new FontFamily("Arial"), 10.0f);
        }

        private void ReportError(string eventError, string result)
        {
            var item = lstLog.Items.Add(eventError);
            item.BackColor = Color.FromArgb(250, 200, 200);
            item.SubItems.Add(result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                Stopwatch timer = new Stopwatch();
                RegularExpression r;

                // performance test of parsing RegEx
                timer.Reset();
                timer.Start();
                r = new RegularExpression(txtRegEx.Text);
                timer.Stop();

                ReportData("Original Expression:\t" + r.OriginalExpression + "\r\nInfix Expression:\t" + r.FormattedExpression + "\r\nPostfix string:\t" + r.PostfixExpression + "\r\n\r\nNon Deterministic Automata has\t\t" + r.NDStateCount + " states.\r\nDeterministic Automata has\t\t" + r.DStateCount + " states.\r\nOptimized Deterministic Automata has\t" + r.OptimizedDStateCount + " states.");
                ReportResult("Parsing '" + txtRegEx.Text + "'", "SUCCESS", r.IsCompiled, timer);

                // performance test of compile MyRegEx class
                timer.Reset();
                timer.Start();
                string classString = r.Compile();
                timer.Stop();

                ReportData("GENERATED c# CLASS SOURCE:\r\n" + classString);

                ReportResult("Compile RegEx as C# class", "SUCCESS", r.IsCompiled, timer);

                // performance test of IsMatch call
                bool result = r.IsMatch(txtInput.Text, timer);
                //bool result = r.IsMatch(pigreco, timer);

                ReportResult("Match('" + txtInput.Text + "')", result.ToString(), r.IsCompiled, timer);

                automataViewer1.Initialize(r);
            }
            catch (RegularExpressionParser.RegularExpressionParserException ex)
            {
                ReportError("PARSING ERROR", ex.ErrorCode.ToString());
            }
            catch (Exception exc)
            {
                ReportError("EXCEPTION", exc.ToString());
            }
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            pnlTestResult.Visible = true;
            btnStartTest.Enabled = false;
            int numberOfTries = 10;
            long[] TicksIntepreted = null;
            long[] TicksCompiled = null;
            int curTry = 0;

            try
            {
                RegularExpression rIntepreted = new RegularExpression("([1-9][0-9]*|0)(,[0-9]*[1-9])?");
                RegularExpression rCompiled = new RegularExpression("([1-9][0-9]*|0)(,[0-9]*[1-9])?");
                string classString = rCompiled.Compile();
                progressBar1.Maximum = (numberOfTries)*(numberOfTries) - 1;

                for (int i = 0; i < numberOfTries; i++)
                {
                    TicksIntepreted = new long[numberOfTries];
                    TicksCompiled = new long[numberOfTries];
                    for (curTry = 0; curTry < numberOfTries; curTry++)
                    {
                        Stopwatch timer = new Stopwatch();

                        timer.Reset();
                        timer.Start();
                        //bool result = rIntepreted.IsMatch(txtInput.Text);
                        bool result = rIntepreted.IsMatch(pigreco);
                        timer.Stop();
                        TicksIntepreted[curTry] = timer.ElapsedTicks;

                        timer.Reset();
                        timer.Start();
                        //result = rCompiled.IsMatch(txtInput.Text, timer);
                        result = rCompiled.IsMatch(pigreco, timer);
                        timer.Stop();
                        TicksCompiled[curTry] = timer.ElapsedTicks;

                        // performance test of parsing RegEx
                        timer.Start();
                        timer.Stop();

                        if (curTry < numberOfTries) progressBar1.Value = curTry + i * numberOfTries;
                    }
                }

                DisplayTestResults(TicksIntepreted, TicksCompiled);
            }
            catch (RegularExpressionParser.RegularExpressionParserException ex)
            {
                ReportError("PARSING ERROR", ex.ErrorCode.ToString());
            }
            catch (Exception exc)
            {
                ReportError("EXCEPTION", exc.ToString());
            }
            finally
            {
                btnStartTest.Enabled = true;
            }
        }

        private void DisplayTestResults(long[] TicksIntepreted, long[] TicksCompiled)
        {
            pnlTestResult.Visible = true;
            double averageInterpreted = TicksIntepreted.Average();
            double averageCompiled = TicksCompiled.Average();
            double maxRes = Math.Max(averageInterpreted, averageCompiled);
            double minRes = Math.Min(averageInterpreted, averageCompiled);

            var str = "Average Time: --MS-- ms / --TICKS-- ticks";
            lblAverageCompiled.Text = str.Replace("--MS--", (averageCompiled / 10000.0).ToString("0.0000")).Replace("--TICKS--", averageCompiled.ToString("0"));
            lblAverageInterpreted.Text = str.Replace("--MS--", (averageInterpreted / 10000.0).ToString("0.0000")).Replace("--TICKS--", averageInterpreted.ToString("0"));

            progressBarCompiled.Maximum = progressBarInterpreted.Maximum = 100;

            progressBarCompiled.Value = (int)((minRes + maxRes - averageCompiled) / maxRes * 100);
            progressBarInterpreted.Value = (int)((minRes + maxRes - averageInterpreted) / maxRes * 100);

            if(averageCompiled<averageInterpreted)
                lblTestResult.Text = "Interpreted version of this regex is " + (averageInterpreted / averageCompiled).ToString("0") + " times slower than compiled version";
            else
                lblTestResult.Text = "Compiled version of this regex is " + (averageCompiled / averageInterpreted).ToString("0") + " slower than interpreted version";


        }

        private bool TestMode
        {
            get { return pnlTest.Visible; }
            set { pnlTest.Visible = value; pnlLog.Visible = !value; btnSwitchPanel.Text = (value ? "Exit Test Mode" : "Interpreted vs. Compiled Test"); }
        }

        private void btnInterpretate_Click(object sender, EventArgs e)
        {
            try
            {

                Stopwatch timer = new Stopwatch();
                RegularExpression r;

                timer.Reset();
                timer.Start();
                r = new RegularExpression(txtRegEx.Text);
                timer.Stop();
                ReportResult("Parsing '" + txtRegEx.Text + "'", "SUCCESS", r.IsCompiled, timer);

                timer.Reset();
                timer.Start();
                bool result = r.IsMatch(txtInput.Text);
                timer.Stop();
                ReportResult("Matching '" + txtInput.Text + "'", result.ToString(), r.IsCompiled, timer);

                ReportData("Original Expression:\t" + r.OriginalExpression + "\r\nInfix Expression:\t" + r.FormattedExpression + "\r\nPostfix string:\t" + r.PostfixExpression + "\r\n\r\nNon Deterministic Automata has\t\t" + r.NDStateCount + " states.\r\nDeterministic Automata has\t\t" + r.DStateCount + " states.\r\nOptimized Deterministic Automata has\t" + r.OptimizedDStateCount + " states.");

                automataViewer1.Initialize(r);
            }
            catch (RegularExpressionParser.RegularExpressionParserException exc)
            {
                ReportError("PARSER ERROR", exc.ToString());
            }
            catch (Exception exc)
            {
                ReportError("EXCEPTION", exc.ToString());
            }
        }

        private void ReportData(string p)
        {
            if (txtData.Text != "") txtData.AppendText("\r\n-----------------------------------\r\n\r\n");
            txtData.AppendText(p + "\r\n");
            splitContainer1.Panel2Collapsed = false;
            ResizeColumns();
        }

        private void btnSwitchPanel_Click(object sender, EventArgs e)
        {
            TestMode = !TestMode;
        }
    }
}

