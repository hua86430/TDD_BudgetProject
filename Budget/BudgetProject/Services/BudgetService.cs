using Budget.Repo;

namespace Budget.Services;

public class BudgetService: IBudgetService
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
        var tempStartDate = start;

        while (tempStartDate <= end)
        {
            var tempEndDate = new DateTime(tempStartDate.Year, tempStartDate.Month , 1).AddMonths(1).AddDays(-1);

            if (tempEndDate > end)
            {
                tempEndDate = end;
            }

            var queryDays = (tempEndDate - tempStartDate).Days + 1;
            var monthAmount = budgets.FirstOrDefault(x => x.YearMonth == tempStartDate.ToString("yyyyMM"))?.Amount ?? 0;

            var daysInMonth = DateTime.DaysInMonth(tempStartDate.Year, tempStartDate.Month);
            totalAmount += monthAmount / daysInMonth * queryDays;

            tempStartDate = tempEndDate.AddDays(1);
        }

        return totalAmount;
    }
}