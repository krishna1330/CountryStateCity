using ClosedXML.Excel;
using CountryStateCityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CountryStateCityLibrary.Services
{
    public class CityService
    {
        private readonly string? fileUrl = FileService.CitiesFile;

        public async Task<List<City>> GetCitiesByStateId(int stateId)
        {
            return await GetCities((row, columns) =>
            {
                var stateCode = row.Cell(columns["state_id"]).TryGetValue<int>(out var stateCodeValue) ? stateCodeValue : 0;
                return stateCode == stateId;
            });
        }

        public async Task<List<City>> GetCitiesByCountryId(int countryId)
        {
            return await GetCities((row, columns) =>
            {
                var countryCode = row.Cell(columns["country_id"]).TryGetValue<int>(out var countryCodeValue) ? countryCodeValue : 0;
                return countryCode == countryId;
            });
        }

        private async Task<List<City>> GetCities(Func<IXLRow, Dictionary<string, int>, bool> filter)
        {
            List<City> cities = new List<City>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(fileUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to download the file. HTTP Status: {response.StatusCode}");
                    }

                    using (var stream = response.Content.ReadAsStream())
                    {
                        using (var workbook = new XLWorkbook(stream))
                        {
                            var worksheet = workbook.Worksheet("cities");

                            var headerRow = worksheet.Row(1);
                            var columns = new Dictionary<string, int>
                            {
                                { "id", headerRow.Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "id")?.WorksheetColumn().ColumnNumber() ?? throw new Exception("Column 'id' not found") },
                                { "name", headerRow.Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "name")?.WorksheetColumn().ColumnNumber() ?? throw new Exception("Column 'name' not found") },
                                { "state_id", headerRow.Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "state_id")?.WorksheetColumn().ColumnNumber() ?? throw new Exception("Column 'state_id' not found") },
                                { "country_id", headerRow.Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "country_id")?.WorksheetColumn().ColumnNumber() ?? throw new Exception("Column 'country_id' not found") }
                            };

                            foreach (var row in worksheet.RowsUsed().Skip(1))
                            {
                                if (filter(row, columns))
                                {
                                    var id = row.Cell(columns["id"]).TryGetValue<int>(out var idValue) ? idValue : 0;
                                    var name = row.Cell(columns["name"]).GetValue<string>();

                                    cities.Add(new City
                                    {
                                        CityId = id,
                                        CityName = name
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<City>();
            }

            return cities;
        }
    }
}
