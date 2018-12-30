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

namespace File_Syncing_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/zhiyan114/File-Syncing-Tool/"); // Take the user to the repository
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Not sure what this can do anything yet xD
        }
        private void WriteConsole(string message)
        {
            textBox4.Text = textBox4.Text + message + "\r\n"; // Proper way to write data to a TextBox alike a console method
        }
        private void ClearConsole()
        {
            textBox4.Text = ""; // Clean up the Textbox text by giving a empty value instead of null
        }
    }
}
