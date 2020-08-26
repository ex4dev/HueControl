using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;

namespace HueControlEvents
{
    public static class EventHelper
    {
        private static HueControlConfig config;
        private static IEnumerable<LocatedBridge> bridges;
        private static List<LocalHueClient> clients = new List<LocalHueClient>();

        public static async Task InitConfig()
        {
            config = HueControlConfig.ReadConfigAsync().Result;
            BridgeLocator locator = new HttpBridgeLocator();
            bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
        }

        public static async Task CallEvent(string cause)
        {
            EventLog.WriteEntry("HueControl", "Handling event " + cause);
            try
            {
                foreach (HueControlConfig.Event e in config.Events)
                {
                    if (e.Cause.Equals(cause))
                    {
                        EventLog.WriteEntry("HueControl", cause + " cause matches " + e.Name + " event.");
                        foreach (KeyValuePair<string, LightCommand> pair in e.Effects)
                        {
                            string light = pair.Key;
                            LightCommand command = pair.Value;

                            await Local();

                            async Task Local()
                            {
                                foreach (LocatedBridge bridge in bridges
                                ) //Iterate through all bridges to find the bridge object that matches the event's bridge
                                {
                                    if (bridge.BridgeId.Equals(e.Bridge))
                                    {
                                        ILocalHueClient client = new LocalHueClient(bridge.IpAddress);

                                        client.Initialize(
                                            ((BridgeJson) (await FileUtils.GetFromAppdataAsyncJson(
                                                "bridge-" + bridge.BridgeId + ".json",
                                                typeof(BridgeJson)))).appId);
                                        await client.SendCommandAsync(command, new List<string> {light});
                                        EventLog.WriteEntry("HueControl", cause + " | " + e.Name + " | " + command.On);
                                        return;
                                    }

                                    EventLog.WriteEntry("HueControl", "No suitable bridges found for event " + e.Name,
                                        EventLogEntryType.Warning);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("HueControl", "Error: " + e.ToString(), EventLogEntryType.Error);
            }
        }

        public class BridgeJson
        {
            public string appId;
            public string id;
            public string ip;

            public BridgeJson(string ip, string id, string appId)
            {
                this.ip = ip;
                this.id = id;
                this.appId = appId;
            }
        }
    }
}