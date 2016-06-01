using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;

namespace ZipWiFiWin10Configurator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WiFiAdapter mFirstAdapter;
        private DispatcherTimer dispatcherTimer;
        private int secondsSinceLastConnectionOk = 1000;
        private DateTime lastConnectionOkDateTime = new DateTime(2000, 1, 1);
        ObservableCollection<ZipNetworkConnection> availableZipConnections = new ObservableCollection<ZipNetworkConnection>();
        ZipNetworkConnection ConnectedZipUnit;
        int ConnectedZipUnitIndex;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void DomainNameSelected(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem selectedItem = (ListViewItem)(WiFiList.SelectedValue);
            if (selectedItem != null)
            {
                String securityType = SecurityType((WiFiAvailableNetwork)selectedItem.Tag);
                if (securityType != null)
                {
                    SecurityTypeOpenRadioButton.IsChecked = false;
                    SecurityTypeWpaRadioButton.IsChecked = false;
                    SecurityTypeWepRadioButton.IsChecked = false;
                    switch (securityType)
                    {
                        case "Open":
                            SecurityTypeOpenRadioButton.IsChecked = true;
                            break;
                        case "WPA":
                            SecurityTypeWpaRadioButton.IsChecked = true;
                            break;
                        case "WEP":
                            SecurityTypeWepRadioButton.IsChecked = true;
                            break;
                        case "Other":
                            break;
                    }
                }
            }
            HiddenDomain.Text = "";
            CheckEnableConfigureButton();
        }

        private String SecurityType(WiFiAvailableNetwork network)
        {
            string securityType = "Other";
            NetworkAuthenticationType AuthAlgorithm = network.SecuritySettings.NetworkAuthenticationType;
            NetworkEncryptionType CipherAlgorithm = network.SecuritySettings.NetworkEncryptionType;

            if ((AuthAlgorithm == NetworkAuthenticationType.Open80211) && (CipherAlgorithm == NetworkEncryptionType.None))
            {
                securityType = "Open";
            }
            else if ((AuthAlgorithm == NetworkAuthenticationType.RsnaPsk) && (CipherAlgorithm == NetworkEncryptionType.Ccmp))
            {
                securityType = "WPA";
            }
            else if ((AuthAlgorithm == NetworkAuthenticationType.Rsna) && (CipherAlgorithm == NetworkEncryptionType.Ccmp))
            {
                securityType = "WPA";
            }
            else if ((AuthAlgorithm == NetworkAuthenticationType.Wpa) && (CipherAlgorithm == NetworkEncryptionType.Tkip))
            {
                securityType = "WPA";
            }
            else if (CipherAlgorithm == NetworkEncryptionType.Wep40)
            {
                securityType = "WEP";
            }
            else if (CipherAlgorithm == NetworkEncryptionType.Wep104)
            {
                securityType = "WEP";
            }
            else if (CipherAlgorithm == NetworkEncryptionType.Wep)
            {
                securityType = "WEP";
            }
            else
            {
                securityType = "Other";
            }
            return securityType;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckEnableConfigureButton();
        }

        private void SecurityTypeChanged(object sender, RoutedEventArgs e)
        {
            CheckEnableConfigureButton();
        }

        private void CheckEnableConfigureButton()
        {
            bool enableButton = (WiFiList.SelectedIndex >= 0 || HiddenDomain.Text.Trim().Length > 0) &&
                ((WiFiPassword.Text.Trim().Length > 0) || (WiFiPasswordBox.Password.Trim().Length > 0)) &&
                ((SecurityTypeOpenRadioButton.IsChecked ?? false) || (SecurityTypeWpaRadioButton.IsChecked ?? false) || (SecurityTypeWepRadioButton.IsChecked ?? false));
            ConfigurWiFiButton.IsEnabled = enableButton;
        }

        private void HiddenDomain_GotFocus(object sender, RoutedEventArgs e)
        {
            if (HiddenDomain.Text.Trim().Length == 0)
            {
                SecurityTypeOpenRadioButton.IsChecked = false;
                SecurityTypeWpaRadioButton.IsChecked = false;
                SecurityTypeWepRadioButton.IsChecked = false;
            }
            WiFiList.SelectedIndex = -1;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = this.ShowPassword.IsChecked;
            if (isChecked != null && isChecked.Value)
            {
                this.WiFiPasswordBox.Visibility = Visibility.Collapsed;
                this.WiFiPassword.Visibility = Visibility.Visible;
                this.WiFiPassword.Text = this.WiFiPasswordBox.Password;
            }
            else
            {
                this.WiFiPasswordBox.Visibility = Visibility.Visible;
                this.WiFiPassword.Visibility = Visibility.Collapsed;
                this.WiFiPasswordBox.Password = this.WiFiPassword.Text;
            }
        }

        private void HiddenDomain_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEnableConfigureButton();
        }

        private void WiFiPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEnableConfigureButton();
        }

        private void WiFiPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckEnableConfigureButton();
        }

        private void ProxyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = this.ShowProxyPassword.IsChecked;
            if (isChecked != null && isChecked.Value)
            {
                this.ProxyPasswordBox.Visibility = Visibility.Collapsed;
                this.ProxyPassword.Visibility = Visibility.Visible;
                this.ProxyPassword.Text = this.ProxyPasswordBox.Password;
            }
            else
            {
                this.ProxyPasswordBox.Visibility = Visibility.Visible;
                this.ProxyPassword.Visibility = Visibility.Collapsed;
                this.ProxyPasswordBox.Password = this.ProxyPassword.Text;
            }

        }

        private void EnableProxyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = this.EnableProxyCheckBox.IsChecked;
            if (isChecked != null && isChecked.Value)
            {
                this.ProxyPasswordBox.IsEnabled = true;
                this.ProxyPassword.IsEnabled = true;
                this.ProxyHostname.IsEnabled = true;
                this.ProxyUsername.IsEnabled = true;
                this.ProxyPort.IsEnabled = true;
                this.ShowProxyPassword.IsEnabled = true;
            }
            else
            {
                this.ProxyPasswordBox.IsEnabled = false;
                this.ProxyPassword.IsEnabled = false;
                this.ProxyHostname.IsEnabled = false;
                this.ProxyUsername.IsEnabled = false;
                this.ProxyPort.IsEnabled = false;
                this.ShowProxyPassword.IsEnabled = false;
            }
        }

        private void ScanWiFiButtonClick(object sender, RoutedEventArgs e)
        {
            CollectZipWiFiNetworks();
        }

        private void ShowPassword_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }

        private void ShowProxyPassword_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ConfigurWiFiButtonClick(object sender, RoutedEventArgs e)
        {

        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CollectZipWiFiNetworks();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private async void dispatcherTimer_Tick(object sender, object e)
        {
            ConnectionProfile connectionProfile = await mFirstAdapter.NetworkAdapter.GetConnectedProfileAsync();
            if(connectionProfile != null)
            {
                String profileName = connectionProfile.ProfileName;
                lastConnectionOkDateTime = DateTime.Now;

                for(int i = 0; i < availableZipConnections.Count; i++)
                {
                    ZipNetworkConnection network = availableZipConnections.ElementAt(i);
                    if (network.Ssid == profileName)
                    {
                        this.ConnectedZipUnit = network;
                        this.ConnectedZipUnitIndex = i;
                        this.WiFiNetworksListView.SelectedIndex = i;
                        updateSelectedNetworkStatus(network, i);
                    } else
                    {
                        if (("" != network.ConnectionStatus) || ("" != network.ConnectionStatus))
                        {
                            network.ConnectionStatus = "";
                            network.Color = "";
                            network.Color = "green";
                            availableZipConnections.RemoveAt(i);
                            availableZipConnections.Insert(i, new ZipNetworkConnection { WiFiNetwork = network.WiFiNetwork, Color = network.Color, ConnectionStatus = network.ConnectionStatus });
                        }
                    }
                }
            }

            secondsSinceLastConnectionOk = (int)(DateTime.Now - lastConnectionOkDateTime).Seconds;

            if (connectionProfile == null)
            {
                for (int i = 0; i < availableZipConnections.Count; i++)
                {
                    ZipNetworkConnection network = availableZipConnections.ElementAt(i);
                    if ((this.ConnectedZipUnit != null) && (this.ConnectedZipUnit.Ssid == network.Ssid))
                    {
                        updateSelectedNetworkStatus(this.ConnectedZipUnit, this.ConnectedZipUnitIndex);
                    }
                    else
                    {
                        if (("" != network.ConnectionStatus) || ("" != network.ConnectionStatus))
                        {
                            network.ConnectionStatus = "";
                            network.Color = "";
                            network.Color = "green";
                            availableZipConnections.RemoveAt(i);
                            availableZipConnections.Insert(i, new ZipNetworkConnection { WiFiNetwork = network.WiFiNetwork, Color = network.Color, ConnectionStatus = network.ConnectionStatus });
                        }
                    }
                }
            }
        }

        private void updateSelectedNetworkStatus(ZipNetworkConnection network, int index)
        {
            if (secondsSinceLastConnectionOk < 2)
            {
                network.ConnectionStatus = "CONNECTED";
                network.Color = "green";
                availableZipConnections.RemoveAt(index);
                availableZipConnections.Insert(index, new ZipNetworkConnection { WiFiNetwork = network.WiFiNetwork, Color = network.Color, ConnectionStatus = network.ConnectionStatus });
            }
            else if (secondsSinceLastConnectionOk < 5)
            {
                if ((network.Color != "green") || (network.ConnectionStatus != "CONNECTED"))
                {
                    network.Color = "green";
                    network.ConnectionStatus = "CONNECTED";
                    availableZipConnections.RemoveAt(index);
                    availableZipConnections.Insert(index, new ZipNetworkConnection { WiFiNetwork = network.WiFiNetwork, Color = network.Color, ConnectionStatus = network.ConnectionStatus });
                }
            }
            else
            {
                if ((network.Color != "red") || (network.ConnectionStatus != "DISCONNECTED"))
                {
                    network.Color = "red";
                    network.ConnectionStatus = "DISCONNECTED";
                    availableZipConnections.RemoveAt(index);
                    availableZipConnections.Insert(index, new ZipNetworkConnection { WiFiNetwork = network.WiFiNetwork, Color = network.Color, ConnectionStatus = network.ConnectionStatus });
                }
            }
        }

        private async void CollectZipWiFiNetworks()
        {
            var accessAllowed = WiFiAdapter.RequestAccessAsync();
            IReadOnlyList<WiFiAdapter> wiFiAdapters;
            wiFiAdapters = await WiFiAdapter.FindAllAdaptersAsync();

            mFirstAdapter = wiFiAdapters.First();

            await System.Threading.Tasks.Task.Run(() => mFirstAdapter.ScanAsync()); 

            List<ZipNetworkConnection> items = new List<ZipNetworkConnection>();
            foreach (WiFiAvailableNetwork availableNetwork in mFirstAdapter.NetworkReport.AvailableNetworks)
            {
                items.Add(new ZipNetworkConnection { WiFiNetwork = availableNetwork });
            }
            this.WiFiNetworksListView.Items.Clear();
            this.WiFiList.Items.Clear();

            String wiFiNamePrefix = "Big";// (String)ApplicationData.Current.LocalSettings.Values["wifi_name_prefix"];
            foreach (ZipNetworkConnection item in items)
            {
                if ((item.Ssid.ToLower().StartsWith(wiFiNamePrefix.ToLower())))
                {
                    availableZipConnections.Add(item);
                }
                else
                {
                    ListViewItem listViewItem = new ListViewItem { Content = item.Ssid, Tag = item.WiFiNetwork };
                    this.WiFiList.Items.Add(listViewItem);
                }
            }
            this.WiFiNetworksListView.ItemsSource = availableZipConnections;
        }

        private async void ConnectToZipUnit(object sender, DoubleTappedRoutedEventArgs e)
        {
            CollectWiFiPasswordDialog dialog = new CollectWiFiPasswordDialog();
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                this.ConnectedZipUnit = (ZipNetworkConnection)this.WiFiNetworksListView.SelectedItem;
                this.ConnectedZipUnitIndex = this.WiFiNetworksListView.SelectedIndex;

                WiFiAvailableNetwork selectedNetwork = this.ConnectedZipUnit.WiFiNetwork;
                WiFiConnectionResult connectionResult;
                if (selectedNetwork.SecuritySettings.NetworkAuthenticationType == Windows.Networking.Connectivity.NetworkAuthenticationType.Open80211)
                {
                    connectionResult = await mFirstAdapter.ConnectAsync(selectedNetwork, WiFiReconnectionKind.Manual);
                }
                else
                {
                    // Only the password potion of the credential need to be supplied
                    var credential = new PasswordCredential();
                    credential.Password = dialog.Password;

                    connectionResult = await mFirstAdapter.ConnectAsync(selectedNetwork, WiFiReconnectionKind.Manual, credential);
                }

                if (connectionResult.ConnectionStatus == WiFiConnectionStatus.Success)
                {
                    int i = 99;
                }
                else
                {
                }
            }
        }
    }
}
