using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HueControlSettings;

namespace HueControl
{
    public partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                HueControlConfig config = HueControlConfig.ReadConfigAsync().Result;
                Dispatcher.Invoke(() =>
                {
                    DebugPrintInfo.IsChecked = config.Debug_PrintInfo;
                    DebugPrintErr.IsChecked = config.Debug_PrintErrors;
                });
            });
        }

        private void AutoDetectBridges(object sender, RoutedEventArgs routedEventArgs)
        {
            NavigationService.Navigate(new Uri("ui/BridgesPage.xaml", UriKind.Relative));
        }

        private void SetUpEvents(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("ui/EventsPage.xaml", UriKind.Relative));
        }

        private void SetUpEventsService(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The events set-up feature is coming soon. You must install the service manually.",
                "Set up events", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void DebugBoxClicked(object sender, RoutedEventArgs routedEventArgs)
        {
            SavingLabel.Text = "Saving...";
            CheckBox box = (CheckBox) sender;
            string boxText = box.Content.ToString();
            bool boxChecked = box.IsChecked.Value;

            Task.Run(() =>
            {
                HueControlConfig config = HueControlConfig.ReadConfigAsync().Result;

                if (boxText.Contains("errors"))
                {
                    config.Debug_PrintErrors = boxChecked;
                } else if (boxText.Contains("info"))
                {
                    config.Debug_PrintInfo = boxChecked;
                }

                var a = config.SaveConfigAsync().Result;
                Dispatcher.Invoke(() =>
                {
                    SavingLabel.Text = "All changes saved.";
                });
            });
        }

        private void ResetApp(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the app? This cannot be undone.",
                "Reset HueControl", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                string path = FileUtils.GetAppdataPath();
                DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete(); 
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true); 
                }

                di.Delete();
                MessageBox.Show("All configuration deleted successfully. It is highly recommended to restart the app.");
            }
        }
    }
}