using FluentAssertions;
using MetisPlaywright.Pages;
using MetisPlaywright.Utils;

namespace MetisPlaywright.Tests
{
    public class PersonOverviewTests : BaseTest
    {
        [Test]
        public async Task T01_PersonOverview_DefaultView()
        {
            var expectedAutoTests1Name = $"{Config.AutoTests1FirstName} {Config.AutoTests1LastName}";

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();

            var actualName = await personOverviewPage.GetPersonFirstLastNamesTextAsync();
            actualName.Trim().Should().Be(expectedAutoTests1Name, "Person name on the overview is not correct.");

            await personOverviewPage.ExpectDefaultViewVisibleAsync();
            await personOverviewPage.ExpectContextsDefaultViewVisibleAsync();
        }

        [Test]
        public async Task T02_PersonOverview_Resources_DefaultView()
        {
            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ExpectResourcesDefaultViewVisibleAsync();
        }

        [Test]
        public async Task T03_PersonOverview_PersonalDetails_DefaultView()
        {
            var expectedName = $"{Config.AutoTests1FirstName} {Config.AutoTests1LastName}";
            const string expectedEmergencyInfo = "-";
            const string expectedCostPerHour = "Not set";

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickPersonalDetailsTabAsync();

            var actualName = await personOverviewPage.GetNameTextAsync();
            var actualEmail = await personOverviewPage.GetEmailTextAsync();
            var actualEmergencyInfo = await personOverviewPage.GetEmergencyInfoTextAsync();
            var actualCostPerHour = await personOverviewPage.GetCostPerHourTextAsync();

            actualName.Trim().Should().Be(expectedName, "Personal Details name is not correct.");
            actualEmail.Trim().Should().Be(Config.CorrectEmailAutoTests1, "Personal Details email is not correct.");
            actualEmergencyInfo.Trim().Should().Be(expectedEmergencyInfo, "Personal Details emergency info is not correct.");
            actualCostPerHour.Trim().Should().Be(expectedCostPerHour, "Personal Details cost per hour is not correct.");
        }

        [Test]
        public async Task T04_PersonOverview_3Dots_Options()
        {
            const string expectedPersonSettingsTitle = "Person Settings";
            const string expectedChangePasswordTitle = "Change Password";

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickPersonThreeDotsAsync();
            await personOverviewPage.ExpectThreeDotsMenuOptionsVisibleAsync();

            await personOverviewPage.ClickEditPersonAsync();
            await personOverviewPage.ExpectPersonSettingsModalVisibleAsync();
            var actualPersonSettingsTitle = await personOverviewPage.GetPersonSettingsTitleAsync();
            actualPersonSettingsTitle.Trim().Should().Be(expectedPersonSettingsTitle, "Person Settings modal title is not correct.");

            await personOverviewPage.ClickCancelBtnAsync();

            await personOverviewPage.ClickPersonThreeDotsAsync();
            await personOverviewPage.ClickChangePasswordAsync();
            var actualChangePasswordTitle = await personOverviewPage.GetModalTitleTextAsync();
            actualChangePasswordTitle.Trim().Should().Be(expectedChangePasswordTitle, "Change Password modal title is not correct.");

            await personOverviewPage.ClickCancelBtnAsync();

            await personOverviewPage.ClickPersonThreeDotsAsync();
            await personOverviewPage.ClickEditScheduleAsync();

            var scheduleOverviewPage = new ScheduleOverviewPage(Fixture.Page);
            await scheduleOverviewPage.ExpectOpenedAsync();

            await Fixture.Page.GoBackAsync();

            await personOverviewPage.ClickPersonThreeDotsAsync();
            await personOverviewPage.ClickNotificationSettingsAsync();

            var notificationSettingsPage = new NotificationSettingsPage(Fixture.Page);
            await notificationSettingsPage.ExpectOpenedAsync();
        }

        [Test]
        public async Task T05_PersonOverview_ClickManageScheduleBtn()
        {
            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickManageScheduleBtnAsync();

            var scheduleOverviewPage = new ScheduleOverviewPage(Fixture.Page);
            await scheduleOverviewPage.ExpectOpenedAsync();
        }

        [Test]
        public async Task T06_PersonOverview_Resources_ClickAddResources()
        {
            const string expectedModalTitle = "Manage Resources";
            var expectedName = $"{Config.AutoTests1FirstName} {Config.AutoTests1LastName}";

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();

            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();
            var actualTitle = await personOverviewPage.GetManageResourcesTitleAsync();
            actualTitle.Trim().Should().Be(expectedModalTitle, "Manage Resources modal title is not correct.");

            var actualName = await personOverviewPage.GetManageResourcesNameValueAsync();
            actualName.Trim().Should().Be(expectedName, "Manage Resources name input value is not correct.");
            await personOverviewPage.ExpectManageResourcesNameInputDisabledAsync();

            await personOverviewPage.ExpectManageResourcesAddModalControlsVisibleAsync();
        }

