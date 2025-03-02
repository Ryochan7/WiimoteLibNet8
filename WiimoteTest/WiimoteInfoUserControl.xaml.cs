using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WiimoteLib;
using WiimoteTest.ViewModels;

namespace WiimoteTest
{
    /// <summary>
    /// Interaction logic for WiimoteInfoUserControl.xaml
    /// </summary>
    public partial class WiimoteInfoUserControl : UserControl
    {
        private WiimoteInfoViewModel wiiInfoVM;

        public WiimoteInfoUserControl()
        {
            InitializeComponent();
        }

        public WiimoteInfoUserControl(Wiimote wm) : this()
        {
            wiiInfoVM = new WiimoteInfoViewModel(wm);
            if (!wm.IsOpened)
            {
                wm.Connect();
            }

            DataContext = wiiInfoVM;

            wiiInfoVM.WmStateDataChanged += WiiInfoVM_WmStateDataChanged;
        }

        private void WiiInfoVM_WmStateDataChanged(object? sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                DataContext = null;
                DataContext = wiiInfoVM;
            });
        }

        public void TearDown()
        {
            DataContext = null;
        }
    }
}
