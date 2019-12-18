using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using CsvHelper;
using WebRequest.Models;

namespace WebRequest.Helpers
{
	public class Helper
	{
		private const string Url = "https://webservicesp.anaf.ro/PlatitorTvaRest/api/v4/ws/tva";

		public static string GetCompanyData(string Cui)
		{
			string payload = string.Empty;
			using (var client = new HttpClient())
			{
				HttpResponseMessage response;
				HttpRequestMessage req = new HttpRequestMessage();
				HttpContent hc = new StringContent("[{\"cui\": " + Cui + ",\"data\": \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"}]");
				hc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				req.Content = hc;
				req.RequestUri = new Uri(Url);
				req.Method = HttpMethod.Post;
				response = client.SendAsync(req).Result;
				response.EnsureSuccessStatusCode();
				payload = response.Content.ReadAsStringAsync().Result;
			}

			return payload;
		}

		public static void WriteError(string text)
		{
			string path = @"Error.log";
			// This text is added only once to the file.
			if (!File.Exists(path))
			{
				// Create a file to write to.
				using (StreamWriter sw = File.CreateText(path))
				{
					sw.WriteLine(DateTime.Now.ToString("yyy-MM-dd H:mm:ss"));
					sw.WriteLine(text);
					sw.WriteLine("----------------------------------------------------------------------------------------------------------------------");
				}
			}

			// This text is always added, making the file longer over time
			// if it is not deleted.
			using (StreamWriter sw = File.AppendText(path))
			{
				sw.WriteLine(DateTime.Now.ToString("yyy-MM-dd H:mm:ss"));
				sw.WriteLine(text);
				sw.WriteLine("----------------------------------------------------------------------------------------------------------------------");
			}
		}

		public static void WriteCsvFile(string fileName, List<Found> records)
		{
			string path = $@"{fileName}.csv";

			using (var writer = new StreamWriter(path))
			using (var csv = new CsvWriter(writer))
			{    
				csv.WriteRecords(records);
			}
		}
	}
}
