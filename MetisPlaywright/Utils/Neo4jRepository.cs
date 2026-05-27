using Neo4j.Driver;

namespace MetisPlaywright.Utils
{
    /// <summary>
    /// Single entry point for Neo4j cleanup operations used by the test suite.
    /// All cypher queries are parameterized to prevent injection.
    /// </summary>
    public sealed class Neo4jRepository
    {
        public Task DeleteAssetTypeByNameAsync(string name) =>
            ExecuteAsync(
                "MATCH (n:AssetType) WHERE n.Name = $name DETACH DELETE n",
                new { name });

        public Task DeleteContextByNameAsync(string name) =>
            ExecuteAsync(
                "MATCH (n) WHERE n.Name = $name DETACH DELETE n",
                new { name });

        public Task DeleteAutoTestCustomerAccountsAsync() =>
            ExecuteAsync(
                """
                MATCH (n)
                WHERE (n:CustomerAccountEntity OR n:CustomerAccount)
                  AND n.Name STARTS WITH $prefix
                DETACH DELETE n
                """,
                new { prefix = "AutoTests" });

        public Task DeleteLabelByDisplayNameAsync(string displayName) =>
            ExecuteAsync(
                "MATCH (n) WHERE n.DisplayName = $displayName DETACH DELETE n",
                new { displayName });

        public Task DeleteNodeByNameAsync(string name) =>
            ExecuteAsync(
                "MATCH (n {Name: $name}) DETACH DELETE n",
                new { name });

        public Task DeleteResourceByNameAsync(string name) =>
            ExecuteAsync(
                "MATCH (n:Resource) WHERE n.Name = $name DETACH DELETE n",
                new { name });

        public Task SetResourceArchivedByNameAsync(string name) =>
            ExecuteAsync(
                "MATCH (n:Resource {Name: $name}) SET n.IsArchived = true RETURN n",
                new { name });

        public Task DeleteResourcesAutoTests1Async() =>
            ExecuteAsync(
                """
                MATCH (p:Person {FirstName: $firstName})-[r:HasResource]->(res:Resource)
                DELETE r
                """,
                new { firstName = Config.AutoTests1FirstName });

        public Task RemovePersonCostPerHourByFirstNameAsync(string firstName) =>
            ExecuteAsync(
                "MATCH (n:Person {FirstName: $firstName}) REMOVE n.CostPerHour RETURN n",
                new { firstName });

        public Task RemovePersonCostPerHourAutoTests1Async() =>
            RemovePersonCostPerHourByFirstNameAsync(Config.AutoTests1FirstName);

        public Task DeleteTemplateByNameAsync(string name) =>
            ExecuteAsync(
                "MATCH (n:Template) WHERE n.Name = $name DETACH DELETE n",
                new { name });

        private static async Task ExecuteAsync(string cypher, object parameters)
        {
            using var driver = GraphDatabase.Driver(
                Config.Neo4jConnectionStringTest,
                AuthTokens.Basic(Config.Neo4jUsernameTest, Config.Neo4jPasswordTest));

            await using var session = driver.AsyncSession();
            await using var tx = await session.BeginTransactionAsync();

            await tx.RunAsync(cypher, parameters);
            await tx.CommitAsync();
        }
    }
}