        [Test]
        public async Task T07_PersonOverview_ManageResources_EmptyFields()
        {
            const string expectedTypeError = "'Type' is required.";
            const string expectedResourceError = "'Resource' must not be equal to '00000000-0000-0000-0000-000000000000'.";

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();
            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();

            await personOverviewPage.ClickManageResourcesSaveAsync();

            var actualTypeError = await personOverviewPage.GetManageResourcesTypeErrorAsync();
            var actualResourceError = await personOverviewPage.GetManageResourcesResourceErrorAsync();

            actualTypeError.Trim().Should().Be(expectedTypeError, "Manage Resources Type error is not correct.");
            actualResourceError.Trim().Should().Be(expectedResourceError, "Manage Resources Resource error is not correct.");
        }

        [Test]
        public async Task T08_PersonOverview_Resources_AddSkill()
        {
            const string resourceType = "Skill";
            const string resourceLevel = "Level 9";

            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.DeleteResourcesAutoTests1Async();

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();
            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();

            await personOverviewPage.SelectManageResourcesTypeAsync(resourceType);
            await personOverviewPage.SelectManageResourcesResourceAsync(Config.AutoTestsSkill1);
            await personOverviewPage.SelectManageResourcesLevelAsync(resourceLevel);
            await personOverviewPage.ClickManageResourcesSaveAsync();

            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await personOverviewPage.ClickSkillTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await neo4jRepo.DeleteResourcesAutoTests1Async();
        }

        [Test]
        public async Task T09_PersonOverview_Resources_AddKnowledge()
        {
            const string resourceType = "Knowledge";

            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.DeleteResourcesAutoTests1Async();

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();
            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();

            await personOverviewPage.SelectManageResourcesTypeAsync(resourceType);
            await personOverviewPage.SelectManageResourcesResourceAsync(Config.AutoTestsKnowledge1);
            await personOverviewPage.ClickManageResourcesSaveAsync();

            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await personOverviewPage.ClickKnowledgeTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await neo4jRepo.DeleteResourcesAutoTests1Async();
        }

        [Test]
        public async Task T10_PersonOverview_Resources_AddCredential()
        {
            const string resourceType = "Credential";
            const string resourceExpiry = "31/12/2027";

            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.DeleteResourcesAutoTests1Async();

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();
            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();

            await personOverviewPage.SelectManageResourcesTypeAsync(resourceType);
            await personOverviewPage.SelectManageResourcesResourceAsync(Config.AutoTestsCredential1);
            await personOverviewPage.FillManageResourcesExpiryAsync(resourceExpiry);
            await personOverviewPage.ClickManageResourcesSaveAsync();

            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await personOverviewPage.ClickCredentialTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await neo4jRepo.DeleteResourcesAutoTests1Async();
        }

        [Test]
        public async Task T11_PersonOverview_Resources_Skill_Archive_Unarchive()
        {
            const string resourceType = "Skill";
            const string resourceLevel = "Level 9";
            const string expectedEmptyMessage = "No resource records";

            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.DeleteResourcesAutoTests1Async();

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();
            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();

            await personOverviewPage.SelectManageResourcesTypeAsync(resourceType);
            await personOverviewPage.SelectManageResourcesResourceAsync(Config.AutoTestsSkill1);
            await personOverviewPage.SelectManageResourcesLevelAsync(resourceLevel);
            await personOverviewPage.ClickManageResourcesSaveAsync();

            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await personOverviewPage.ClickResourceThreeDotsArchiveAsync(Config.AutoTestsSkill1);

            await personOverviewPage.ClickResourcesAllTabAsync();
            await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await personOverviewPage.ClickSkillTabAsync();
            await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await personOverviewPage.ClickResourcesArchivedTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await personOverviewPage.ClickResourceThreeDotsUnarchiveAsync(Config.AutoTestsSkill1);

            var archivedCountAfterUnarchive = await personOverviewPage.GetGridTotalRecordsAsync();
            if (archivedCountAfterUnarchive == 0)
            {
                var emptyMessage = await personOverviewPage.GetGridEmptyMessageAsync();
                emptyMessage.Should().Be(expectedEmptyMessage,
                    "Archived tab with no rows after unarchive should show the empty grid message.");
            }
            else
            {
                await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);
            }

            await personOverviewPage.ClickResourcesAllTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await personOverviewPage.ClickSkillTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsSkill1);

