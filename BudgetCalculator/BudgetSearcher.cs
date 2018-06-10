namespace BudgetCalculator
{
    public class BudgetSearcher
    {
        public BudgetSearcher(int year, int month)
        {
            this.Year = year;
            this.Month = month;
        }

        public int Year { get; }
        public int Month { get; }
    }
}