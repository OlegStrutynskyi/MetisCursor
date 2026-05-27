using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class LabelsTests : BaseTest
    {
        [Test]
        public async Task T01_Labels_DefaultView()
        {
            const string expectedTitle = "Labels";

            var labelsPage = new LabelsPage(Fixture.Page);
            await labelsPage.OpenForAutoTests1Async();
            var actualTitle = await labelsPage.GetLabelsPageTitleAsync();

            actualTitle.Should().Be(expectedTitle, "Labels page title is not correct.");
        }

        [Test]
        public async Task T02_Labels_ClickCreateLabel()
        {
            var labelsPage = new LabelsPage(Fixture.Page);
            await labelsPage.OpenForAutoTests1Async();
            await labelsPage.ClickCreateLabelBtnAsync();
            await labelsPage.ExpectCreateLabelModalVisibleAsync();
        }

        [Test]
        public async Task T03_Labels_CreateLabelModal_View()
        {
            const string expectedTitle = "Create Label";

            var labelsPage = new LabelsPage(Fixture.Page);
            await labelsPage.OpenForAutoTests1Async();
            await labelsPage.ClickCreateLabelBtnAsync();

            var actualTitle = await labelsPage.GetCreateLabelTitleAsync();
            await labelsPage.ExpectCreateLabelControlsVisibleAsync();

            actualTitle.Should().Be(expectedTitle, "Create Label modal title is not correct.");
        }

        [Test]
        public async Task T04_Labels_CreateLabelModal_EmptyFields()
        {
            const string expectedNameError = "'Display Name' is required";

            var labelsPage = new LabelsPage(Fixture.Page);
            await labelsPage.OpenForAutoTests1Async();
            await labelsPage.ClickCreateLabelBtnAsync();
            await labelsPage.ClickCreateLabelSaveAsync();
            var nameError = await labelsPage.GetCreateLabelNameErrorAsync();

            nameError.Should().Contain(expectedNameError, "Name error message should be displayed when trying to save Label with empty Name.");
        }

        [Test]
        public async Task T05_Labels_CreateLabel()
        {
            const string expectedName = "AutoTests Label 1";
            var neo4jRepo = new Neo4jRepository();

            //ensure clean state in case a previous run left the Label behind
            await neo4jRepo.DeleteLabelByDisplayNameAsync(expectedName);

            try
            {
                var labelsPage = new LabelsPage(Fixture.Page);
                await labelsPage.OpenForAutoTests1Async();
                var numberBefore = await labelsPage.GetGridTotalRecordsAsync();
                await labelsPage.ClickCreateLabelBtnAsync();
                await labelsPage.FillCreateLabelNameAsync(expectedName);
                await labelsPage.ClickCreateLabelSaveAsync();

                var leftMenuPage = new LeftMenuPage(Fixture.Page);
                await leftMenuPage.ClickLabelsIconAsync();

                var labelsPage2 = new LabelsPage(Fixture.Page);
                var numberAfter = await labelsPage2.GetGridTotalRecordsAsync();
                var labelNames = await labelsPage2.GetLabelNamesAsync();

                numberAfter.Should().Be(numberBefore + 1, "Total number of Labels should increase by 1 after creating a new Label.");
                labelNames.Should().Contain(expectedName, "Newly created Label should be present in the grid.");
            }
            finally
            {
                await neo4jRepo.DeleteLabelByDisplayNameAsync(expectedName);
            }
        }
    }
}
