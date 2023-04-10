﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShipGui
{
    public partial class Game : Form
    {
        int[] shipSizes = new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        public Game(bool[,] field)
        {
            InitializeComponent();
            CreateGameField(field);
            CreateAndFillSecondGameField();
            // Используйте массив field для создания поля с уже расставленными кораблями
        }
        private void CreateGameField(bool[,] field)
        {
            // Создайте поле кнопок так же, как в методе CreateButtonField
            // ...
            string Alphabet = "ABCDRFGHIJ";
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
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
                    this.Controls.Add(button);
                }
            }
            // Затем используйте массив field для определения расположения кораблей:
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j])
                    {
                        // Клетка [i, j] содержит корабль
                        // Измените свойства соответствующей кнопки, чтобы отобразить корабль
                        // Например:
                        int index = i * 10 + j;
                        this.Controls[index].BackColor = Color.Blue;
                        this.Controls[index].Click += BotHit_Click;
                    }
                    else
                    {
                        int index = i * 10 + j;
                        this.Controls[index].Click += BotMiss_Click;
                    }
                }
            }
        }

        private void CreateAndFillSecondGameField()
        {
            bool[,] field = GenerateRandomField();
            CreateSecondGameField(field);
        }

        private void CreateSecondGameField(bool[,] field)
        {
            int fieldWidth = 10 * 40; // Ширина первого поля в пикселях
            int spacing = 20; // Расстояние между полями в пикселях
            string Alphabet = "ABCDRFGHIJ";
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Button button = new Button();
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.LightGray;
                    button.Width = 40;
                    button.Height = 40;
                    button.Left = fieldWidth + spacing + j * button.Width;
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
                    this.Controls.Add(button);
                }
            }
            // Затем используйте массив field для определения расположения кораблей:
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j])
                    {
                        // Клетка [i, j] содержит корабль
                        // Измените свойства соответствующей кнопки, чтобы отобразить корабль
                        int index = (10 + i) * 10 + j;
                        this.Controls[index].BackColor = Color.Yellow; //DEBUG DELETE THIS 
                        this.Controls[index].Click += ButtonHit_Click;
                    }
                    else
                    {
                        int index = (10 + i) * 10 + j;
                        this.Controls[index].Click += ButtonMiss_Click;
                    }
                }
            }
        }
        private void ButtonHit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text = "×";
            button.Font = new Font("Microsoft Sans Serif", 24);
            button.Enabled = false;
            BotShoot();
        }
        private void ButtonMiss_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text = "⚫";
            button.Font = new Font("Microsoft Sans Serif", 19);
            button.Enabled = false;
            BotShoot();
        }
        private void BotShoot()
        {
                Random rnd = new Random();
                int x = rnd.Next(1, 10);
                int y = rnd.Next(1, 10);
                int index = y * 10 + x;
                Button button = this.Controls[index] as Button;
                button.PerformClick();
        }
        private void BotHit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text = "×";
            button.Font = new Font("Microsoft Sans Serif", 24);
            button.Enabled = false;

        }
        private void BotMiss_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text = "⚫";
            button.Font = new Font("Microsoft Sans Serif", 19);
            button.Enabled = false;
        }
        private bool[,] GenerateRandomField()
        {
            bool[,] field = new bool[10, 10];
            Random random = new Random();
            foreach (int shipSize in shipSizes)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = random.Next(1, 10);
                    int y = random.Next(1, 10);
                    bool horizontal = random.Next(0, 2) == 0;
                    if (CanPlaceShip(field, x, y, shipSize, horizontal))
                    {
                        PlaceShip(field, x, y, shipSize, horizontal);
                        placed = true;
                    }
                }
            }
            return field;
        }

        private bool CanPlaceShip(bool[,] field, int x, int y, int shipSize, bool horizontal)
        {
            if (horizontal)
            {
                if (x + shipSize > 10) return false;
                for (int i = x; i < x + shipSize; i++)
                {
                    if (IsOccupiedOrAdjacent(field, i, y)) return false;
                }
            }
            else
            {
                if (y + shipSize > 10) return false;
                for (int i = y; i < y + shipSize; i++)
                {
                    if (IsOccupiedOrAdjacent(field, x, i)) return false;
                }
            }
            return true;
        }

        private bool IsOccupiedOrAdjacent(bool[,] field, int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < 10 && j >= 0 && j < 10 && field[i, j]) return true;
                }
            }
            return false;
        }

        private void PlaceShip(bool[,] field, int x, int y, int shipSize, bool horizontal)
        {
            if (horizontal)
            {
                for (int i = x; i < x + shipSize; i++)
                {
                    field[i, y] = true;
                }
            }
            else
            {
                for (int i = y; i < y + shipSize; i++)
                {
                    field[x, i] = true;
                }
            }
        }
    }
}