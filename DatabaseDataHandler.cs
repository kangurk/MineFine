using System;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MineFine
{
    public sealed class DatabaseDataHandler
    {
        private static DatabaseDataHandler instance = null;
        private static readonly object padlock = new object();
        string dbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "Ores.db3");
        Dictionary<string, int> imageResources = new Dictionary<string, int>() {
            {"Copper_Ore",Resource.Drawable.Copper_Ore},
            {"Tin_Ore",Resource.Drawable.Tin_Ore},
            {"Iron_Ore",Resource.Drawable.Iron_Ore},
            {"Silver_Ore",Resource.Drawable.Silver_Ore},
            {"Mithril_Ore",Resource.Drawable.Mithril_Ore},
            {"Adamantite_Ore",Resource.Drawable.Adamantite_Ore},
            {"Runite_Ore",Resource.Drawable.Runite_Ore},
            {"Coal_Ore",Resource.Drawable.Coal_Ore}};
        SqliteConnection connection;
        ObservableCollection<Ore> oreCount = new ObservableCollection<Ore>();
        int experience;
        int expLevel;
        string currentOre = "Copper_Ore";
        int currency = 0;

        DatabaseDataHandler()
        {

            if (!File.Exists(dbPath))
            {
                Console.WriteLine("Creating database");
                // Need to create the database before seeding it with some data
                SqliteConnection.CreateFile(dbPath);

                var commands = new[] {
                "CREATE TABLE UserData (experience INTEGER, expLevel INTEGER, currency INTEGER);",
                "INSERT INTO UserData (experience, expLevel,currency) VALUES ('0','1','1')",
                "CREATE TABLE Ores (oreName VARCHAR(30), oreCount INTEGER, oreCurrencyValue INTEGER, oreExpRate INTEGER);",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Copper_Ore','0','10','10')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Tin_Ore', '0', '17', '17')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Iron_Ore', '0', '30', '30')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Silver_Ore', '0', '40', '40')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Coal_Ore', '0', '50', '50')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Mithril_Ore', '0', '80', '80')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Adamantite_Ore', '0', '95', '95')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate) VALUES ('Runite_Ore', '0', '125', '125')"};
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
        private void executeCommand(string query)
        {
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = query;
                var r = contents.ExecuteNonQuery();
            }
        }
        private void getOreData()
        {
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = "SELECT * from Ores";
                var r = contents.ExecuteReader();
                while (r.Read())
                {
                    oreCount.Add(new Ore(r["oreName"].ToString(), imageResources[r["oreName"].ToString()], Convert.ToInt32(r["oreCount"].ToString()),
                        Convert.ToInt32(r["oreCurrencyValue"].ToString()), Convert.ToInt32(r["oreExpRate"].ToString())));
                }
            }
        }
        /// <summary>
        /// Gets the stored data from the local database (prob vahetan nime ära millalgi xd)
        /// </summary>
        public Tuple<int,int> getDataFromDatabase()
        {
            connection.Open();

            getOreData();
            getExp();
            getExpLevel();
            getCurrency();
            connection.Close();

            return Tuple.Create(experience, expLevel);
        }
        /// <summary>
        /// Gets the stored data from the local database (prob vahetan nime ära millalgi xd)
        /// </summary>
        private void getExp()
        {
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = "SELECT experience from userData";
                var r = contents.ExecuteReader();
                Console.WriteLine("Reading data");
                while (r.Read())
                {
                    Console.WriteLine("experience = {0}", Convert.ToInt32(r["experience"].ToString()));
                    experience = Convert.ToInt32(r["experience"].ToString());
                }
            }
        }
        private void getCurrency()
        {
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = "SELECT currency from userData";
                var r = contents.ExecuteReader();
                Console.WriteLine("Reading data");
                while (r.Read())
                {
                    Currency = Convert.ToInt32(r["currency"].ToString());
                }
            }
        }
        private void getExpLevel()
        {
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = "SELECT expLevel from userData";
                var r = contents.ExecuteReader();
                Console.WriteLine("Reading data");
                while (r.Read())
                {
                    Console.WriteLine("expLevel = {0}", Convert.ToInt32(r["expLevel"].ToString()));
                    expLevel = Convert.ToInt32(r["expLevel"].ToString());
                }
            }
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
        /// Saves Data to database (exp, level, ore amounts)
        /// </summary>
        /// <param name="exp">player experience</param>
        /// <param name="explevel"> player level</param>
        public void saveDataDatabase(int exp, int explevel)
        {
            connection.Open();
            executeCommand("UPDATE userData SET experience ='" + exp + "', expLevel ='"+ explevel+"' ;");
            foreach (var item in oreCount)
            {
                executeCommand("UPDATE Ores SET oreCount =" + item.OreCount + " WHERE oreName = '" + item.Name + "';");
            }
            connection.Close();
        }
        public void saveDataDatabaseOre(string query)
        {
            connection.Open();
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = query;
                var r = contents.ExecuteNonQuery();
            }
            connection.Close();
        }

        public string CurrentOre { get => currentOre; set => currentOre = value; }
        public int Currency { get => currency; set => currency = value; }

    }
}