            await neo4jRepo.DeleteResourcesAutoTests1Async();
        }

        [Test]
        public async Task T12_PersonOverview_Resources_Credential_Archive_Unarchive()
        {
            const string resourceType = "Credential";
            const string resourceExpiry = "31/12/2027";
            const string expectedEmptyMessage = "No resource records";

            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.DeleteResourcesAutoTests1Async();

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();
            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();

            await personOverviewPage.SelectManageResourcesTypeAsync(resourceType);
            await personOverviewPage.SelectManageResourcesResourceAsync(Config.AutoTestsCredential1);
            await personOverviewPage.FillManageResourcesExpiryAsync(resourceExpiry);
            await personOverviewPage.ClickManageResourcesSaveAsync();

            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await personOverviewPage.ClickResourceThreeDotsArchiveAsync(Config.AutoTestsCredential1);

            await personOverviewPage.ClickResourcesAllTabAsync();
            await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await personOverviewPage.ClickCredentialTabAsync();
            await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await personOverviewPage.ClickResourcesArchivedTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await personOverviewPage.ClickResourceThreeDotsUnarchiveAsync(Config.AutoTestsCredential1);

            var archivedCountAfterUnarchive = await personOverviewPage.GetGridTotalRecordsAsync();
            if (archivedCountAfterUnarchive == 0)
            {
                var emptyMessage = await personOverviewPage.GetGridEmptyMessageAsync();
                emptyMessage.Should().Be(expectedEmptyMessage,
                    "Archived tab with no rows after unarchive should show the empty grid message.");
            }
            else
            {
                await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);
            }

            await personOverviewPage.ClickResourcesAllTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await personOverviewPage.ClickCredentialTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsCredential1);

            await neo4jRepo.DeleteResourcesAutoTests1Async();
        }

        [Test]
        public async Task T13_PersonOverview_Resources_Knowledge_Archive_Unarchive()
        {
            const string resourceType = "Knowledge";
            const string expectedEmptyMessage = "No resource records";

            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.DeleteResourcesAutoTests1Async();

            var personOverviewPage = new PersonOverviewPage(Fixture.Page);
            await personOverviewPage.OpenForAutoTests1Async();
            await personOverviewPage.ClickResourcesTabAsync();
            await personOverviewPage.ClickAddResourceBtnAsync();
            await personOverviewPage.ExpectManageResourcesModalVisibleAsync();

            await personOverviewPage.SelectManageResourcesTypeAsync(resourceType);
            await personOverviewPage.SelectManageResourcesResourceAsync(Config.AutoTestsKnowledge1);
            await personOverviewPage.ClickManageResourcesSaveAsync();

            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await personOverviewPage.ClickResourceThreeDotsArchiveAsync(Config.AutoTestsKnowledge1);

            await personOverviewPage.ClickResourcesAllTabAsync();
            await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await personOverviewPage.ClickKnowledgeTabAsync();
            await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await personOverviewPage.ClickResourcesArchivedTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await personOverviewPage.ClickResourceThreeDotsUnarchiveAsync(Config.AutoTestsKnowledge1);

            var archivedCountAfterUnarchive = await personOverviewPage.GetGridTotalRecordsAsync();
            if (archivedCountAfterUnarchive == 0)
            {
                var emptyMessage = await personOverviewPage.GetGridEmptyMessageAsync();
                emptyMessage.Should().Be(expectedEmptyMessage,
                    "Archived tab with no rows after unarchive should show the empty grid message.");
            }
            else
            {
                await personOverviewPage.ExpectResourceNotDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);
            }

            await personOverviewPage.ClickResourcesAllTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await personOverviewPage.ClickKnowledgeTabAsync();
            await personOverviewPage.ExpectResourceDisplayedInResourcesGridAsync(Config.AutoTestsKnowledge1);

            await neo4jRepo.DeleteResourcesAutoTests1Async();
        }

        [Test]
        public async Task T14_PersonOverview_PersonDetails_CostPerHour()
        {
            const string expectedNotSet = "Not set";
            const string expectedCostTen = "10.00";
            const string expectedCostTwentyTwo = "22.22";

            var neo4jRepo = new Neo4jRepository();
            await neo4jRepo.RemovePersonCostPerHourAutoTests1Async();

            try
            {
                var personOverviewPage = new PersonOverviewPage(Fixture.Page);
                await personOverviewPage.OpenForAutoTests1Async();
                await personOverviewPage.ClickPersonalDetailsTabAsync();

                var actualCostNotSet = await personOverviewPage.GetCostPerHourTextAsync();
                actualCostNotSet.Trim().Should().Be(expectedNotSet, "Cost Per Hour should be Not set initially.");

                await personOverviewPage.FillCostPerHourAsync(expectedCostTen);
                await personOverviewPage.ClickCostPerHourSaveBtnAsync();
                await personOverviewPage.ExpectCostPerHourTextAsync($"£{expectedCostTen}");

                await personOverviewPage.FillCostPerHourAsync(expectedCostTwentyTwo);
                await personOverviewPage.ClickCostPerHourSaveBtnAsync();
                await personOverviewPage.ExpectCostPerHourTextAsync($"£{expectedCostTwentyTwo}");

                await personOverviewPage.ClearCostPerHourInputAsync();
                await personOverviewPage.ClickCostPerHourSaveBtnAsync();
                await personOverviewPage.ExpectCostPerHourTextAsync(expectedNotSet);
            }
            finally
            {
                await neo4jRepo.RemovePersonCostPerHourAutoTests1Async();
            }
        }
    }
}
