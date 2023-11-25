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

    [Test]
    public void invalid_date_time()
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

        var amount = QueryAmount(new DateTime(2023, 10, 31), new DateTime(2023, 10, 1));

        AmountShouldBe(amount, 0);
    }

    [Test]
    public void query_one_day()
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

        var amount = QueryAmount(new DateTime(2023, 10, 1), new DateTime(2023, 10, 1));

        AmountShouldBe(amount, 10);
    }
    
    [Test]
    public void query_cross_month()
    {
        GivenBudgetRepo(
            new List<Budget.Models.Budget>()
            {
                new Budget.Models.Budget()
                {
                    YearMonth = "202310",
                    Amount = 310
                },
                new Budget.Models.Budget()
                {
                    YearMonth = "202311",
                    Amount = 600
                }
            });

        var amount = QueryAmount(new DateTime(2023, 10, 30), new DateTime(2023, 11, 5));

        AmountShouldBe(amount, 120);
    }
    [Test]
    public void query_cross_month_with_one_month_no_budget()
    {
        GivenBudgetRepo(
            new List<Budget.Models.Budget>()
            {
                new Budget.Models.Budget()
                {
                    YearMonth = "202309",
                    Amount = 900
                },
                new Budget.Models.Budget()
                {
                    YearMonth = "202311",
                    Amount = 600
                }
            });

        var amount = QueryAmount(new DateTime(2023, 9, 30), new DateTime(2023, 11, 5));
        AmountShouldBe(amount, 130);

    }
    
    [Test]
    public void leap_year()
    {
        GivenBudgetRepo(
            new List<Budget.Models.Budget>()
            {
                new Budget.Models.Budget()
                {
                    YearMonth = "201602",
                    Amount = 290
                },
            });

        var amount = QueryAmount(new DateTime(2016, 2, 1), new DateTime(2016, 2, 1));
        AmountShouldBe(amount, 10);

    }
    
    [Test]
    public void cross_year()
    {
        GivenBudgetRepo(
            new List<Budget.Models.Budget>()
            {
                new Budget.Models.Budget()
                {
                    YearMonth = "202112",
                    Amount = 620
                },
                new Budget.Models.Budget()
                {
                    YearMonth = "202201",
                    Amount = 310
                }
            });

        var amount = QueryAmount(new DateTime(2021, 12, 30), new DateTime(2022, 1, 5));
        AmountShouldBe(amount, 90);

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