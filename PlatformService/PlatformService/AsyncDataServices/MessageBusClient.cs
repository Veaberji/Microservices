using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private const string Exchange = "trigger";

    private bool _isDisposed;

    private readonly IConfiguration _config;
    private IConnection _connectiion;
    private IModel _channel;
    public MessageBusClient(IConfiguration config)
    {
        _config = config;
        Init();
    }

    public void PublishPlatform(PlatformPublishDto platform)
    {
        var message = JsonSerializer.Serialize(platform);

        if (_connectiion.IsOpen)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(Exchange, string.Empty, null, body);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                if (_connectiion.IsOpen)
                {
                    _connectiion.Close();
                    _channel.Close();
                }
            }

            _isDisposed = true;
        }
    }
    private void Init()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQHost"],
            Port = int.Parse(_config["RabbitMQPort"])
        };

        try
        {
            _connectiion = factory.CreateConnection();
            _channel = _connectiion.CreateModel();

            _channel.ExchangeDeclare(Exchange, ExchangeType.Fanout);
            _connectiion.ConnectionShutdown += ConnectionShutdown;
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }
    }

    private void ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("ConnectionShutdown");
    }
}