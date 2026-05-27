using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class ResourceManagerTests : BaseTest
    {
        [Test]
        public async Task T01_ResourceManager_DefaultView()
        {
            const string expectedTitle = "Resource Manager";
            const string expectedMessage = "No resource records";

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForEmptyTenantAsync();
            await resourceManagerPage.ExpectDefaultTabsAndToolbarVisibleAsync();

            var actualTitle = await resourceManagerPage.GetPageTitleTextAsync();
            var actualMessage = await resourceManagerPage.GetGridEmptyMessageAsync();

            actualTitle.Should().Be(expectedTitle, "Resource Manager page title is not correct.");
            actualMessage.Should().Be(expectedMessage, "Grid empty message is not correct when there are no resources.");
        }

        [Test]
        public async Task T02_ResourceManager_ClickManageAssetTypes_OpensAssetTypesPage()
        {
            const string expectedTitle = "Asset Types";

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            var assetTypesPage = await resourceManagerPage.ClickManageAssetTypesBtnAsync();
            var actualTitle = await assetTypesPage.GetPageTitleTextAsync();
            actualTitle.Should().Be(expectedTitle, "Manage Asset Types should open the Asset Types page.");
        }

        [Test]
        public async Task T03_ResourceManager_ClickCreateResourceBtn()
        {
            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await resourceManagerPage.ClickCreateResourceBtnAsync();
            await resourceManagerPage.ExpectResourceSettingsModalVisibleAsync();
        }

        [Test]
        public async Task T04_ResourceManager_ResourceSettings_View()
        {
            const string expectedModalTitle = "Resource Settings";

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await resourceManagerPage.ClickCreateResourceBtnAsync();
            var actualModalTitle = await resourceManagerPage.GetResourceSettingsTitleAsync();
            await resourceManagerPage.ExpectResourceSettingsControlsVisibleAsync();

            actualModalTitle.Should().Be(expectedModalTitle, "Resource Settings modal title is not correct.");
        }

        [Test]
        public async Task T05_ResourceManager_ResourceSettings_TypeDropdownOptions()
        {
            var expectedOptions = new[] { "Asset", "Consumable", "Credential", "Knowledge", "Skill" };

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await resourceManagerPage.ClickCreateResourceBtnAsync();
            var options = await resourceManagerPage.GetResourceTypeOptionsAsync();

            options.Should().BeEquivalentTo(expectedOptions, "Type dropdown options are not correct.");
        }

        [Test]
        public async Task T06_ResourceManager_ResourceSettings_EmptyFields()
        {
            const string expectedNameError = "'Name' is required";
            const string expectedTypeError = "'Type' is required";

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await resourceManagerPage.ClickCreateResourceBtnAsync();
            await resourceManagerPage.ClickResourceSettingsSaveAsync();
            var nameError = await resourceManagerPage.GetResourceSettingsNameErrorAsync();
            var typeError = await resourceManagerPage.GetResourceSettingsTypeErrorAsync();

            nameError.Should().Contain(expectedNameError, "Name error message should indicate that the field is required.");
            typeError.Should().Contain(expectedTypeError, "Type error message should indicate that the field is required.");
        }

        [Test]
        public async Task T07_ResourceManager_CreateNewSkill()
        {
            await CreateResourceAndVerifyAsync("Autotests Skill 1", "Skill");
        }

        [Test]
        public async Task T08_ResourceManager_CreateNewCredential()
        {
            await CreateResourceAndVerifyAsync("Autotests Credential 1", "Credential");
        }

        [Test]
        public async Task T09_ResourceManager_CreateNewKnowledge()
        {
            await CreateResourceAndVerifyAsync("Autotests Knowledge 1", "Knowledge");
        }

        [Test]
        public async Task T10_ResourceManager_CreateNewConsumable()
        {
            await CreateResourceAndVerifyAsync(
                name: "Autotests Consumable 1",
                type: "Consumable",
                fillExtras: async page =>
                {
                    await page.FillResourceSettingsVolumeAsync("111");
                    await page.SelectUnitOfMeasureAsync("Foot");
                });
        }

        [Test]
        public async Task T11_ResourceManager_CreateNewAsset()
        {
            await CreateResourceAndVerifyAsync(
                name: "Autotests Asset 1",
                type: "Asset",
                fillExtras: page => page.SelectAssetTypeAsync("DONT DELETE Asset Type 1"));
        }

        [Test]
        public async Task T12_ResourceManager_AllTab()
        {
            const string expectedStatus = "Active";

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await resourceManagerPage.ClickAllTabAsync();

            var statuses = await resourceManagerPage.GetResourceStatusValuesAsync();
            statuses.Should().NotBeEmpty("All tab should show at least one resource in the grid.");
            statuses.Should().OnlyContain(s => string.Equals(s.Trim(), expectedStatus, StringComparison.Ordinal),
                $"Every visible row on the All tab should have Status '{expectedStatus}'.");
        }

        [Test]
        public async Task T13_ResourceManager_AssetTab()
        {
            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await AssertResourceTypeTabMatchesAllTabCountAsync(resourceManagerPage, () => resourceManagerPage.ClickAssetTabAsync(), "Asset");
        }

        [Test]
        public async Task T14_ResourceManager_ConsumableTab()
        {
            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await AssertResourceTypeTabMatchesAllTabCountAsync(resourceManagerPage, () => resourceManagerPage.ClickConsumableTabAsync(), "Consumable");
        }

        [Test]
        public async Task T15_ResourceManager_CredentialTab()
        {
            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await AssertResourceTypeTabMatchesAllTabCountAsync(resourceManagerPage, () => resourceManagerPage.ClickCredentialTabAsync(), "Credential");
        }

        [Test]
        public async Task T16_ResourceManager_KnowledgeTab()
        {
            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await AssertResourceTypeTabMatchesAllTabCountAsync(resourceManagerPage, () => resourceManagerPage.ClickKnowledgeTabAsync(), "Knowledge");
        }

        [Test]
        public async Task T17_ResourceManager_SkillTab()
        {
            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await AssertResourceTypeTabMatchesAllTabCountAsync(resourceManagerPage, () => resourceManagerPage.ClickSkillTabAsync(), "Skill");
        }

        [Test]
        public async Task T18_ResourceManager_ArchivedTab()
        {
            const string expectedEmptyMessage = "No resource records";
            const string expectedArchivedStatus = "Archived";

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();
            await resourceManagerPage.ClickArchivedTabAsync();

            var archivedCount = await resourceManagerPage.GetGridTotalRecordsAsync();

            if (archivedCount == 0)
            {
                var emptyMessage = await resourceManagerPage.GetGridEmptyMessageAsync();
                emptyMessage.Should().Be(expectedEmptyMessage,
                    "Archived tab with no rows should show the empty grid message.");
            }
            else
            {
                var names = await resourceManagerPage.GetResourceNamesAsync();
                var statuses = await resourceManagerPage.GetResourceStatusValuesAsync();

                names.Should().HaveCount(archivedCount,
                    "Name column should list the same number of rows as the paginator total on the Archived tab.");
                statuses.Should().HaveCount(archivedCount,
                    "Status column should list the same number of rows as the paginator total on the Archived tab.");
                statuses.Should().OnlyContain(s => string.Equals(s.Trim(), expectedArchivedStatus, StringComparison.Ordinal),
                    $"Every row on the Archived tab should have Status '{expectedArchivedStatus}'.");

                var pickedName = names[0].Trim();

                await resourceManagerPage.ClickAllTabAsync();
                var allNames = await resourceManagerPage.GetResourceNamesAsync();
                allNames.Should().NotContain(n => string.Equals(n.Trim(), pickedName, StringComparison.Ordinal),
                    "A resource shown on the Archived tab must not appear on the All tab.");
            }
        }

        [Test]
        public async Task T19_ResourceManager_Archive_Unarchive_Resource()
        {
            const string skill2Name = "DONT DELETE Skill 2 ARCHIVED";
            const string expectedEmptyMessage = "No resource records";
            const string expectedActiveStatus = "Active";
            const string expectedArchivedStatus = "Archived";

            await new Neo4jRepository().SetResourceArchivedByNameAsync(skill2Name);

            var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
            await resourceManagerPage.OpenForAutoTests1Async();

            await resourceManagerPage.ClickAllTabAsync();
            var namesOnAllInitially = await resourceManagerPage.GetResourceNamesAsync();
            namesOnAllInitially.Should().NotContain(n => RowNameEquals(n, skill2Name),
                "Skill2 should not appear on the All tab while it is archived.");

            await resourceManagerPage.ClickSkillTabAsync();
            var namesOnSkillInitially = await resourceManagerPage.GetResourceNamesAsync();
            namesOnSkillInitially.Should().NotContain(n => RowNameEquals(n, skill2Name),
                "Skill2 should not appear on the Skill tab while it is archived.");

            await resourceManagerPage.ClickArchivedTabAsync();
            var namesOnArchivedBefore = await resourceManagerPage.GetResourceNamesAsync();
            namesOnArchivedBefore.Should().Contain(n => RowNameEquals(n, skill2Name),
                "Skill2 should appear on the Archived tab before unarchive.");
            var skill2StatusOnArchivedBefore = await resourceManagerPage.GetSkill2StatusTextAsync();
            skill2StatusOnArchivedBefore.Should().Be(expectedArchivedStatus,
                "Skill2 should show Archived status on the Archived tab before unarchive.");

            await resourceManagerPage.ClickSkill2ThreeDotsUnarchiveAsync();

            var archivedCountAfterUnarchive = await resourceManagerPage.GetGridTotalRecordsAsync();
            if (archivedCountAfterUnarchive == 0)
            {
                var emptyMessageAfterUnarchive = await resourceManagerPage.GetGridEmptyMessageAsync();
                emptyMessageAfterUnarchive.Should().Be(expectedEmptyMessage,
                    "Archived tab with no rows after unarchive should show the empty grid message.");
            }
            else
            {
                var namesOnArchivedAfterUnarchive = await resourceManagerPage.GetResourceNamesAsync();
                namesOnArchivedAfterUnarchive.Should().NotContain(n => RowNameEquals(n, skill2Name),
                    "Skill2 should not appear on the Archived tab after unarchive when other archived rows remain.");
            }

            await resourceManagerPage.ClickAllTabAsync();
            var namesOnAllAfterUnarchive = await resourceManagerPage.GetResourceNamesAsync();
            namesOnAllAfterUnarchive.Should().Contain(n => RowNameEquals(n, skill2Name),
                "Skill2 should appear on the All tab after unarchive.");
            var skill2StatusOnAll = await resourceManagerPage.GetSkill2StatusTextAsync();
            skill2StatusOnAll.Should().Be(expectedActiveStatus,
                "Skill2 status on the All tab should be Active after unarchive.");

            await resourceManagerPage.ClickSkillTabAsync();
            var namesOnSkillAfterUnarchive = await resourceManagerPage.GetResourceNamesAsync();
            namesOnSkillAfterUnarchive.Should().Contain(n => RowNameEquals(n, skill2Name),
                "Skill2 should appear on the Skill tab after unarchive.");

            await resourceManagerPage.ClickSkill2ThreeDotsArchiveAsync();

            var namesOnSkillAfterArchive = await resourceManagerPage.GetResourceNamesAsync();
            namesOnSkillAfterArchive.Should().NotContain(n => RowNameEquals(n, skill2Name),
                "Skill2 should disappear from the Skill tab after archive.");

            await resourceManagerPage.ClickArchivedTabAsync();
            var namesOnArchivedEnd = await resourceManagerPage.GetResourceNamesAsync();
            namesOnArchivedEnd.Should().Contain(n => RowNameEquals(n, skill2Name),
                "Skill2 should appear on the Archived tab again after archive.");
        }

        private static bool RowNameEquals(string cellText, string expectedName) =>
            string.Equals(cellText.Trim(), expectedName, StringComparison.Ordinal);

        private async Task AssertResourceTypeTabMatchesAllTabCountAsync(
            ResourceManagerPage resourceManagerPage,
            Func<Task> clickTargetTabAsync,
            string expectedType)
        {
            const string expectedEmptyMessage = "No resource records";

            await resourceManagerPage.ClickAllTabAsync();
            var typesOnAllTab = await resourceManagerPage.GetResourceTypeValuesAsync();
            var matchCount = typesOnAllTab.Count(t => string.Equals(t.Trim(), expectedType, StringComparison.Ordinal));

            await clickTargetTabAsync();

            if (matchCount == 0)
            {
                var emptyMessage = await resourceManagerPage.GetGridEmptyMessageAsync();
                emptyMessage.Should().Be(expectedEmptyMessage,
                    $"{expectedType} tab with no matching resources on All should show the empty grid message.");
            }
            else
            {
                var typesOnTargetTab = await resourceManagerPage.GetResourceTypeValuesAsync();
                typesOnTargetTab.Should().HaveCount(matchCount,
                    $"{expectedType} tab should list the same number of rows as {expectedType} resources counted on the All tab.");
                typesOnTargetTab.Should().OnlyContain(t => string.Equals(t.Trim(), expectedType, StringComparison.Ordinal),
                    $"Every row on the {expectedType} tab should have Type '{expectedType}'.");
            }
        }

        private async Task CreateResourceAndVerifyAsync(
            string name,
            string type,
            Func<ResourceManagerPage, Task>? fillExtras = null)
        {
            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.DeleteResourceByNameAsync(name);

            try
            {
                var resourceManagerPage = new ResourceManagerPage(Fixture.Page);
                await resourceManagerPage.OpenForAutoTests1Async();
                var numberBefore = await resourceManagerPage.GetGridTotalRecordsAsync();

                await resourceManagerPage.ClickCreateResourceBtnAsync();
                await resourceManagerPage.FillResourceSettingsNameAsync(name);
                await resourceManagerPage.SelectResourceTypeAsync(type);
                if (fillExtras is not null)
                {
                    await fillExtras(resourceManagerPage);
                }
                await resourceManagerPage.ClickResourceSettingsSaveAsync();

                var numberAfter = await resourceManagerPage.GetGridTotalRecordsAsync();
                var names = await resourceManagerPage.GetResourceNamesAsync();

                numberAfter.Should().Be(numberBefore + 1, "Total records should increase by 1 after creating a new resource.");
                names.Should().Contain(name, "Created resource should be visible in the grid with the correct name.");
            }
            finally
            {
                await neo4jRepo.DeleteResourceByNameAsync(name);
            }
        }
    }
}
