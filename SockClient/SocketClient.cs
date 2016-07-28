
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SockClient
{
    class SocketClient
    {
        Thread threadClient = null; // 创建用于接收服务端消息的 线程；  
        Socket sockClient = null;
        MyDelegate md = new MyDelegate();
        public bool Connect(object sender,string sip,string sport)
        {
            IPAddress ip = IPAddress.Parse(sip);
            IPEndPoint endPoint = new IPEndPoint(ip, int.Parse(sport));
            sockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                md.DoShowMSGFunc(sender, "与服务器[" + sip + "]连接中……");
                sockClient.Connect(endPoint);

            }
            catch (SocketException se)
            {
                md.DoShowMSGFunc(sender, "异常：" + se.Message);
                return false;
                //this.Close();  
            }            
            md.DoShowMSGFunc(sender, "与服务器连接成功！！！");
            threadClient = new Thread(RecMsg);
            threadClient.IsBackground = true;
            threadClient.Start( sender);
            return true;
        }
        void RecMsg(object sender)
        {
            while (true)
            {
                // 定义一个2M的缓存区；  
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                sockClient.Receive(arrMsgRec);
               
                MyMessage mmsg = new MyMessage ();
                mmsg= JsonHelper.DeserializeJsonToObject<MyMessage>(Encoding.UTF8.GetString(arrMsgRec));
             
                // 将接受到的数据存入到输入  arrMsgRec中；  
                if(mmsg==null)
                {
                    md.DoShowMSGFunc(sender, "异常：没有获取到数据");
                    return;
                }

                if (mmsg.id == "0") // 表示接收到的是消息数据；  
                {
                    md.DoShowMSGFunc(sender, "服务器：" + mmsg.msg);
                }
                //if (arrMsgRec[0] == 1) // 表示接收到的是文件数据；  
                //{

                //    try
                //    {
                //        SaveFileDialog sfd = new SaveFileDialog();

                //        if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                //        {// 在上边的 sfd.ShowDialog（） 的括号里边一定要加上 this 否则就不会弹出 另存为 的对话框，而弹出的是本类的其他窗口，，
                //           这个一定要注意！！！【解释：加了this的sfd.ShowDialog(this)，“另存为”窗口的指针才能被SaveFileDialog的对象调用，
                //            若不加thisSaveFileDialog 的对象调用的是本类的其他窗口了，当然不弹出“另存为”窗口。】  


                //            string fileSavePath = sfd.FileName;// 获得文件保存的路径；  
                //            // 创建文件流，然后根据路径创建文件；  
                //            using (FileStream fs = new FileStream(fileSavePath, FileMode.Create))
                //            {
                //                fs.Write(arrMsgRec, 1, length - 1);
                //                ShowMsg("文件保存成功：" + fileSavePath);
                //            }
                //        }
                //    }
                //    catch (Exception aaa)
                //    {
                //        MessageBox.Show(aaa.Message);
                //    }
                //}
            }
        }
        public void SendMsg(object sender,string msg)
        {
            string strMsg = Dns.GetHostName()+ "" + "   -->" + msg ;
            MyMessage my = new MyMessage();
            my.id = "0";
            my.msg = msg;
            byte[] arrMsg = Encoding.UTF8.GetBytes(JsonHelper.ToJson(my));          
            sockClient.Send(arrMsg); // 发送消息；             
            md.DoShowMSGFunc(sender, strMsg);          
        }  
  
    }
}
