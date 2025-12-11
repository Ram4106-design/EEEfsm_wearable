namespace FSMSimulator
{
    partial class SimulatorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelTop = new System.Windows.Forms.Panel();
            this.grpInputs = new System.Windows.Forms.GroupBox();
            this.grpOutputs = new System.Windows.Forms.GroupBox();
            this.pnlDiagram = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.trkSpeed = new System.Windows.Forms.TrackBar();
            this.waveformPanel1 = new FSMSimulator.Controls.WaveformPanel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).BeginInit();
            this.SuspendLayout();

            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelTop);
            this.splitContainer1.Panel1.Controls.Add(this.pnlControls);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.waveformPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 700);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 0;

            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.pnlDiagram);
            this.panelTop.Controls.Add(this.grpOutputs);
            this.panelTop.Controls.Add(this.grpInputs);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 50);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1000, 350);
            this.panelTop.TabIndex = 1;

            // 
            // grpInputs
            // 
            this.grpInputs.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpInputs.Location = new System.Drawing.Point(0, 0);
            this.grpInputs.Name = "grpInputs";
            this.grpInputs.Size = new System.Drawing.Size(150, 350);
            this.grpInputs.TabIndex = 0;
            this.grpInputs.TabStop = false;
            this.grpInputs.Text = "Sensor Inputs";

            // 
            // grpOutputs
            // 
            this.grpOutputs.Dock = System.Windows.Forms.DockStyle.Right;
            this.grpOutputs.Location = new System.Drawing.Point(850, 0);
            this.grpOutputs.Name = "grpOutputs";
            this.grpOutputs.Size = new System.Drawing.Size(150, 350);
            this.grpOutputs.TabIndex = 1;
            this.grpOutputs.TabStop = false;
            this.grpOutputs.Text = "Actuator Outputs";

            // 
            // pnlDiagram
            // 
            this.pnlDiagram.BackColor = System.Drawing.Color.White;
            this.pnlDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDiagram.Location = new System.Drawing.Point(150, 0);
            this.pnlDiagram.Name = "pnlDiagram";
            this.pnlDiagram.Size = new System.Drawing.Size(700, 350);
            this.pnlDiagram.TabIndex = 2;
            this.pnlDiagram.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDiagram_Paint);

            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnRunTest);
            this.pnlControls.Controls.Add(this.cboTests);
            this.pnlControls.Controls.Add(this.trkSpeed);
            this.pnlControls.Controls.Add(this.lblSpeed);
            this.pnlControls.Controls.Add(this.btnReset);
            this.pnlControls.Controls.Add(this.btnStop);
            this.pnlControls.Controls.Add(this.btnStart);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 0);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1000, 50);
            this.pnlControls.TabIndex = 0;

            // 
            // cboTests
            // 
            this.cboTests.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTests.FormattingEnabled = true;
            this.cboTests.Location = new System.Drawing.Point(550, 12);
            this.cboTests.Name = "cboTests";
            this.cboTests.Size = new System.Drawing.Size(200, 23);
            this.cboTests.TabIndex = 5;

            // 
            // btnRunTest
            // 
            this.btnRunTest.Location = new System.Drawing.Point(760, 12);
            this.btnRunTest.Name = "btnRunTest";
            this.btnRunTest.Size = new System.Drawing.Size(75, 23);
            this.btnRunTest.TabIndex = 6;
            this.btnRunTest.Text = "Run Test";
            this.btnRunTest.UseVisualStyleBackColor = true;
            this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);

            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(93, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);

            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(174, 12);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(270, 16);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(42, 15);
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "Speed:";

            // 
            // trkSpeed
            // 
            this.trkSpeed.Location = new System.Drawing.Point(318, 3);
            this.trkSpeed.Maximum = 1000;
            this.trkSpeed.Minimum = 10;
            this.trkSpeed.Name = "trkSpeed";
            this.trkSpeed.Size = new System.Drawing.Size(200, 45);
            this.trkSpeed.TabIndex = 4;
            this.trkSpeed.Value = 100;
            this.trkSpeed.TickFrequency = 100;
            this.trkSpeed.Scroll += new System.EventHandler(this.trkSpeed_Scroll);

            // 
            // waveformPanel1
            // 
            this.waveformPanel1.BackColor = System.Drawing.Color.Black;
            this.waveformPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.waveformPanel1.Location = new System.Drawing.Point(0, 0);
            this.waveformPanel1.Name = "waveformPanel1";
            this.waveformPanel1.Size = new System.Drawing.Size(1000, 296);
            this.waveformPanel1.TabIndex = 0;

            // 
            // timerClock
            // 
            this.timerClock.Interval = 100;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);

            // 
            // SimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SimulatorForm";
            this.Text = "FSM Wearable Simulator";
            this.Load += new System.EventHandler(this.SimulatorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox grpInputs;
        private System.Windows.Forms.GroupBox grpOutputs;
        private System.Windows.Forms.Panel pnlDiagram;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TrackBar trkSpeed;
        private System.Windows.Forms.ComboBox cboTests;
        private System.Windows.Forms.Button btnRunTest;
        private FSMSimulator.Controls.WaveformPanel waveformPanel1;
        private System.Windows.Forms.Timer timerClock;
    }
}
