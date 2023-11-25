using Budget.Repo;
using Budget.Services;
using NSubstitute;

namespace BudgetUnitTesting;

[TestFixture]
public class BudgetTests
{
    private IBudgetRepo? _budgetRepo;
    private BudgetService _budgetService;

    [SetUp]
    public void SetUp()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Test]
    public void when_no_budget()

    {
        GivenBudgetRepo(new List<Budget.Models.Budget>());

        var amount = QueryAmount(new DateTime(2023, 10, 1), new DateTime(2023, 10, 31));
        AmountShouldBe(amount, 0);
    }

    [Test]
    public void when_query_one_month()
    {
        GivenBudgetRepo(
            new List<Budget.Models.Budget>()
            {
                new Budget.Models.Budget()
                {
                    YearMonth = "202310",
                    Amount = 310
                }
            });

        var amount = QueryAmount(new DateTime(2023, 10, 1), new DateTime(2023, 10, 31));

        AmountShouldBe(amount, 310);
    }

    private void GivenBudgetRepo(List<Budget.Models.Budget> budgets)
    {
        _budgetRepo.GetAll().Returns(budgets);
    }

    private static void AmountShouldBe(decimal amount, int expected)
    {
        Assert.That(amount, Is.EqualTo(expected));
    }

    private decimal QueryAmount(DateTime start, DateTime end)
    {
        var amount = _budgetService.Query(start, end);
        return amount;
    }

    private void AmountShouldBe(DateTime start, DateTime end, int expected)
    {
        var amount = _budgetService.Query(start, end);
        Assert.That(amount, Is.EqualTo(expected));
    }
}