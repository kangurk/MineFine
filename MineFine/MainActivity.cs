using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using Android.Content.PM;
using System;
using Android.Views;
using System.Threading;

namespace MineFine
{
    [Activity(Label = "minefine", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        double nextLevel = 83;
        string question;
        string positive;
        string negative;

        //class on padlockitud, et ainult 1 instance saab sellest olla
        DatabaseDataHandler databaseDataHandler = DatabaseDataHandler.Instance;
        Ore currentActiveOre;

        Button toShop;
        ImageView mainImage;
        TextView totalExp;
        TextView totalLevel;
        System.Timers.Timer timer;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Main);
            toShop = FindViewById<Button>(Resource.Id.toShop);
            mainImage = FindViewById<ImageView>(Resource.Id.mainImage);
            totalExp = FindViewById<TextView>(Resource.Id.totalExp);
            totalLevel = FindViewById<TextView>(Resource.Id.Level);

            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimedEvent;

            initializeDatabase();
            
            totalExp.Text = "total experience is " + databaseDataHandler.UserData.Experience.ToString();
            totalLevel.Text = "Your current level is " + databaseDataHandler.UserData.Level.ToString();
            defineNextlevel(databaseDataHandler.UserData.Level);

            toShop.Click += delegate
            {
                StartActivity(typeof(ShopActivity));
            };

            mainImage.Click += delegate
            {
                if (!timer.Enabled)
                {
                    currentActiveOre.OreCount += 1;
                    databaseDataHandler.UserData.Experience += currentActiveOre.OreExpRate;
                    totalExp.Text = "total experience is " + databaseDataHandler.UserData.Experience.ToString();
                    Level();
                    totalLevel.Text = "Your current level is " + databaseDataHandler.UserData.Level.ToString();
                    randomEvents();
                    oreCooldown();
                }
                
            };
        }
        private int Level()
        {
            if (databaseDataHandler.UserData.Experience > nextLevel)
            {
                defineNextlevel(databaseDataHandler.UserData.Level + 1);
                databaseDataHandler.UserData.Level += 1;
                levelUp();
            }
            return 0;
        }
        private void defineNextlevel(int userLevel)
        {
            double tempNextLevel = 0;
                for (int i = 0; i < userLevel; i++)
                {
                    double levelDifference = 83 * Math.Pow(1.104, i);
                    tempNextLevel += levelDifference;
                }
            nextLevel = tempNextLevel;
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
            var chance = eventChance.NextInt(2);
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
                    databaseDataHandler.UserData.Experience += 100;
                    totalExp.Text = "total experience is " + databaseDataHandler.UserData.Experience.ToString();
                });
                alert.SetNegativeButton(negative, (senderAlert, args) =>
                {
                    databaseDataHandler.UserData.Experience -= 100;
                    totalExp.Text = "total experience is " + databaseDataHandler.UserData.Experience.ToString();
                });
                Dialog dialog = alert.Create();
                dialog.SetCanceledOnTouchOutside(false);
                dialog.SetCancelable(false);
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
            mainImage.SetImageResource(Resource.Drawable.Depleted_Ore);
            timer.Enabled = true;

        }
        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            RunOnUiThread(() => mainImage.SetImageResource(currentActiveOre.Image));
        }

        /// <summary>
        /// gets data from local database
        /// </summary>
        private void initializeDatabase()
        {
            databaseDataHandler.initializeData();


        }
        protected override void OnPause()
        {
            base.OnPause();
            databaseDataHandler.saveDataDatabase(); // ei tea millal(/)kuhu seda panna veel
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            databaseDataHandler.saveDataDatabase(); // ei tea millal(/)kuhu seda panna veel
        }
        protected override void OnResume()
        {
            base.OnResume();
            currentActiveOre = databaseDataHandler.OreObservableList.Select((p) => new { Ore = p }).Where(p => p.Ore.Name == databaseDataHandler.UserData.CurrentOre).First().Ore;
            if (!timer.Enabled)
            {
                mainImage.SetImageResource(currentActiveOre.Image);
                timer.Interval = 4000 / databaseDataHandler.UserData.UserPickaxe.CoolDownRate;
            }
           
            
            
        }
        
    }

    class oreCooldownTimer
    {
        public int counter = 0;
        public Timer tmr;

    }
}
