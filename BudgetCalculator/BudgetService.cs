using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    public class BudgetService
    {
        private IBudgetRepository _respository;
        private DateTime _startDate;
        private DateTime _endDate;

        public BudgetService(IBudgetRepository respository)
        {
            _respository = respository;
        }

        public int CalculateBudget(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException();

            this._startDate = startDate;
            this._endDate = endDate;

            return IsTheSameMonth()
               ? IsTheSameMonth() ? CalculateTheSameMonth() : CalculateDifferentMonth()
               : CalculateDifferentMonth();
        }

        private bool IsTheSameYear()
        {
            return _startDate.Year == _endDate.Year;
        }

        private bool IsTheSameMonth()
        {
            return _startDate.Month == _endDate.Month;
        }

        private int CalculateTheSameMonth()
        {
            int diffDays = _endDate.Subtract(_startDate).Days + 1;
            Budget budget = this._respository.GetBudget(_startDate.Year, _startDate.Month);

            return diffDays * budget.DayOfMonthMoney;
        }

        private int CalculateDifferentMonth()
        {
            IEnumerable<Budget> ieBg = this._respository.GetBudgets(new List<Tuple<int, int>> {
                    Tuple.Create<int,int>(_startDate.Year, _startDate.Month),
                    Tuple.Create<int,int>(_endDate.Year, _endDate.Month)}
                );

            int sum = ieBg.Sum(o => o.Amount);

            if (_startDate.Day != 1)
            {
                Budget startBg = ieBg.FirstOrDefault(o => o.Month.Equals(_startDate.Month));
                if (startBg != null)
                    sum -= _startDate.Day * startBg.DayOfMonthMoney;

                if (_endDate.Day != DateTime.DaysInMonth(_endDate.Year, _endDate.Month))
                {
                    Budget endBg = ieBg.FirstOrDefault(o => o.Month.Equals(_endDate.Month));
                    if (endBg != null)
                        sum -= (DateTime.DaysInMonth(_endDate.Year, _endDate.Month) - _endDate.Day - 1) * endBg.DayOfMonthMoney;
                }
            }

            return sum;
        }
    }
}