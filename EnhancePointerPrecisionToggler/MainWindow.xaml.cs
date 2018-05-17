using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace EnhancePointerPrecisionToggler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotifyIcon _icon;
        private readonly RegistryKey _rk;

        public MainWindow()
        {
            InitializeComponent();

            Hide();
            _icon = new NotifyIcon();
            _rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            _rk.SetValue("EnhancePointerPrecisionToggler", Assembly.GetEntryAssembly().Location);

            Redraw();
            _icon.Visible = true;
            _icon.ContextMenu = new ContextMenu();

            _icon.MouseClick += Click;
            _icon.ContextMenu.Popup += Exit;
        }

        private void Redraw()
        {
            if (Win32Wrapper.IsToggled)
            {
                _icon.Icon = Properties.Resources.mouse_green;
            }
            else
            {
                _icon.Icon = Properties.Resources.mouse_red;
            }
        }

        private void Click(object sender, EventArgs eventArgs)
        {
            Win32Wrapper.IsToggled = !Win32Wrapper.IsToggled;
            Redraw();
        }


        private void Exit(object sender, EventArgs mouseEventArgs)
        {
            Win32Wrapper.IsToggled = !Win32Wrapper.IsToggled;
            _rk.DeleteValue("EnhancePointerPrecisionToggler", false);
            Close();
        }

        ~MainWindow()
        {
            _icon.Dispose();
        }
    }
}