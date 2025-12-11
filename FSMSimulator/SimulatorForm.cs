using FSMSimulator.Controls;
using System.Drawing.Drawing2D;

namespace FSMSimulator
{
    public partial class SimulatorForm : Form
    {
        private FSM_Wearable _fsm;
        private CheckBox[] _chkInputs;
        private LedIndicator[] _ledOutputs;
        private StateNode[] _stateNodes;
        private long _tickCount = 0;

        public SimulatorForm()
        {
            InitializeComponent();
            _fsm = new FSM_Wearable();
        }

        private void SimulatorForm_Load(object sender, EventArgs e)
        {
            InitializeInputs();
            InitializeOutputs();
            InitializeDiagram();
            InitializeTests();
            UpdateSpeed();
        }

        private void InitializeInputs()
        {
            _chkInputs = new CheckBox[6];
            for (int i = 0; i < 6; i++)
            {
                _chkInputs[i] = new CheckBox
                {
                    Text = $"Sensor {i + 1} (S{i + 1})",
                    Tag = i,
                    Location = new Point(20, 30 + i * 40),
                    AutoSize = true
                };
                _chkInputs[i].CheckedChanged += Input_CheckedChanged;
                grpInputs.Controls.Add(_chkInputs[i]);
            }
        }

        private void InitializeOutputs()
        {
            _ledOutputs = new LedIndicator[6];
            for (int i = 0; i < 6; i++)
            {
                var lbl = new Label
                {
                    Text = $"Actuator {i + 1}",
                    Location = new Point(50, 35 + i * 45),
                    AutoSize = true
                };
                
                _ledOutputs[i] = new LedIndicator
                {
                    Location = new Point(15, 30 + i * 45),
                    Size = new Size(30, 30),
                    OnColor = Color.Red
                };
                
                grpOutputs.Controls.Add(lbl);
                grpOutputs.Controls.Add(_ledOutputs[i]);
            }
        }

        private void InitializeDiagram()
        {
            // Create nodes for diagram
            _stateNodes = new StateNode[4];
            
            // IDLE
            _stateNodes[(int)State.IDLE] = new StateNode 
            { 
                StateName = "IDLE", 
                BaseColor = Color.CornflowerBlue,
                Location = new Point(300, 50) 
            };

            // LIGHT
            _stateNodes[(int)State.LIGHT_DEHY] = new StateNode 
            { 
                StateName = "LIGHT", 
                BaseColor = Color.Orange,
                Location = new Point(300, 250) 
            };

            // SEVERE
            _stateNodes[(int)State.SEVERE_DEHY] = new StateNode 
            { 
                StateName = "SEVERE", 
                BaseColor = Color.Red,
                Location = new Point(100, 150) 
            };

            // ACTIVITY
            _stateNodes[(int)State.ACTIVITY_ALERT] = new StateNode 
            { 
                StateName = "ACTIVITY", 
                BaseColor = Color.LimeGreen,
                Location = new Point(500, 150) 
            };

            foreach (var node in _stateNodes)
            {
                pnlDiagram.Controls.Add(node);
            }
        }

        private void Input_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is CheckBox chk && chk.Tag is int index)
            {
                _fsm.SetSensor(index, chk.Checked);
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            _tickCount++;
            _fsm.ClockTick();
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Update LEDs
            for (int i = 0; i < 6; i++)
            {
                _ledOutputs[i].IsOn = _fsm.ActuatorOutputs[i];
            }

            // Update State Diagram
            for (int i = 0; i < 4; i++)
            {
                _stateNodes[i].IsActive = (int)_fsm.CurrentState == i;
            }
            pnlDiagram.Invalidate(); // Redraw arrows

            // Update Waveform
            waveformPanel1.AddFrame(_tickCount, _fsm.SensorInputs, _fsm.ActuatorOutputs, (int)_fsm.CurrentState);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timerClock.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerClock.Stop();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _fsm.Reset();
            _tickCount = 0;
            waveformPanel1.Clear();
            
            // Reset UI inputs
            foreach (var chk in _chkInputs) chk.Checked = false;
            
            UpdateUI();
        }

        private void trkSpeed_Scroll(object sender, EventArgs e)
        {
            UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            // Invert logic: High value = Low interval (Fast)
            int val = trkSpeed.Maximum - trkSpeed.Value + trkSpeed.Minimum;
            timerClock.Interval = val;
        }

        private void pnlDiagram_Paint(object sender, PaintEventArgs e)
        {
            // Draw arrows between states
            // Simplified: Draw lines from center to center
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var pen = new Pen(Color.Gray, 2))
            {
                pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);
                
                Point GetCenter(State s) 
                {
                    var node = _stateNodes[(int)s];
                    return new Point(node.Left + node.Width/2, node.Top + node.Height/2);
                }

                // Draw all possible transitions (simplified visualization)
                // IDLE -> LIGHT, SEVERE, ACTIVITY
                var pIdle = GetCenter(State.IDLE);
                var pLight = GetCenter(State.LIGHT_DEHY);
                var pSevere = GetCenter(State.SEVERE_DEHY);
                var pActivity = GetCenter(State.ACTIVITY_ALERT);

                e.Graphics.DrawLine(pen, pIdle, pLight);
                e.Graphics.DrawLine(pen, pIdle, pSevere);
                e.Graphics.DrawLine(pen, pIdle, pActivity);
                
                // Inter-state
                e.Graphics.DrawLine(pen, pLight, pSevere);
                e.Graphics.DrawLine(pen, pLight, pActivity);
                e.Graphics.DrawLine(pen, pActivity, pSevere);
                
                // Back to IDLE
                // (Visual clutter if we draw all, just drawing main ones)
            }
        }


        private void InitializeTests()
        {
            cboTests.Items.Clear();
            foreach (var test in TestCases.AllTests)
            {
                cboTests.Items.Add(test.Name);
            }
            if (cboTests.Items.Count > 0) cboTests.SelectedIndex = 0;
        }

        private async void btnRunTest_Click(object sender, EventArgs e)
        {
            if (cboTests.SelectedIndex < 0) return;
            
            var test = TestCases.AllTests[cboTests.SelectedIndex];
            
            // 1. Reset
            btnReset_Click(sender, e);
            await Task.Delay(100);

            // 2. Set Inputs
            for (int i = 0; i < 6; i++)
            {
                _chkInputs[i].Checked = test.Inputs[i];
            }

            // 3. Run for a few cycles (Synchronizer needs 2-3 cycles)
            for(int i=0; i<5; i++)
            {
                timerClock_Tick(sender, e);
                await Task.Delay(50); // Visual delay
            }

            // 4. Verify
            bool stateMatch = _fsm.CurrentState == test.ExpectedState;
            bool outputMatch = true;
            for(int i=0; i<6; i++)
            {
                if (_fsm.ActuatorOutputs[i] != test.ExpectedOutputs[i]) outputMatch = false;
            }

            if (stateMatch && outputMatch)
            {
                MessageBox.Show("Test Passed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Test Failed!\nExpected State: {test.ExpectedState}\nActual State: {_fsm.CurrentState}", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
