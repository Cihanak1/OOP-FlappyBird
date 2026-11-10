using System.Drawing;

namespace FlappyBird;

public class Bird
{
    public float X { get; set; }
    public float Y { get; set; }
    public int Width { get; } = 34;
    public int Height { get; } = 24;

    public float VelocityY { get; private set; }
    public float Gravity { get; set; } = 0.5f;
    public float JumpStrength { get; set; } = -8.5f;

    public Bird(float startX, float startY)
    {
        X = startX;
        Y = startY;
        VelocityY = 0;
    }

    public void Update()
    {
        VelocityY += Gravity;
        Y += VelocityY;
    }

    public void Jump()
    {
        VelocityY = JumpStrength;
    }

    public void Reset(float startX, float startY)
    {
        X = startX;
        Y = startY;
        VelocityY = 0;
    }

    public Rectangle GetBounds()
    {
        return new Rectangle((int)X, (int)Y, Width, Height);
    }

    public void Draw(Graphics g)
    {
        int x = (int)X;
        int y = (int)Y;

        using (Brush bodyBrush = new SolidBrush(Color.Gold))
        using (Pen borderPen = new Pen(Color.FromArgb(160, 100, 0), 2))
        {
            g.FillEllipse(bodyBrush, x, y, Width, Height);
            g.DrawEllipse(borderPen, x, y, Width, Height);
        }

        using (Brush wingBrush = new SolidBrush(Color.DarkOrange))
        {
            g.FillEllipse(wingBrush, x + 4, y + 8, 14, 10);
        }

        using (Brush eyeWhiteBrush = new SolidBrush(Color.White))
        using (Brush eyePupilBrush = new SolidBrush(Color.Black))
        {
            g.FillEllipse(eyeWhiteBrush, x + 20, y + 4, 8, 8);
            g.FillEllipse(eyePupilBrush, x + 24, y + 6, 4, 4);
        }

        using (Brush beakBrush = new SolidBrush(Color.OrangeRed))
        {
            Point[] beakPoints =
            {
                new Point(x + 28, y + 10),
                new Point(x + 36, y + 14),
                new Point(x + 28, y + 18)
            };
            g.FillPolygon(beakBrush, beakPoints);
        }
    }
}
