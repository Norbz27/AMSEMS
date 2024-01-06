using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS
{
    public partial class CustomNotificationControl : UserControl
    {
        public CustomNotificationControl()
        {
            InitializeComponent();
        }

        public string HeaderTitle
        {
            get { return lblHtittle.Text; }
            set { lblHtittle.Text = value; }
        }

        public string NotificationText
        {
            get { return lbltitle.Text; }
            set { lbltitle.Text = value; }
        }

        public string Span
        {
            get { return lblSpan.Text; }
            set { lblSpan.Text = value; }
        }

        public void UpdateContent(string htitle, string notificationText, string span)
        {
            HeaderTitle = htitle;
            NotificationText = notificationText;
            Span = span;
        }
    }

}
