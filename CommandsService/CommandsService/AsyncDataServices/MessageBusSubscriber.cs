using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandsService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService, IDisposable
{
    private const string Exchange = "trigger";

    private bool _isDisposed;

    private readonly IConfiguration _config;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connectiion;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
    {
        _config = config;
        _eventProcessor = eventProcessor;
        Init();
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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += MessageReceived;

        _channel.BasicConsume(_queueName, true, consumer);

        return Task.CompletedTask;
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
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, Exchange, string.Empty);

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


    private void MessageReceived(object? sender, BasicDeliverEventArgs e)
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());
        _eventProcessor.ProcessEventAsync(message).Wait();
    }
}