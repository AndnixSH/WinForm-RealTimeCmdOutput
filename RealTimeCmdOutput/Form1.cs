using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealTimeCmdOutput
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void runBtn_Click(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                startProgram("ping.exe", "8.8.8.8");
                TxtBoxOutput("Done :)");
            });
        }

        private void startProgram(string filename, string commandLine)
        {
            var fileName = filename;
            var arguments = commandLine;

            var info = new ProcessStartInfo();
            info.FileName = fileName;
            info.Arguments = arguments;

            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;

            using (var p = new Process())
            {
                p.StartInfo = info;
                p.EnableRaisingEvents = true;

                p.OutputDataReceived += (s, o) =>
                {
                    TxtBoxOutput(o.Data);
                };
                p.ErrorDataReceived += (s, o) =>
                {
                    TxtBoxOutput(o.Data);
                };
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
            }
        }

        public void TxtBoxOutput(string text)
        {
            BeginInvoke(new Action(delegate ()
            {
                richTextBoxOutput.AppendText(text + Environment.NewLine);
                richTextBoxOutput.ScrollToCaret();
            }));
        }
    }
}
