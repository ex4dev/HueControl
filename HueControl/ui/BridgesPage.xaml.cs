using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HueControlSettings;
using HueControlSettings.bridges;
using HueControlSettings.ui;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;

namespace HueControl
{
    public partial class BridgesPage : Page
    {
        public static string selectedItem = null;

        public BridgesPage()
        {
            InitializeComponent();


            Task.Run(() =>
            {
                foreach (var bridge in BridgeManager.FindHueBridges().Result)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ListBox.Items.Remove(SearchingItem);
                        ListBox.Items.Add(bridge.IpAddress);
                    });
                }
            });
        }

        private void ListBox_OnSelected(object sender, RoutedEventArgs e)
        {
            ListBox box = (ListBox) sender;
            if (box == null) OptionsPanel.Visibility = Visibility.Collapsed;

            string item;

            try
            {
                item = (string) box.SelectedItem;
            }
            catch (InvalidCastException)
            {
                return;
            }

            if (item.Equals("SearchingItem")) OptionsPanel.Visibility = Visibility.Collapsed;

            selectedItem = item;
            OptionsPanel.Visibility = Visibility.Visible;

            Task.Run(() =>
            {
                LocatedBridge bridge = BridgeManager.FindHueBridgeByIP(item).Result;
                Dispatcher.Invoke(() =>
                {
                    OptionIP.Text = "Bridge IP: " + bridge.IpAddress;
                    OptionID.Text = "Bridge ID: " + bridge.BridgeId;
                    bool setUp = BridgeManager.IsSetUp(bridge.BridgeId);
                    OptionSetUp.IsEnabled = !setUp;
                    OptionSetUp.Content = setUp ? "Already set up" : "Set up...";
                    OptionDeleteConfig.Visibility = setUp ? Visibility.Visible : Visibility.Collapsed;
                    OptionMakeDefault.Visibility = setUp ? Visibility.Visible : Visibility.Collapsed;
                    bool isDefault = HueControlConfig.ReadConfig().DefaultBridge.Equals(bridge.BridgeId);
                    OptionMakeDefault.IsEnabled = !isDefault;
                    OptionMakeDefault.Content = isDefault ? "Already in use" : "Use bridge";
                });
            });
        }

        private void OptionAppKeyGen_OnClick(object sender, RoutedEventArgs e)
        {
            new BridgeKeyGenWindow(selectedItem).ShowDialog();
        }

        private void OptionDeleteConfig_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "This will delete all info stored in your computer about this bridge. You'll have to run setup again. It will not affect any other devices.",
                "Delete Bridge", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel,
                MessageBoxOptions.None);
            if (result == MessageBoxResult.OK)
            {
                Task.Run(() =>
                {
                    FileUtils.DeleteFromAppdata("bridge-" +
                        BridgeManager.FindHueBridgeByIP(selectedItem).Result.BridgeId + ".json");
                });

                NavigationService.Navigate(new Uri("ui/HomePage.xaml", UriKind.Relative));
            }
        }

        private void OptionMakeDefault_OnClick(object sender, RoutedEventArgs e)
        {
            ((Button) sender).IsEnabled = false;
            ((Button) sender).Content = "Already in use";
            Task.Run(() =>
            {
                string bridge = BridgeManager.FindHueBridgeByIP(selectedItem).Result.BridgeId;
                BridgeManager.UpdateUsedBridge(bridge, selectedItem);
                HueControlConfig config = HueControlConfig.ReadConfigAsync().Result;
                config.DefaultBridge = bridge;
                var a = config.SaveConfigAsync().Result;
            });
        }
        
    }
}