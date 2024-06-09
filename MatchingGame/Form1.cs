using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Eslestirme : Form
    {
        Random random = new Random();
        List<string> icons;

        Label firstClicked = null;
        Label secondClicked = null;

        bool gameStarted = false; // Oyun baþladýðýný kontrol eden deðiþken

        public Eslestirme()
        {
            InitializeComponent();
            InitializeIconsList();
            AssignIconsToSquares();
            DisableAllLabels(); // Baþlangýçta tüm týklamalarý devre dýþý býrak
        }

        private void InitializeIconsList()
        {
            icons = new List<string>()
            {
                "!", "!", "N", "N", ",", ",", "k", "k",
                "b", "b", "v", "v", "w", "w", "z", "z"
            };
        }

        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    if (icons.Count == 0)
                    {
                        // Ýkonlar tükendi, bu yüzden döngüyü kýr
                        break;
                    }
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void DisableAllLabels()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.Click -= i1_Click; // Click olayýný devre dýþý býrak
                }
            }
        }

        private void EnableAllLabels()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.Click += i1_Click; // Click olayýný etkinleþtir
                }
            }
        }

        private void i1_Click(object sender, EventArgs e)
        {
            if (!gameStarted)
                return; // Oyun baþlamadýysa týklamayý iþlemez

            if (timer2.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null && iconLabel.ForeColor == iconLabel.BackColor)
                    return;
            }

            MessageBox.Show("Tebrikler! Tüm eþleþtirmeleri buldunuz.", "Oyun Bitti");
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int sure;
            if (int.TryParse(txtSure.Text, out sure))
            {
                sure++;
                txtSure.Text = sure.ToString();
            }
            else
            {
                txtSure.Text = "1";
            }
        }

        private void btnBasla_Click(object sender, EventArgs e)
        {
            txtSure.Text = "0";
            timer1.Start();
            InitializeIconsList();
            AssignIconsToSquares();
            gameStarted = true; // Oyunun baþladýðýný iþaretle
            EnableAllLabels(); // Click olaylarýný etkinleþtir
        }
    }
}
