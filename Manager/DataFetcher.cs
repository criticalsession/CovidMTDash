using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manager
{
    public class DataFetcher
    {
        public static ProcessedCovidData? Get(string wwwrootPath)
        {
            string processedPath = Path.Combine(wwwrootPath, "processed-data.json");

            ProcessedCovidData? data = GetAlreadyProcessedData(processedPath);
            return data ?? GetFromRemoteAndProcess(processedPath);
        }

        private static ProcessedCovidData? GetAlreadyProcessedData(string processedPath)
        {
            if (File.Exists(processedPath))
            {
                try
                {
                    ProcessedCovidData? data = JsonSerializer.Deserialize<ProcessedCovidData>(File.ReadAllText(processedPath));
                    if (data != null)
                        if (data.Date.Date == DateTime.Now.Date) return data;
                }
                catch { }
            }

            return null;
        }

        public static ProcessedCovidData? GetFromRemoteAndProcess(string processedPath)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("https://raw.githubusercontent.com/COVID19-Malta/COVID19-Data/master/COVID-19%20Malta%20-%20Aggregate%20Data%20Set.csv").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var rawData = responseContent.ReadAsStringAsync().Result;

                    string[] rows = rawData.Split('\n');
                    int index = rows.Length - 1;

                    if (String.IsNullOrEmpty(rows.Last())) index--;

                    string[] latestEntry = rows[index--].Split(',');
                    string[] beforeLatest = rows[index--].Split(',');

                    ProcessedCovidData data = new ProcessedCovidData();
                    data.Date = DateTime.ParseExact(latestEntry[0], "dd/MM/yyyy",CultureInfo.InvariantCulture);
                    data.NewCases = Convert.ToInt32(latestEntry[1]);
                    data.TotalCases = Convert.ToInt32(latestEntry[2]);
                    data.TotalRecovered = Convert.ToInt32(latestEntry[3]);
                    data.TotalDeaths = Convert.ToInt32(latestEntry[4]);
                    data.ActiveCases = Convert.ToInt32(latestEntry[5]);
                    data.NewDeaths = data.TotalDeaths - Convert.ToInt32(beforeLatest[4]);
                    data.NewRecovered = data.TotalRecovered - Convert.ToInt32(beforeLatest[3]);

                    File.WriteAllText(processedPath, JsonSerializer.Serialize(data));
                    return data;
                }

                return null;
            }
        }
    }
}
