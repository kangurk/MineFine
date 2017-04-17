using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
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
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Shop);
            Currency = FindViewById<TextView>(Resource.Id.gp);
            Currency.Text = databaseDataHandler.UserData.Currency.ToString();
            Pickaxe = FindViewById<Button>(Resource.Id.Upgrade);
            pickaxeText();

            oreAdapter = new OreAdapter(this, databaseDataHandler.OreObservableList);

            shopListView = FindViewById<ListView>(Resource.Id.shopMenu);
            shopListView.Adapter = oreAdapter;
            shopListView.ItemClick += ShopListView_ItemClick;

            Pickaxe.Click += delegate
            {
                if (databaseDataHandler.UserData.Currency >= databaseDataHandler.pickaxes[databaseDataHandler.UserData.CurrentPickaxeIndex].Cost)
                {
                    databaseDataHandler.UserData.Currency -= databaseDataHandler.pickaxes[databaseDataHandler.UserData.CurrentPickaxeIndex].Cost;
                    databaseDataHandler.UserData.CurrentPickaxeIndex += 1;
                    
                    pickaxeText();
                }
                else
                {
                    Toast.MakeText(this, "You need "+ databaseDataHandler.pickaxes[databaseDataHandler.UserData.CurrentPickaxeIndex].Cost+" gold coins to buy this item", ToastLength.Short).Show();
                }
            };

        }

        private void ShopListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            Bundle args = new Bundle();
            args.PutString("oreName", databaseDataHandler.OreObservableList[e.Position].Name);
            args.PutString("oreCount", databaseDataHandler.OreObservableList[e.Position].OreCount.ToString());
            if (databaseDataHandler.OreObservableList[e.Position].IsOreUnlockedByUser)
            {
                args.PutString("leftBtnText", "Select");
            }
            else
            {
                args.PutString("leftBtnText", "Buy");
            }
            
            createFragment(args, e.Position);

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
            newFragment.Dismissed += (s, e) => { Currency.Text = e.Text; databaseDataHandler.OreObservableList[pos].OreCount = 0; newFragment.Dismiss(); oreAdapter.NotifyDataSetChanged(); };
            //Add fragment
            newFragment.Show(ft, "shopDialog");

        }
        private void pickaxeText()
        {
            if (databaseDataHandler.UserData.CurrentPickaxeIndex < 8)
                Pickaxe.Text = "Upgrade pickaxe to " + databaseDataHandler.pickaxes[databaseDataHandler.UserData.CurrentPickaxeIndex+1].Name;
            else
                Pickaxe.Text = "Pickaxe fully upgraded!";
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
    }
    public class DialogEventArgs : EventArgs
    {
        public string Text { get; set; }

    }

    public delegate void DialogEventHandler(object sender, DialogEventArgs args);
}
