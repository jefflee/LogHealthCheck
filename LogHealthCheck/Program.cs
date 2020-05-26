using System;
using System.Net.Http;
using System.Threading.Tasks;
using NLog;

namespace LogHealthCheck
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await MainAsync2();
        }

        public static async Task MainAsync()
        {
            var logger = LogManager.GetCurrentClassLogger();

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://udn.com")
            };

            while (true)
            {
                try
                {
                    var response = await client.GetAsync(String.Empty);
                    string content = await response.Content.ReadAsStringAsync();

                    logger.Info(content);
                    Console.WriteLine(content);
                }
                catch (Exception ex)
                {
                    logger.Error("Exception");
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                }
                await Task.Delay(60000);
            }
        }

        public static async Task MainAsync2()
        {
            var logger = LogManager.GetCurrentClassLogger();

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://udn.com")
            };

            Parallel.For(0, 1000, k =>
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            Console.WriteLine(k);
                            var response = await client.GetAsync(String.Empty);
                            string content = await response.Content.ReadAsStringAsync();

                            logger.Info(content);
                            Console.WriteLine(content);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Exception");
                            logger.Error(ex);
                            Console.WriteLine(ex.Message);
                        }
                    }).Wait();
                }
            );

        }
    }
}
