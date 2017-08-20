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

        private void button1_Click(object sender, EventArgs e)
        {
            button1.UseVisualStyleBackColor = false;
            button1.BackColor = Color.Black;

            // shell commands
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";

            if (passwordbox.Text == password)
            {
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
            }
            else if (string.IsNullOrEmpty(passwordbox.Text))
                status.Text = "Please type your password.";
            else
            {
                if (attempts != 5)
                {
                    if (attempts == 4)
                        status.Text = "[" + (attempts++) + "/5] Wrong password, the system will\nterminate next time.";
                    else
                        status.Text = "[" + (attempts++) + "/5] Wrong password, try again.";
                }
                else
                {
                    Form2 form2 = new Locker.Form2();
                    form2.Show(this);
                    status.Text = "Out of tries :(";
                }
            }         
        }

        private void status_Click(object sender, EventArgs e) { }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            button1.BackColor = Color.Black;
        }
    }
}

