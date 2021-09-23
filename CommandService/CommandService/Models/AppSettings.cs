namespace CommandService.Models
{
    public class AppSettings
    {
        public string DatabaseConnectionString { get; set; }
        public string DatabasePassword { get; set; }
        public RabbitMQConfig RabbitMQConfig { get; set; }
        public string GrpcPlatform { get; set; }
    }

    public class RabbitMQConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
