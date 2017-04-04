using Android.App;
using Android.OS;
using Android.Widget;

namespace MineFine
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
            shopListView.ItemClick += ShopListView_ItemClick;
        }

        private void ShopListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, databaseDataHandler.getObservable()[e.Position].Name, ToastLength.Short).Show();
            //todo messagebox
        }
    }
}
