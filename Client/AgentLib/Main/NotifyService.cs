using NativeUsbLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using USBNotifyLib.Win32API;

namespace USBNotifyLib
{
    internal partial class NotifyService
    {
        ///// <summary>
        ///// 保存 Task cancel token
        ///// </summary>
        //private CancellationTokenSource _tokenSource => new CancellationTokenSource();

        ///// <summary>
        ///// 保存所有 task, 可以控制取消任務
        ///// </summary>
        //private ConcurrentBag<Task> _notifier_Tasks => new ConcurrentBag<Task>();




        //#region Tasks
        //public void Start_Notifier()
        //{
        //    try
        //    {
        //        //Start_Notifier_Disk();

        //        Start_Notifier_Volume();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public void Close_Notifier()
        //{
        //    try
        //    {
        //        //Close_Notifier_Disk();

        //        Close_Notifier_Volume();

        //        _tokenSource?.Cancel();

        //        //wait 10s
        //        Task.WhenAll(_notifier_Tasks.ToArray()).Wait(10000);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //#endregion

        //#region Volume Notify Task

        //#region Volume start close
        ///// <summary>
        ///// Disk Notifier
        ///// </summary>
        //private USBNotifier _notifier_Volume;

        ///// <summary>
        ///// Start Disk USB notifier
        ///// </summary>
        //private void Start_Notifier_Volume()
        //{
        //    _notifier_Tasks.Add(Task.Run(() =>
        //    {
        //        Form volumeForm = new Form();
        //        _notifier_Volume = new USBNotifier(volumeForm.Handle, USetupApi.GUID_DEVINTERFACE.GUID_DEVINTERFACE_VOLUME);
        //        _notifier_Volume.Arrival += Notifier_Volume_Arrival;
        //        _notifier_Volume.Removal += _notifier_Volume_Removal;
        //        Application.Run(volumeForm);

        //    }, _tokenSource.Token));
        //}

        //private void _notifier_Volume_Removal(object sender, USBEvent e)
        //{
        //    Console.WriteLine(e.Type);
        //    Console.WriteLine(e.DevicePath);
        //}

        ///// <summary>
        ///// Close Disk USB notifier
        ///// </summary>
        //private void Close_Notifier_Volume()
        //{
        //    if (_notifier_Volume != null)
        //    {
        //        _notifier_Volume.Arrival -= Notifier_Volume_Arrival;
        //        _notifier_Volume.Dispose();
        //    }
        //}
        //#endregion

        //#region Notifier_Volume_Arrival
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Notifier_Volume_Arrival(object sender, USBEvent e)
        //{
        //    _notifier_Tasks.Add(Task.Run(() =>
        //    {
        //        new UsbFilter().Filter_Scan_All_USB_Disk();

        //    }, _tokenSource.Token));
        //}
        //#endregion

        //#endregion

        //#region Disk Notify Task

        //#region Disk start close
        ///// <summary>
        ///// Disk Notifier
        ///// </summary>
        //private USBNotifier _notifier_Disk;

        ///// <summary>
        ///// Start Disk USB notifier
        ///// </summary>
        //private void Start_Notifier_Disk()
        //{
        //    _notifier_Tasks.Add(Task.Run(() =>
        //    {
        //        Form diskForm = new Form();
        //        _notifier_Disk = new USBNotifier(diskForm.Handle, USetupApi.GUID_DEVINTERFACE.GUID_DEVINTERFACE_DISK);
        //        _notifier_Disk.Arrival += Notifier_Disk_Arrival;
        //        Application.Run(diskForm);

        //    }, _tokenSource.Token));
        //}

        ///// <summary>
        ///// Close Disk USB notifier
        ///// </summary>
        //private void Close_Notifier_Disk()
        //{
        //    if (_notifier_Disk != null)
        //    {
        //        _notifier_Disk.Arrival -= Notifier_Disk_Arrival;
        //        _notifier_Disk.Dispose();
        //    }
        //}
        //#endregion

        //#region Notifier_Disk_Arrival
        ///// <summary>
        ///// Disk USB Arrival Event handler
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Notifier_Disk_Arrival(object sender, USBEvent e)
        //{
        //    _notifier_Tasks.Add(Task.Run(() =>
        //    {
        //        var usb = new NotifyUSB { DiskPath = e.DevicePath };
        //        new UsbFilter().Filter_NotifyUSB_Use_DiskPath(usb);
                
        //    }, _tokenSource.Token));
        //}
        //#endregion

        //#endregion
    }
}
