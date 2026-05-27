namespace MetisPlaywright.Utils
{
    public static class Config
    {
        // Application URLs
        public const string BaseUrl = "https://app-web-metis-test-uksouth-3ijrwlqbxpdr6.azurewebsites.net/";
        public const string BaseUrlAPI = "https://app-api-metis-test-uksouth-3ijrwlqbxpdr6.azurewebsites.net/api/";

        // Default test credentials (positive flow)
        public const string CorrectPassword = "Test123!";

        // Negative-flow credentials
        public const string InvalidEmail = "aaa.com";
        public const string IncorrectEmail = "aaa@example.com";
        public const string IncorrectPassword = "aaa123!";

        // Brand-new autotests user (used by both /setup flow and new-person creation)
        public const string NewAutotestsUser = "metis_autotests_new@endtest-mail.io";
        public const string CorrectEmailNewCompany1 = NewAutotestsUser;

        // Pre-seeded "AutoTests1" tenant fixtures
        public const string CorrectEmailAutoTests1 = "metis_autotests1@endtest-mail.io";
        public const string AutoTests1FirstName = "AutoTestUserOne";
        public const string AutoTests1LastName = "AutoTestLastName";
        public const string AutoTests1Company = "AutoTestCompanyOne";

        // Pre-seeded "AutoTests2" tenant fixtures
        public const string CorrectEmailAutoTests2 = "metis_autotests2@endtest-mail.io";
        public const string AutoTests2FirstName = "AutoTestUserTwo";
        public const string AutoTests2LastName = "AutoTestLastName";
        public const string AutoTests2Company = "AutoTestCompanyTwo";

        public const string DefaultCoreLabel = "Job";

        // Pre-seeded empty tenant
        public const string CorrectEmailEmptyAutoTests1 = "metis_autotests_empty@endtest-mail.io";
        public const string AutoTestsEmptyFirstName = "AutoTestUserEmpty";
        public const string AutoTestsEmptyLastName = "AutoTestLastName";
        public const string AutoTestsEmptyCompany = "AutoTestCompanyEmpty";

        // Stable domain fixtures used by assertions (must not be deleted by cleanup jobs)
        public const string AutoTestsCustomer1 = "DONT DELETE Customer 1";
        public const string AutoTestsContext1 = "DONT DELETE Context 1";
        public const string AutoTestsContext1Description = "This is a Context created by an automated test.";
        public const string AutoTestsAssetType1 = "DONT DELETE Asset Type 1";
        public const string AutoTestsSkill1 = "DONT DELETE Skill 1";
        public const string AutoTestsCredential1 = "DONT DELETE Credential 1";
        public const string AutoTestsKnowledge1 = "DONT DELETE Knowledge 1";
        public const string AutoTestsAsset1 = "DONT DELETE Asset 1";
        public const string AutoTestsConsumable1 = "DONT DELETE Consumable 1";
        public const string AutoTestsConsumableVolume = "100";
        public const string AutoTestsConsumableUnitOfMeasure = "Foot";
        public const string AutoTestsSkill2Archived = "DONT DELETE Skill 2 ARCHIVED";

        // Databases
        public const string Neo4jConnectionStringTest = "neo4j+s://4f1d2c8b.databases.neo4j.io";
        public const string Neo4jUsernameTest = "neo4j";
        public const string Neo4jPasswordTest = "0hfQ_UxUfOvesJ4Z7RNxFGjSFA2pdd1Nov1iVC6zAwY";
        public const string PostgreSqlConnectionStringTest = "Server=psql-metis-test-uksouth-3ijrwlqbxpdr6.postgres.database.azure.com;Database=postgres;Port=5432;User Id=metispostgresadmin;Password=nQTp60vGzRzkNUsCQJXGrNn5ufiWI652;Ssl Mode=Prefer;";
    }
}
