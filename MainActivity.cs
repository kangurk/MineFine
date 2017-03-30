using Android.App;
using Android.Widget;
using Android.OS;

namespace minefine
{
    [Activity(Label = "minefine", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string currentOre = "Copper";
        int experience = 0;
        int expLevel = 1;
        double nextLevel = 83;
        int cooldownSeconds = 5;
        

        Dictionary<string, int> oreExp = new Dictionary<string, int>()
        {
            {"Copper", 10},
            {"Tin", 17},
            {"Iron", 30},
            {"Silver", 40},
            {"Coal", 50},
            {"Mithril", 80},
            {"Adamantite", 95},
            {"Runite",125 },
        };
        Dictionary<string, int> oreCount = new Dictionary<string, int>()
        {
            {"Copper", 0},
            {"Tin", 0},
            {"Iron", 0},
            {"Silver", 0},
            {"Coal", 0},
            {"Mithril", 0},
            {"Adamantite", 0},
            {"Runite", 0},
        };
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            var toShop = FindViewById<Button>(Resource.Id.toShop);
            var mainImage = FindViewById<ImageView>(Resource.Id.mainImage);
            var expHour = FindViewById<TextView>(Resource.Id.expHour);
            var totalExp = FindViewById<TextView>(Resource.Id.totalExp);
            var totalLevel = FindViewById<TextView>(Resource.Id.Level);

            totalExp.Text = "total experience is " + experience.ToString();

            toShop.Click += delegate
            {
                StartActivity(typeof(ShopActivity));
            };

            mainImage.Click += delegate
            {
                    int clickExp = oreExp[currentOre];
                    int Count = oreCount[currentOre];
                    oreCount[currentOre] = Count + 1;
                    experience += clickExp;
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
            if(chance == 1)
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

    }
}

