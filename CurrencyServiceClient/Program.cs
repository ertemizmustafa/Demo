using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using CurrencyService.Extensions;

namespace CurrencyServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Service Address: {serviceUrl}");
            try
            {
                using var currenyService = new CurrencyService.CurrencyService(serviceUrl);
                var result = currenyService.GetCurrencies();
                var json = JsonSerializer.Serialize(result, options: new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All), WriteIndented = true });
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"\nContent: {json}");

                Console.ForegroundColor = ConsoleColor.Cyan;
                var fileResult = result.GenerateExcelFile().SaveFile();
                Console.WriteLine($"\nGenerate excel and save at: {fileResult.Message}");
                fileResult = result.GenerateCsvFile().SaveFile();
                Console.WriteLine($"\nGenerate csv and save at: {fileResult.Message} ");
                fileResult = result.GenerateJsonFile().SaveFile();
                Console.WriteLine($"\nGenerate json and save at: {fileResult.Message} ");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Process Error: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
