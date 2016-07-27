using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SockClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        SocketClient sc = new SocketClient();
        private void butConnect_Click(object sender, EventArgs e)
        {
            if(sc.Connect(rtbShowMSG, txbIP.Text, txbPORT.Text))
            {
               // butConnect.Enabled = false;
            }
           
        }

        private void butSend_Click(object sender, EventArgs e)
        {
            sc.SendMsg(rtbShowMSG, rtbSendMse.Text);
            rtbSendMse.Clear();
        }

    }
}
