using ClosedXML.Excel;
using CountryStateCityLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountryStateCityLibrary.Services
{
    public class CountryService
    {
        private readonly string? fileUrl = FileService.CountriesFile;

        public async Task<List<Country>> GetCountries()
        {
            List<Country> countries = new List<Country>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(fileUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to download the file. HTTP Status: {response.StatusCode}");
                    }

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var workbook = new XLWorkbook(stream))
                        {
                            var worksheet = workbook.Worksheet("countries");

                            var idColumn = worksheet.Row(1).Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "id");
                            var nameColumn = worksheet.Row(1).Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "name");
                            var phoneCodeColumn = worksheet.Row(1).Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "phone_code");
                            var currencyColumn = worksheet.Row(1).Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "currency");

                            if (idColumn == null || nameColumn == null || phoneCodeColumn == null || currencyColumn == null)
                            {
                                throw new Exception("Required columns not found in the Excel file.");
                            }

                            int idColumnIndex = idColumn.WorksheetColumn().ColumnNumber();
                            int nameColumnIndex = nameColumn.WorksheetColumn().ColumnNumber();
                            int phoneCodeColumnIndex = phoneCodeColumn.WorksheetColumn().ColumnNumber();
                            int currencyColumnIndex = currencyColumn.WorksheetColumn().ColumnNumber();

                            foreach (var row in worksheet.RowsUsed().Skip(1))
                            {
                                var id = row.Cell(idColumnIndex).TryGetValue<int>(out var idValue) ? idValue : 0;
                                var name = row.Cell(nameColumnIndex).GetValue<string>();
                                var phoneCode = row.Cell(phoneCodeColumnIndex).TryGetValue<int>(out var phoneCodeValue) ? phoneCodeValue : 0;
                                var currency = row.Cell(currencyColumnIndex).GetValue<string>();

                                countries.Add(new Country
                                {
                                    CountryId = id,
                                    CountryName = name,
                                    PhoneCode = phoneCode,
                                    Currency = currency
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Country>();
            }

            return countries;
        }
    }
}