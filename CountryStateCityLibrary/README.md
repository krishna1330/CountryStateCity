A library to fetch countries, states, and cities. You can get the countries, states and cities list and their details also.

Usage:

    For Countries:
        
        1. Get all countries:
        
            CountryService countryService = new CountryService();
            List<Country> countries = await countryService.GetCountries();
            foreach (var country in countries)
            {
                Console.WriteLine($"ID: {country.CountryId}, Name: {country.CountryName}, PhoneCode: {country.PhoneCode}, Currency: {country.Currency}");
            }

            Output:

            ID: 1, Name: Afghanistan, PhoneCode: 93, Currency: AFN
            ID: 2, Name: Aland Islands, PhoneCode: 358, Currency: EUR
            ID: 3, Name: Albania, PhoneCode: 355, Currency: ALL
            ID: 4, Name: Algeria, PhoneCode: 213, Currency: DZD
            ID: 5, Name: American Samoa, PhoneCode: 1, Currency: USD
            ID: 6, Name: Andorra, PhoneCode: 376, Currency: EUR

    For States:
        
        1. Get all states by Country Id
            
            StateService stateService = new StateService();
            List<State> states = await stateService.GetStatesByCountryId(1);
            foreach (var state in states)
            {
                Console.WriteLine(state.StateId + " " + state.StateName);
            }

            Output:

            3901 Badakhshan
            3871 Badghis
            3875 Baghlan
            3884 Balkh
            3872 Bamyan
            3892 Daykundi
            3899 Farah
            3889 Faryab
            3870 Ghazni

    For Cities:

        1. Get all cities by State Id
            
            List<City> cities = await cityService.GetCitiesByStateId(3901);
            foreach (var city in cities)
            {
                Console.WriteLine(city.CityId + " " + city.CityName);
            }

            Output:

            52 Ashkasham
            68 Fayzabad
            78 Jurm
            84 Khandud
            115 Raghistan
            131 Wakhan