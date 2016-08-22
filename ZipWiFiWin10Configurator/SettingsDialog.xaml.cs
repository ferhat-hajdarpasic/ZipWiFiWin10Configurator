using System;
using System.Collections.Generic;
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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZipWiFiWin10Configurator
{
    public sealed partial class SettingsDialog : ContentDialog
    {
        public SettingsDialog()
        {
            this.InitializeComponent();
            Settings settings = new Settings();
            this.Hostname.Text = settings.Hostname;
            this.Port.Text = settings.Port.ToString();
            this.WiFiDomainPRefix.Text = settings.WiFiDomainPrefix;
            this.WiFiDomainPassword.Text = settings.WiFiDomainPassword;

            this.ProxyHostname.Text = settings.Hostname;
            this.ProxyPort.Text = settings.Hostname;
            this.ProxyUsername.Text = settings.Hostname;
            this.ProxyPassword.Text = settings.Hostname;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Settings settings = new Settings();
            settings.Hostname = this.Hostname.Text;
            settings.Port= int.Parse(this.Port.Text);
            settings.WiFiDomainPrefix = this.WiFiDomainPRefix.Text;
            settings.WiFiDomainPassword = this.WiFiDomainPassword.Text;

            settings.ProxyHost = this.ProxyHostname.Text;
            settings.ProxyPort = this.ProxyPort.Text;
            settings.ProxyUsername = this.ProxyUsername.Text;
            settings.ProxyPassword = this.ProxyPassword.Text;
        }
    }
}
