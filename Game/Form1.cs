using Game.GameBoard.GameBoard;
using static Game.BoardActionController;

namespace Game
{
    public partial class Form1 : Form
    {
        private BoardActionController _controller;
        private const int _boardSize = 7;
        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
        Button AIButton = new Button();

        public Form1()
        {
            InitializeComponent();
            // Creates board
            _controller = new BoardActionController(_boardSize);
            TileWithButton[,] TilesToDraw = _controller.GameBoard.BoardTiles;
            DrawTiles(TilesToDraw, CreateFlowLayoutPanal(_boardSize));
            Form1_Resize(null, null);
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
            _controller.TileClicked((TileWithButton)sender);
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
            Difficulty difficulty = _controller.changeDifficulty();
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
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Width > Height)
            {
                flowLayoutPanel.Location = new Point((Width - Height) / 2, 0);
                flowLayoutPanel.Width = Height - flowLayoutPanel.Width / (_boardSize + 1);
                flowLayoutPanel.Height = Height;
            }
            else
            {
                flowLayoutPanel.Location = new Point(0, 0);
                flowLayoutPanel.Width = Width - flowLayoutPanel.Width / (_boardSize + 1);
                flowLayoutPanel.Height = Width;
            }

            foreach (var item in flowLayoutPanel.Controls)
            {
                if (item is TileWithButton)
                {
                    ((TileWithButton)item).Width = flowLayoutPanel.Width / (_boardSize + 1);
                    ((TileWithButton)item).Height = flowLayoutPanel.Height / (_boardSize + 1);
                }
            }

            AIButton.Width = flowLayoutPanel.Width / (_boardSize + 1);
            AIButton.Height = flowLayoutPanel.Height / (_boardSize + 1);
            AIButton.Location = new Point(Width - AIButton.Width - 30, 0);
        }
    }
}
