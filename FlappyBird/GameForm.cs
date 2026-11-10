using System.Drawing;
using System.Windows.Forms;

namespace FlappyBird;

public partial class GameForm : Form
{
    private readonly Bird bird;
    private readonly List<Pipe> pipes;
    private readonly System.Windows.Forms.Timer gameTimer;
    private readonly Random random;

    private const int GroundHeight = 80;
    private int score = 0;
    private bool isGameOver = false;

    public GameForm()
    {
        InitializeComponent();

        DoubleBuffered = true;
        KeyPreview = true;

        random = new Random();
        bird = new Bird(80, 240);
        pipes = new List<Pipe>();

        gameTimer = new System.Windows.Forms.Timer();
        gameTimer.Interval = 20;
        gameTimer.Tick += GameTimer_Tick;

        KeyDown += GameForm_KeyDown;
        Paint += GameForm_Paint;

        gameTimer.Start();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        if (isGameOver)
        {
            return;
        }

        bird.Update();

        int groundY = ClientSize.Height - GroundHeight;

        if (bird.Y <= 0 || bird.Y + bird.Height >= groundY)
        {
            GameOver();
            return;
        }

        if (pipes.Count == 0 || ClientSize.Width - pipes[pipes.Count - 1].X >= 200)
        {
            SpawnPipe(groundY);
        }

        for (int i = pipes.Count - 1; i >= 0; i--)
        {
            Pipe pipe = pipes[i];
            pipe.Update();

            if (pipe.CheckCollision(bird.GetBounds()))
            {
                GameOver();
                return;
            }

            if (!pipe.IsPassed && bird.X > pipe.X + pipe.Width)
            {
                pipe.IsPassed = true;
                score++;
            }

            if (pipe.X + pipe.Width < 0)
            {
                pipes.RemoveAt(i);
            }
        }

        Invalidate();
    }

    private void SpawnPipe(int groundY)
    {
        int gap = 140;
        int minHeight = 60;
        int maxHeight = groundY - gap - 60;
        int topHeight = random.Next(minHeight, maxHeight);

        pipes.Add(new Pipe(ClientSize.Width, topHeight, gap, groundY));
    }

    private void GameOver()
    {
        isGameOver = true;
        gameTimer.Stop();
        Invalidate();
    }

    private void GameForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
        {
            if (!isGameOver)
            {
                bird.Jump();
            }
        }
    }

    private void GameForm_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        using (Brush skyBrush = new SolidBrush(Color.FromArgb(112, 197, 206)))
        {
            g.FillRectangle(skyBrush, 0, 0, ClientSize.Width, ClientSize.Height);
        }

        foreach (Pipe pipe in pipes)
        {
            pipe.Draw(g);
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

        using (Font scoreFont = new Font("Segoe UI", 18, FontStyle.Bold))
        using (Brush scoreBrush = new SolidBrush(Color.White))
        using (Brush shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
        {
            string scoreText = "Puan: " + score;
            g.DrawString(scoreText, scoreFont, shadowBrush, 17, 17);
            g.DrawString(scoreText, scoreFont, scoreBrush, 15, 15);
        }

        if (isGameOver)
        {
            using (Font titleFont = new Font("Segoe UI", 24, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.Red))
            using (Brush shadowBrush = new SolidBrush(Color.Black))
            {
                string msg = "OYUN BİTTİ!";
                SizeF size = g.MeasureString(msg, titleFont);
                float x = (ClientSize.Width - size.Width) / 2;
                float y = (ClientSize.Height - size.Height) / 2 - 30;

                g.DrawString(msg, titleFont, shadowBrush, x + 2, y + 2);
                g.DrawString(msg, titleFont, textBrush, x, y);
            }
        }
    }
}
