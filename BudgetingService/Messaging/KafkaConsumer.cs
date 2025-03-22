using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BudgetingService.Messaging
{
    public class KafkaConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, string> _consumer;

        public KafkaConsumer(IOptions<KafkaSettings> options)
        {
            var settings = options.Value;
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = settings.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<TKey, string>(config).Build();
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            _consumer.Subscribe(topics);
        }

        public void Consume(CancellationToken cancellationToken, Action<ConsumeResult<TKey, string>> handleMessage)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    handleMessage(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Ошибка потребления: {ex.Error.Reason}");
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                _consumer.Consume(stoppingToken, consumeResult =>
                {
                    try
                    {
                        
                        var message = JsonSerializer.Deserialize<TransactionCreatedEvent>(consumeResult.Message.Value);
                        _logger.LogInformation($"Получено событие создания транзакции");
                        /*Примерное описание алгоритма дальше:
                          получение всех бюджетов пользователя: var budgets = await _budgetRepository.FindAllByUserId(message.userId).ToListAsync();
                          получение значения категории транзакции из message
                          получение значения value транзакции из message
                          цикл по всем полученным бюджетам:
                          если categoryId бюджета равен categoryId транзакции или бюджет не имеет categoryId то
                            если currency бюджета и транзакции не равны то
                              отправка restApi запроса в IntegrationService для перевода значений в одну транзакцию, передавая (budgetCurrency, transactionCurrency, transactionValue) и получение нового transactionValue
                            вызов метода update из budgetingService прибавляя к currValue transactionValue
                            нахождение процента currValue/plannedAmount * 100
                            если percent больше budget.warningThreshold и warningShowed != true
                              warningShowed = true
                              отправка запроса в NotificationService для вывода уведомления пользователю
                          конец цикла
                          */
                            
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка обработки события бюджета");
                    }
                });
            }, stoppingToken);
        }
    }
}
