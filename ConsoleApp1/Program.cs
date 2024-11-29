using CountryStateCityLibrary.Models;
using CountryStateCityLibrary.Services;
CountryService countryService = new CountryService();

//List<Country> countries = countryService.GetCountries();

//Console.WriteLine("Countries:");
//foreach (var country in countries)
//{
//    Console.WriteLine($"ID: {country.CountryId}, Name: {country.CountryName}, PhoneCode: {country.PhoneCode}, Currency: {country.Currency}");
//}

//if (countries.Any())
//{
//    Console.WriteLine("Countries:");
//    foreach (var country in countries)
//    {
//        Console.WriteLine($"ID: {country.CountryId}, Name: {country.CountryName}, PhoneCode: {country.PhoneCode}, Currency: {country.Currency}");
//    }
//}
//else
//{
//    Console.WriteLine("No countries found.");
//}

StateService stateService = new StateService();

List<State> states = stateService.GetStatesByCountryId(1);

foreach (var state in states)
{
    Console.WriteLine(state.StateId + " " + state.StateName);
}

//CityService cityService = new CityService();

//List<City> cities = cityService.GetCitiesByStateId(3901);

//foreach (var city in cities)
//{
//    Console.WriteLine(city.CityId + " " + city.CityName);
//}

//List<City> citiesByCountry = cityService.GetCitiesByCountryId(1);

//foreach (var city in citiesByCountry)
//{
//    Console.WriteLine(city.CityId + " " + city.CityName);
//}