namespace Exercise1
{
    partial class frmTestExercise1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTestExercise1));
            this.pnlTopContainer = new System.Windows.Forms.Panel();
            this.pnlTopLeftContainer = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRegEx = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.pnlTopRightContainer = new System.Windows.Forms.Panel();
            this.btnSwitchPanel = new System.Windows.Forms.Button();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnInterpretate = new System.Windows.Forms.Button();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txtData = new System.Windows.Forms.TextBox();
            this.pnlTest = new System.Windows.Forms.Panel();
            this.pnlTestResult = new System.Windows.Forms.Panel();
            this.lblTestResult = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.progressBarCompiled = new System.Windows.Forms.ProgressBar();
            this.lblAverageCompiled = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBarInterpreted = new System.Windows.Forms.ProgressBar();
            this.lblAverageInterpreted = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlProgressContainer = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnStartTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.automataViewer1 = new Exercise1.AutomataViewer();
            this.pnlTopContainer.SuspendLayout();
            this.pnlTopLeftContainer.SuspendLayout();
            this.pnlTopRightContainer.SuspendLayout();
            this.pnlLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.pnlTest.SuspendLayout();
            this.pnlTestResult.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlProgressContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopContainer
            // 
            this.pnlTopContainer.Controls.Add(this.pnlTopLeftContainer);
            this.pnlTopContainer.Controls.Add(this.pnlTopRightContainer);
            this.pnlTopContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlTopContainer.Name = "pnlTopContainer";
            this.pnlTopContainer.Size = new System.Drawing.Size(1195, 132);
            this.pnlTopContainer.TabIndex = 4;
            // 
            // pnlTopLeftContainer
            // 
            this.pnlTopLeftContainer.Controls.Add(this.label3);
            this.pnlTopLeftContainer.Controls.Add(this.txtRegEx);
            this.pnlTopLeftContainer.Controls.Add(this.label2);
            this.pnlTopLeftContainer.Controls.Add(this.txtInput);
            this.pnlTopLeftContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopLeftContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLeftContainer.Name = "pnlTopLeftContainer";
            this.pnlTopLeftContainer.Padding = new System.Windows.Forms.Padding(12);
            this.pnlTopLeftContainer.Size = new System.Drawing.Size(984, 132);
            this.pnlTopLeftContainer.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "String to match";
            // 
            // txtRegEx
            // 
            this.txtRegEx.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtRegEx.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRegEx.Location = new System.Drawing.Point(12, 30);
            this.txtRegEx.Name = "txtRegEx";
            this.txtRegEx.Size = new System.Drawing.Size(960, 31);
            this.txtRegEx.TabIndex = 8;
            this.txtRegEx.Text = "[a-g]*.[b-f]*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Regular Expression";
            // 
            // txtInput
            // 
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtInput.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.Location = new System.Drawing.Point(12, 89);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(960, 31);
            this.txtInput.TabIndex = 9;
            this.txtInput.Text = "abccdgkbb";
            // 
            // pnlTopRightContainer
            // 
            this.pnlTopRightContainer.Controls.Add(this.btnSwitchPanel);
            this.pnlTopRightContainer.Controls.Add(this.btnCompile);
            this.pnlTopRightContainer.Controls.Add(this.btnInterpretate);
            this.pnlTopRightContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTopRightContainer.Location = new System.Drawing.Point(984, 0);
            this.pnlTopRightContainer.Name = "pnlTopRightContainer";
            this.pnlTopRightContainer.Size = new System.Drawing.Size(211, 132);
            this.pnlTopRightContainer.TabIndex = 8;
            // 
            // btnSwitchPanel
            // 
            this.btnSwitchPanel.Location = new System.Drawing.Point(0, 91);
            this.btnSwitchPanel.Name = "btnSwitchPanel";
            this.btnSwitchPanel.Size = new System.Drawing.Size(200, 30);
            this.btnSwitchPanel.TabIndex = 12;
            this.btnSwitchPanel.Text = "Interpreted vs. Compiled Test";
            this.btnSwitchPanel.UseVisualStyleBackColor = true;
            this.btnSwitchPanel.Click += new System.EventHandler(this.btnSwitchPanel_Click);
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(0, 51);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(200, 30);
            this.btnCompile.TabIndex = 11;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnInterpretate
            // 
            this.btnInterpretate.Location = new System.Drawing.Point(-1, 12);
            this.btnInterpretate.Name = "btnInterpretate";
            this.btnInterpretate.Size = new System.Drawing.Size(200, 30);
            this.btnInterpretate.TabIndex = 10;
            this.btnInterpretate.Text = "Intepretate";
            this.btnInterpretate.UseVisualStyleBackColor = true;
            this.btnInterpretate.Click += new System.EventHandler(this.btnInterpretate_Click);
            // 
            // pnlLog
            // 
            this.pnlLog.Controls.Add(this.splitContainer1);
            this.pnlLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLog.Location = new System.Drawing.Point(0, 132);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(1195, 592);
            this.pnlLog.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstLog);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1195, 592);
            this.splitContainer1.SplitterDistance = 500;
            this.splitContainer1.SplitterWidth = 12;
            this.splitContainer1.TabIndex = 0;
            // 
            // lstLog
            // 
            this.lstLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader2,
            this.columnHeader3});
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLog.Location = new System.Drawing.Point(0, 0);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(500, 592);
            this.lstLog.TabIndex = 1;
            this.lstLog.UseCompatibleStateImageBehavior = false;
            this.lstLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Operation";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Result";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Elapsed Time";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Mode";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtData);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.automataViewer1);
            this.splitContainer2.Size = new System.Drawing.Size(683, 592);
            this.splitContainer2.SplitterDistance = 355;
            this.splitContainer2.SplitterWidth = 12;
            this.splitContainer2.TabIndex = 0;
            // 
            // txtData
            // 
            this.txtData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtData.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtData.Location = new System.Drawing.Point(0, 0);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.ReadOnly = true;
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtData.Size = new System.Drawing.Size(683, 355);
            this.txtData.TabIndex = 2;
            this.txtData.WordWrap = false;
            // 
            // pnlTest
            // 
            this.pnlTest.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTest.Controls.Add(this.pnlTestResult);
            this.pnlTest.Controls.Add(this.pnlProgressContainer);
            this.pnlTest.Controls.Add(this.label1);
            this.pnlTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTest.Location = new System.Drawing.Point(0, 132);
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Padding = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.pnlTest.Size = new System.Drawing.Size(1195, 592);
            this.pnlTest.TabIndex = 6;
            this.pnlTest.Visible = false;
            // 
            // pnlTestResult
            // 
            this.pnlTestResult.Controls.Add(this.lblTestResult);
            this.pnlTestResult.Controls.Add(this.panel3);
            this.pnlTestResult.Controls.Add(this.panel1);
            this.pnlTestResult.Controls.Add(this.label6);
            this.pnlTestResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTestResult.Location = new System.Drawing.Point(12, 128);
            this.pnlTestResult.Name = "pnlTestResult";
            this.pnlTestResult.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.pnlTestResult.Size = new System.Drawing.Size(1171, 156);
            this.pnlTestResult.TabIndex = 5;
            this.pnlTestResult.Visible = false;
            // 
            // lblTestResult
            // 
            this.lblTestResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTestResult.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestResult.Location = new System.Drawing.Point(0, 107);
            this.lblTestResult.Name = "lblTestResult";
            this.lblTestResult.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.lblTestResult.Size = new System.Drawing.Size(1171, 40);
            this.lblTestResult.TabIndex = 7;
            this.lblTestResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.progressBarCompiled);
            this.panel3.Controls.Add(this.lblAverageCompiled);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 67);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.panel3.Size = new System.Drawing.Size(1171, 40);
            this.panel3.TabIndex = 5;
            // 
            // progressBarCompiled
            // 
            this.progressBarCompiled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarCompiled.Location = new System.Drawing.Point(93, 12);
            this.progressBarCompiled.Name = "progressBarCompiled";
            this.progressBarCompiled.Size = new System.Drawing.Size(828, 28);
            this.progressBarCompiled.TabIndex = 7;
            // 
            // lblAverageCompiled
            // 
            this.lblAverageCompiled.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAverageCompiled.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAverageCompiled.Location = new System.Drawing.Point(921, 12);
            this.lblAverageCompiled.Name = "lblAverageCompiled";
            this.lblAverageCompiled.Size = new System.Drawing.Size(250, 28);
            this.lblAverageCompiled.TabIndex = 8;
            this.lblAverageCompiled.Text = "Average Time: 0.001 ms / 12938 ticks";
            this.lblAverageCompiled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(0, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 28);
            this.label5.TabIndex = 6;
            this.label5.Text = "Compiled performance";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBarInterpreted);
            this.panel1.Controls.Add(this.lblAverageInterpreted);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1171, 28);
            this.panel1.TabIndex = 4;
            // 
            // progressBarInterpreted
            // 
            this.progressBarInterpreted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarInterpreted.Location = new System.Drawing.Point(93, 0);
            this.progressBarInterpreted.Name = "progressBarInterpreted";
            this.progressBarInterpreted.Size = new System.Drawing.Size(828, 28);
            this.progressBarInterpreted.TabIndex = 4;
            // 
            // lblAverageInterpreted
            // 
            this.lblAverageInterpreted.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAverageInterpreted.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAverageInterpreted.Location = new System.Drawing.Point(921, 0);
            this.lblAverageInterpreted.Name = "lblAverageInterpreted";
            this.lblAverageInterpreted.Size = new System.Drawing.Size(250, 28);
            this.lblAverageInterpreted.TabIndex = 6;
            this.lblAverageInterpreted.Text = "Average Time: 0.001 ms / 12938 ticks";
            this.lblAverageInterpreted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 28);
            this.label4.TabIndex = 5;
            this.label4.Text = "Interpreted performance";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 27);
            this.label6.TabIndex = 6;
            this.label6.Text = "Test Result";
            // 
            // pnlProgressContainer
            // 
            this.pnlProgressContainer.Controls.Add(this.progressBar1);
            this.pnlProgressContainer.Controls.Add(this.btnStartTest);
            this.pnlProgressContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProgressContainer.Location = new System.Drawing.Point(12, 100);
            this.pnlProgressContainer.Name = "pnlProgressContainer";
            this.pnlProgressContainer.Size = new System.Drawing.Size(1171, 28);
            this.pnlProgressContainer.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1085, 28);
            this.progressBar1.TabIndex = 4;
            // 
            // btnStartTest
            // 
            this.btnStartTest.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnStartTest.Location = new System.Drawing.Point(1085, 0);
            this.btnStartTest.Name = "btnStartTest";
            this.btnStartTest.Size = new System.Drawing.Size(86, 28);
            this.btnStartTest.TabIndex = 3;
            this.btnStartTest.Text = "Run Test";
            this.btnStartTest.UseVisualStyleBackColor = true;
            this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.label1.Size = new System.Drawing.Size(1171, 100);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // automataViewer1
            // 
            this.automataViewer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.automataViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.automataViewer1.Location = new System.Drawing.Point(0, 0);
            this.automataViewer1.Name = "automataViewer1";
            this.automataViewer1.Size = new System.Drawing.Size(683, 225);
            this.automataViewer1.TabIndex = 4;
            // 
            // frmTestExercise1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 724);
            this.Controls.Add(this.pnlTest);
            this.Controls.Add(this.pnlLog);
            this.Controls.Add(this.pnlTopContainer);
            this.Name = "frmTestExercise1";
            this.Text = "My RegEx Parser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlTopContainer.ResumeLayout(false);
            this.pnlTopLeftContainer.ResumeLayout(false);
            this.pnlTopLeftContainer.PerformLayout();
            this.pnlTopRightContainer.ResumeLayout(false);
            this.pnlLog.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.pnlTest.ResumeLayout(false);
            this.pnlTestResult.ResumeLayout(false);
            this.pnlTestResult.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlProgressContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopContainer;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Panel pnlTest;
        private System.Windows.Forms.Panel pnlTopLeftContainer;
        private System.Windows.Forms.Panel pnlTopRightContainer;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Button btnInterpretate;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtRegEx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlProgressContainer;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnStartTest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSwitchPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lstLog;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtData;
        private AutomataViewer automataViewer1;
        private System.Windows.Forms.Panel pnlTestResult;
        private System.Windows.Forms.Label lblTestResult;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ProgressBar progressBarCompiled;
        private System.Windows.Forms.Label lblAverageCompiled;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBarInterpreted;
        private System.Windows.Forms.Label lblAverageInterpreted;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
    }
}

