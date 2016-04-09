using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfraCommander
{
    public partial class MainForm : Form
    {
        String RxString1;
        String StringCMD;
        String StrCom;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

         [DllImport("user32.dll")]
         static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);


        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Resize_1(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
            }
         
        }

        private void InfraCommander_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM7";
            serialPort1.BaudRate = 9600;

            serialPort1.Open();
            try
            {
                if (serialPort1.IsOpen)
                {
                    buttonStart.Enabled = false;
                    buttonSto.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //Log.write("UnauthorizedAccessException");
            }
           
            
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString1 = serialPort1.ReadLine();
            if(!RxString1.Contains("FFFF"))
           StringCMD = RxString1;
            this.Invoke(new EventHandler(DisplayText));
            Commander(StringCMD);

        }

        private void DisplayText(object sender, EventArgs e)
        {
            //textBox1.Text = RxString;
            label2.Text = RxString1;
        }

        private void Commander(String com)
        {
            Console.WriteLine(com.Length);
            try
            {
                if (com.Length > 4)
                {
                    Console.WriteLine(com.Remove(6, 1));
                    StrCom = com.Remove(6, 1);

                    switch (StrCom)
                    {
                        case "BD906F":
                            Console.WriteLine("Sound");
                            Process.Start("I:\\Software\\PlaybackDevice\\PlaybackDevice.exe");
                            break;
                        case "BD30CF":
                            keybd_event((byte)Keys.VolumeUp, 0, 0, 0); // increase volume
                            break;
                        case "BD08F7":
                            keybd_event((byte)Keys.VolumeDown, 0, 0, 0); // increase volume
                            break;
                        case "BD20DF":
                            keybd_event((byte)Keys.VolumeMute, 0, 0, 0); // increase volume

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
        }

        private void buttonSto_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonStart.Enabled = true;
                buttonSto.Enabled = false;
            }
        }
    }
}
