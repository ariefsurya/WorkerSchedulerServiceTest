using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Threading.Tasks;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Add Quartz.NET services
        services.AddQuartz(q =>
        {
            // Register the job with Quartz.NET
            q.UseMicrosoftDependencyInjectionJobFactory();

            // Define the job
            var jobKey = new JobKey("MonthlyJob");
            q.AddJob<MonthlyJob>(opts => opts.WithIdentity(jobKey));

            // Create a trigger that runs on the 1st of every month at 12:00 AM
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("MonthlyTrigger")
                .WithCronSchedule("0 0 0 1 * ?")); // 1st of every month at midnight


            // Define the 6 months job
            var sixMonthsJobKey = new JobKey("SixMonthsJob");
            q.AddJob<SixMonthsJob>(opts => opts.WithIdentity(sixMonthsJobKey));
            q.AddTrigger(opts => opts
                .ForJob(sixMonthsJobKey)
                .WithIdentity("SixMonthsTrigger")
                .WithCronSchedule("0 0 0 1 1,7 ? *")); // Runs on 1st January and 1st July at midnight

            // Define the yearly job
            var yearlyJobKey = new JobKey("YearlyJob");
            q.AddJob<YearlyJob>(opts => opts.WithIdentity(yearlyJobKey));
            q.AddTrigger(opts => opts
                .ForJob(yearlyJobKey)
                .WithIdentity("YearlyTrigger")
                .WithCronSchedule("0 0 0 1 1 ? *")); // Runs on the 1st of January at midnight
        });

        // Register the Quartz hosted service manually
        services.AddHostedService<QuartzHostedService>();
    })
    .Build();

await host.RunAsync();
