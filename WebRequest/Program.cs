using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebRequest.Helpers;
using WebRequest.Models;

namespace WebRequest
{
	class Program
	{
		private string Cui { get; set; }
		static async Task Main(string[] args)
		{
			try
			{
				if (args.Length > 0)
				{
					//-c 31534882
					if (args.Contains("-c"))
					{
						var content = Helper.GetCompanyData(args[1]);
						File.WriteAllText($@"{args[1]}.csv", content);
						Console.WriteLine(content);

						//Write to file
						List<Found> csv = JsonConvert.DeserializeObject<AnafModel>(content).found;
						Helper.WriteCsvFile(args[1],csv);
						return;
					}
					//-help
					if (args.Contains("-help"))
					{
						Console.WriteLine("-c [cui companie]	Interogare ANAF, intoarce detaliile companiei in format json si le stocheaa intr-un fisier csv dupa care le printeaza in consola.");
						Console.WriteLine(string.Empty);
						Console.WriteLine("-d Current directory.");
						Console.WriteLine(string.Empty);
						Console.WriteLine("-v[V]	Arata versiunea curenta a programului.");
						Console.WriteLine(string.Empty);
						Console.WriteLine("-help	Acest help printat pe ecran.");
						return;
					}
					if (args.Contains("-v") || args.Contains("-V"))
					{
						string ver = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
						Console.WriteLine(ver);
						return;
					}
				}
				else
				{
					Console.WriteLine(@"Trebuie sa furnizati parametrii corecti. Pentru ajutor lansati programul cu -help pentru detalii.");
				}
				//Console.ReadLine();
			}
			catch (Exception ex)
			{
				Helper.WriteError(ex.Message);
			}
		}
	}
}
