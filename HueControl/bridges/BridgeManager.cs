using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;

namespace HueControlSettings.bridges
{
    public static class BridgeManager
    {
        public static async Task<IEnumerable<LocatedBridge>> FindHueBridges()
        {
            HttpBridgeLocator locator = new HttpBridgeLocator();
            return await locator.LocateBridgesAsync(TimeSpan.FromSeconds(10));
        }

        public static async Task<LocatedBridge> FindHueBridgeByIP(string ip)
        {
            foreach (var bridge in await FindHueBridges())
            {
                if (bridge.IpAddress.Equals(ip))
                    return bridge;
            }

            return null;
        }

        public static async Task<LocatedBridge> FindHueBridgeByID(string id)
        {
            foreach (var bridge in await FindHueBridges())
            {
                if (bridge.BridgeId.Equals(id))
                    return bridge;
            }

            return null;
        }

        public static async Task<string> GenerateAppKey()
        {
            try
            {
                return await Common.HueClient.RegisterAsync("HueControl", System.Net.Dns.GetHostName());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsSetUp(string id)
        {
            return File.Exists(Path.Combine(FileUtils.GetAppdataPath(), "bridge-" + id + ".json"));
        }

        public static void UpdateUsedBridge(string id, string ip)
        {
            try
            {
                Common.BridgeID = id;
                LocalHueClient client = new LocalHueClient(ip);
                BridgeJson bridgeJson = (BridgeJson) FileUtils.GetFromAppdataAsyncJson("bridge-" + id + ".json", typeof(BridgeJson)).Result;
                client.Initialize(bridgeJson.appId);
                Common.HueClient = client;

                Task.Run(() => { Common.Bridge = Common.HueClient.GetBridgeAsync().Result; });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public class BridgeJson
    {
        public string ip;
        public string id;
        public string appId;

        public BridgeJson(string ip, string id, string appId)
        {
            this.ip = ip;
            this.id = id;
            this.appId = appId;
        }
    }
}