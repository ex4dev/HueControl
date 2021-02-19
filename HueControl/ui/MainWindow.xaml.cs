using System;
using System.Windows;
using System.Windows.Controls;

namespace HueControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private dynamic frame = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        /*private void OnContentUpdated(object sender, EventArgs eventArgs)
        {
            Frame frame = (Frame) sender;
            string s = frame.Source.OriginalString;

            if (s.Equals("ui/HomePage.xaml"))
            {
                Header.Visibility = Visibility.Collapsed;
            }
            else if (s.Equals("ui/OptionsPage.xaml"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Options";
                Subtitle.Text = "Configure and customize HueControl.";
            }
            else if (s.Equals("ui/LightsPage.xaml"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Lights";
                Subtitle.Text = "View and set the properties of each light. Select a light below for more options.";
            }
            else if (s.Equals("ui/EventsPage.xaml"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Events";
                Subtitle.Text = "Respond to events with changes in lighting";
            }
            else if (s.Equals("ui/BridgesPage.xaml"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Bridges";
                Subtitle.Text = "Select a bridge below for more options.";
            }
        } */

        private void OnFrameLoaded(dynamic sender, EventArgs e)
        {
            frame = sender;
            MessageBox.Show("Frame loaded.");
            
            //MessageBox.Show(sender.Parent.ToString());
            //string tabName = sender.Parent.Header;
            //sender.Navigate(new Uri(tabName + "Page.xaml", UriKind.Relative));
        }

        private void OnTabChanged(dynamic sender, RoutedEventArgs e)
        {
            if (frame == null) return;
            frame.Navigate(new Uri("ui/" + sender.Header + "Page.xaml", UriKind.Relative));
        }
    }
}