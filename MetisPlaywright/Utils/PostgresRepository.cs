using Npgsql;

namespace MetisPlaywright.Utils
{
    public sealed class PostgresRepository
    {
        public Task DeleteCompanyByAdminEmailAsync(string adminEmail)
        {
            const string sql = """
                DELETE FROM public."Chats"                       WHERE "CompanyId" = (SELECT "CompanyId" FROM public."Companies" WHERE "SystemAdminEmail" = @email);
                DELETE FROM public."Files"                       WHERE "CompanyId" = (SELECT "CompanyId" FROM public."Companies" WHERE "SystemAdminEmail" = @email);
                DELETE FROM schedule."AvailabilitySettings"      WHERE "CompanyId" = (SELECT "CompanyId" FROM public."Companies" WHERE "SystemAdminEmail" = @email);
                DELETE FROM schedule."UnavailabilitySettings"    WHERE "CompanyId" = (SELECT "CompanyId" FROM public."Companies" WHERE "SystemAdminEmail" = @email);
                DELETE FROM public."UserPermissions"             WHERE "CompanyId" = (SELECT "CompanyId" FROM public."Companies" WHERE "SystemAdminEmail" = @email);
                DELETE FROM public."Companies"                   WHERE "SystemAdminEmail" = @email;
                DELETE FROM public."Users"                       WHERE "UserName"         = @email;
                """;

            return ExecuteNonQueryAsync(sql, "email", adminEmail);
        }

        public Task DeleteUserByEmailAsync(string email) =>
            ExecuteNonQueryAsync(
                "DELETE FROM public.\"Users\" WHERE \"Email\" = @email",
                "email",
                email);

        private static async Task ExecuteNonQueryAsync(string sql, string parameterName, object parameterValue)
        {
            await using var connection = new NpgsqlConnection(Config.PostgreSqlConnectionStringTest);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue(parameterName, parameterValue);
            await command.ExecuteNonQueryAsync();
        }
    }
}
