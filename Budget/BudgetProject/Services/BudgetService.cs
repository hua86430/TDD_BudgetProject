using Budget.Repo;

namespace Budget.Services;

public class BudgetService
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

        var queryDays = (end-start).Days + 1;

        var budgets = _budgetRepo.GetAll();
        var amount = budgets.FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"))?.Amount ?? 0;

        var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
        return (amount / daysInMonth) * queryDays;
    }
}