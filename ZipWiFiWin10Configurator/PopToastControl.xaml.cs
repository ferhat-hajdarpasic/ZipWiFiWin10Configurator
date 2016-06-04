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

namespace ZipWiFiWin10Configurator
{
    public sealed partial class PopToastControl : UserControl
    {
        public PopToastControl()
        {
            this.InitializeComponent();
        }
        private void ButtonPopToast_Click(object sender, RoutedEventArgs e)
        {
            ToastHelper.PopCustomToast(TextBoxPayload.Text);
        }

        private void ButtonToggleShowPayload_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxPayload.Visibility == Visibility.Visible)
            {
                TextBoxPayload.Visibility = Visibility.Collapsed;
                ButtonToggleShowPayload.Content = "Show Payload";
            }

            else
            {
                TextBoxPayload.Visibility = Visibility.Visible;
                ButtonToggleShowPayload.Content = "Hide Payload";
            }
        }

        private static readonly DependencyProperty PayloadProperty = DependencyProperty.Register("Payload", typeof(string), typeof(PopToastControl), new PropertyMetadata(""));

        public string Payload
        {
            get { return GetValue(PayloadProperty) as string; }
            set { SetValue(PayloadProperty, value); }
        }
    }
}
