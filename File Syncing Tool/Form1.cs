using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace File_Syncing_Tool
{
    public partial class Form1 : Form
    {
        string ConfigFileName = @"Config.xml"; // Default Configuration File Name
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WriteConsole("Software Loaded...");
            if (!File.Exists(ConfigFileName))
            {
                // Create the XML Configuration File and the required attribute from Scratch and also shows a messagebox which assume the user is new to this software as the configuration file isn't existing
                XmlDocument config = new XmlDocument();
                XmlElement TheHeadXML = config.CreateElement("FileSyncingTool");
                config.AppendChild(TheHeadXML);
                XmlAttribute CreditData = config.CreateAttribute("Credits");
                CreditData.Value = "A Software developed by zhiyan114";
                TheHeadXML.Attributes.Append(config.CreateAttribute("Credit"));
                TheHeadXML.Attributes.Append(CreditData);
                TheHeadXML.Attributes.Append(config.CreateAttribute("OpenLocation"));
                TheHeadXML.Attributes.Append(config.CreateAttribute("SaveLocation"));
                TheHeadXML.Attributes.Append(config.CreateAttribute("TimePerSave"));
                config.Save(ConfigFileName);
                MessageBox.Show("Thank You for using File Syncing Tool for the first time. Software developed by zhiyan114. This software will allows automatically backup all the files from a folder to another location. This software is also open source so you can check it out. If you have any issues, you may report the issues.","First Time",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        private void WriteConsole(string message)
        {
            textBox4.Text = textBox4.Text + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt") + ": " + message + "\r\n"; // Proper way to write data to a TextBox alike a console method
        }
        private void ClearConsole()
        {
            textBox4.Text = ""; // Clean up the Textbox text by giving a empty value instead of null
        }

        private void button5_Click(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Properties.Resources.Tray;
            notifyIcon1.ShowBalloonTip(5000,"File Syncing Tool","The software is now minimized on the system tray. Click on it to show the UI again.",ToolTipIcon.Info);
            Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to delete the configuration file? Once it deleted, it cannot be recovered and the software will automatically shutdown.","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if(File.Exists(ConfigFileName))
                {
                    File.Delete(ConfigFileName);
                    MessageBox.Show("The configuration File is now deleted. Press OK and the software will automatically shutdown.","Config Deleted",MessageBoxButtons.OK,MessageBoxIcon.Information);
                } else
                {
                    MessageBox.Show("The Configuration File does not exist, please ensure that it not deleted while the software is running. If it is please restart the software immediately. Press OK and the software will shutdown.","Config File Not Existed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                Close(); // You need the configuration file in order for the software to fully function. If it not existed, software require shutdown to prevent unhandled/undiscovered exception
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Check if the logs is empty so that user doesn't export nothing (which creates a useless file)
            if(string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("You cannot export the console logs since it already empty", "Empty Logs", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            using (SaveFileDialog SaveLogLocation = new SaveFileDialog())
            {
                if(SaveLogLocation.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(SaveLogLocation.FileName, textBox4.Text);
                        MessageBox.Show("The Console logs has been successfully saved under this path:\n"+SaveLogLocation.ToString(),"Logs Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    } catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Error While Saving Logs",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                } else
                {
                    MessageBox.Show("There was an error while selecting a save location OR You probably canceled it.","Unable to Save Log",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Check if the logs is empty so that user doesn't clear nothing
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("You cannot clear the console since it already empty","Empty Logs",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            DialogResult ClearConsoleResult = MessageBox.Show("Would you like to display a log that shows you cleared the console? \n\n Yes - Clear with Log \n\n No - Clear without Log \n\n Cancel - Don't clear the Log", "Clearing Logs", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (ClearConsoleResult == DialogResult.Yes)
            {
                ClearConsole();
                WriteConsole("Console Logs was Cleared");
                MessageBox.Show("All the logs on the console has now been cleared with a log.","Console Cleared",MessageBoxButtons.OK,MessageBoxIcon.Information);
            } else if(ClearConsoleResult == DialogResult.No)
            {
                ClearConsole();
                MessageBox.Show("All the logs on the console has now been cleared without a log.", "Console Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/zhiyan114/File-Syncing-Tool/"); // Take the user to the repository
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // This will get each XML Node and Save the necessary data into it
            XmlDocument Config = new XmlDocument();
            Config.Load(ConfigFileName);
            foreach(XmlNode MultiObj in Config.SelectNodes("FileSyncingTool"))
            {
                // Define each XML Nodes
                XmlNode OpenFileLocation = MultiObj.SelectSingleNode("OpenLocation");
                XmlNode SaveFileLocation = MultiObj.SelectSingleNode("SaveLocation");
                XmlNode TimePerSave = MultiObj.SelectSingleNode("TimePerSave");
                // Saves the data into the XML and notify the user that it has been saved
                OpenFileLocation.Value = textBox1.Text;
                SaveFileLocation.Value = textBox2.Text;
                TimePerSave.Value = textBox3.Text;
                Config.Save(ConfigFileName);
                MessageBox.Show("All the configuration has been saved","Config Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Does almost same thing as saving the data
            XmlDocument Config = new XmlDocument();
            Config.Load(ConfigFileName);
            foreach (XmlNode MultiObj in Config.SelectNodes("FileSyncingTool"))
            {
                // Define each XML Nodes
                XmlNode OpenFileLocation = MultiObj.SelectSingleNode("OpenLocation");
                XmlNode SaveFileLocation = MultiObj.SelectSingleNode("SaveLocation");
                XmlNode TimePerSave = MultiObj.SelectSingleNode("TimePerSave");
                // Perform a check to see if the XML Node does not exist
                if(OpenFileLocation != null && SaveFileLocation != null && TimePerSave != null)
                {
                    // Load the configuration file and tell the user that it was loaded
                    textBox1.Text = OpenFileLocation.Value;
                    textBox2.Text = SaveFileLocation.Value;
                    textBox3.Text = TimePerSave.Value;
                    MessageBox.Show("All the configurations has been successfully loaded.", "Config Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    // Tell the user that the required XML Node is null
                    MessageBox.Show("Sorry but the configuration file are missing datas, please try again by deleting the configuration file and restart the software.","Missing Configuration",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }

        }
    }
}
