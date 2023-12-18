using System;
using System.IO;
using System.Linq;
using Npgsql;

namespace SqlFileExecution
{
    //TO ME AS THE DEVELOOPER, THIS CODE BASICALLY STILL ON PLACEHOLDER STAGE.
    //I NEED TO CHECK THIS CODE AGAIN AND PROBABLY FIX THIS CODE.
    //DO NOT USE THIS PROGRAM YET.
    //DO NOT USE THIS PROGRAM YET.
    //DO NOT USE THIS PROGRAM YET.
    //DO NOT USE THIS PROGRAM YET.
    //DO NOT USE THIS PROGRAM YET.
    //DO NOT USE THIS PROGRAM YET.
    //DO NOT USE THIS PROGRAM YET.
    //DO NOT USE THIS PROGRAM YET.

    class Program
    {
        static void Main(string[] args)
        {
            string accountConnectionString = "Host=myserver;Username=myuser;Password=mypassword;Database=mydatabase";
            string historyConnectionString = "Host=myserver;Username=myuser;Password=mypassword;Database=MigrationHistory";

            string folderPath = @"path\to\your\folder"; // Replace with your folder path

            try
            {
                // Check if MigrationHistory database exists, if not, create it
                using (NpgsqlConnection createConn = new NpgsqlConnection(accountConnectionString))
                {
                    createConn.Open();
                    NpgsqlCommand createDbCommand = new NpgsqlCommand("CREATE DATABASE IF NOT EXISTS MigrationHistory", createConn);
                    createDbCommand.ExecuteNonQuery();
                }

                string[] sqlFiles = Directory.GetFiles(folderPath, "*.sql");

                using (NpgsqlConnection connection = new NpgsqlConnection(accountConnectionString))
                using (NpgsqlConnection historyConnection = new NpgsqlConnection(historyConnectionString))
                {
                    connection.Open();
                    historyConnection.Open();

                    foreach (string sqlFilePath in sqlFiles)
                    {
                        string fileName = Path.GetFileName(sqlFilePath);

                        // Check if the file has been executed already
                        string checkQuery = "SELECT COUNT(*) FROM ExecutionHistory WHERE FileName = @fileName";
                        using (NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, historyConnection))
                        {
                            checkCommand.Parameters.AddWithValue("@fileName", fileName);
                            int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                            if (count == 0)
                            {
                                string sqlContent = File.ReadAllText(sqlFilePath);

                                using (NpgsqlCommand command = new NpgsqlCommand(sqlContent, connection))
                                {
                                    command.ExecuteNonQuery();
                                    Console.WriteLine($"Executed: {sqlFilePath}");

                                    // Log execution details
                                    string logQuery = "INSERT INTO ExecutionHistory (FileName, TableName, DatabaseName, ExecutionDate) VALUES (@fileName, @tableName, @dbName, @executionDate)";
                                    using (NpgsqlCommand logCommand = new NpgsqlCommand(logQuery, historyConnection))
                                    {
                                        logCommand.Parameters.AddWithValue("@fileName", fileName);
                                        logCommand.Parameters.AddWithValue("@tableName", "YourTableNameHere"); // Adjust as per your table name
                                        logCommand.Parameters.AddWithValue("@dbName", "YourDatabaseNameHere"); // Adjust as per your database name
                                        logCommand.Parameters.AddWithValue("@executionDate", DateTime.Now);
                                        logCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Skipped: {sqlFilePath} (Already executed)");
                            }
                        }
                    }
                }

                Console.WriteLine("All SQL files executed and logged successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
