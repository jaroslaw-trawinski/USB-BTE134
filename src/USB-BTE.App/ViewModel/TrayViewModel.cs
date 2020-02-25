using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using USB_BTE.App.EventArgs;
using USB_BTE.App.Properties;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace USB_BTE.App.ViewModel
{
    public class TrayViewModel
    {
        private readonly NotifyIcon _notifyIcon;
        
        public event EventHandler OnDoubleClick;
        public event EventHandler<MenuItemClickEventArgs> OnMenuItemClick;

        /// <summary>
        ///     Initialize the instance.
        /// </summary>
        public TrayViewModel()
        {
            _notifyIcon = new NotifyIcon
            {
                Text = @"USB-BTE",
                Visible = true,
                ContextMenu = new ContextMenu()
            };
            _notifyIcon.DoubleClick += Ni_DoubleClick;
        }

        public void SetMenuItems(IEnumerable<TrayMenuItemViewModel> items)
        {
            _notifyIcon.ContextMenu.MenuItems.Clear();
            foreach (var item in items)
            {
                var menuItem = new MenuItem()
                {
                    Text = item.Text,
                    
                };
                menuItem.Click += (sender, args) =>
                {
                    OnMenuItemClick?.Invoke(sender, new MenuItemClickEventArgs()
                    {
                        Id = item.Id
                    });
                };
                _notifyIcon.ContextMenu.MenuItems.Add(menuItem);
            }
        }


        public void SetIcon(Icon icon)
        {
            _notifyIcon.Icon = icon;
        }
        
        private void Ni_DoubleClick(object sender, System.EventArgs e)
        {
            OnDoubleClick?.Invoke(sender, e);
        }
    }
}