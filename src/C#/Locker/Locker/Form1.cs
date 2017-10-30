using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            passwordbox.Select();
            button1.MouseEnter += new EventHandler(button1_MouseEnter);
            button1.MouseLeave += new EventHandler(button1_MouseLeave);
        }

        private void e(object sender, MouseEventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.Black;
            button1.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.next2));
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.next));
        }

        private void button2_Click(object sender, EventArgs e) { Close(); }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.UseVisualStyleBackColor = false;
            button2.BackColor = Color.Transparent;
        }

        // Logic for locking/unlocking mechanism

        protected const string password = "12345";
        string path = @"./locked";
        int attempts = 1;

        private void passwordbox_TextChanged(object sender, EventArgs e) { }

        private void pass_check()
        {
            // shell commands
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";

            if (passwordbox.Text == password)
            {
                pictureBox1.Image = ((System.Drawing.Image)(Properties.Resources.cat3));
                label4.Width = 0;
                if (Directory.Exists(path))
                {
                    startInfo.Arguments = "/C rmdir locked && attrib +h +s  ";
                    process.StartInfo = startInfo;
                    process.Start();
                    Close();
                }
                else
                {
                    Directory.CreateDirectory(path);
                    startInfo.Arguments = "/C attrib +h +s locked && attrib -h -s   && explorer  ";
                    process.StartInfo = startInfo;
                    process.Start();

                    status.Text = "";
                }

                label2.BackColor = Color.MediumSpringGreen;
            }
            else if (string.IsNullOrEmpty(passwordbox.Text))
            {
                status.Text = "Please type your password.";
                label2.BackColor = Color.Red;
            }
            else
            {
                if (attempts != 5)
                {
                    if (attempts == 4)
                    {
                        status.ForeColor = Color.Purple;
                        label4.BackColor = Color.Purple;
                        label2.BackColor = Color.Purple;
                        status.Text = "Wrong password, the system will\nterminate next time.";
                    }
                    else
                    {
                        pictureBox1.Image = ((System.Drawing.Image)(Properties.Resources.error));
                        label2.BackColor = Color.Red;
                        status.Text = "Wrong password, try again.";
                    }
                    tm.Enabled = true;
                    attempts++;
                }
                else
                {
                    Form2 form2 = new Locker.Form2();
                    form2.Show(this);
                    status.Text = "Out of tries :(";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.UseVisualStyleBackColor = false;
            button1.BackColor = Color.Black;

            pass_check();  
        }

        private void passwordbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pass_check();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void status_Click(object sender, EventArgs e) { }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            button1.BackColor = Color.Black;
        }

        private void tm_Tick(object sender, EventArgs e)
        {
            if (label4.Width >= 235) this.tm.Enabled = false;
            else label4.Width += 47;
            ((System.Windows.Forms.Timer)sender).Stop();
        }
    }
}

