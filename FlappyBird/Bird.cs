using System.Drawing;

namespace FlappyBird;

public class Bird
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Size { get; } = 30;

    public int VelocityY { get; set; }
    public int Gravity { get; set; } = 1;
    public int JumpStrength { get; set; } = -12;

    public Bird(int startX, int startY)
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

    public void Reset(int startX, int startY)
    {
        X = startX;
        Y = startY;
        VelocityY = 0;
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(X, Y, Size, Size);
    }

    public void Draw(Graphics g)
    {
        g.FillEllipse(Brushes.Yellow, X, Y, Size, Size);
        g.DrawEllipse(Pens.Black, X, Y, Size, Size);
    }
}
