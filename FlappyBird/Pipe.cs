using System.Drawing;

namespace FlappyBird;

public class Pipe
{
    public int X { get; set; }
    public int Width { get; } = 60;
    public int TopHeight { get; }
    public int Gap { get; }
    public int GroundY { get; }
    public int Speed { get; set; } = 3;
    public bool IsPassed { get; set; } = false;

    public Pipe(int startX, int topHeight, int gap, int groundY)
    {
        X = startX;
        TopHeight = topHeight;
        Gap = gap;
        GroundY = groundY;
    }

    public void Update()
    {
        X -= Speed;
    }

    public Rectangle GetTopBounds()
    {
        return new Rectangle(X, 0, Width, TopHeight);
    }

    public Rectangle GetBottomBounds()
    {
        int bottomY = TopHeight + Gap;
        int bottomHeight = GroundY - bottomY;
        return new Rectangle(X, bottomY, Width, bottomHeight);
    }

    public bool CheckCollision(Rectangle birdBounds)
    {
        return birdBounds.IntersectsWith(GetTopBounds()) || birdBounds.IntersectsWith(GetBottomBounds());
    }

    public void Draw(Graphics g)
    {
        using (Brush pipeBrush = new SolidBrush(Color.FromArgb(115, 191, 46)))
        using (Pen borderPen = new Pen(Color.FromArgb(80, 140, 30), 2))
        {
            Rectangle topRect = GetTopBounds();
            g.FillRectangle(pipeBrush, topRect);
            g.DrawRectangle(borderPen, topRect);

            Rectangle bottomRect = GetBottomBounds();
            g.FillRectangle(pipeBrush, bottomRect);
            g.DrawRectangle(borderPen, bottomRect);

            g.FillRectangle(pipeBrush, X - 3, TopHeight - 20, Width + 6, 20);
            g.DrawRectangle(borderPen, X - 3, TopHeight - 20, Width + 6, 20);

            int bottomY = TopHeight + Gap;
            g.FillRectangle(pipeBrush, X - 3, bottomY, Width + 6, 20);
            g.DrawRectangle(borderPen, X - 3, bottomY, Width + 6, 20);
        }
    }
}
