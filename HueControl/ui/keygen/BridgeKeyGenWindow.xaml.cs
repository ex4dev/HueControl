using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using HueControlSettings.bridges;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;

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
            LocalHueClient client = new LocalHueClient(bridge);
            Task.Run(() =>
            {
                string appKey;
                do
                {
                    appKey = BridgeManager.GenerateAppKey(client).Result;
                    Thread.Sleep(1000);
                } while (appKey == null);

                Dispatcher.Invoke(() =>
                {
                    Instructions.Text = "A key has been generated. We're saving it now. Don't close this window.";
                });

                LocatedBridge locatedBridge = BridgeManager.FindHueBridgeByIP(bridge).Result;
                bool a = FileUtils.SaveToAppdataAsyncJson("bridge-" + locatedBridge.BridgeId + ".json",
                    new BridgeJson(locatedBridge.IpAddress, locatedBridge.BridgeId, appKey)).Result;

                Dispatcher.Invoke(() =>
                {
                    Instructions.Text = "Success! You're ready to go. You may now close this window.";
                });
            });
        }
    }
}