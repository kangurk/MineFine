using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MineFine
{
    [Activity(Label = "ShopActivity")]
    public class ShopActivity : Activity
    {
        DatabaseDataHandler databaseDataHandler = DatabaseDataHandler.Instance;
        TextView Currency;
        Button Pickaxe;
        ListView shopListView;
        OreAdapter oreAdapter;
        int currentPickaxe = 0;
        Dictionary<int, string> pickaxes = new Dictionary<int, string>()
        {
            {0, "Bronze pickaxe"},
            {1, "Iron pickaxe"},
            {2, "Steel pickaxe"},
            {3, "Black pickaxe"},
            {4, "Mithril pickaxe"},
            {5, "Adamant pickaxe"},
            {6, "Rune pickaxe"},
            {7, "Dragon pickaxe"},
            {8, "3rd age pickaxe"}
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Shop);
            Currency = FindViewById<TextView>(Resource.Id.gp);
            Currency.Text = databaseDataHandler.Currency.ToString();
            Pickaxe = FindViewById<Button>(Resource.Id.Upgrade);
            pickaxeText();

            oreAdapter = new OreAdapter(this, databaseDataHandler.getObservable());
           
            shopListView = FindViewById<ListView>(Resource.Id.shopMenu);
            shopListView.Adapter = oreAdapter;
            shopListView.ItemClick += ShopListView_ItemClick;

            Pickaxe.Click += delegate
                {
                     if (databaseDataHandler.Currency >= (currentPickaxe * 10000))
                     {
                            currentPickaxe++;
                            pickaxeText();
                     }
                };
            
        }

        private void ShopListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, databaseDataHandler.getObservable()[e.Position].Name, ToastLength.Short).Show();
            Bundle args = new Bundle();
            args.PutString("oreName", databaseDataHandler.getObservable()[e.Position].Name);
            args.PutString("oreCount", databaseDataHandler.getObservable()[e.Position].OreCount.ToString());
            createFragment(args,e.Position);
            
        }
        

        private void createFragment(Bundle args, int pos)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag("shopDialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            // Create and show the dialog.
            DialogFragmentCustom newFragment = DialogFragmentCustom.NewInstance(args);
            newFragment.Dismissed += (s, e) => { Currency.Text = e.Text; databaseDataHandler.getObservable()[pos].OreCount = 0; newFragment.Dismiss(); oreAdapter.NotifyDataSetChanged(); };
            //Add fragment
            newFragment.Show(ft, "shopDialog");
            
        }
    private void pickaxeText()
        {
            if (currentPickaxe < 8)
                Pickaxe.Text = "Upgrade pickaxe to " + pickaxes[currentPickaxe + 1];
            else
                Pickaxe.Text = "Pickaxe fully upgraded!";
        }

    }
    public class DialogEventArgs : EventArgs
    {
        public string Text { get; set; }

    }

    public delegate void DialogEventHandler(object sender, DialogEventArgs args);
}
