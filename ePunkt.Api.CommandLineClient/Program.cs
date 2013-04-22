using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using System;
using System.Linq;
using ePunkt.Api.Models;
using ePunkt.Api.Responses;

namespace ePunkt.Api.CommandLineClient
{
    internal class Program
    {
        private static void Main()
        {
            var apiKey = new ApiKey
                {
                    MandatorId = Utilities.Settings.Get("MandatorId", 0),
                    Key = Utilities.Settings.Get("ApiKey", ""),
                    ClientInfo = "ePunkt.Api.CommandLineClient v" + typeof (Program).Assembly.GetName().Version
                };
            var tokenCache = new ApiTokenCache();

            using (var client = new ApiHttpClient(new Uri(Utilities.Settings.Get("ApiEndpoint", "")), () => apiKey, () => tokenCache))
            {
                Console.Write("Pinging for the first time ... ");
                Console.Write(client.SendAndReadAsync<string>(new PingRequest()).Result);
                Console.WriteLine(" " + client.ElapsedMillisecondsInLastCall + " ms.");

                Console.Write("Pinging for the second time ... ");
                Console.Write(client.SendAndReadAsync<string>(new PingRequest("Pong")).Result + " ... ");
                Console.WriteLine(" " + client.ElapsedMillisecondsInLastCall + " ms");


                Console.WriteLine();
                Console.Write("Loading published jobs (default channel) with all the information (cached, 2 times) ...");
                var jobs = client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest("")).Result;
                Console.Write(" " + jobs.Jobs.Count() + " jobs, " + client.ElapsedMillisecondsInLastCall + " ms, ");
                jobs = client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest("")).Result;
                Console.Write(" " + jobs.Jobs.Count() + " jobs, " + client.ElapsedMillisecondsInLastCall + " ms, ");

                Console.WriteLine();
                Console.Write("Loading published jobs (Karriere.at channel) with all the information (cached, 1 time) ...");
                jobs = client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest("Karriere.at")).Result;
                Console.Write(" " + jobs.Jobs.Count() + " jobs, " + client.ElapsedMillisecondsInLastCall + " ms, ");

                Console.WriteLine();
                Console.Write("Loading published jobs (ePunkt channel) with all the information (cached, 2 times) ...");
                jobs = client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest("ePunkt")).Result;
                Console.Write(" " + jobs.Jobs.Count() + " jobs, " + client.ElapsedMillisecondsInLastCall + " ms, ");
                jobs = client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest("ePunkt")).Result;
                Console.Write(" " + jobs.Jobs.Count() + " jobs, " + client.ElapsedMillisecondsInLastCall + " ms, ");

                Console.WriteLine();
                Console.Write("Loading mandator ... ");
                var mandator = client.SendAndReadAsync<Mandator>(new MandatorRequest(new Uri("http://localhost"))).Result;
                Console.WriteLine(client.ElapsedMillisecondsInLastCall + " ms.");
                Console.WriteLine(mandator.Translations.Count() + " translations, " + mandator.JobProfiles.Count() + " job profiles, " + mandator.Regions.Count() + " top-level regions.");
            }

            Console.WriteLine();
            Console.WriteLine("Everything done. Goodbye.");
            Console.ReadLine();
        }
    }
}
