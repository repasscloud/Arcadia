using System;
using System.IO;
using System.Linq;
using Npgsql;

namespace PgCsvInserter
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = string.Empty;
            string user = string.Empty;
            string password = string.Empty;
            string database = string.Empty;
            string csvFilePath = string.Empty;
            int version = 1; // Default version

            // Parse command-line arguments
            foreach (var arg in args)
            {
                if (arg.StartsWith("--host="))
                    host = arg.Substring("--host=".Length);
                else if (arg.StartsWith("--user="))
                    user = arg.Substring("--user=".Length);
                else if (arg.StartsWith("--pass="))
                    password = arg.Substring("--pass=".Length);
                else if (arg.StartsWith("--db="))
                    database = arg.Substring("--db=".Length);
                else if (arg.StartsWith("--csv="))
                    csvFilePath = arg.Substring("--csv=".Length);
                else if (arg.StartsWith("--version="))
                    int.TryParse(arg.Substring("--version=".Length), out version);
            }

            // Validate required parameters
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(database))
            {
                Console.WriteLine("Usage: --host=\"xyz\" --user=\"xyz\" --pass=\"xyz\" --db=\"xyz\" --csv=\"path_to_csv_file\" [--version=i]");
                return;
            }

            string connectionString = $"Host={host};Username={user};Password={password};Database={database}";
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string tableName = $"ctaintents_{currentDate}";

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                Console.WriteLine("Connected to the database.");

                // 1. Create IntentLabel table if it doesn't exist
                string createIntentLabelTable = @"
                    CREATE TABLE IF NOT EXISTS IntentLabel (
                        Id SERIAL PRIMARY KEY,
                        Intent TEXT UNIQUE NOT NULL,
                        Version INT NOT NULL
                    );";
                ExecuteNonQuery(connection, createIntentLabelTable);
                Console.WriteLine("Checked/Created IntentLabel table.");

                // 2. Drop and recreate ctaintents_YYYYMMDD table
                string dropTable = $@"DROP TABLE IF EXISTS {tableName};";
                string createCtaIntentsTable = $@"
                    CREATE TABLE {tableName} (
                        Id SERIAL PRIMARY KEY,
                        ""Text"" TEXT NOT NULL,
                        Label INT NOT NULL REFERENCES IntentLabel(Id)
                    );";

                string createUniqueIndex = $@"
                    CREATE UNIQUE INDEX {tableName}_text_ux ON {tableName}(""Text"");";

                ExecuteNonQuery(connection, dropTable);
                ExecuteNonQuery(connection, createCtaIntentsTable);
                ExecuteNonQuery(connection, createUniqueIndex);
                Console.WriteLine($"Dropped and recreated table {tableName} with unique index.");

                // 3. Process the CSV file
                if (!string.IsNullOrEmpty(csvFilePath) && File.Exists(csvFilePath))
                {
                    var csvData = File.ReadAllLines(csvFilePath)
                                      .Skip(1) // Skip header
                                      .Select(line => line.Split('\t'))
                                      .Where(fields => fields.Length >= 2)
                                      .Select(fields => new { Text = fields[0].Trim(), Label = fields[1].Trim() });

                    foreach (var entry in csvData.Select(x => x.Label).Distinct())
                    {
                        // Check if the label exists in IntentLabel, if not, insert it
                        string checkLabelQuery = $"SELECT COUNT(*) FROM IntentLabel WHERE Intent = @Intent";
                        using var checkCommand = new NpgsqlCommand(checkLabelQuery, connection);
                        checkCommand.Parameters.AddWithValue("@Intent", entry);
                        long count = (long)checkCommand.ExecuteScalar()!;

                        if (count == 0)
                        {
                            string insertLabelQuery = "INSERT INTO IntentLabel (Intent, Version) VALUES (@Intent, @Version)";
                            using var insertCommand = new NpgsqlCommand(insertLabelQuery, connection);
                            insertCommand.Parameters.AddWithValue("@Intent", entry);
                            insertCommand.Parameters.AddWithValue("@Version", version);
                            insertCommand.ExecuteNonQuery();
                            Console.WriteLine($"Inserted new label: {entry} with version {version}");
                        }
                    }

                    // 4. Insert data from CSV into ctaintents_YYYYMMDD table
                    foreach (var entry in csvData)
                    {
                        string insertDataQuery = $@"
                            INSERT INTO {tableName} (""Text"", Label)
                            VALUES (@Text, (SELECT Id FROM IntentLabel WHERE Intent = @Label))
                            ON CONFLICT DO NOTHING;";

                        using var insertDataCommand = new NpgsqlCommand(insertDataQuery, connection);
                        insertDataCommand.Parameters.AddWithValue("@Text", entry.Text);
                        insertDataCommand.Parameters.AddWithValue("@Label", entry.Label);
                        insertDataCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine("CSV data successfully inserted into the database.");
                }
                else
                {
                    Console.WriteLine("CSV file not found or not specified.");
                }

                connection.Close();
                Console.WriteLine("Connection closed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Helper method to execute non-query commands
        static void ExecuteNonQuery(NpgsqlConnection connection, string sql)
        {
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
