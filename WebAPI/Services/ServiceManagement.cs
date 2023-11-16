namespace WebAPI.Services
{
    public class ServiceManagement : IServiceManagement
    {
        public void CreateSponsor()
        {
            Console.Out.WriteLine($"Creating a Sponsor (long running task) at {DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}");
        }

        public void SendEmail()
        {
            Console.Out.WriteLine($"sending an Email (short running task) at {DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}");
        }

        public void SyncData()
        {
            Console.Out.WriteLine($"Syncing Data (short running task) at {DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}");
        }

        public void UpdateDatabase()
        {
            Console.Out.WriteLine($"Updating Database (long running task) at {DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}");
        }
    }
}
