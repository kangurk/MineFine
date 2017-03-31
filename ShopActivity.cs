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
        DatabaseDataHandler databaseDataHandler = DatabaseDataHandler.Instance;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            

            SetContentView(Resource.Layout.Shop);
            var text = FindViewById<TextView>(Resource.Id.Ore);

            var oreAdapter = new OreAdapter(this, databaseDataHandler.getObservable());
            var shopListView = FindViewById<ListView>(Resource.Id.shopMenu);
            shopListView.Adapter = oreAdapter;

        }
    }
}
