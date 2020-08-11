using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HueControlSettings;
using HueControlSettings.bridges;
using Microsoft.VisualBasic.CompilerServices;
using Q42.HueApi;

namespace HueControl
{
    public partial class LightsPage : Page
    {
        string selectedItem;

        public LightsPage()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                foreach (var light in Common.HueClient.GetBridgeAsync().Result.Lights)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ListBox.Items.Remove(SearchingItem);
                        ListBox.Items.Add(light.Id + ": " + light.Name);
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

            OptionsPanel.Visibility = Visibility.Visible;

            Task.Run(() =>
            {
                var client = Common.HueClient;
                string name = item.Split(": ")[1];
                string id = item.Split(": ")[0];

                selectedItem = id;
                Light light = client.GetLightAsync(id).Result;

                Dispatcher.Invoke(() =>
                {
                    OptionName.Text = "Name: " + name;
                    OptionID.Text = "ID: " + id;
                    OptionModel.Text = "Model: " + light.ModelId;
                    OptionVersion.Text = "Software Version: " + light.SoftwareVersion;
                    State state = light.State;
                    OptionEnabled.IsChecked = state.On;
                    OptionBrightness.Value = state.Brightness;
                });
            });
        }

        private void BrightnessChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                //var client = Common.HueClient;
                LightCommand command = new LightCommand();
                command.Brightness = Convert.ToByte(e.NewValue);

                OptionBrightnessText.Text = Convert.ToString(Math.Round(e.NewValue / 254 * 100) + "%");
                Task.Run(() =>
                {
                    LightCommandQueue.QueueCommand(command, selectedItem);
                    //client.SendCommandAsync(command, new List<string>() {selectedItem});
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void LightEnabled(object sender, RoutedEventArgs e)
        {
            try
            {
                LightCommand command = new LightCommand();
                CheckBox box = (CheckBox) sender;
                command.On = box.IsChecked;
                Task.Run(() =>
                {
                    Common.HueClient.SendCommandAsync(command,
                        new List<string>() {selectedItem}); //Don't queue on/off switch toggles
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}