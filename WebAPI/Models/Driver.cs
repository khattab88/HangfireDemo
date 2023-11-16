namespace WebAPI.Models
{
    public class Driver
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public int DriverNumber { get; set; }
        public int Status { get; set; } = 1;
    }
}
