using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi;

namespace HueControlSettings
{
    public static class LightCommandQueue
    {
        private static Dictionary<string, LightCommand> commands = new Dictionary<string, LightCommand>();
        private static bool QueueRunning = false;
        public static void QueueCommand(LightCommand command, string light)
        {
            if (command.Brightness != null)
            {
                if (command.Brightness < 1) command.On = false;
                else command.On = true;
            }

            commands[light] = command;
        }

        public static void StartQueue()
        {
            if (QueueRunning) return;
            QueueRunning = true;
            Task.Run(() =>
            {
                while (QueueRunning)
                {
                    try
                    {
                        Dictionary<string, LightCommand> commandsI = new Dictionary<string, LightCommand>(commands);
                        foreach (KeyValuePair<string, LightCommand> entry in commandsI)
                        {
                            Common.HueClient.SendCommandAsync(entry.Value, new List<string>() {entry.Key});
                            commands.Remove(entry.Key);
                            // do something with entry.Value or entry.Key
                        }

                        Thread.Sleep(1000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        Environment.Exit(-1);
                    }
                }
            });
        }

        public static void StopQueue()
        {
            QueueRunning = false;
        }
    }
}