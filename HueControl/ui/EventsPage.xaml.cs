using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HueControlSettings;
using HueControlSettings.ui;
using Q42.HueApi;

namespace HueControl
{
    public partial class Events : Page
    {
        private HueControlConfig config;
        private string SelectedAction;
        private string SelectedEvent;

        public Events()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                config = HueControlConfig.ReadConfigAsync().Result;
                List<HueControlConfig.Event> events = config.Events;
                string bridge = Common.BridgeID;

                Dispatcher.Invoke(() =>
                {
                    if (events.Count > 0)
                        foreach (HueControlConfig.Event ev in events)
                        {
                            if (bridge.Equals(ev.Bridge))
                                Dispatcher.Invoke(() =>
                                {
                                    SearchingItemEvents.Visibility = Visibility.Collapsed;
                                    EventList.Items.Add(ev.Name);
                                });
                        }
                    else
                        SearchingItemEvents.Content = "No events defined for this bridge.";
                });
            });
        }

        private void NewEvent(object sender, RoutedEventArgs args)
        {
            InputDialog dialog = new InputDialog("Enter event name and cause. (name:cause)");
            dialog.ShowDialog();
            string input = dialog.Answer;
            HueControlConfig.Event e =
                new HueControlConfig.Event(input.Split(":")[0], input.Split(":")[1], Common.BridgeID);
            config.Events.Add(e);
            Task.Run(() =>
            {
                HueControlConfig a = config.SaveConfigAsync().Result;
            });
        }

        private void NewAction(object sender, RoutedEventArgs args)
        {
            HueControlConfig.Event e = config.Events.Find(e2 => e2.Name.Equals(SelectedEvent));
            InputDialog dialog = new InputDialog("Enter the ID of the light to be used for this event.");
            dialog.ShowDialog();
            string key = dialog.Answer;
            LightCommand value = new LightCommand();
            e.Effects.Add(key, value);
            Task.Run(() =>
            {
                HueControlConfig a = config.SaveConfigAsync().Result;
            });
        }

        private void EventSelected(object sender, RoutedEventArgs args)
        {
            ListBox box = (ListBox) sender;

            string item;

            try
            {
                item = (string) box.SelectedItem;
            }
            catch (InvalidCastException)
            {
                return;
            }

            SelectedEvent = item;

            if (item == null) return;
            if (item.Equals("SearchingItem")) ActionPanel.Visibility = Visibility.Collapsed;
            OptionsPanel.Visibility = Visibility.Collapsed;

            ActionPanel.Visibility = Visibility.Visible;
            ActionList.Items.Clear();

            HueControlConfig.Event e = config.Events.Find(e2 => e2.Name.Equals(item));

            if (e.Effects.Count > 0)
            {
                foreach (KeyValuePair<string, LightCommand> pair in e.Effects) ActionList.Items.Add(pair.Key);
            }
            else
            {
                SearchingItemActions.Visibility = Visibility.Visible;
                SearchingItemActions.Content = "No actions defined for this event.";
                ActionList.Items.Add(SearchingItemActions);
            }
        }

        private void ActionSelected(object sender, RoutedEventArgs e)
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

            SelectedAction = item;
            if (item == null) return;
            if (item.Equals("SearchingItem")) OptionsPanel.Visibility = Visibility.Collapsed;

            OptionsPanel.Visibility = Visibility.Visible;
        }

        private void RemoveEvent(object sender, RoutedEventArgs args)
        {
            config.Events.RemoveAll(delegate(HueControlConfig.Event e) { return (e.Name.Equals(SelectedEvent)); });
            Task.Run(() =>
            {
                var a = config.SaveConfigAsync().Result;
            });
        }

        private void RemoveAction(object sender, RoutedEventArgs args)
        {
            HueControlConfig.Event e = config.Events.Find(e2 => e2.Name.Equals(SelectedEvent));
            e.Effects.Remove(SelectedAction);
            Task.Run(() =>
            {
                var a = config.SaveConfigAsync().Result;
            });
        }
    }
}