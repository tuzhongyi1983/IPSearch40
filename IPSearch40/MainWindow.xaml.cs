using Howell.Net.NetworkDevice.Common;
using IPSearch40.Converters;
using IPSearch40.Excels;
using IPSearch40.Models;
using IPSearch40.NetworkDevices;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

namespace IPSearch40
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetFileVersion();
            this.Title = String.Format("IP搜索工具 {0}.{1}", version.Major, version.Minor);
            this.DataContext = this;
            IsSelectedAll = false;
            Devices = new ObservableCollection<DeviceModel>();
            DeviceCatalogs = new ObservableCollection<string>(DeviceCatalogConverter.GetValues());
            DeviceTypes = new ObservableCollection<string>(DeviceTypeConverter.GetValues());
            CollectionView = (CollectionView)CollectionViewSource.GetDefaultView(Devices);
            DeviceIPSetting = new DeviceIPSettingModel() { DHCP = true };
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Threading.ThreadPool.SetMaxThreads(4096, 4096);
                System.Threading.ThreadPool.SetMinThreads(1024, 1024);
                //加载工厂数据
                Exception[] exceptions = NetworkDeviceProviderFactories.Load();
                foreach (var item in exceptions)
                {
                    Helper.Logger.Error("Load Exceptions", item);
                }
                Helper.Logger.Warn("Application Startup!");
                DeviceType = DeviceType.All;
                DeviceCatalog = DeviceCatalog.Normal;
                MainGridEnabled = true;
            }
            catch (Exception ex)
            {
                Helper.Logger.Error(String.Format("Window_Loaded error."), ex);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                EndSearch();
                Exception[] exceptions = NetworkDeviceProviderFactories.Unload();
                foreach (var item in exceptions)
                {
                    Helper.Logger.Error("Unload Exceptions", item);
                }
                Helper.Logger.Warn("Application Closing!");
            }
            catch (Exception ex)
            {
                Helper.Logger.Error(String.Format("Window_Closing error."), ex);
            }
        }
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        private CollectionView CollectionView { get; set; }
        #endregion
        #region Column Sorting
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void lvDeviceDataHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
                ListSortDirection direction;

                if (headerClicked != null)
                {
                    if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                    {
                        if (headerClicked != _lastHeaderClicked)
                        {
                            direction = ListSortDirection.Ascending;
                        }
                        else
                        {
                            if (_lastDirection == ListSortDirection.Ascending)
                            {
                                direction = ListSortDirection.Descending;
                            }
                            else
                            {
                                direction = ListSortDirection.Ascending;
                            }
                        }

                        string header = headerClicked.Column.Header as string;
                        if (header == null) header = "IsSelected";
                        Sort(DeviceModel.GetPropertyName(header), direction);

                        _lastHeaderClicked = headerClicked;
                        _lastDirection = direction;
                    }
                }
            }
            catch { }
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            if (lvDeviceData.Items.Count <= 0) return;
            if (sortBy == null) return;
            ICollectionView dataView = CollectionViewSource.GetDefaultView(lvDeviceData.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void lvDeviceData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
        private void HandleItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectedDeviceItem() == true)
                {
                    ShowDeviceIPSetting();
                }
            }
            catch { }
        }
        #endregion
        #region DependencyProperties
        public static readonly DependencyProperty IsSelectedAllProperty = DependencyProperty.Register("IsSelectedAll", typeof(bool), typeof(MainWindow), new PropertyMetadata(OnIsSelectedAllPropertyChanged));

        public bool IsSelectedAll
        {
            get { return (bool)GetValue(IsSelectedAllProperty); }
            set { SetValue(IsSelectedAllProperty, value); }
        }
        private static void OnIsSelectedAllPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.NewValue == null) return;
                MainWindow mainWindow = obj as MainWindow;
                if (mainWindow != null)
                {
                    foreach (var item in mainWindow.CollectionView.OfType<DeviceModel>())
                    {
                        item.IsSelected = Convert.ToBoolean(e.NewValue);
                    }
                }
            }
            catch { }
        }

        public static readonly DependencyProperty DevicesProperty = DependencyProperty.Register("Devices", typeof(ObservableCollection<DeviceModel>), typeof(MainWindow), new PropertyMetadata(null));
        public ObservableCollection<DeviceModel> Devices
        {
            get { return (ObservableCollection<DeviceModel>)this.GetValue(DevicesProperty); }
            set { this.SetValue(DevicesProperty, value); }
        }
        public static readonly DependencyProperty DeviceCatalogsProperty = DependencyProperty.Register("DeviceCatalogs", typeof(ObservableCollection<String>), typeof(MainWindow), new PropertyMetadata(null));
        public ObservableCollection<String> DeviceCatalogs
        {
            get { return (ObservableCollection<String>)this.GetValue(DeviceCatalogsProperty); }
            set { this.SetValue(DeviceCatalogsProperty, value); }
        }
        public static readonly DependencyProperty DeviceCatalogProperty = DependencyProperty.Register("DeviceCatalog", typeof(DeviceCatalog), typeof(MainWindow), new PropertyMetadata(OnDeviceCatalogPropertyChanged));

        public DeviceCatalog DeviceCatalog
        {
            get { return (DeviceCatalog)GetValue(DeviceCatalogProperty); }
            set { SetValue(DeviceCatalogProperty, value); }
        }
        private static void OnDeviceCatalogPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.NewValue == null) return;
                MainWindow mainWindow = obj as MainWindow;
                if ((DeviceCatalog)e.NewValue == DeviceCatalog.Intelligent)
                {
                    mainWindow.FactoryType = "Howell8000";
                }
                else
                {
                    mainWindow.FactoryType = "Howell5198";
                }
                mainWindow.EndSearch();
                mainWindow.BeginSearch();
            }
            catch { }
        }

        public static readonly DependencyProperty DeviceTypesProperty = DependencyProperty.Register("DeviceTypes", typeof(ObservableCollection<String>), typeof(MainWindow), new PropertyMetadata(null));
        public ObservableCollection<String> DeviceTypes
        {
            get { return (ObservableCollection<String>)this.GetValue(DeviceTypesProperty); }
            set { this.SetValue(DeviceTypesProperty, value); }
        }

        public static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register("DeviceType", typeof(DeviceType), typeof(MainWindow), new PropertyMetadata(OnDeviceTypePropertyChanged));

        public DeviceType DeviceType
        {
            get { return (DeviceType)GetValue(DeviceTypeProperty); }
            set { SetValue(DeviceTypeProperty, value); }
        }
        private static void OnDeviceTypePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.NewValue == null) return;
            }
            catch { }
        }
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(String), typeof(MainWindow), new PropertyMetadata(null));

        public String SearchText
        {
            get { return (String)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty DeviceIPSettingProperty = DependencyProperty.Register("DeviceIPSetting", typeof(DeviceIPSettingModel), typeof(MainWindow), new PropertyMetadata(null));

        public DeviceIPSettingModel DeviceIPSetting
        {
            get { return (DeviceIPSettingModel)GetValue(DeviceIPSettingProperty); }
            set { SetValue(DeviceIPSettingProperty, value); }
        }
        public static readonly DependencyProperty FlyoutOpenedProperty = DependencyProperty.Register("FlyoutOpened", typeof(Boolean), typeof(MainWindow), new PropertyMetadata(OnFlyoutOpenedPropertyChanged));

        public Boolean FlyoutOpened
        {
            get { return (Boolean)GetValue(FlyoutOpenedProperty); }
            set { SetValue(FlyoutOpenedProperty, value); }
        }
        private static void OnFlyoutOpenedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.NewValue == null) return;
                MainWindow mainWindow = obj as MainWindow;
                mainWindow.MainGridEnabled = !(Boolean)e.NewValue;

            }
            catch { }
        }
        public static readonly DependencyProperty MainGridEnabledProperty = DependencyProperty.Register("MainGridEnabled", typeof(Boolean), typeof(MainWindow), new PropertyMetadata(null));

        public Boolean MainGridEnabled
        {
            get { return (Boolean)GetValue(MainGridEnabledProperty); }
            set { SetValue(MainGridEnabledProperty, value); }
        }
        #endregion
        #region Commands
        private ICommand openIPSettingFlyoutCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenIPSettingFlyoutCommand
        {
            get
            {
                return this.openIPSettingFlyoutCommand ?? (this.openIPSettingFlyoutCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => this.Flyouts.Items.Count > 0,
                    ExecuteDelegate = x => this.ShowDeviceIPSetting()
                });
            }
        }

        private ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return this.refreshCommand ?? (this.refreshCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => this.RefreshSearch()
                });
            }
        }


        private ICommand textBoxButtonSearchCommand;

        public ICommand TextBoxButtonSearchCommand
        {
            get
            {
                return this.textBoxButtonSearchCommand ?? (this.textBoxButtonSearchCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x =>
                    {
                        if (String.IsNullOrEmpty(this.SearchText) == false)
                        {
                            this.CollectionView.Filter = SearchFilter;
                            return;
                        }
                        this.CollectionView.Filter = null;
                    }
                });
            }
        }

        private ICommand setDeviceIPCommand;
        public ICommand SetDeviceIPCommand
        {
            get
            {
                return this.setDeviceIPCommand ?? (this.setDeviceIPCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => SetDeviceIP()
                });
            }
        }

        private ICommand deviceItemDoubleClickCommand;
        public ICommand DeviceItemDoubleClickCommand
        {
            get
            {
                return this.deviceItemDoubleClickCommand ?? (this.deviceItemDoubleClickCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x =>
                    {
                        if (SelectedDeviceItem() == true)
                        {
                            ShowDeviceIPSetting();
                        }
                    }
                });
            }
        }
        
        private ICommand openExplorerCommand;
        public ICommand OpenExplorerCommand
        {
            get
            {
                return this.openExplorerCommand ?? (this.openExplorerCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x =>
                    {
                        DeviceModel deviceModel = this.lvDeviceData.SelectedItem as DeviceModel;
                        if (deviceModel == null) return;
                        System.Diagnostics.Process.Start("iexplore.exe", String.Format("http://{0}:80/", deviceModel.IPAddress));
                    }
                });
            }
        }
        private ICommand syncOSDConfigCommand;
        public ICommand SyncOSDConfigCommand
        {
            get
            {
                return this.syncOSDConfigCommand ?? (this.syncOSDConfigCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x =>
                    {
                        DeviceModel model = lvDeviceData.SelectedItem as DeviceModel;
                        if (model == null) return;
                        SyncOSDConfig(model);
                    }
                });
            }
        }

        private ICommand exportExcelDataCommand;
        public ICommand ExportExcelDataCommand
        {
            get
            {
                return this.exportExcelDataCommand ?? (this.exportExcelDataCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => ExportExcelData()
                });
            }
        }
        private ICommand importExcelDataCommand;
        public ICommand ImportExcelDataCommand
        {
            get
            {
                return this.importExcelDataCommand ?? (this.importExcelDataCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => ImportExcelData()
                });
            }
        }
        #endregion
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Methods
        private Boolean SelectedDeviceItem()
        {
            DeviceModel deviceModel = this.lvDeviceData.SelectedItem as DeviceModel;
            if (deviceModel == null) return false;
            foreach (var item in this.Devices)
            {
                item.IsSelected = false;
            }
            deviceModel.IsSelected = true;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void ToggleFlyout(int index)
        {
            var flyout = this.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }
            flyout.IsOpen = !flyout.IsOpen;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ShowDeviceIPSetting()
        {
            try
            {
                IList<DeviceModel> selectedDevices = Devices.Where(x => x.IsSelected).ToList();
                if (selectedDevices.Count <= 0) return;
                if (selectedDevices.Count == 1)
                {
                    DeviceIPSetting.DHCP = selectedDevices[0].DHCP;
                    DeviceIPSetting.No = selectedDevices[0].No;
                    DeviceIPSetting.IPAddress = selectedDevices[0].IPAddress;
                    DeviceIPSetting.Port = selectedDevices[0].Port;
                    DeviceIPSetting.SubnetMask = selectedDevices[0].SubnetMask;
                    DeviceIPSetting.Gateway = selectedDevices[0].Gateway;
                    DeviceIPSetting.PhysicalAddress = selectedDevices[0].PhysicalAddress;
                    DeviceIPSetting.OSDName = selectedDevices[0].Name;
                    DeviceIPSetting.OSDNameEnabled = false;
                    DeviceIPSetting.IPAddressText = "IP地址:";
                    //DeviceIPSetting.Password = null;
                    chkOSDNameEnabled.IsEnabled = true;
                }
                else
                {
                    DeviceIPSetting.DHCP = false;
                    DeviceIPSetting.No = "N/A";
                    DeviceIPSetting.IPAddress = "0.0.0.0";
                    DeviceIPSetting.Port = selectedDevices[0].Port;
                    DeviceIPSetting.SubnetMask = selectedDevices[0].SubnetMask;
                    DeviceIPSetting.Gateway = selectedDevices[0].Gateway;
                    DeviceIPSetting.PhysicalAddress = null;
                    DeviceIPSetting.OSDName = null;                    
                    DeviceIPSetting.OSDNameEnabled = false;
                    DeviceIPSetting.IPAddressText = "起始IP地址:";
                    //DeviceIPSetting.Password = null;
                    chkOSDNameEnabled.IsEnabled = false;
                }
                ToggleFlyout(0);
            }
            catch { }
        }

        private void SetDeviceIP()
        {
            try
            {
                //批量修改IP地址
                if (DeviceIPSetting.No == "N/A")
                {
                    ValidationResult result = DeviceIPSetting.Validate();
                    if (result.IsValid == false)
                    {
                        this.ShowMessageAsync("参数错误", result.ErrorContent.ToString());
                        return;
                    }
                    UInt32 ipAddress = System.Net.IPAddress.Parse(DeviceIPSetting.IPAddress).ToUInt32();
                    DeviceModel[] deviceModels = this.Devices.Where(x => x.IsSelected).ToArray();
                    foreach (var item in deviceModels)
                    {
                        NetworkAddress networkAddress = new NetworkAddress()
                        {
                            AddressingType = DeviceIPSetting.DHCP ? Howell.Industry.NetworkAddressingType.Dynamic : Howell.Industry.NetworkAddressingType.Static,
                            HttpPort = item.NetworkDeviceInformation.HttpPort,
                            IPAddress = item.NetworkDeviceInformation.NetworkInterface.IPAddress,
                            Port = DeviceIPSetting.Port,
                        };                        
                        if (ipAddress != 0)
                        {
                            //ipAddress = ipAddress.GetAddressBytes();
                            networkAddress.IPAddress.IPv4Address.Address = ipAddress.ToIPAddress().ToString();
                            ++ipAddress;
                        }
                        if (DeviceIPSetting.SubnetMask != "0.0.0.0")
                        {
                            networkAddress.IPAddress.IPv4Address.Subnetmask = DeviceIPSetting.SubnetMask;
                        }
                        if (DeviceIPSetting.Gateway != "0.0.0.0")
                        {
                            networkAddress.IPAddress.IPv4Address.DefaultGateway = DeviceIPSetting.Gateway;
                        }
                        if (FactoryType == "Howell8000")
                        {
                            //必须使用原始的MAC地址，区分大小写
                            DeviceSearcher.SetNetworkAddress(item.NetworkDeviceInformation.NetworkInterface.PhyscialAddress, DeviceIPSetting.Password, networkAddress);

                        }
                        else
                        {
                            DeviceSearcher.SetNetworkAddress(networkAddress.IPAddress.IPv4Address.Address,
                                networkAddress.IPAddress.IPv4Address.Subnetmask,
                                networkAddress.IPAddress.IPv4Address.DefaultGateway,
                                networkAddress.Port, item.NetworkDeviceInformation.NetworkInterface.PhyscialAddress);
                        }
                        item.IPAddress = networkAddress.IPAddress.IPv4Address.Address;
                        item.SubnetMask = networkAddress.IPAddress.IPv4Address.Subnetmask;
                        item.Gateway = networkAddress.IPAddress.IPv4Address.DefaultGateway;
                        item.Port = networkAddress.Port;
                        item.DHCP = (networkAddress.AddressingType == Howell.Industry.NetworkAddressingType.Dynamic);                        
                    }
                    this.ShowMessageAsync("修改IP地址成功", null).ContinueWith(x =>
                    {
                        //成功后，关闭悬浮窗口
                        ThreadStart threadStart = () =>
                        {
                            ToggleFlyout(0);
                        };
                        Application.Current.Dispatcher.BeginInvoke(threadStart);

                    });
                    return;
                }
                else
                {
                    DeviceModel item = this.Devices.Where(x => x.IsSelected).FirstOrDefault();
                    if (item == null)
                    {
                        this.ShowMessageAsync("没有任何选中项", null);
                        return;
                    }
                    ValidationResult result = DeviceIPSetting.Validate();
                    if (result.IsValid == false)
                    {
                        this.ShowMessageAsync("参数错误", result.ErrorContent.ToString());
                        return;
                    }
                    NetworkAddress networkAddress = new NetworkAddress()
                    {
                        AddressingType = DeviceIPSetting.DHCP ? Howell.Industry.NetworkAddressingType.Dynamic : Howell.Industry.NetworkAddressingType.Static,
                        HttpPort = item.NetworkDeviceInformation.HttpPort,
                        IPAddress = item.NetworkDeviceInformation.NetworkInterface.IPAddress,
                        Port = DeviceIPSetting.Port,
                    };
                    networkAddress.IPAddress.IPv4Address.Address = DeviceIPSetting.IPAddress;
                    networkAddress.IPAddress.IPv4Address.DefaultGateway = DeviceIPSetting.Gateway;
                    networkAddress.IPAddress.IPv4Address.Subnetmask = DeviceIPSetting.SubnetMask;

                    if (FactoryType == "Howell8000")
                    {
                        //必须使用原始的MAC地址，区分大小写
                        DeviceSearcher.SetNetworkAddress(item.NetworkDeviceInformation.NetworkInterface.PhyscialAddress, DeviceIPSetting.Password, networkAddress);
                    }
                    else
                    {
                        DeviceSearcher.SetNetworkAddress(networkAddress.IPAddress.IPv4Address.Address, 
                            networkAddress.IPAddress.IPv4Address.Subnetmask,
                            networkAddress.IPAddress.IPv4Address.DefaultGateway,
                            networkAddress.Port, item.NetworkDeviceInformation.NetworkInterface.PhyscialAddress);
                    }
                    item.IPAddress = networkAddress.IPAddress.IPv4Address.Address;
                    item.SubnetMask = networkAddress.IPAddress.IPv4Address.Subnetmask;
                    item.Gateway = networkAddress.IPAddress.IPv4Address.DefaultGateway;
                    item.Port = networkAddress.Port;
                    item.DHCP = (networkAddress.AddressingType == Howell.Industry.NetworkAddressingType.Dynamic);

                    if (DeviceIPSetting.OSDNameEnabled)
                    {
                        NetworkDeviceHelper.SetOSDName(item, DeviceIPSetting.OSDName);
                        item.Name = DeviceIPSetting.OSDName;
                    }
                    this.ShowMessageAsync("修改IP地址成功", null).ContinueWith(x =>
                     {
                         //成功后，关闭悬浮窗口
                         ThreadStart threadStart = () =>
                         {
                             ToggleFlyout(0);
                         };
                         Application.Current.Dispatcher.BeginInvoke(threadStart);

                     });
                }
            }
            catch (Exception ex)
            {
                Helper.Logger.Error(String.Format("设置IP地址失败, No:{0},IP:{1}", DeviceIPSetting.No, DeviceIPSetting.IPAddress), ex);
                this.ShowMessageAsync("修改IP失败", ex.Message);
            }
        }

        private Boolean SearchFilter(Object obj)
        {
            DeviceModel device = obj as DeviceModel;
            if (device == null) return false;
            if (String.IsNullOrEmpty(this.SearchText) == true) return true;
            if (device.Name != null && device.Name.Contains(this.SearchText)) return true;
            if (device.Model != null && device.Model.Contains(this.SearchText)) return true;
            if (device.IPAddress != null && device.IPAddress.Contains(this.SearchText)) return true;
            if (device.SubnetMask != null && device.SubnetMask.Contains(this.SearchText)) return true;
            if (device.Gateway != null && device.Gateway.Contains(this.SearchText)) return true;
            if (device.Software != null && device.Software.Contains(this.SearchText)) return true;
            if (DeviceTypeConverter.GetString(device.DeviceType).Contains(this.SearchText)) return true;
            return false;
        }

        private void ExportExcelData()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files | *.xlsx"; ;
                if (saveFileDialog.ShowDialog() == true)
                {
                    IList<DeviceExcelData> list = new List<DeviceExcelData>();
                    DeviceModel[] selectedDevices = this.Devices.Where(x => x.IsSelected).ToArray();
                    foreach (var item in selectedDevices)
                    {
                        DeviceExcelData data = new DeviceExcelData()
                        {
                            DeviceType = DeviceTypeConverter.GetString(item.DeviceType),
                            DHCP = Boolean2ChineseConverter.GetString(item.DHCP),
                            Dns1 = item.Dns1,
                            Dns2 = item.Dns2,
                            Gateway = item.Gateway,
                            Hardware = item.Hardware,
                            Software = item.Software,
                            IPAddress = item.IPAddress,
                            Username = item.Username,
                            Password = item.Password,
                            Model = item.Model,
                            Name = item.Name,
                            No = item.No,
                            PhysicalAddress = item.PhysicalAddress,
                            Port = item.Port,
                            ProtocolType = item.ProtocolType,
                            SubnetMask = item.SubnetMask,
                            VideoChannelCount = item.VideoChannelCount,
                        };
                        list.Add(data);
                    }
                    ExcelPackageExporter.ExportDeviceExcelStream(list, saveFileDialog.FileName);
                    this.ShowMessageAsync("导出数据成功", null);
                }
            }
            catch(Exception ex)
            {
                Helper.Logger.Error("导出数据失败", ex);
                this.ShowMessageAsync("导出数据失败", ex.Message);
            }
        }

        private void ImportExcelData()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files | *.xlsx"; ;
                if (openFileDialog.ShowDialog() == true)
                {
                    IList<DeviceExcelData> deviceExcelDatas = ExcelPackageExporter.ImportDeviceExcelStream(openFileDialog.FileName);
                    int currentCount = 0;
                    int maxCount = 100;
                    foreach (var excelData in deviceExcelDatas)
                    {
                        DeviceModel item = this.Devices.Where(x => x.PhysicalAddress.ToUpper() == excelData.PhysicalAddress.ToUpper()).SingleOrDefault();
                        if (item == null) continue;
                        NetworkAddress networkAddress = new NetworkAddress()
                        {
                            AddressingType = Boolean2ChineseConverter.GetBoolean(excelData.DHCP) ? Howell.Industry.NetworkAddressingType.Dynamic : Howell.Industry.NetworkAddressingType.Static,
                            HttpPort = item.NetworkDeviceInformation.HttpPort,
                            IPAddress = item.NetworkDeviceInformation.NetworkInterface.IPAddress,
                            Port = item.NetworkDeviceInformation.Port,
                        };
                        if(excelData.Port > 0 && excelData.Port <= 65535)
                        {
                            networkAddress.Port = excelData.Port;
                        }
                        if (String.IsNullOrEmpty(excelData.IPAddress)==false && excelData.IPAddress!="0.0.0.0")
                        {
                            networkAddress.IPAddress.IPv4Address.Address = excelData.IPAddress;
                        }
                        if (String.IsNullOrEmpty(excelData.SubnetMask) == false && excelData.SubnetMask != "0.0.0.0")
                        {
                            networkAddress.IPAddress.IPv4Address.Subnetmask = excelData.SubnetMask;
                        }
                        if (String.IsNullOrEmpty(excelData.Gateway) == false && excelData.Gateway != "0.0.0.0")
                        {
                            networkAddress.IPAddress.IPv4Address.DefaultGateway = excelData.Gateway;
                        }
                        if(String.IsNullOrEmpty(excelData.Password)==false)
                        {
                            item.Password = excelData.Password;
                        }
                        if (String.IsNullOrEmpty(excelData.Username) == false)
                        {
                            item.Username = excelData.Username;
                        }
                        if (FactoryType == "Howell8000")
                        {
                            //必须使用原始的MAC地址，区分大小写
                            DeviceSearcher.SetNetworkAddress(item.NetworkDeviceInformation.NetworkInterface.PhyscialAddress, excelData.Password, networkAddress);                            
                        }
                        else
                        {
                            DeviceSearcher.SetNetworkAddress(networkAddress.IPAddress.IPv4Address.Address,
                                networkAddress.IPAddress.IPv4Address.Subnetmask,
                                networkAddress.IPAddress.IPv4Address.DefaultGateway, networkAddress.Port, item.NetworkDeviceInformation.NetworkInterface.PhyscialAddress);
                        }
                        item.IPAddress = networkAddress.IPAddress.IPv4Address.Address;
                        item.SubnetMask = networkAddress.IPAddress.IPv4Address.Subnetmask;
                        item.Gateway = networkAddress.IPAddress.IPv4Address.DefaultGateway;
                        item.Port = networkAddress.Port;
                        item.DHCP = (networkAddress.AddressingType == Howell.Industry.NetworkAddressingType.Dynamic);
                        
                    }
                    System.Threading.Thread.Sleep(2000);
                    foreach (var excelData in deviceExcelDatas)
                    {
                        DeviceModel item = this.Devices.Where(x => x.PhysicalAddress.ToUpper() == excelData.PhysicalAddress.ToUpper()).SingleOrDefault();
                        if (item == null) continue;
                        do
                        {
                            if (currentCount < maxCount)
                            {
                                Interlocked.Increment(ref currentCount);
                                Task task = Task.Run(() =>
                                {
                                    try
                                    {
                                        NetworkDeviceHelper.SetOSDName(item, excelData.Name);
                                        item.Name = excelData.Name;
                                        item.SettingSucceed = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        item.SettingSucceed = false;
                                        Helper.Logger.Error(String.Format("SetOSDName error, DeviceIP:{0}", item.IPAddress), ex);                                        
                                    }
                                })
                                .ContinueWith(x =>
                                {
                                    Interlocked.Decrement(ref currentCount);
                                });
                                break;
                            }
                            System.Threading.Thread.Sleep(100);
                        } while (true);
                    }
                    while (currentCount > 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    this.ShowMessageAsync("导入数据完成", null);
                }
            }
            catch (Exception ex)
            {
                Helper.Logger.Error("导入数据失败", ex);
                this.ShowMessageAsync("导入数据失败", ex.Message);
            }
        }

        private void SyncOSDConfig(DeviceModel deviceModel)
        {
            this.MainGridEnabled = false;
            try
            {
                OSDConfig osd = NetworkDeviceHelper.GetOSDConfig(deviceModel);
                int currentCount = 0;
                int maxCount = 100;
                DeviceModel[] selectedDevices = this.Devices.Where(x => x.IsSelected == true).ToArray();
                foreach (var item in selectedDevices)
                {
                    do
                    {
                        if (currentCount < maxCount)
                        {
                            Interlocked.Increment(ref currentCount);
                            Task task = Task.Run(() =>
                            {
                                CopyOSDConfig(item, osd);
                            })
                            .ContinueWith(x =>
                            {
                                Interlocked.Decrement(ref currentCount);
                            });
                            break;
                        }
                        System.Threading.Thread.Sleep(100);
                    } while (true);
                }
                while(currentCount >0)
                {
                    System.Threading.Thread.Sleep(100);
                }
                this.ShowMessageAsync("同步OSD参数完成",null);
            }
            catch(Exception ex)
            {
                this.ShowMessageAsync("同步OSD参数失败", ex.Message.ToString());
                Helper.Logger.Error(String.Format("SyncOSDConfig {0} error.", deviceModel?.IPAddress), ex);
            }
            finally
            {
                this.MainGridEnabled = true;
            }
        }

        private void CopyOSDConfig(DeviceModel deviceModel, OSDConfig osd)
        {
            //Object[] data = state as Object[];
            //DeviceModel deviceModel = data[0] as DeviceModel;
            //OSDConfig osd = data[1] as OSDConfig;
            try
            {
                NetworkDeviceHelper.CopyOSDConfig(deviceModel, osd);
                deviceModel.SettingSucceed = true;
            }
            catch (Exception ex)
            {
                deviceModel.SettingSucceed = false;
                Helper.Logger.Error(String.Format("CopyOSDConfig failed. DeviceIP:{0}", deviceModel.IPAddress), ex);
            }
            //DeviceSettingDelegate parameterizedThreadStart = (x,y) =>
            //{
            //    x.SettingSucceed = y;
            //};
            //Application.Current.Dispatcher.BeginInvoke(parameterizedThreadStart, deviceModel,succeed);
        }
        private  delegate void DeviceSettingDelegate(DeviceModel deviceModel, Boolean succeed);
        #region Searching
        private NetworkDeviceSearcher DeviceSearcher { get; set; }
        private Int32 No = 0;
        private String FactoryType = "Howell5198";
        private void RefreshSearch()
        {
            EndSearch();
            BeginSearch();
        }
        /// <summary>
        /// 开始搜索
        /// </summary>
        private void BeginSearch()
        {
            lock (this)
            {
                try
                {
                    this.No = 0;
                    this.Devices.Clear();
                    this.DeviceSearcher = NetworkDeviceProviderFactories.GetFactory(FactoryType).CreateSearcher();
                    this.DeviceSearcher.NetworkDeviceSearched += DeviceSearcher_NetworkDeviceSearched;
                    this.Devices.Clear();
                    this.DeviceSearcher.BeginSearch(10000);
                }
                catch (Exception ex)
                {
                    Helper.Logger.Error(String.Format("BeginSearch {0} error.", this.DeviceCatalog), ex);
                }
            }
        }
        /// <summary>
        /// 结束搜索
        /// </summary>
        private void EndSearch()
        {
            lock (this)
            {
                try
                {
                    if (DeviceSearcher != null)
                    {
                        //DeviceSearcher.EndSearch();
                        DeviceSearcher.Dispose();
                        DeviceSearcher = null;
                    }
                }
                catch (Exception ex)
                {
                    Helper.Logger.Error(String.Format("EndSearch {0} error.", this.DeviceCatalog), ex);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceSearcher_NetworkDeviceSearched(object sender, NetworkDeviceSearchedEventArgs e)
        {
            try
            {
                ParameterizedThreadStart parameterizedThreadStart = x =>
                {
                    lock (this)
                    {
                        NetworkDeviceInformation deviceModel = x as NetworkDeviceInformation;
                        if (this.DeviceSearcher.ProtocolType.Contains(deviceModel.ProtocolType)==false) return;
                        if (deviceModel != null)
                        {
                            if (this.Devices.Where(y => y.PhysicalAddress == deviceModel.NetworkInterface?.PhyscialAddress?.ToUpper()).SingleOrDefault() == null)
                            {
                                String howellModel = null;
                                DeviceType howellDeviceType = Helper.GetDeviceType(deviceModel.Model, (FactoryType == "Howell8000") ? false : true, Helper.AllDevices, deviceModel.Classification, out howellModel);
                                if (Helper.AllDevices == false && howellDeviceType == DeviceType.None) return;
                                DeviceModel device = new DeviceModel()
                                {
                                    DHCP = (deviceModel.NetworkInterface?.AddressingType == Howell.Industry.NetworkAddressingType.Dynamic) ? true : false,
                                    Dns1 = deviceModel.NetworkInterface?.IPAddress?.IPv4Address?.PrimaryDNS,
                                    Dns2 = deviceModel.NetworkInterface?.IPAddress?.IPv4Address?.SecondaryDNS,
                                    Gateway = deviceModel.NetworkInterface?.IPAddress?.IPv4Address?.DefaultGateway,
                                    IPAddress = deviceModel.NetworkInterface?.IPAddress?.IPv4Address?.Address,
                                    SubnetMask = deviceModel.NetworkInterface?.IPAddress?.IPv4Address?.Subnetmask,
                                    Hardware = deviceModel.Firmware,
                                    Software = deviceModel.Software,
                                    IsSelected = false,
                                    Model = howellModel,
                                    Name = deviceModel.Name,
                                    No = (++this.No).ToString("0000"),
                                    Username = "admin",
                                    Password = (FactoryType == "Howell8000") ? "howell1409" : "12345",
                                    PhysicalAddress = deviceModel.NetworkInterface.PhyscialAddress.ToUpper(),
                                    Port = deviceModel.Uri.Port,
                                    SerialNumber = deviceModel.SerialNumber,
                                    VideoChannelCount = deviceModel.VideoCaptureInputChannelCount ?? 0 + deviceModel.VideoNetworkInputChannelCount ?? 0,
                                    DeviceType = howellDeviceType,
                                    NetworkDeviceInformation = deviceModel,
                                    ProtocolType = deviceModel.ProtocolType
                                };
                                //如果是摄像机,读取OSD名称
                                if (device.DeviceType == DeviceType.IPCamera ||
                                    device.DeviceType == DeviceType.FaceCamera ||
                                    device.DeviceType == DeviceType.IntelligentCamera ||
                                    device.DeviceType == DeviceType.VehicleCamera)
                                {
                                    System.Threading.ThreadPool.QueueUserWorkItem(ReadOSDName, device);
                                }
                                this.Devices.Add(device);
                                CheckIPConflict(device.IPAddress);
                            }
                        }
                    }
                };
                Application.Current.Dispatcher.BeginInvoke(parameterizedThreadStart, e.DeviceInfo);
            }
            catch (Exception ex)
            {
                Helper.Logger.Error(String.Format("DeviceSearcher_NetworkDeviceSearched {0} error.", this.DeviceCatalog), ex);
            }
        }
        private void CheckIPConflict(String ipAddress)
        {
            DeviceModel [] deviceModels = this.Devices.Where(x => x.IPAddress == ipAddress).ToArray();
            if(deviceModels.Length > 1)
            {
                foreach (var item in deviceModels)
                {
                    item.IPConflict = true;
                }
            }
        }
        private void ReadOSDName(Object state)
        {
            DeviceModel model = state as DeviceModel;
            Int32 retryTimes = 0;
            while (retryTimes++ < 3)
            {
                try
                {
                    if (model == null) return;
                    model.Name = NetworkDeviceHelper.GetOSDConfig(model).Name;
                    break;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000);
                    Helper.Logger.Error(String.Format("ReadOSDName failed, IPAddress:{0}.", model?.IPAddress), ex);
                }
            }
        }
        #endregion
        #endregion
    }
}
