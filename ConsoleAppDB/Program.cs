using System;
using System.Data.SQLite;

namespace ConsoleAppDB
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection sqlite_conn = CreateConnection();
            CreateTable(sqlite_conn);
            //InsertData(sqlite_conn);
            ReadData(sqlite_conn);
        }
 
        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
 
            return sqlite_conn;
        }
 
        static void CreateTable(SQLiteConnection conn)
        {
            string CreatePersonTable = @"CREATE TABLE IF NOT EXISTS Person(id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                                     name TEXT, 
                                                     lastName TEXT, 
                                                     SSN_ID INTEGER,
                                                     FOREIGN KEY (SSN_ID) REFERENCES SSN(id) )";
            
            
            string CreateSSNTable = @"CREATE TABLE IF NOT EXISTS SSN(id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                                     number INTEGER, 
                                                     person_id INTEGER, 
                                                     FOREIGN KEY (person_id) REFERENCES Person(id))";
            
            
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = CreatePersonTable;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = CreateSSNTable;
            sqlite_cmd.ExecuteNonQuery();
        }
 
        static void InsertData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
           
            sqlite_cmd.CommandText = "INSERT INTO Person(name, lastName, SSN_ID) VALUES ('john', 'doe', NULL)";
            sqlite_cmd.ExecuteNonQuery();

            sqlite_cmd.CommandText = "INSERT INTO SSN(number, Person_id) VALUES (56565, 1);";
            sqlite_cmd.ExecuteNonQuery();
            
        }
 
        static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Person";
 
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            
            Console.WriteLine($"{sqlite_datareader.GetName(0), -3} {sqlite_datareader.GetName(1), -8} {sqlite_datareader.GetName(2), 8}");
            
            while (sqlite_datareader.Read())
            {
                Console.WriteLine($"{sqlite_datareader.GetInt32(0), -3} {sqlite_datareader.GetString(1), -8} {sqlite_datareader.GetString(2), 8}");
            }

            Console.WriteLine();

            SQLiteCommand sqlite_cmd1 = conn.CreateCommand();
            sqlite_cmd1.CommandText = "SELECT * FROM SSN";
 
            sqlite_datareader = sqlite_cmd1.ExecuteReader();
            
            Console.WriteLine($"{sqlite_datareader.GetName(0), -3} {sqlite_datareader.GetName(1), -8} ");
            
            while (sqlite_datareader.Read())
            {
                Console.WriteLine($"{sqlite_datareader.GetInt32(0), -3} {sqlite_datareader.GetInt32(1), -8}");
            }

            conn.Close();
        }
    }
}