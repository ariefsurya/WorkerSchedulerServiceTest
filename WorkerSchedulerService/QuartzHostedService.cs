using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Threading;
using System.Threading.Tasks;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private IScheduler _scheduler;

    public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Create the scheduler
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        _scheduler.JobFactory = _jobFactory;

        // Start the scheduler
        await _scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_scheduler != null)
        {
            // Shutdown the scheduler gracefully
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}
