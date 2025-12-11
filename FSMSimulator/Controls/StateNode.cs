using System.Drawing.Drawing2D;

namespace FSMSimulator.Controls
{
    public class StateNode : Control
    {
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                Invalidate();
            }
        }

        public string StateName { get; set; } = "STATE";
        public Color BaseColor { get; set; } = Color.Blue;

        public StateNode()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(100, 60);
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = this.ClientRectangle;
            rect.Inflate(-2, -2);

            // Draw shadow if active
            if (IsActive)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(rect);
                    using (var brush = new PathGradientBrush(path))
                    {
                        brush.CenterColor = Color.FromArgb(100, Color.Yellow);
                        brush.SurroundColors = new[] { Color.Transparent };
                        e.Graphics.FillEllipse(brush, rect);
                    }
                }
            }

            // Draw Node
            using (var brush = new SolidBrush(IsActive ? ControlPaint.Light(BaseColor) : BaseColor))
            {
                e.Graphics.FillEllipse(brush, rect);
            }

            using (var pen = new Pen(IsActive ? Color.Yellow : Color.Black, IsActive ? 3 : 1))
            {
                e.Graphics.DrawEllipse(pen, rect);
            }

            // Draw Text
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            
            using (var brush = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString(StateName, this.Font, brush, rect, sf);
            }
        }
    }
}
