using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WiimoteCollection mWC;
        MainWindowViewModel winVm;

        public MainWindow()
        {
            InitializeComponent();

            // find all wiimotes connected to the system
            mWC = new WiimoteCollection();
            int index = 1;

            try
            {
                mWC.FindAllWiimotes();
            }
            catch (WiimoteNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote not found error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (WiimoteException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unknown error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            winVm = new MainWindowViewModel(mWC);

            //foreach (Wiimote wm in mWC)
            //{
            //    if (!wm.IsOpened)
            //    {
            //        wm.Connect();
            //    }
            //}

            DataContext = winVm;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            winVm.TearDown();
        }
    }
}