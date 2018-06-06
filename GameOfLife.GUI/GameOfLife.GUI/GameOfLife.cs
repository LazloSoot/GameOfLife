using GameOfLife.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameOfLife.GUI
{
    public partial class GameOfLife : Form
    {
        private Life game;
        private int cellWidth,
                    cellHeight,
                    Width,
                    Height;

        private Label[,] cells;
        private Color aliveColor,bornColor, diedColor, emptyColor;
        private bool isFirstPartOfStep;
        private bool isGameStarted;

        public GameOfLife()
        {
            InitializeComponent();
            InitGame();
        }
        

        private void InitGame()
        {
            cellWidth = cellHeight = 20;
            Width = panel1.Width / cellWidth;
            Height = panel1.Height / cellHeight;

            game = new Life(Height, Width);
            cells = new Label[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    AddCell(x, y);
                }
            }

            aliveColor = Color.DarkBlue;
            bornColor = Color.Yellow;
            diedColor = Color.DarkRed;
            emptyColor = Color.White;
            isFirstPartOfStep = true;
        }

        private void AddCell(int x, int y)
        {
            var cell = new Label
            {
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Location = new System.Drawing.Point(x * cellWidth, y * cellHeight),
                Size = new System.Drawing.Size(cellWidth + 1, cellHeight + 1),
                Text = "",
                BackColor = emptyColor,
                Parent = panel1
            };
            cell.MouseClick += Cell_MouseClick;

            cells[x, y] = cell;
        }

        private void Cell_MouseClick(object sender, MouseEventArgs e)
        {
            int x = ((Label)sender).Location.X / cellWidth;
            int y = ((Label)sender).Location.Y / cellHeight;
            int color = game.Turn(x, y);
            cells[x, y].BackColor = color == 1 ? aliveColor : emptyColor;
        }

        private void StepForwardBtn_Click(object sender, EventArgs e)
        {
            if(isFirstPartOfStep)
            {
                game.PrepeareGeneration();
            }
            else
            {
                game.CommitGeneration();
            }
            Refresh();
            isFirstPartOfStep = !isFirstPartOfStep;
        }

        private void StartStopBtn_Click(object sender, EventArgs e)
        {
            isGameStarted = !isGameStarted;

            if (isGameStarted)
            {
                Timer.Enabled = true;
                StartStopBtn.Text = "Pause";
            }
            else
            {
                Timer.Enabled = false;
                StartStopBtn.Text = "Start game";
                isFirstPartOfStep = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            game.PrepeareGeneration();
            game.CommitGeneration();
            Refresh();
        }

        private new void Refresh()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    switch (game.GetMap(x, y))
                    {
                        case 0:
                            cells[x, y].BackColor = emptyColor;
                            break;
                        case 1:
                            cells[x, y].BackColor = aliveColor;
                            break;
                        case 2:
                            cells[x, y].BackColor = diedColor;
                            break;
                        case -1:
                            cells[x, y].BackColor = bornColor;
                            break;
                    }
                }
            }
        }
    }
}
