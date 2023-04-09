﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BattleShipGui
{
    public partial class Game : Form
    {
        private int[] shipSizes = new int[] { 4, 3, 3, 2, 2, 2 };
        private int currentShipIndex = 0;
        private int currentShipSize = 0;
        private bool[,] field = new bool[10, 10];
        private bool isVertical = true;
        private Label shipsLabel;


        public Game()
        {
            InitializeComponent();
            CreateButtonField(10, 10);
            CreateControls();
            this.KeyDown += Game_KeyDown;
            this.KeyPreview = true;
        }

        private void CreateButtonField(int rows, int columns)
        {
            string Alphabet = "ABCDRFGHIJ";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = new Button();
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.LightGray;
                    button.Width = 40;
                    button.Height = 40;
                    button.Left = j * button.Width;
                    button.Top = i * button.Height;
                    if (i == 0 || j == 0)
                    {
                        if (i == 0 && j > 0)
                        {
                            button.Enabled = false;
                            button.BackColor = Color.LightGray;
                            button.Text = Alphabet[j - 1].ToString();
                        }
                        if (j == 0 && i > 0)
                        {
                            button.Enabled = false;
                            button.BackColor = Color.LightGray;
                            button.Text = i.ToString();
                        }
                        if (j == 0 && i == 0)
                        {
                            button.Enabled = false;
                            button.BackColor = Color.LightGray;
                        }
                    }
                    button.Click += Button_Click;
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;
                    this.Controls.Add(button);
                }
            }
        }
        private void CreateControls()
        {
            shipsLabel = new Label();
            shipsLabel.Text = GetShipsText();
            shipsLabel.Left = field.GetLength(1) * 50 + 10;
            shipsLabel.Top = 0;
            shipsLabel.AutoSize = true;
            this.Controls.Add(shipsLabel);

            Button rotateButton = new Button();
            rotateButton.Text = "Повернуть";
            rotateButton.Width = 100;
            rotateButton.Height = 50;
            rotateButton.Left = field.GetLength(1) * 50 + 10;
            rotateButton.Top = shipsLabel.Bottom + 10;
            rotateButton.Click += RotateButton_Click;
            this.Controls.Add(rotateButton);

            Button startButton = new Button();
            startButton.Text = "Начать";
            startButton.Width = 100;
            startButton.Height = 50;
            startButton.Left = field.GetLength(1) * 50 + 10;
            startButton.Top = rotateButton.Bottom + 10;
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);
        }
        private string GetShipsText()
        {
            string text = "Корабли:\n";
            for (int i = 0; i < shipSizes.Length; i++)
            {
                text += $"{i + 1}: {shipSizes[i]}";
                if (i == currentShipIndex)
                {
                    text += " (текущий)";
                }
                text += "\n";
            }
            return text;
        }
        private void RotateButton_Click(object sender, EventArgs e)
        {
            isVertical = !isVertical;
            UnhighlightAll();
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (currentShipIndex == shipSizes.Length)
            {
                MessageBox.Show("Игра началась!");
            }
            else
            {
                MessageBox.Show("Сначала расставьте все корабли!");
            }
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                isVertical = !isVertical;
                UnhighlightAll();
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (currentShipIndex < shipSizes.Length)
            {
                if (currentShipSize == 0)
                {
                    currentShipSize = shipSizes[currentShipIndex];
                }

                Button button = (Button)sender;
                int row = button.Top / button.Height;
                int column = button.Left / button.Width;

                if (CanPlaceShip(row, column))
                {
                    PlaceShip(row, column);
                    currentShipIndex++;
                    currentShipSize = 0;
                    shipsLabel.Text = GetShipsText();

                }
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (currentShipIndex < shipSizes.Length)
            {
                if (currentShipSize == 0)
                {
                    currentShipSize = shipSizes[currentShipIndex];
                }

                Button button = (Button)sender;
                int row = button.Top / button.Height;
                int column = button.Left / button.Width;

                if (CanPlaceShip(row, column))
                {
                    HighlightShip(row, column);
                }
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            UnhighlightAll();
        }

        private bool CanPlaceShip(int row, int column)
        {
            if (isVertical)
            {
                if (row + currentShipSize > 10)
                {
                    return false;
                }

                for (int i = row; i < row + currentShipSize; i++)
                {
                    if (field[i, column])
                    {
                        return false;
                    }

                    if (column > 0 && field[i, column - 1])
                    {
                        return false;
                    }

                    if (column < 10 - 1 && field[i, column + 1])
                    {
                        return false;
                    }
                }

                if (row > 0)
                {
                    for (int j = Math.Max(column - 1, 0); j <= Math.Min(column + currentShipSize - 1, 10 - 1); j++)
                    {
                        if (field[row - 1, j])
                        {
                            return false;
                        }
                    }
                }

                if (row + currentShipSize < 10)
                {
                    for (int j = Math.Max(column - 1, 0); j <= Math.Min(column + currentShipSize - 1, 10 - 1); j++)
                    {
                        if (field[row + currentShipSize, j])
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (column + currentShipSize > 10)
                {
                    return false;
                }

                for (int i = column; i < column + currentShipSize; i++)
                {
                    if (field[row, i])
                    {
                        return false;
                    }

                    if (row > 0 && field[row - 1, i])
                    {
                        return false;
                    }

                    if (row < 10 - 1 && field[row + 1, i])
                    {
                        return false;
                    }
                }

                if (column > 0)
                {
                    for (int j = Math.Max(row - 1, 0); j <= Math.Min(row + currentShipSize - 1, 10 - 1); j++)
                    {
                        if (field[j, column - 1])
                        {
                            return false;
                        }
                    }
                }

                if (column + currentShipSize < 10)
                {
                    for (int j = Math.Max(row - 1, 0); j <= Math.Min(row + currentShipSize - 1, 10 - 1); j++)
                    {
                        if (field[j, column + currentShipSize])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void PlaceShip(int row, int column)
        {
            if (isVertical)
            {
                for (int i = row; i < row + currentShipSize; i++)
                {
                    field[i, column] = true;
                    int index = i * 10 + column;
                    this.Controls[index].BackColor = Color.Blue;
                }
            }
            else
            {
                for (int i = column; i < column + currentShipSize; i++)
                {
                    field[row, i] = true;
                    int index = row * 10 + i;
                    this.Controls[index].BackColor = Color.Blue;
                }
            }
        }

        private void HighlightShip(int row, int column)
        {
            if (isVertical)
            {
                for (int i = row; i < row + currentShipSize; i++)
                {
                    int index = i * 10 + column;
                    this.Controls[index].BackColor = Color.LightBlue;
                }
            }
            else
            {
                for (int i = column; i < column + currentShipSize; i++)
                {
                    int index = row * 10 + i;
                    this.Controls[index].BackColor = Color.LightBlue;
                }
            }
        }

        private void UnhighlightAll()
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (!field[i, j])
                    {
                        int index = i * 10 + j;
                        this.Controls[index].BackColor = SystemColors.Control;
                    }
                }
            }
        }
    }
}
