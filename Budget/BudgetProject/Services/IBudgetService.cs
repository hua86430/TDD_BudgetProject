namespace Budget.Services;

public interface IBudgetService
{
    decimal Query(DateTime start, DateTime end);
}