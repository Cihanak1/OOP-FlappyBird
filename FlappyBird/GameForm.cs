using System.Drawing;
using System.Windows.Forms;

namespace FlappyBird;

public partial class GameForm : Form
{
    private readonly Bird bird;
    private readonly System.Windows.Forms.Timer gameTimer;
    private const int GroundHeight = 80;

    public GameForm()
    {
        InitializeComponent();

        DoubleBuffered = true;
        KeyPreview = true;

        bird = new Bird(80, 240);

        gameTimer = new System.Windows.Forms.Timer();
        gameTimer.Interval = 20;
        gameTimer.Tick += GameTimer_Tick;

        KeyDown += GameForm_KeyDown;
        Paint += GameForm_Paint;

        gameTimer.Start();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        bird.Update();

        int groundY = ClientSize.Height - GroundHeight;
        if (bird.Y + bird.Height >= groundY)
        {
            bird.Y = groundY - bird.Height;
        }

        Invalidate();
    }

    private void GameForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
        {
            bird.Jump();
        }
    }

    private void GameForm_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        using (Brush skyBrush = new SolidBrush(Color.FromArgb(112, 197, 206)))
        {
            g.FillRectangle(skyBrush, 0, 0, ClientSize.Width, ClientSize.Height);
        }

        int groundY = ClientSize.Height - GroundHeight;

        using (Brush grassBrush = new SolidBrush(Color.FromArgb(115, 191, 46)))
        {
            g.FillRectangle(grassBrush, 0, groundY, ClientSize.Width, 14);
        }

        using (Brush dirtBrush = new SolidBrush(Color.FromArgb(222, 216, 149)))
        {
            g.FillRectangle(dirtBrush, 0, groundY + 14, ClientSize.Width, GroundHeight - 14);
        }

        bird.Draw(g);

        using (Font font = new Font("Segoe UI", 11, FontStyle.Bold))
        using (Brush textBrush = new SolidBrush(Color.White))
        using (Brush textShadow = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
        {
            string hint = "Zıplamak için BOŞLUK (Space) tuşuna basın";
            SizeF size = g.MeasureString(hint, font);
            float textX = (ClientSize.Width - size.Width) / 2;
            float textY = 24;

            g.DrawString(hint, font, textShadow, textX + 1, textY + 1);
            g.DrawString(hint, font, textBrush, textX, textY);
        }
    }
}
