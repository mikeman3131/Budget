using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    public class BudgetRetriver
    {
        private IBudgetRepository _respository;
        private BudgeProcessor _processor;

        public BudgetRetriver(IBudgetRepository respository)
        {
            _respository = respository;
        }

        public int CalculateBudget(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException();

            IEnumerable<Budget> ieBg = this._respository.GetBudgets(new[] {
                new BudgetSearcher(startDate.Year, startDate.Month),
                new BudgetSearcher(endDate.Year, endDate.Month)
            });
            this._processor = new BudgeProcessor(startDate, endDate, ieBg);

            return _processor.IsTheSameYear()
               ? _processor.IsTheSameMonth() ? _processor.CalculateTheSameMonth() : _processor.CalculateDifferentMonth()
               : _processor.CalculateDifferentMonth();
        }
    }
}