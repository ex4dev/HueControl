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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangeContent(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            string s = button.Content.ToString();

            if (s.Equals("HueControl"))
                contentFrame.Navigate(new Uri("ui/HomePage.xaml", UriKind.Relative));
            else if (s.Equals("Options"))
                contentFrame.Navigate(new Uri("ui/OptionsPage.xaml", UriKind.Relative));
            else if (s.Equals("Lights"))
                contentFrame.Navigate(new Uri("ui/LightsPage.xaml", UriKind.Relative));
            else if (s.Equals("Events"))
                contentFrame.Navigate(new Uri("ui/EventsPage.xaml", UriKind.Relative));
            else if (s.Equals("Bridges")) contentFrame.Navigate(new Uri("ui/BridgesPage.xaml", UriKind.Relative));
        }

        private void OnContentUpdated(object sender, EventArgs eventArgs)
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
        }
    }
}