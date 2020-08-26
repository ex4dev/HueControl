using System.ServiceProcess;

namespace HueControlEvents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceBase.Run(new HueControlService());
        }
    }
}