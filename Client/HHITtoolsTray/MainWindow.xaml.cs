using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace HHITtoolsTray
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();

#if DEBUG
            Visibility = Visibility.Visible;
#endif

            AppManager.Start();
        }

        #region This.Close()
        private void Window_Closed(object sender, EventArgs e)
        {
#if DEBUG
            //Debugger.Break();
#endif
            AppManager.Stop();
        }
        #endregion

    }
}
