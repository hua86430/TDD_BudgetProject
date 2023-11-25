using Budget.Repo;

namespace Budget.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0;
        }

        var totalAmount = 0;
        var budgets = _budgetRepo.GetAll();

        if (IsSameMonth(start, end))
        {
            var queryDays = Math.Abs((start - end).Days) + 1;

            var currentSameMonthAmount = budgets.FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"))?.Amount ?? 0;

            totalAmount += currentSameMonthAmount / GetTotalFullMonthDaysByDateTime(start) * queryDays;
        }
        else
        {
            var startMonthAmount = GetAmountByDateTime(start, budgets) / GetTotalFullMonthDaysByDateTime(start) * GetDiffStartDays(start);
            var endMonthAmount = GetAmountByDateTime(end, budgets) / GetTotalFullMonthDaysByDateTime(end) * end.Day;

            var inQueryRangeMonthAmount = budgets
                .Where(budget => IsLessThenQueryMonth(start, budget) && IsMoreThenQueryMonth(end, budget))
                .Sum(x => x.Amount);

            totalAmount += startMonthAmount + endMonthAmount + inQueryRangeMonthAmount;
        }

        return totalAmount;
    }

    private static int GetAmountByDateTime(DateTime start, List<Models.Budget> budgets)
    {
        return budgets.FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"))?.Amount ?? 0;
    }

    private static int GetDiffStartDays(DateTime start)
    {
        return Math.Abs(start.Day - DateTime.DaysInMonth(start.Year, start.Month)) + 1;
    }

    private static bool IsMoreThenQueryMonth(DateTime end, Models.Budget x)
    {
        return int.Parse(x.YearMonth) < int.Parse(end.ToString("yyyyMM"));
    }

    private static bool IsLessThenQueryMonth(DateTime start, Models.Budget x)
    {
        return int.Parse(start.ToString("yyyyMM")) < int.Parse(x.YearMonth);
    }

    private static int GetTotalFullMonthDaysByDateTime(DateTime start)
    {
        return DateTime.DaysInMonth(start.Year, start.Month);
    }

    private static bool IsSameMonth(DateTime start, DateTime end)
    {
        return start.ToString("yyyyMM") == end.ToString("yyyyMM");
    }
}