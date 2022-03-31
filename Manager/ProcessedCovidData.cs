using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ProcessedCovidData
    {
        public DateTime Date { get; set; }
        public int NewCases { get; set; }
        public int TotalCases { get; set; }
        public int NewRecovered { get; set; }
        public int TotalRecovered { get; set; }
        public int NewDeaths { get; set; }
        public int TotalDeaths { get; set; }
        public int ActiveCases { get; set; }
    }
}
