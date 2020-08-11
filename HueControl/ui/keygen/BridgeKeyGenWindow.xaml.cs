using System.Collections.Generic;
using System.Windows;

namespace HueControlSettings.ui
{
    public partial class BridgeKeyGenWindow : Window
    {
        private readonly string bridge;
        private readonly List<string> pages = new List<string>();
        public BridgeKeyGenWindow(string bridge)
        {
            this.bridge = bridge;
            InitializeComponent();
            BridgeNameLabel.Text = "for Hue bridge " + bridge;
        }
    }
}