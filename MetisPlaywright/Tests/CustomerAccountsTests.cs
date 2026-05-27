using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class CustomerAccountsTests : BaseTest
    {
        [Test]
        public async Task T01_CustomerAccounts_DefaultView()
        {
            const string expectedTitle = "Customer Accounts";
            const string expectedMessage = "No customer records";
            var customerAccountsPage = new CustomerAccountsPage(Fixture.Page);
            await customerAccountsPage.OpenForEmptyTenantAsync();
            var actualTitle = await customerAccountsPage.GetPageTitleAsync();
            var actualMessage = await customerAccountsPage.GetCustomerGridEmptyMessageAsync();
            actualTitle.Should().Be(expectedTitle, "Customer Accounts page title is not correct.");
            actualMessage.Should().Be(expectedMessage, "The empty grid message is not correct.");
        }
        
        [Test]
        public async Task T02_CustomerAccounts_NotEmptyGrid()
        {
            var customerAccountsPage = new CustomerAccountsPage(Fixture.Page);
            await customerAccountsPage.OpenForAutoTests1Async();
            var names = await customerAccountsPage.GetCustomerNamesAsync();
            names.Count.Should().BeGreaterThan(0, "The customer grid should contain at least one customer");
        }

        [Test]
        public async Task T03_CustomerAccounts_ClickCreateNewCustomer()
        {
            const string expectedTitle = "Create New Customer Account";
            var customerAccountsPage = new CustomerAccountsPage(Fixture.Page);
            await customerAccountsPage.OpenForEmptyTenantAsync();
            var companyDetailsPage = await customerAccountsPage.ClickCreateCustomerAccountBtnAsync();
            var actualTitle = await companyDetailsPage.GetPageTitleAsync();
            actualTitle.Should().Be(expectedTitle, "Create New Customer Account page should open after clicking Create Customer Account");
        }

        [Test]
        public async Task T04_CustomerAccounts_CreateNewCustomer()
        {
            var expectedName = $"AutoTests Customer {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            var customerRepository = new Neo4jRepository();

            var customerAccountsPage = new CustomerAccountsPage(Fixture.Page);
            await customerAccountsPage.OpenForAutoTests1Async();
            await Fixture.Page.WaitForTimeoutAsync(2000); // Wait for the grid to load and display the correct customer count
            var numberBefore = await customerAccountsPage.GetCustomerGridTotalRecordsAsync();
            TestContext.Out.WriteLine($"Number of customers before creating a new one: {numberBefore}");

            try
            {
                var companyDetailsPage = await customerAccountsPage.ClickCreateCustomerAccountBtnAsync();
                await companyDetailsPage.FillCustomerNameAsync(expectedName);
                await companyDetailsPage.SelectRandomIndustryOptionAsync();
                await companyDetailsPage.ClickSaveAsync();

                var customerAccountsPageAfterCreate = new CustomerAccountsPage(Fixture.Page);
                await Fixture.Page.WaitForTimeoutAsync(2000); // Wait for the grid to load and display the correct customer count
                var numberAfter = await customerAccountsPageAfterCreate.GetCustomerGridTotalRecordsAsync();
                TestContext.Out.WriteLine($"Number of customers after creating a new one: {numberAfter}");
                numberAfter.Should().Be(numberBefore + 1, "The customer count should increase after creating a new customer");

                var namesBeforeDeletion = await customerAccountsPageAfterCreate.GetCustomerNamesAsync();
                namesBeforeDeletion.Should().Contain(expectedName, "The new customer should be in the grid");
            }
            finally
            {
                await customerRepository.DeleteAutoTestCustomerAccountsAsync();
            }

            await Fixture.Page.ReloadAsync();
            await Fixture.Page.WaitForTimeoutAsync(2000); // Wait for the grid to load and display the correct customer count
            var numberAfterDeletion = await customerAccountsPage.GetCustomerGridTotalRecordsAsync();
            TestContext.Out.WriteLine($"Number of customers after deleting the test customer: {numberAfterDeletion}");
            numberAfterDeletion.Should().Be(numberBefore, "The customer count should be the same as before after deleting the test customer");

            var namesAfterDeletion = await customerAccountsPage.GetCustomerNamesAsync();
            namesAfterDeletion.Should().NotContain(expectedName, "The deleted customer should not be in the grid");
        }
    }
}
