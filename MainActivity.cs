using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Content.PM;

namespace minefine
{
    [Activity(Label = "minefine", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        string currentOre = "Copper_Ore";
        int experience = 0;
        int expLevel = 1;
        double nextLevel = 83;
        int cooldownSeconds = 5;

        //class on padlockitud, et ainult 1 instance saab sellest olla
        DatabaseDataHandler databaseDataHandler = DatabaseDataHandler.Instance;


        Dictionary<string, int> oreExp = new Dictionary<string, int>()
        {
            {"Copper_Ore", 10},
            {"Tin_Ore", 17},
            {"Iron_Ore", 30},
            {"Silver_Ore", 40},
            {"Coal_Ore", 50},
            {"Mithril_Ore", 80},
            {"Adamantite_Ore", 95},
            {"Runite_Ore",125 },
        };
        ObservableCollection<Ore> oreCount;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            var toShop = FindViewById<Button>(Resource.Id.toShop);
            var mainImage = FindViewById<ImageView>(Resource.Id.mainImage);
            var expHour = FindViewById<TextView>(Resource.Id.expHour);
            var totalExp = FindViewById<TextView>(Resource.Id.totalExp);
            var totalLevel = FindViewById<TextView>(Resource.Id.Level);

            initializeDatabase();

            totalExp.Text = "total experience is " + experience.ToString();
            toShop.Click += delegate
            {
                
                StartActivity(typeof(ShopActivity));
            };

            mainImage.Click += delegate
            {
                //translation for this line = selecti observablelistist Ore class mille Ore.Name on sama nimega mis currentOre ja liida sellele 1 
                //ja kuna observablecollection on reference databasedatahandler classi variablist siis muutub see value ka databasedatahandler classi observablecollectionis
                oreCount.Select((p, i) => new { Ore = p, Index = i }).Where(p => p.Ore.Name == currentOre).First().Ore.OreCount += 1;

                experience += oreExp[currentOre];
                totalExp.Text = "total experience is " + experience.ToString();
                Level();
                totalLevel.Text = "Your current level is " + expLevel.ToString();
                randomEvents();
            };
        }
        private int Level()
        {
            if (experience > nextLevel)
            {
                double temp = nextLevel * 1.1;
                double together = nextLevel + temp;
                expLevel += 1;
                nextLevel = together;
            }
            return 0;
        }
        private void randomEvents()
        {
            Java.Util.Random eventChance = new Java.Util.Random();
            var chance = eventChance.NextInt(100);
            if (chance == 1)
            {
                var alert = new AlertDialog.Builder(this);
                alert.SetView(LayoutInflater.Inflate(Resource.Layout.RandomEvent, null));
                alert.Create().Show();
            }
        }

        private void levelUp()
        {
            //var levelupText = FindViewById<TextView>(Resource.Id.levelupText);
            //levelupText.Text = "You've achieved level " + expLevel.ToString();
            var alert = new AlertDialog.Builder(this);
            alert.SetView(LayoutInflater.Inflate(Resource.Layout.levelUp, null));
            alert.Create().Show();
        }
        private void oreCooldown()
        {

        }
        private void experienceHour()
        {

        }
        /// <summary>
        /// gets data from local database and sets all that data to the observablecollection, oreCount is that observablecollection
        /// </summary>
        private void initializeDatabase()
        {
            databaseDataHandler.getDataTest();
            experience = databaseDataHandler.getExp();
            oreCount = databaseDataHandler.getObservable();
        }
        protected override void OnPause()
        {
            base.OnPause();
            databaseDataHandler.saveOreData(); // ei tea millal(/)kuhu seda panna veel
            databaseDataHandler.saveExpData(experience);
            
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            databaseDataHandler.saveOreData(); // ei tea millal(/)kuhu seda panna veel
            databaseDataHandler.saveExpData(experience);
        }

    }
}
