using System.Net.Http;

namespace HRMS_Web.Extensions
{
    public class MyBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MyBackgroundService is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(50));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            //_logger.LogInformation("MyBackgroundService is calling API.");

            //HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7217/api/Filter/UpdateAllFixedCharges");

            //// Check if the response is successful
            //if (response.IsSuccessStatusCode)
            //{
            //    // Parse the response and do something with it
            //    string responseBody = await response.Content.ReadAsStringAsync();
            //    //Console.WriteLine(responseBody);
            //}
            //else
            //{
            //    // Log the error or handle it in some other way
            //    Console.WriteLine($"API call failed with status code {response.StatusCode}");
            //}

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MyBackgroundService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
