using System;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace minefine
{
    public sealed class DatabaseDataHandler
    {
        private static DatabaseDataHandler instance = null;
        private static readonly object padlock = new object();
        string dbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "Ores.db3");

        SqliteConnection connection;
        ObservableCollection<Ore> oreCount = new ObservableCollection<Ore>();
        int experience;
        
        DatabaseDataHandler()
        {
            
            if (!File.Exists(dbPath))
            {
                Console.WriteLine("Creating database");
                // Need to create the database before seeding it with some data
                SqliteConnection.CreateFile(dbPath);

                var commands = new[] {
                "CREATE TABLE Exp (experience INTEGER);",
                "INSERT INTO Exp (experience) VALUES ('0')",
                "CREATE TABLE Ores (oreName VARCHAR(30), oreCount INTEGER);",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Copper_Ore', '0')",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Tin_Ore', '0')",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Iron_Ore', '0')",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Silver_Ore', '0')",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Coal_Ore', '0')",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Mithril_Ore', '0')",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Adamantite_Ore', '0')",
                "INSERT INTO Ores (oreName, oreCount) VALUES ('Runite', '0')"};
                connection = new SqliteConnection("Data Source=" + dbPath);
                connection.Open();

                foreach (var command in commands)
                {
                    executeCommand(command);
                }
                connection.Close();
            }
            else
            {
                Console.WriteLine("Database already exists");
                // Open connection to existing database file
                connection = new SqliteConnection("Data Source=" + dbPath);
                
            }
        }


        public static DatabaseDataHandler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DatabaseDataHandler();
                    }
                    return instance;
                }
            }
        }
        
        /// <summary>
        /// Execute a sql command that isn't supposed to return anything, ex: UPDATE, INSERT, CREATE etc (no connection opening or closing)
        /// </summary>
        /// <param name="query">The sql Query to execute</param>
        public void executeCommand(string query)
        {
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = query;
                var r = contents.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Gets the stored data from the local database (prob vahetan nime ära millalgi xd)
        /// </summary>
        public void getDataTest()
        {
            connection.Open();
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = "SELECT * from Ores";
                var r = contents.ExecuteReader();
                Console.WriteLine("Reading data");
                while (r.Read()) { 
                    Console.WriteLine("oreName = {0} oreCount = {1}",r["oreName"].ToString(), Convert.ToInt32(r["oreCount"].ToString()));
                    oreCount.Add(new Ore(r["oreName"].ToString(), r["oreName"].ToString(), Convert.ToInt32(r["oreCount"].ToString())));
                }
            }
            connection.Close();
        }
        /// <summary>
        /// Gets the stored data from the local database (prob vahetan nime ära millalgi xd)
        /// </summary>
        public int getExp()
        {
            connection.Open();
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = "SELECT experience from Exp";
                var r = contents.ExecuteReader();
                Console.WriteLine("Reading data");
                while (r.Read())
                {
                    Console.WriteLine("experience = {0}", Convert.ToInt32(r["experience"].ToString()));
                    experience = Convert.ToInt32(r["experience"].ToString());
                }
            }
            connection.Close();
            return experience;
        }


        /// <summary>
        /// returns: Basically a list of Ore() Classes
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Ore> getObservable()
        {
            return oreCount;
        }
        /// <summary>
        /// Saves the Ore data to the local database
        /// </summary>
        public void saveOreData()
        {
            connection.Open();
            foreach (var item in oreCount)
            {
                executeCommand("UPDATE Ores SET oreCount =" + item.OreCount + " WHERE oreName = '" + item.Name + "';");
            }
            connection.Close();
        }
        /// <summary>
        /// Saves the Exp data to the local database
        /// </summary>
        public void saveExpData(int exp)
        {
            connection.Open();
            executeCommand("UPDATE Exp SET experience ='" + exp + "' ;");
            connection.Close();
        }

    }
}
