using System;

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