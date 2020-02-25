using System;
using System.Drawing;
using System.Windows.Forms;
using USB_BTE.App.Properties;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace USB_BTE.App.ViewModel
{
    public class TrayViewModel
    {
        private readonly NotifyIcon _notifyIcon;
        private bool _enabled;
        private Icon Icon => _enabled ? Resources.ball_grey : Resources.ball_green;

        /// <summary>
        ///     Initialize the instance.
        /// </summary>
        public TrayViewModel()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Icon,
                Text = "Tray test", Visible = true
            };

            _notifyIcon.DoubleClick += Ni_DoubleClick;

            /* menu */
            var mMenu = new ContextMenu();
            mMenu.MenuItems.Add(0, new MenuItem("Open", new System.EventHandler(TrayOpen_Click)));
            mMenu.MenuItems.Add(1, new MenuItem("Close", new System.EventHandler(TrayExit_Click)));
            _notifyIcon.ContextMenu = mMenu;
        }

        private void TrayOpen_Click(object sender, EventArgs e)
        {

        }

        private void TrayExit_Click(object sender, EventArgs e)
        {

        }

        private void Ni_DoubleClick(object sender, EventArgs e)
        {
            _enabled = !_enabled;
            _notifyIcon.Icon = Icon;
        }
    }
}