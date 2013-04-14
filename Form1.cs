using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UdpClient_Chat_2
{
    public partial class Form1 : Form
    {
        private Thread listenThread;
        private bool listenFlag = true;
        private IPEndPoint remoteIPEndPoint = null;
        IPAddress remoteIP;
        Int32 remotePort;
        Int32 localPort;
        private UdpClient udpClient;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //設置對方IP以及Port
            remoteIP = IPAddress.Parse(textBox3.Text);
            remotePort = Int32.Parse(textBox4.Text);
            listBox1.Items.Add("對方的傳送資訊已設置");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //傳送資料，呼叫send()
            send();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //設置我方的監聽Port
            localPort = Int32.Parse(textBox1.Text);
            udpClient = new UdpClient(localPort);
            listenThread = new Thread(new ThreadStart(listen));
            listenThread.Start();
            listBox1.Items.Add("本地端監聽Port已設置");
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //關閉執行緒以及關閉連線
            try
            {
                listenThread.Abort();
                udpClient.Close();
            }
            catch
            {
            }
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //程式開啟不做任何動作
        }

        private void listen()
        {
            //設定編碼類型
            Encoding encoding = Encoding.Unicode;
            while (listenFlag == true)
            {
                Byte[] data = udpClient.Receive(ref remoteIPEndPoint);
                //取得對方訊息
                String strData = encoding.GetString(data);
                //取得遠端電腦的IP以及Port
                string get_remoteIP = remoteIPEndPoint.Address.ToString();
                string get_remotePort = remoteIPEndPoint.Port.ToString();

                //顯示訊息
                listBox1.Items.Add(get_remoteIP + ":" + get_remotePort + "說：" + strData);
            }
        }

        private void send()
        {
            //
            //定義Udp
            //
            UdpClient udpClient_Send = new UdpClient();
            //
            //判斷IP是否有問題
            //
            try
            {
                //設置
                IPEndPoint send_remoteIP = new IPEndPoint(remoteIP, remotePort);
                Byte[] data_buffer = null;
                Encoding encoding = Encoding.Unicode;
                //讀取輸入的字串
                string str = textBox2.Text;

                data_buffer = encoding.GetBytes(str.ToCharArray());
                //傳送訊息到遠端電腦
                udpClient_Send.Send(data_buffer, data_buffer.Length, send_remoteIP);
                textBox2.Clear();
                listBox1.Items.Add("我說" + "： " + str);
                //關閉連線
                udpClient_Send.Close();
            }
            catch
            {
                listBox1.Items.Add("Please check you IP address!");
                return;
            }
        }
    }
}
