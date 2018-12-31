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
        int TimeBeforeSync = 0;
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
            // Let user select a location to sync the file from
            using (FolderBrowserDialog FromFolder = new FolderBrowserDialog())
            {
                // Check if the user selected a folder, if it not OK assume the user canceled or there is probably an error
                if (FromFolder.ShowDialog() == DialogResult.OK)
                {
                    // Even tho I can just directly define it to the textbox, this method just used in-case there is a future update to the software
                    string FromLocation = FromFolder.SelectedPath;
                    textBox1.Text = FromLocation;
                    WriteConsole("From Location Selected...");
                } else
                {
                    MessageBox.Show("Sorry but there might be an error while you selecting a folder or it probably just you canceled to select one","Unable to Select A Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if it empty cause I know people going to try it
            if(string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please make sure that you have all the informations loaded then try again","Missing Information",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            // Yeh people are really dumb to have this existed which checks if the folder exists since people are going to try it with random values...
            if(!Directory.Exists(textBox1.Text) || !Directory.Exists(textBox2.Text))
            {
                MessageBox.Show("The folder you selected was missing or invalid, please select again","Invalid Folder",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            // This is another check to see if the Time Value is number because people will ignore the warning
            if(!int.TryParse(textBox3.Text,out int DisposedValue))
            {
                MessageBox.Show("The Time To Backup was given an invalid int value. Please try again by putting the correct value (Seriously, you ignore the existing warning...)","Invalid Number",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            button4.Enabled = true;
            button3.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            TimeBeforeSync = int.Parse(textBox3.Text);
            label7.ForeColor = Color.Blue;
            label7.Text = TimeBeforeSync.ToString();
            timer1.Start();
            WriteConsole("Syncing Timer Started...");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // This just stops the timer so there isn't anything to check as it doesn't really involved user inputing values
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            button4.Enabled = false;
            button3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            TimeBeforeSync = 0;
            label7.ForeColor = Color.Red;
            label7.Text = "Syncing Stopped";
            timer1.Stop();
            WriteConsole("Syncing Timer Stopped...");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Let user select a location to sync the file from
            using (FolderBrowserDialog ToFolder = new FolderBrowserDialog())
            {
                // Check if the user selected a folder, if it not OK assume the user canceled or there is probably an error
                if (ToFolder.ShowDialog() == DialogResult.OK)
                {
                    // Even tho I can just directly define it to the textbox, this method just used in-case there is a future update to the software
                    string ToLocation = ToFolder.SelectedPath;
                    textBox2.Text = ToLocation;
                    WriteConsole("To Location Selected...");
                }
                else
                {
                    MessageBox.Show("Sorry but there might be an error while you selecting a folder or it probably just you canceled to select one", "Unable to Select A Folder",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to delete the configuration file? Once it deleted, it cannot be recovered and the software will automatically shutdown.","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if(File.Exists(ConfigFileName))
                {
                    File.Delete(ConfigFileName);
                    WriteConsole("Config has been deleted...");
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
                WriteConsole("Config has been saved...");
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
                    WriteConsole("Config has been loaded...");
                    MessageBox.Show("All the configurations has been successfully loaded.", "Config Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    // Tell the user that the required XML Node is null
                    MessageBox.Show("Sorry but the configuration file are missing datas, please try again by deleting the configuration file and restart the software.","Missing Configuration",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Too lazy to implement this into the button to show message, this checks if the text is int (Not Float)
            // Checks if the textbox is empty, don't want this not a number message to popup when the user have not even typed anything
            if(!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                // Now check if the text is a valid int (not float nor double)
                if (!int.TryParse(textBox3.Text, out int DisposedValue))
                {
                    MessageBox.Show("Make sure the Time To Backup you selected contain numbers only (int)", "Invalid Value",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Here is where all the syncing does the work and the check ofc
            if(TimeBeforeSync == 0)
            {
                // File syncing here then do other useful stuff such as resetting the timer and create a log
                DeleteFile(textBox2.Text); // Clean up the directory location to ensure that none of the old file still there
                CopyFile(textBox1.Text,textBox2.Text); // Copy the newer file into it
                TimeBeforeSync = int.Parse(textBox3.Text);
                label7.Text = textBox3.Text;
                WriteConsole("File Successfully synced...");
            } else
            {
                TimeBeforeSync--;
                label7.Text = TimeBeforeSync.ToString();
            }
        }
        // Copies File From A Directory. Ref: https://stackoverflow.com/questions/7146021/copy-all-files-in-directory
        void CopyFile(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

            foreach (var directory in Directory.GetDirectories(sourceDir))
                CopyFile(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
        // Deletes all file from a Directory (Re-Written). Ref: https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory
        void DeleteFile(string Folder)
        {
            DirectoryInfo DirDelete = new DirectoryInfo(Folder); // Ugh can't be disposed
            foreach(FileInfo File in DirDelete.EnumerateFiles())
            {
                File.Delete();
            }
            foreach(DirectoryInfo Directory in DirDelete.EnumerateDirectories())
            {
                Directory.Delete(true);
            }
        }
    }
}
