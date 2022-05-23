using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace iniSync
{
    public partial class FormMain : Form
    {
        string lastFolder1 = null;
        string lastFolder2 = null;
        string file1 = null;
        string file2 = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            file1 = null;
            if (lastFolder1 != null)
            {
                if (Directory.Exists(lastFolder1))
                {
                    openFileDialog1.InitialDirectory = lastFolder1;
                }
            }
            else
            {
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                file1 = openFileDialog1.FileName;
                lastFolder1 = Path.GetDirectoryName(openFileDialog1.FileName);
                textBox1.Text = file1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            file2 = null;
            if (lastFolder2 != null)
            {
                if (Directory.Exists(lastFolder2))
                {
                    openFileDialog2.InitialDirectory = lastFolder2;
                }
            }
            else
            {
                openFileDialog2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                file2 = openFileDialog2.FileName;
                lastFolder2 = Path.GetDirectoryName(openFileDialog2.FileName);
                textBox2.Text = file2;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(file1) && File.Exists(file2))
            {
                string currentSection = null;
                List<string> cacheFile = new List<string>();
                cacheFile.AddRange(File.ReadAllLines(file1));
                foreach (string line in cacheFile)
                {
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        currentSection = line.Replace("[", "").Replace("]", "");
                    }
                    if (line.Contains("="))
                    {
                        if (FuncParser.keyExists(file2, currentSection, line.Remove(line.IndexOf('='))))
                        {
                            FuncParser.iniWrite(file2, currentSection, line.Remove(line.IndexOf('=')), line.Remove(0, line.IndexOf('=') + 1));
                        }
                    }
                }
                cacheFile = null;
            }
        }
    }
}
