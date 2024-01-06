using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS
{
    internal class NotificationAdapter
    {
        private FlowLayoutPanel flowLayoutPanel;

        public NotificationAdapter(FlowLayoutPanel flowLayoutPanel)
        {
            this.flowLayoutPanel = flowLayoutPanel;
        }

        public void AddNotification(string htitle, string notificationText, string span)
        {
            CustomNotificationControl notificationControl = new CustomNotificationControl();
            notificationControl.UpdateContent(htitle, notificationText, span);
            flowLayoutPanel.Controls.Add(notificationControl);
        }
    }
}
