using System.Threading.Tasks;

namespace HueControlEvents
{
    public class HueControlService : System.ServiceProcess.ServiceBase
    {
        public HueControlService()
        {
            CanShutdown = true;
            CanStop = true;

            var task = Task.Run(async () => { await EventHelper.InitConfig(); });

            task.Wait();

            Task.Run(async () => { await EventHelper.CallEvent("startup"); });
        }

        protected override async void OnShutdown()
        {
            await EventHelper.CallEvent("shutdown");
        }

        protected override async void OnStop()
        {
            await EventHelper.CallEvent("stop");
        }
    }
}