using Game.GameBoard.GameBoard;
using System.Windows.Forms;
using static Game.PostActionHandler;

namespace Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PostActionHandler postActionHandler = new PostActionHandler();
        private int BoardSize = 7;
        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
        Button AIButton = new Button();

        private void Form1_Load(object sender, EventArgs e)
        {
            // Creates board
            TileWithButton[,] TilesToDraw = postActionHandler.LoadVisuals(BoardSize);
            BoardSize = TilesToDraw.GetLength(0);
            DrawTiles(TilesToDraw, CreateFlowLayoutPanal(BoardSize));
            ResizeForm();
            flowLayoutPanel.Show();
        }

        // Creates FlowLayoutPanel
        private FlowLayoutPanel CreateFlowLayoutPanal(int Size)
        {
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel.WrapContents = true;
            flowLayoutPanel.Padding = new Padding(10);

            return flowLayoutPanel;
        }

        // Creates tiles
        private void DrawTiles(TileWithButton[,] TilesToDraw, FlowLayoutPanel panel)
        {
            for (int i = 0; i < TilesToDraw.GetLength(0); i++)
            {
                for (int j = 0; j < TilesToDraw.GetLength(1); j++)
                {
                    TilesToDraw[i, j].Click += new EventHandler(DynamicButton_Click);
                    panel.Controls.Add(TilesToDraw[i, j]);
                }
            }
            Controls.Add(panel);
            DrawAITile();
        }

        // Button to do a move
        private void DynamicButton_Click(object sender, EventArgs e)
        {
            postActionHandler.TileClicked((TileWithButton)sender);
        }

        // Button to change AI difficulty
        private void DrawAITile()
        {
            AIButton.BackColor = Color.LightGreen;
            AIButton.Text = "AI: Easy";
            AIButton.Click += new EventHandler(AIButton_Click);
            Controls.Add(AIButton);
        }

        // Change AI difficulty
        private void AIButton_Click(object sender, EventArgs e)
        {
            Difficulty difficulty = postActionHandler.changeDifficulty();
            if (difficulty == Difficulty.Easy)
            {
                AIButton.BackColor = Color.LightGreen;
                AIButton.Text = "AI: Easy";
            }
            else if (difficulty == Difficulty.Medium)
            {
                AIButton.BackColor = Color.Orange;
                AIButton.Text = "AI: Medium";
            }
            else if (difficulty == Difficulty.Hard)
            {
                AIButton.BackColor = Color.Red;
                AIButton.Text = "AI: Hard";
            }
        }

        // Resize form
        DateTime lastUpdate = DateTime.Now;
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (DateTime.Now - lastUpdate < TimeSpan.FromMilliseconds(100)) return;
            lastUpdate = DateTime.Now;
            ResizeForm();
        }

        // Resize form
        private void ResizeForm()
        {
            if (Width > Height)
            {
                flowLayoutPanel.Location = new Point((Width - Height) / 2, 0);
                flowLayoutPanel.Width = Height - flowLayoutPanel.Width / (BoardSize + 1);
                flowLayoutPanel.Height = Height;
            }
            else
            {
                flowLayoutPanel.Location = new Point(0, 0);
                flowLayoutPanel.Width = Width - flowLayoutPanel.Width / (BoardSize + 1);
                flowLayoutPanel.Height = Width;
            }

            foreach (var item in flowLayoutPanel.Controls)
            {
                if (item is TileWithButton)
                {
                    ((TileWithButton)item).Width = flowLayoutPanel.Width / (BoardSize + 1);
                    ((TileWithButton)item).Height = flowLayoutPanel.Height / (BoardSize + 1);
                }
            }

            AIButton.Width = flowLayoutPanel.Width / (BoardSize + 1);
            AIButton.Height = flowLayoutPanel.Height / (BoardSize + 1);
            AIButton.Location = new Point(Width - AIButton.Width - 30, 0);
        }
    }
}
