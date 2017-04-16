using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Content.PM;
using System;

namespace MineFine
{
    [Activity(Label = "minefine", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        
        int experience = 0;
        int expLevel = 1;
        double nextLevel = 83;
        string question;
        string positive;
        string negative;

        //class on padlockitud, et ainult 1 instance saab sellest olla
        DatabaseDataHandler databaseDataHandler = DatabaseDataHandler.Instance;
        Ore currentActiveOre;
        
        Button toShop;
        ImageView mainImage;
        TextView expHour;
        TextView totalExp;
        TextView totalLevel;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Main);
            toShop = FindViewById<Button>(Resource.Id.toShop);
            mainImage = FindViewById<ImageView>(Resource.Id.mainImage);
            expHour = FindViewById<TextView>(Resource.Id.expHour);
            totalExp = FindViewById<TextView>(Resource.Id.totalExp);
            totalLevel = FindViewById<TextView>(Resource.Id.Level);

            initializeDatabase();

            totalExp.Text = "total experience is " + experience.ToString();
            totalLevel.Text = "Your current level is " + expLevel.ToString();

            
            toShop.Click += delegate
            {
                StartActivity(typeof(ShopActivity));
            };

            mainImage.Click += delegate
            {
                //translation for this line = selecti observablelistist Ore class mille Ore.Name on sama nimega mis currentOre ja liida sellele 1 
                //ja kuna observablecollection on reference databasedatahandler classi variablist siis muutub see value ka databasedatahandler classi observablecollectionis
                currentActiveOre.OreCount += 1;
                experience += currentActiveOre.OreExpRate;
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
            List<String> events = new List<String>();
            events.Add("Genie");
            events.Add("Surprise exam");
            events.Add("Frog");
            events.Add("Quiz");
            Random npcChance = new Random();
            var chance1 = npcChance.Next(1, 4);
            Java.Util.Random eventChance = new Java.Util.Random();
            var chance = eventChance.NextInt(100);
            if (chance == 1)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("RandomEvent!!");
                switch (events[chance1 - 1])
                {
                    case "Genie":
                        alert.SetIcon(Resource.Drawable.genie);
                        positive = "Rub the lamp!";
                        negative = "Dismiss genie!";
                        question = "A genie has appeared, rub the lamp for a fortune";
                        break;
                    case "Surprise exam":
                        alert.SetIcon(Resource.Drawable.exam);
                        positive = "Mine Fine";
                        negative = "Ore Clicker";
                        question = "Which of the is the correct pronunciation";
                        break;
                    case "Frog":
                        alert.SetIcon(Resource.Drawable.frog);
                        positive = "Of course!";
                        negative = "Ew, no!";
                        question = "Will you kiss the frog?";
                        break;
                    case "Quiz":
                        alert.SetIcon(Resource.Drawable.quiz);
                        positive = "No";
                        negative = "Yes";
                        question = "Can you land on sun during the night?";
                        break;
                }
                alert.SetMessage(question);
                alert.SetPositiveButton(positive, (senderAlert, args) =>
                {
                    //auhind kui õigesti vastad
                });
                alert.SetNegativeButton(negative, (senderAlert, args) =>
                {
                    //auhind kui valesti vastad
                });
                Dialog dialog = alert.Create();
                dialog.Show();
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
        /// gets data from local database
        /// </summary>
        private void initializeDatabase()
        {
            Tuple<int,int> values = databaseDataHandler.getDataFromDatabase();
            experience = values.Item1;
            expLevel = values.Item2;
            

        }
        protected override void OnPause()
        {
            base.OnPause();
            databaseDataHandler.saveDataDatabase(experience, expLevel); // ei tea millal(/)kuhu seda panna veel
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            databaseDataHandler.saveDataDatabase(experience, expLevel); // ei tea millal(/)kuhu seda panna veel
        }
        protected override void OnResume()
        {
            base.OnResume();
            currentActiveOre = databaseDataHandler.getObservable().Select((p) => new { Ore = p }).Where(p => p.Ore.Name == databaseDataHandler.CurrentOre).First().Ore;
            mainImage.SetImageResource(currentActiveOre.Image);
        }

    }
}
