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
        g.FillRectangle(Brushes.Green, GetTopBounds());
        g.DrawRectangle(Pens.Black, GetTopBounds());

        g.FillRectangle(Brushes.Green, GetBottomBounds());
        g.DrawRectangle(Pens.Black, GetBottomBounds());
    }
}
