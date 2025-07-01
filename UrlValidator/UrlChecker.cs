using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UrlValidator
{
    // Ensure these classes are public
    public class UrlCheckItem
    {
        public string Url { get; set; }
        public string AffectedType { get; set; }
        public string AffectedName { get; set; }
        public string AffectedInfoId { get; set; }
        public string AffectedTileId { get; set; }
    }

    public class UrlStatus
    {
        public string Url { get; set; }
        public string AffectedType { get; set; }
        public string AffectedName { get; set; }
        public string AffectedInfoId { get; set; }
        public string AffectedTileId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class Summary
    {
        public int TotalUrls { get; set; }
        public int TotalSuccess { get; set; }
        public int TotalFailed { get; set; }
    }

    public class UrlChecker
    {
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static List<UrlStatus> lastUrlStatuses = new List<UrlStatus>();

        public List<UrlStatus> CheckUrls(List<UrlCheckItem> urlItems)
        {
            try
            {
                return CheckUrlsAsync(urlItems).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in CheckUrls: {ex.Message}");
                return new List<UrlStatus>();
            }
        }

        private async Task<List<UrlStatus>> CheckUrlsAsync(List<UrlCheckItem> urlItems)
        {
            var urlStatuses = new List<UrlStatus>();
            var tasks = new List<Task<UrlStatus>>();

            foreach (var urlItem in urlItems)
            {
                tasks.Add(CheckUrlAsync(urlItem));
            }

            try
            {
                var results = await Task.WhenAll(tasks);
                urlStatuses.AddRange(results);
                lastUrlStatuses = urlStatuses; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Exception in CheckUrlsAsync - {ex.Message}");
            }

            return urlStatuses;
        }

        public Summary GetSummary()
        {
            int totalSuccess = 0, totalFailed = 0;
            int totalUrls = lastUrlStatuses.Count;

            foreach (var status in lastUrlStatuses)
            {
                if (status.StatusCode >= 200 && status.StatusCode < 300)
                {
                    totalSuccess++;
                }
                else
                {
                    totalFailed++;
                }
            }

            return new Summary
            {
                TotalUrls = totalUrls,
                TotalSuccess = totalSuccess,
                TotalFailed = totalFailed
            };
        }

        private async Task<UrlStatus> CheckUrlAsync(UrlCheckItem urlItem)
        {
            var urlStatus = new UrlStatus
            {
                Url = urlItem.Url,
                AffectedType = urlItem.AffectedType,
                AffectedName = urlItem.AffectedName,
                AffectedInfoId = urlItem.AffectedInfoId,
                AffectedTileId = urlItem.AffectedTileId
            };
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await client.GetAsync(urlItem.Url);
                stopwatch.Stop();

                urlStatus.StatusCode = (int)response.StatusCode;
                urlStatus.Message = response.ReasonPhrase;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                urlStatus.StatusCode = 0;
                urlStatus.Message = ex.Message;
            }

            return urlStatus;
        }
    }
}