using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Management;
using System.Media;

namespace 아두이노_윈폼
{
    public partial class Form1 : Form
    {
        static SoundPlayer kick;
        static SoundPlayer hihat;
        static SoundPlayer percussion;
        static SoundPlayer snare;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PortCombo.DataSource = SerialPort.GetPortNames();
            kick = new SoundPlayer(@"C:\Users\user\Desktop\TS-VinylPack\TS-VinylPack\Pack 5\kick.wav");
            hihat = new SoundPlayer(@"C:\Users\user\Desktop\TS-VinylPack\TS-VinylPack\Pack 5\hihat.wav");
            percussion = new SoundPlayer(@"C:\Users\user\Desktop\TS-VinylPack\TS-VinylPack\Pack 5\percussion3.wav");
            snare = new SoundPlayer(@"C:\Users\user\Desktop\TS-VinylPack\TS-VinylPack\Pack 5\snare.wav");
            textbox.Select(textbox.Text.Length, 0);
            textbox.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen) 
            {
                serialPort1.PortName = PortCombo.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                serialPort1.Open();

                textbox.AppendText("포트가 열렸습니다.");
                textbox.Text += Environment.NewLine;
                PortCombo.Enabled = false;
            }
            else 
            {
                textbox.AppendText("포트가 이미 열려 있습니다.");
                textbox.Text += Environment.NewLine;
            }
            textbox.Select(textbox.Text.Length, 0);
            textbox.ScrollToCaret();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e) 
        {
            this.Invoke(new EventHandler(MySerialReceived));
        }

        private void MySerialReceived(object s, EventArgs e) 
        {
            string strReceive = serialPort1.ReadLine();

            int change_int = int.Parse(strReceive);

            int ck_piezonum = change_int / 1000;
            int ck_piezovalue = change_int % 1000;


            try
            {
                string msg = "페이조센서 " + ck_piezonum + " : " + ck_piezovalue;

                if (ck_piezonum == 1)
                {
                    textbox.AppendText(msg);
                    textbox.Text += Environment.NewLine;
                    kick.PlaySync();
                }
                else if (ck_piezonum == 2)
                {
                    textbox.AppendText(msg);
                    textbox.Text += Environment.NewLine;
                    hihat.PlaySync();
                }
                else if (ck_piezonum == 3)
                {
                    textbox.AppendText(msg);
                    textbox.Text += Environment.NewLine;
                    snare.PlaySync();
                }
                else if (ck_piezonum == 4)
                {
                    textbox.AppendText(msg);
                    textbox.Text += Environment.NewLine;
                    percussion.PlaySync();
                }

                textbox.Select(textbox.Text.Length, 0);
                textbox.ScrollToCaret();
            }
            catch (IndexOutOfRangeException ex)
            {
                textbox.AppendText(ex.Message);
            }
            finally 
            {
                strReceive = null;
                change_int = 0;
                ck_piezonum = 0;
                ck_piezovalue = 0;
            }
            
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();

                textbox.AppendText("포트가 닫혔습니다.");
                textbox.Text += Environment.NewLine;
                PortCombo.Enabled = true;
            }
            else 
            {
                textbox.AppendText("포트가 이미 닫혀 있습니다.");
                textbox.Text += Environment.NewLine;
            }
            textbox.Select(textbox.Text.Length, 0);
            textbox.ScrollToCaret();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1 != null && serialPort1.IsOpen)
                serialPort1.Close();
        }
    }
}
