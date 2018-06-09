using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalculator
{
    public class Buget
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int Money { get; set; }

        public int dayOfMonth()
        {
            return this.Money /
                   (DateTime.DaysInMonth(this.Year, this.Month));
        }
    }
}