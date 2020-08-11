using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HueControl
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            {
                contentFrame.Navigate(new Uri("ui/HomePage.xaml", UriKind.Relative));
                Header.Visibility = Visibility.Collapsed;
            } else if (s.Equals("Options"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Options";
                Subtitle.Text = "Configure and customize HueControl.";
                contentFrame.Navigate(new Uri("ui/OptionsPage.xaml", UriKind.Relative));
            } else if (s.Equals("Lights"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Lights";
                Subtitle.Text = "View and set the properties of each light. Select a light below for more options.";
                contentFrame.Navigate(new Uri("ui/LightsPage.xaml", UriKind.Relative));
            } else if (s.Equals("Events"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Events";
                Subtitle.Text = "Coming soon: Respond to events with changes in lighting";
                contentFrame.Navigate(new Uri("ui/EventsPage.xaml", UriKind.Relative));
            } else if (s.Equals("Bridges"))
            {
                Header.Visibility = Visibility.Visible;
                Title.Text = "Bridges";
                Subtitle.Text = "Select a bridge below for more options.";
                contentFrame.Navigate(new Uri("ui/BridgesPage.xaml", UriKind.Relative));
            }
        }
    }
}