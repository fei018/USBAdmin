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
using System.Windows.Shapes;
using HHITtoolsTray;
using AgentLib;

namespace HHITtoolsTray.PrintWindow
{
    /// <summary>
    /// SetPrinterWin.xaml 的互動邏輯
    /// </summary>
    public partial class SetPrinterWin : Window
    {
        public SetPrinterWin()
        {
            InitializeComponent();
          
            PipeClient_Tray.Entity_Tray.AddSitePrinterCompletedEvent += Entity_Tray_AddSitePrinterListCompletedEvent;
        }

        private void Entity_Tray_AddSitePrinterListCompletedEvent(object sender, AgentLib.PipeEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>{
                EnableProgressBar(false);
                txtResult.Text = e.Msg;
            }));
        }

        private void TrayPipe_AddPrintTemplateCompletedEvent(object sender, AgentLib.PipeEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                EnableProgressBar(false);
                txtResult.Text = e.Msg;
            }));
        }

        private void btnAddPrinter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtResult.Text = "";

                EnableProgressBar(true);

                //Task.Run(() =>
                //{
                //    try
                //    {
                //        PipeClientTray.Entity_Tray?.PushMsg_ToAgent_AddPrintTemplate();
                //    }
                //    catch (Exception ex)
                //    {
                //        Dispatcher.Invoke(() =>
                //        {
                //            EnableProgressBar(false);
                //            txtResult.Text = ex.GetBaseException().Message;
                //        });
                //    }
                //});

                Task.Run(() =>
                {
                    try
                    {
                        var sitePrinterList = new AgentHttpHelp().GetSitePrinterList_Http();
                        PipeClient_Tray.Entity_Tray?.PushMsg_ToAgent_SitePrinterToAdd(sitePrinterList);
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            EnableProgressBar(false);
                            txtResult.Text = ex.GetBaseException().Message;
                        });
                    }
                });
            }
            catch (Exception)
            {
                EnableProgressBar(false);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetPrinterWin_Closed(object sender, EventArgs e)
        {
            WinSingleOpen.SetPrinterWin = false;
        }

        private void EnableProgressBar(bool enable)
        {
            if (enable)
            {
                btnAddPrinter.IsEnabled = false;
                btnCancel.IsEnabled = false;
                progressBar1.Visibility = Visibility.Visible;
            }
            else
            {
                btnAddPrinter.IsEnabled = true;
                btnCancel.IsEnabled = true;
                progressBar1.Visibility = Visibility.Hidden;
            }
        }
    }
}
