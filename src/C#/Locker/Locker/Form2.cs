using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Locker
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            Cursor.Hide();
            InitializeComponent();
            BackColor = Color.Black;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;

            var synth = new SpeechSynthesizer();
            var builder = new PromptBuilder();
            builder.StartVoice("Microsoft Zira Desktop");
            builder.AppendText(System.Environment.MachineName + ", I am shutting down the system in 1, 2, 3.");
            builder.EndVoice();
            synth.SpeakAsync(new Prompt(builder));

            // terminating system
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C timeout 4 && shutdown -s -f -t 0";
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
