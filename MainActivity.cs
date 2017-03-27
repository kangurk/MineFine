using Android.App;
using Android.Widget;
using Android.OS;

namespace minefine
{
    [Activity(Label = "minefine", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string pictureName = "Copper";
        int experience = 0;
        int copperCount = 0;
        int tinCount = 0;
        int ironCount = 0;
        int silverCount = 0;
        int coalCount = 0;
        int mithrilCount = 0;
        int adamantiteCount = 0;
        int runiteCount = 0;
        int expLevel = 1;
        double nextLevel = 83;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);
            var toShop = FindViewById<Button>(Resource.Id.toShop);
            var mainImage = FindViewById<ImageView>(Resource.Id.mainImage);
            var expHour = FindViewById<TextView>(Resource.Id.expHour);
            var totalExp = FindViewById<TextView>(Resource.Id.totalExp);
            var totalLevel = FindViewById<TextView>(Resource.Id.Level);

            switch (pictureName)
            {
                case "Copper": mainImage.SetImageResource(Resource.Drawable.Copper_rock); break;
                case "Tin": mainImage.SetImageResource(Resource.Drawable.Tin_rock); break;
                case "Iron":mainImage.SetImageResource(Resource.Drawable.Iron_rock); break;
                case "Silver": mainImage.SetImageResource(Resource.Drawable.Silver_rock); break;
                case "Coal":mainImage.SetImageResource(Resource.Drawable.Coal_rock); break;
                case "Mithril": mainImage.SetImageResource(Resource.Drawable.Mithril_rock); break;
                case "Adamantite": mainImage.SetImageResource(Resource.Drawable.Adamantite_rock); break;
                case "Runite": mainImage.SetImageResource(Resource.Drawable.Runite_rock); break;
            }
            totalExp.Text = "total experience is " + experience.ToString();

            toShop.Click += delegate
            {
                StartActivity(typeof(ShopActivity));
            };

            mainImage.Click += delegate
            {
                switch (pictureName) {
                    case "Copper": experience += 10; copperCount += 1; break;
                    case "Tin": experience += 17; tinCount += 1; break;
                    case "Iron": experience += 30; ironCount += 1; break;
                    case "Silver":experience += 40; silverCount += 1; break;
                    case "Coal": experience += 50; coalCount += 1; break;
                    case "Mithril":experience += 80; mithrilCount += 1; break;
                    case "Adamantite":experience += 95; adamantiteCount += 1; break;
                    case "Runite":experience += 125; runiteCount += 1; break;
                }
                totalExp.Text = "total experience is "+experience.ToString();
                level();
                totalLevel.Text = "Your current level is " +expLevel.ToString();
            };
        }
        private int level()
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
    
    }
}

