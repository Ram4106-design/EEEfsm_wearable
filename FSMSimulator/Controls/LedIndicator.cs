using System.Drawing.Drawing2D;

namespace FSMSimulator.Controls
{
    public class LedIndicator : Control
    {
        private bool _isOn;
        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                Invalidate();
            }
        }

        public Color OnColor { get; set; } = Color.Red;
        public Color OffColor { get; set; } = Color.DarkGray;

        public LedIndicator()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(30, 30);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = this.ClientRectangle;
            rect.Inflate(-2, -2);

            using (var brush = new SolidBrush(IsOn ? OnColor : OffColor))
            {
                e.Graphics.FillEllipse(brush, rect);
            }

            using (var pen = new Pen(Color.Black, 1))
            {
                e.Graphics.DrawEllipse(pen, rect);
            }
            
            // Shine effect
            if (IsOn)
            {
                var shineRect = new Rectangle(rect.X + 5, rect.Y + 5, rect.Width / 3, rect.Height / 3);
                using (var brush = new SolidBrush(Color.FromArgb(150, Color.White)))
                {
                    e.Graphics.FillEllipse(brush, shineRect);
                }
            }
        }
    }
}
