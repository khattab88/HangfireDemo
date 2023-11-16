namespace WebAPI.Services
{
    public interface IServiceManagement
    {
        void SendEmail();
        void UpdateDatabase();
        void CreateSponsor();
        void SyncData();
    }
}
