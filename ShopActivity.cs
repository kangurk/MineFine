using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace minefine
{
    [Activity(Label = "ShopActivity")]
    public class ShopActivity : Activity
    {
        DatabaseDataHandler databaseDataHandler = DatabaseDataHandler.Instance;
        string currentOre = "Copper_Ore";
        ObservableCollection<Ore> oreList;
        TextView Currency;
        int gp = 0;
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            oreList = databaseDataHandler.getObservable();

            SetContentView(Resource.Layout.Shop);
            Currency = FindViewById<TextView>(Resource.Id.gp);

            var oreAdapter = new OreAdapter(this, oreList);
            var shopListView = FindViewById<ListView>(Resource.Id.shopMenu);
            shopListView.Adapter = oreAdapter;
            shopListView.ItemClick += ShopListView_ItemClick;
        }

        private void ShopListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, oreList[e.Position].Name, ToastLength.Short).Show();
            //todo messagebox
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetMessage("Do you want to sell or choose this ore to click?");
            alert.SetPositiveButton("Sell ore", (senderAlert, args) => {
                gp = gp + (oreExp[currentOre] * oreList[e.Position].OreCount);
                Currency.Text = gp.ToString() + "gp";
                int oreCount = 0;
                
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Dialog dialogg = alert.Create();
                dialogg.Hide();
            });
            alert.SetNeutralButton("Choose ore", (senderAlert, args) => {
                currentOre = oreList[e.Position].Name;
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}
