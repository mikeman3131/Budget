using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    public class BudgeProcessor
    {
        public BudgeProcessor(DateTime startDate, DateTime endDate, IEnumerable<Budget> ieBg)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Budgets = ieBg;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public IEnumerable<Budget> Budgets { get; }

        public bool IsTheSameYear()
        {
            return StartDate.Year == EndDate.Year;
        }

        public bool IsTheSameMonth()
        {
            return StartDate.Month == EndDate.Month;
        }

        public int CalculateTheSameMonth()
        {
            return (EndDate.Subtract(StartDate).Days + 1) * this.Budgets.First().DayOfMonthMoney;
        }

        public int CalculateDifferentMonth()
        {
            int sum = Budgets.Sum(o => o.Amount);

            if (StartDate.Day != 1)
            {
                Budget startBg = Budgets.FirstOrDefault(o => o.Month.Equals(StartDate.Month));
                if (startBg != null)
                    sum -= StartDate.Day * startBg.DayOfMonthMoney;
            }

            if (EndDate.Day != DateTime.DaysInMonth(EndDate.Year, EndDate.Month))
            {
                Budget endBg = Budgets.FirstOrDefault(o => o.Month.Equals(EndDate.Month));
                if (endBg != null)
                    sum -= (DateTime.DaysInMonth(EndDate.Year, EndDate.Month) - EndDate.Day - 1) * endBg.DayOfMonthMoney;
            }

            return sum;
        }
    }
}