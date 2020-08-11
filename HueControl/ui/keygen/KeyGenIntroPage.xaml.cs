using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using HueControl;
using HueControlSettings.bridges;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;

namespace HueControlSettings.ui.keygen
{
    public partial class KeyGenIntroPage : Page
    {
        public KeyGenIntroPage()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                string appKey;
                do
                {
                    appKey = BridgeManager.GenerateAppKey().Result;
                    Thread.Sleep(1000);
                } while (appKey == null);

                Dispatcher.Invoke(() =>
                {
                    Instructions.Text = "A key has been generated. We're saving it now. Don't close this window.";
                });

                LocatedBridge bridge = BridgeManager.FindHueBridgeByIP(BridgesPage.selectedItem).Result;
                var a = FileUtils.SaveToAppdataAsyncJson("bridge-" + bridge.BridgeId + ".json",
                    new BridgeJson(bridge.IpAddress, bridge.BridgeId, appKey)).Result;

                Dispatcher.Invoke(() =>
                {
                    Instructions.Text = "Success! You're ready to go. You may now close this window.";
                });

            });
        }
    }
}