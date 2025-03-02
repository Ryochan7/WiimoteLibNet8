using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WiimoteLib;

namespace WiimoteTest.ViewModels
{
    class MainWindowViewModel
    {
        private List<TabItem> wiimoteInfoItems = new List<TabItem>();
        public List<TabItem> WiimoteInfoList => wiimoteInfoItems;

        private List<WiimoteInfoUserControl> wiimoteInfoControls =
            new List<WiimoteInfoUserControl>();

        private WiimoteCollection wmCollection;
        public WiimoteCollection WmCollection => wmCollection;

        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => selectedIndex = value;
        }

        public MainWindowViewModel(WiimoteCollection wMC)
        {
            this.wmCollection = wMC;

            uint index = 1;
            foreach (Wiimote wm in wMC)
            {
                TabItem temp = new TabItem() { Header = $"Wiimote {index}" };
                //temp.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                WiimoteInfoUserControl tempInfoControl = new WiimoteInfoUserControl(wm);
                temp.Content = tempInfoControl;

                wiimoteInfoItems.Add(temp);
                wiimoteInfoControls.Add(tempInfoControl);
                wm.SetLEDs((int)index);

                index++;
            }

            // Select first available Wiimote if found. Otherwise, empty selection
            selectedIndex = wiimoteInfoItems.Count > 0 ? 0 : -1;
        }

        public void TearDown()
        {
            foreach(WiimoteInfoUserControl control in wiimoteInfoControls)
            {
                control.TearDown();
            }

            wiimoteInfoItems.Clear();
        }
    }
}
