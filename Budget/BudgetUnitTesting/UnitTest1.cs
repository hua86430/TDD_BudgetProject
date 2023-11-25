using Budget.Services;

namespace BudgetUnitTesting;

[TestFixture]
public class BudgetTests
{
    private BudgetService _budgetService;

    [SetUp]
    public void SetUp()
    {
        _budgetService = new BudgetService();
    }

    [Test]
    public void when_no_budget()
    {
        AmountShouldBe(new DateTime(2023, 10, 1), new DateTime(2023, 10, 31), 0);
    }

    [Test]
    public void when_query_one_month()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        
        AmountShouldBe(new DateTime(2023, 10, 1), new DateTime(2023, 10, 31), 0);
    }

    private void AmountShouldBe(DateTime start, DateTime end, int expected)
    {
        var amount = _budgetService.Query(start, end);
        Assert.That(amount, Is.EqualTo(expected));
    }
}

public interface IBudgetRepo
{ }