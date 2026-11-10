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

        if (!isGameOver && pipes.Count == 0)
        {
            using (Font hintFont = new Font("Segoe UI", 10, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.White))
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
            {
                string hint = "Zıplamak için Boşluk tuşuna basın";
                SizeF size = g.MeasureString(hint, hintFont);
                float x = (ClientSize.Width - size.Width) / 2;
                float y = 55;

                g.DrawString(hint, hintFont, shadowBrush, x + 1, y + 1);
                g.DrawString(hint, hintFont, textBrush, x, y);
            }
        }

        if (isGameOver)
        {
            int boxWidth = 320;
            int boxHeight = 150;
            int boxX = (ClientSize.Width - boxWidth) / 2;
            int boxY = (ClientSize.Height - boxHeight) / 2 - 20;

            using (Brush boxBrush = new SolidBrush(Color.FromArgb(190, 0, 0, 0)))
            using (Pen borderPen = new Pen(Color.White, 2))
            {
                g.FillRectangle(boxBrush, boxX, boxY, boxWidth, boxHeight);
                g.DrawRectangle(borderPen, boxX, boxY, boxWidth, boxHeight);
            }

            using (Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold))
            using (Font infoFont = new Font("Segoe UI", 13, FontStyle.Bold))
            using (Font restartFont = new Font("Segoe UI", 10, FontStyle.Regular))
            using (Brush redBrush = new SolidBrush(Color.Salmon))
            using (Brush whiteBrush = new SolidBrush(Color.White))
            using (Brush yellowBrush = new SolidBrush(Color.Gold))
            {
                string title = "Oyun Bitti";
                SizeF titleSize = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, redBrush, (ClientSize.Width - titleSize.Width) / 2, boxY + 16);

                string scoreInfo = "Puan: " + score;
                SizeF scoreSize = g.MeasureString(scoreInfo, infoFont);
                g.DrawString(scoreInfo, infoFont, yellowBrush, (ClientSize.Width - scoreSize.Width) / 2, boxY + 60);

                string restartInfo = "Tekrar oynamak için R tuşuna bas";
                SizeF restartSize = g.MeasureString(restartInfo, restartFont);
                g.DrawString(restartInfo, restartFont, whiteBrush, (ClientSize.Width - restartSize.Width) / 2, boxY + 104);
            }
        }
    }
}
