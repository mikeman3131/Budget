using System;
using System.Collections.Generic;

namespace BudgetCalculator
{
    public interface IBudgetRepository
    {
        Budget GetBudget(int year, int month);
        IEnumerable<Budget> GetBudgets(IEnumerable<Tuple<int, int>> condition);
    }
}