﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryStateCityLibrary.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public int PhoneCode { get; set; }
        public string? Currency { get; set; }
    }
}
