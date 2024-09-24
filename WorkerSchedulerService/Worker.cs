namespace WorkerSchedulerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Timer _timer;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); // Run every 5 minutes

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Scheduled task is running at: {time}", DateTimeOffset.Now);

            // Place your task logic here.
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
