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



        DatabaseDataHandler()
        {

            if (!File.Exists(dbPath))
            {
                // Need to create the database before seeding it with some data
                SqliteConnection.CreateFile(dbPath);

                var commands = new[] {
                "CREATE TABLE UserData (experience INTEGER, expLevel INTEGER, currency INTEGER, currentPickaxe INTEGER);",
                "INSERT INTO UserData (experience, expLevel,currency,currentPickaxe) VALUES ('0','1','1','0')",
                "CREATE TABLE Ores (oreName VARCHAR(30), oreCount INTEGER, oreCurrencyValue INTEGER, oreExpRate INTEGER, isOreUnlocked bit);",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Copper_Ore','0','10','10','1')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Tin_Ore', '0', '17', '17','0')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Iron_Ore', '0', '30', '30','0')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Silver_Ore', '0', '40', '40','0')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Coal_Ore', '0', '50', '50','0')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Mithril_Ore', '0', '80', '80','0')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Adamantite_Ore', '0', '95', '95','0')",
                "INSERT INTO Ores (oreName, oreCount,oreCurrencyValue,oreExpRate, isOreUnlocked) VALUES ('Runite_Ore', '0', '125', '125','0')"};
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
                while (r.Read())//multiple tables
                {
                    OreObservableList.Add(new Ore(r["oreName"].ToString(), imageResources[r["oreName"].ToString()], Convert.ToInt32(r["oreCount"].ToString()),
                        Convert.ToInt32(r["oreCurrencyValue"].ToString()), Convert.ToInt32(r["oreExpRate"].ToString()),(bool)r["isOreUnlocked"]));
                }
            }
        }
        /// <summary>
        /// Gets the stored data from the local database (prob vahetan nime ära millalgi xd)
        /// </summary>
        public void initializeData()
        {
            //init some values 
            OreObservableList = new ObservableCollection<Ore>();
            UserData = new User();
            UserData.CurrentOre = "Copper_Ore";

            connection.Open();
            getOreData();
            getUserData();
            connection.Close();
        }
        /// <summary>
        /// Gets the stored data from the local database (prob vahetan nime ära millalgi xd)
        /// </summary>
        private void getUserData()
        {
            using (var contents = connection.CreateCommand())
            {
                contents.CommandText = "SELECT * from UserData";
                var r = contents.ExecuteReader();
                Console.WriteLine("Reading data");
                UserData.CurrentPickaxeIndex = Convert.ToInt32(r["currentPickaxe"].ToString());
                UserData.UserPickaxe = pickaxes[UserData.CurrentPickaxeIndex];
                UserData.Experience = Convert.ToInt32(r["experience"].ToString());
                UserData.Currency = Convert.ToInt32(r["currency"].ToString());
                UserData.Level = Convert.ToInt32(r["expLevel"].ToString());
            }
        }
        
        
        
        /// <summary>
        /// Saves Data to database
        /// </summary>
        public void saveDataDatabase()
        {
            connection.Open();
            executeCommand("UPDATE userData SET experience ='" + UserData.Experience + "', expLevel ='" + UserData.Level + "', currency ='" + UserData.Currency + "', currentPickaxe ='" + UserData.UserPickaxe + "' ;");
            foreach (var item in OreObservableList)
            {
                executeCommand("UPDATE Ores SET oreCount =" + item.OreCount + ", isOreUnlocked = " + Convert.ToByte(item.IsOreUnlockedByUser) + " WHERE oreName = '" + item.Name + "';");
            }
            connection.Close();
        }

        public ObservableCollection<Ore> OreObservableList { get; set; }

        public User UserData { get; set; }

        public List<Pickaxe> pickaxes = new List<Pickaxe>()
        {
            {new Pickaxe("Bronze Pickaxe",1, 0)},
            {new Pickaxe("Iron Pickaxe",2, 30000)},
            {new Pickaxe("Steel Pickaxe",3, 50000)},
            {new Pickaxe("Black Pickaxe",4, 70000)},
            {new Pickaxe("Mithril Pickaxe",5, 90000)},
            {new Pickaxe("Adamant Pickaxe",6, 110000)},
            {new Pickaxe("Rune Pickaxe",7, 200000)},
            {new Pickaxe("Dragon Pickaxe",8, 300000)},
            {new Pickaxe("3rd age Pickaxe",10, 500000)}
        };

    }

}
