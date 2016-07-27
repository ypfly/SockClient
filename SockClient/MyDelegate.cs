using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SockClient
{
    class MyDelegate
    {
        delegate void SetShowRichTextBoxText(object sendr, string msg);

      public  void DoShowMSGFunc(object sendr, string msg)
        {
            string message = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss ") + msg ;
            RichTextBox rtb = (RichTextBox)sendr;
            if (rtb.InvokeRequired)
            {
                SetShowRichTextBoxText stbt = new SetShowRichTextBoxText(ShowMSG);
                rtb.Invoke(stbt, new object[] { rtb, message });
            }
            else
            {
                rtb.AppendText(msg+"\n");
            }
        }
        void ShowMSG(object sendr, string msg)
        {
            if (sendr == null)
                return;
            RichTextBox rb = (RichTextBox)sendr;
            rb.AppendText(msg+"\n");  
        }
    }
}
