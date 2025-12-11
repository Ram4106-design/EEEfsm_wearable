using System.Drawing.Drawing2D;

namespace FSMSimulator.Controls
{
    public class WaveformPanel : Control
    {
        public class Frame
        {
            public long Time { get; set; }
            public bool[] Inputs { get; set; } = new bool[6];
            public bool[] Outputs { get; set; } = new bool[6];
            public int State { get; set; }
        }

        private List<Frame> _history = new List<Frame>();
        private int _maxHistory = 1000;
        private int _scrollOffset = 0;
        private int _zoom = 10; // Pixels per tick

        public WaveformPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            this.Height = 300;
        }

        public void AddFrame(long time, bool[] inputs, bool[] outputs, int state)
        {
            var frame = new Frame
            {
                Time = time,
                Inputs = (bool[])inputs.Clone(),
                Outputs = (bool[])outputs.Clone(),
                State = state
            };
            
            _history.Add(frame);
            if (_history.Count > _maxHistory)
            {
                _history.RemoveAt(0);
            }
            
            // Auto scroll
            _scrollOffset = Math.Max(0, _history.Count * _zoom - this.Width + 100);
            
            Invalidate();
        }

        public void Clear()
        {
            _history.Clear();
            _scrollOffset = 0;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;

            if (_history.Count == 0) return;

            // Define layout
            int startY = 30;
            int rowHeight = 25;
            int labelWidth = 80;

            // Draw Labels
            var labels = new[] { "CLK", "RST_N" }
                .Concat(Enumerable.Range(1, 6).Select(i => $"S{i}"))
                .Concat(Enumerable.Range(1, 6).Select(i => $"A{i}"))
                .Concat(new[] { "STATE" })
                .ToArray();

            using (var font = new Font("Consolas", 9))
            using (var brush = new SolidBrush(Color.LightGray))
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    g.DrawString(labels[i], font, brush, 5, startY + i * rowHeight);
                }
            }

            // Draw Grid
            using (var pen = new Pen(Color.FromArgb(50, 50, 50)))
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    int y = startY + i * rowHeight + rowHeight;
                    g.DrawLine(pen, 0, y, Width, y);
                }
            }

            // Draw Signals
            int signalCount = labels.Length;
            
            // Helper to get value
            Func<Frame, int, int> getValue = (f, idx) =>
            {
                if (idx == 0) return (int)(f.Time % 2); // CLK (fake)
                if (idx == 1) return 1; // RST (assume high for now, need to track)
                if (idx >= 2 && idx <= 7) return f.Inputs[idx - 2] ? 1 : 0;
                if (idx >= 8 && idx <= 13) return f.Outputs[idx - 8] ? 1 : 0;
                return 0; // State handled separately
            };

            using (var penGreen = new Pen(Color.Lime, 1))
            using (var penRed = new Pen(Color.Red, 1))
            using (var penYellow = new Pen(Color.Yellow, 1))
            {
                for (int i = 0; i < _history.Count - 1; i++)
                {
                    var f1 = _history[i];
                    var f2 = _history[i+1];
                    
                    int x1 = labelWidth + i * _zoom - _scrollOffset;
                    int x2 = labelWidth + (i + 1) * _zoom - _scrollOffset;

                    if (x2 < labelWidth) continue;
                    if (x1 > Width) break;

                    for (int j = 0; j < signalCount; j++)
                    {
                        int yBase = startY + j * rowHeight + rowHeight - 5;
                        
                        if (j == signalCount - 1) // State (Bus)
                        {
                            // Draw bus
                            // Simplified for now
                        }
                        else // Digital
                        {
                            int v1 = getValue(f1, j);
                            int v2 = getValue(f2, j);
                            
                            int y1 = yBase - (v1 * 15);
                            int y2 = yBase - (v2 * 15);

                            g.DrawLine(penGreen, x1, y1, x2, y1);
                            if (v1 != v2)
                            {
                                g.DrawLine(penGreen, x2, y1, x2, y2);
                            }
                        }
                    }
                }
            }
        }
    }
}
