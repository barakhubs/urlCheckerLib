using System;
using System.Collections.Generic;
using UrlValidator; // Important: Use the UrlValidator namespace

class Program
{
    static void Main()
    {
        Console.WriteLine("===== URL Checker Console Application =====");

        // Use UrlValidator.UrlCheckItem to ensure correct type
        List<UrlCheckItem> urlItems = new List<UrlCheckItem>
        {
            new UrlCheckItem 
            {
                Url = "https://www.google.com",
                AffectedType = "Tile",
                AffectedName = "Google Search"
            },
            new UrlCheckItem 
            {
                Url = "https://www.github.com", 
                AffectedType = "Tile", 
                AffectedName = "GitHub Repository"
            },
            new UrlCheckItem 
            {
                Url = "https://www.nonexistentwebsite12345.com", 
                AffectedType = "Link", 
                AffectedName = "Invalid Website Test"
            },
            new UrlCheckItem 
            {
                Url = "https://www.microsoft.com", 
                AffectedType = "Tile", 
                AffectedName = "Microsoft Home"
            }
        };

        // Create an instance of UrlChecker
        UrlChecker urlChecker = new UrlChecker();
        Console.WriteLine($"URLs: {urlItems.ToString()}");
        // Call the CheckUrls method
        List<UrlStatus> results = urlChecker.CheckUrls(urlItems);

        // Display results
        Console.WriteLine("\n===== URL Status Results =====");
        foreach (var status in results)
        {
            Console.WriteLine($"URL: {status.Url}");
            Console.WriteLine($"Affected Type: {status.AffectedType}");
            Console.WriteLine($"Affected Name: {status.AffectedName}");
            Console.WriteLine($"Status Code: {status.StatusCode}");
            Console.WriteLine($"Message: {status.Message}");
            Console.WriteLine("--------------------------------");
        }

        // Get summary
        Summary summary = urlChecker.GetSummary();
        Console.WriteLine("\n===== Summary =====");
        Console.WriteLine($"Total URLs Checked: {summary.TotalUrls}");
        Console.WriteLine($"Successful Requests: {summary.TotalSuccess}");
        Console.WriteLine($"Failed Requests: {summary.TotalFailed}");

        Console.WriteLine("\n===== Test Completed =====");
        Console.ReadLine(); // Keep console open
    }
}