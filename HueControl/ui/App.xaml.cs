using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HueControlSettings;
using HueControlSettings.bridges;

namespace HueControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            Task.Run(() =>
            {
                HueControlConfig config = HueControlConfig.ReadConfigAsync().Result;
                BridgeManager.UpdateUsedBridge(config.DefaultBridge,
                    BridgeManager.FindHueBridgeByID(config.DefaultBridge).Result.IpAddress);
                
                LightCommandQueue.StartQueue();
            });
        }
    }
}