using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalculator
{
    public class BugetService
    {
        private IRespository _respository;

        public BugetService(IRespository respository)
        {
            _respository = respository;
        }


        public int GetBuget(DateTime startDateTime, DateTime endDateTime)
        {
            var bugets = this._respository.GetBudget();
            if ((startDateTime.Year == endDateTime.Year) &&
                (startDateTime.Month == endDateTime.Month))
            {
               
                var buget = bugets.Where(x => x.Month == startDateTime.Month).FirstOrDefault();
                var diff_days = endDateTime.Subtract(startDateTime).TotalDays+1;
                

                return (int)(buget.dayOfMonth() * diff_days);
            }

            if ((startDateTime.Month != endDateTime.Month))
            {
                var diff_days = endDateTime.Subtract(startDateTime).TotalDays+1;
                var enumerable = Enumerable.Range(startDateTime.Month, endDateTime.Month);
                var budgets= bugets.Where(o => enumerable.Contains(o.Month)).ToList();
                var sum= budgets.Take(budgets.Count - 1).Sum(o => o.Money);

                var lastMonthDays =
                endDateTime.Subtract(
                new DateTime(endDateTime.Year, endDateTime.Month, 1)).Days+1;
                sum += lastMonthDays * bugets.Last().dayOfMonth();
                return sum;
            }

            return 10;
        }
    }
}
