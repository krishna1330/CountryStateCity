using ClosedXML.Excel;
using CountryStateCityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CountryStateCityLibrary.Services
{
    public class StateService
    {
        private readonly string fileUrl = "https://evolutyzblobimages.blob.core.windows.net/egifting/states.xlsx";

        public List<State> GetStatesByCountryId(int countryId)
        {
            List<State> states = new List<State>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(fileUrl).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to download the file. HTTP Status: {response.StatusCode}");
                    }

                    using (var stream = response.Content.ReadAsStream())
                    {
                        using (var workbook = new XLWorkbook(stream))
                        {
                            var worksheet = workbook.Worksheet("states");

                            var idColumn = worksheet.Row(1).Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "id");
                            var nameColumn = worksheet.Row(1).Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "name");
                            var countryIdColumn = worksheet.Row(1).Cells().FirstOrDefault(cell => cell.Value.ToString().Trim().ToLower() == "country_id");

                            if (idColumn == null || nameColumn == null || countryIdColumn == null)
                            {
                                throw new Exception("Required columns not found in the Excel file.");
                            }

                            int idColumnIndex = idColumn.WorksheetColumn().ColumnNumber();
                            int nameColumnIndex = nameColumn.WorksheetColumn().ColumnNumber();
                            int countryIdColumnIndex = countryIdColumn.WorksheetColumn().ColumnNumber();

                            foreach (var row in worksheet.RowsUsed().Skip(1))
                            {
                                var countryCode = row.Cell(countryIdColumnIndex).TryGetValue<int>(out var countryCodeValue) ? countryCodeValue : 0;

                                if (countryCode == countryId)
                                {
                                    var id = row.Cell(idColumnIndex).TryGetValue<int>(out var idValue) ? idValue : 0;
                                    var name = row.Cell(nameColumnIndex).GetValue<string>();

                                    states.Add(new State
                                    {
                                        StateId = id,
                                        StateName = name
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
                return new List<State>();
            }

            return states;
        }
    }
}
