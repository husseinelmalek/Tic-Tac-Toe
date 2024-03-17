using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace firstScreen
{
    public partial class secondScreen : Form
    {
        private bool isXTurn = true;
        private Button[,] buttons;
        private string player1Name;
        private string player2Name;
        private bool playerX;
        private bool playerO;


        public secondScreen(string _player1, string _player2,bool _playerX,bool _playerO)
        {
            player1Name = _player1;
            player2Name = _player2;
            playerX = _playerX;
            playerO = _playerO;
            InitializeComponent();
            InitializeGame();
            label2.Text = $"{_player1} {(playerX ?"X": "O")}";
            label3.Text = $"{_player2} {(playerO ?"O": "X")}";
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;

        }

        private void InitializeGame()
        {
            buttons = new Button[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Size = new System.Drawing.Size(85, 85);
                    buttons[i, j].Location = new System.Drawing.Point((i + 1) * 90, (j + 1) * 90);
                    buttons[i, j].Font = new System.Drawing.Font("Arial", 20);
                    buttons[i, j].Tag = new Tuple<int, int>(i, j);
                    buttons[i, j].Click += Button_Click;
                    Controls.Add(buttons[i, j]);
                }
            }
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            Button button = (Button)sender;
            Tuple<int, int> position = (Tuple<int, int>)button.Tag;

            if (button.Text != "" || CheckForWinner())
                return;

            if (isXTurn)
                button.Text = label2.Text.Contains(" X") ? "X" : "O";
            else
                button.Text = label3.Text.Contains(" O") ? "O" : "X";

            isXTurn = !isXTurn;

            if (CheckForWinner())
            {
                string winner = button.Text =="O" ? "O" : "X";
                MessageBox.Show($"Player {(winner=="X" ? (label2.Text.EndsWith("X") ? label2.Text.Split(" ")[0] : label3.Text.Split(" ")[0]): 
                    (label2.Text.EndsWith("O") ? label2.Text.Split(" ")[0] : label3.Text.Split(" ")[0]))} wins!");
                if (winner == "O")
                {
                    if(label2.Text.EndsWith('O'))
                    {
                    textBox1.Text = (int.Parse(textBox1.Text) + 1).ToString();
                    }
                    if (label3.Text.EndsWith('O'))
                    {
                        textBox2.Text = (int.Parse(textBox2.Text) + 1).ToString();
                    }


                }
                else if (winner == "X")
                {

                    if (label2.Text.EndsWith('X'))
                    {
                        textBox1.Text = (int.Parse(textBox1.Text) + 1).ToString();
                    } 
                    if (label3.Text.EndsWith('X'))
                    {
                        textBox2.Text = (int.Parse(textBox2.Text) + 1).ToString();
                    }
                   
                }

            }
            else if (IsBoardFull())
            {
                MessageBox.Show("It's a draw!", "Game Over");
                ResetGame();
            }

        }

        private void ResetGame()
        {
            foreach (Button button in buttons)
            {
                button.Text = "";
            }

            isXTurn = true;
        }

        private bool IsBoardFull()
        {
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                    return false;
            }
            return true;
        }

        private bool CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Text != "" && buttons[i, 0].Text == buttons[i, 1].Text && buttons[i, 1].Text == buttons[i, 2].Text)
                    return true;
            }

            // Check columns
            for (int j = 0; j < 3; j++)
            {
                if (buttons[0, j].Text != "" && buttons[0, j].Text == buttons[1, j].Text && buttons[1, j].Text == buttons[2, j].Text)
                    return true;
            }

            // Check diagonals
            if (buttons[0, 0].Text != "" && buttons[0, 0].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 2].Text)
                return true;

            if (buttons[0, 2].Text != "" && buttons[0, 2].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 0].Text)
                return true;

            return false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ResetGame();
            textBox1.Text = "0";
            textBox2.Text = "0";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Server = DESKTOP-91QVAOD\\SQLEXPRESS ; Database = XOGame ; Integrated Security = SSPI ; TrustServerCertificate = True";

            SqlCommand command = new SqlCommand();
            command.CommandText = " insert into Score(playerX,playerXScore,PlayerO,playerOScore,time) values(@px,@sx,@po,@so,@t)";
            command.Parameters.AddWithValue("px", label2.Text.Contains(" X") ? player1Name : player2Name);
            command.Parameters.AddWithValue("sx", label2.Text.Contains(" X") ? int.Parse(textBox1.Text): int.Parse(textBox2.Text));
            command.Parameters.AddWithValue("po", label3.Text.Contains(" O") ? player2Name :player1Name);
            command.Parameters.AddWithValue("so", label3.Text.Contains(" O") ? int.Parse(textBox2.Text) : int.Parse(textBox1.Text));
            command.Parameters.AddWithValue("t", DateTime.Now);

            command.Connection = con;
            con.Open();
            int effectedRow = command.ExecuteNonQuery();
            
            con.Close();

            this.Hide();
            thirdScreen thirdScreen = new thirdScreen();
            thirdScreen.ShowDialog();
            this.Close();
            

        }
    }
}
