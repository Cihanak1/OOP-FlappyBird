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

        if (bird.Y <= 0 || bird.Y + bird.Size >= groundY)
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

    private void RestartGame()
    {
        score = 0;
        isGameOver = false;
        bird.Reset(80, 240);
        pipes.Clear();
        gameTimer.Start();
        Invalidate();
    }

    private void GameForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (isGameOver)
        {
            if (e.KeyCode == Keys.R)
            {
                RestartGame();
            }
            return;
        }

        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
        {
            bird.Jump();
        }
    }

    private void GameForm_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        g.Clear(Color.LightSkyBlue);

        foreach (Pipe pipe in pipes)
        {
            pipe.Draw(g);
        }

        int groundY = ClientSize.Height - GroundHeight;
        g.FillRectangle(Brushes.SandyBrown, 0, groundY, ClientSize.Width, GroundHeight);
        g.DrawLine(Pens.Black, 0, groundY, ClientSize.Width, groundY);

        bird.Draw(g);

        using (Font scoreFont = new Font("Arial", 14, FontStyle.Bold))
        {
            g.DrawString("Puan: " + score, scoreFont, Brushes.Black, 15, 15);
        }

        if (!isGameOver && pipes.Count == 0)
        {
            using (Font hintFont = new Font("Arial", 11, FontStyle.Regular))
            {
                string hint = "Zıplamak için Boşluk tuşuna basın";
                SizeF size = g.MeasureString(hint, hintFont);
                g.DrawString(hint, hintFont, Brushes.Black, (ClientSize.Width - size.Width) / 2, 50);
            }
        }

        if (isGameOver)
        {
            using (Font titleFont = new Font("Arial", 22, FontStyle.Bold))
            using (Font textFont = new Font("Arial", 12, FontStyle.Regular))
            {
                string title = "Oyun Bitti!";
                SizeF titleSize = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, Brushes.Red, (ClientSize.Width - titleSize.Width) / 2, 220);

                string scoreText = "Toplam Puan: " + score;
                SizeF scoreSize = g.MeasureString(scoreText, textFont);
                g.DrawString(scoreText, textFont, Brushes.Black, (ClientSize.Width - scoreSize.Width) / 2, 265);

                string restartText = "Yeniden başlamak için R tuşuna basın";
                SizeF restartSize = g.MeasureString(restartText, textFont);
                g.DrawString(restartText, textFont, Brushes.Black, (ClientSize.Width - restartSize.Width) / 2, 305);
            }
        }
    }
}
