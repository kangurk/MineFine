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
        ObservableCollection<Ore> oreList;
        TextView Currency;
        ListView shopListView;
        OreAdapter oreAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            oreList = databaseDataHandler.getObservable();

            SetContentView(Resource.Layout.Shop);
            Currency = FindViewById<TextView>(Resource.Id.gp);
            Currency.Text = databaseDataHandler.Currency.ToString();

            oreAdapter = new OreAdapter(this, oreList);
           
            shopListView = FindViewById<ListView>(Resource.Id.shopMenu);
            shopListView.Adapter = oreAdapter;
            shopListView.ItemClick += ShopListView_ItemClick;

            
        }

        private void ShopListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, oreList[e.Position].Name, ToastLength.Short).Show();
            Bundle args = new Bundle();
            args.PutString("oreName", oreList[e.Position].Name);
            args.PutString("oreCount", oreList[e.Position].OreCount.ToString());
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
            newFragment.Dismissed += (s, e) => { Currency.Text = e.Text; oreList[pos].OreCount = 0; newFragment.Dismiss(); oreAdapter.NotifyDataSetChanged(); };
            //Add fragment
            newFragment.Show(ft, "shopDialog");
            
        }


    }
    public class DialogEventArgs : EventArgs
    {
        public string Text { get; set; }

    }

    public delegate void DialogEventHandler(object sender, DialogEventArgs args);
}
