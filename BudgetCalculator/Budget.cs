using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalculator
{
    public class Budget
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int Amount { get; set; }

        public int DayOfMonthMoney => this.Amount /
                   (DateTime.DaysInMonth(this.Year, this.Month));
    }
}