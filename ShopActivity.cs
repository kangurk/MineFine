using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace minefine
{
    [Activity(Label = "ShopActivity")]
    public class ShopActivity : Activity
    {
        Ore[] ores;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ores = new Ore[] { new Ore("Copper", "Copper_rock"),
                               new Ore("Tin", "Tin_rock"),
                               new Ore("Iron", "Iron_rock"),
                               new Ore("Silver", "Silver_rock"),
                               new Ore("Coal", "Coal_rock"),
                               new Ore("Mithril", "Mithril_rock"),
                               new Ore("Adamantite", "Adamantite_rock"),
                               new Ore("Runite", "Runite_rock"),
                               };
    
            SetContentView(Resource.Layout.Shop);
            var text = FindViewById<TextView>(Resource.Id.Ore);
            var shop = FindViewById<ListView>(Resource.Id.shopMenu);

            shop.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.ActivityListItem, ores);

        }
    }
}