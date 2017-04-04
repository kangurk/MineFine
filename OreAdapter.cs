using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;

namespace MineFine
{
    public class OreAdapter : BaseAdapter<Ore>
    {
        ObservableCollection<Ore> ores;
        Activity context;
        Dictionary<string, int> imageResources = new Dictionary<string, int>() {
            { "Copper_Ore",Resource.Drawable.Copper_Ore},
            {"Tin_Ore",Resource.Drawable.Tin_Ore},
            {"Iron_Ore",Resource.Drawable.Iron_Ore},
            {"Silver_Ore",Resource.Drawable.Silver_Ore},
            {"Mithril_Ore",Resource.Drawable.Mithril_Ore},
            {"Adamantite_Ore",Resource.Drawable.Adamantite_Ore},
            {"Runite_Ore",Resource.Drawable.Runite_Ore},
            {"Coal_Ore",Resource.Drawable.Coal_Ore}};
        public OreAdapter(Activity context, ObservableCollection<Ore> ores) : base()
        {
            this.context = context;
            this.ores = ores;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Ore this[int position]
        {
            get { return ores[position]; }
        }

        public override int Count
        {
            get { return ores.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ShopListViewLayout, null);
            }

            var oreObject = ores[position];

            view.FindViewById<TextView>(Resource.Id.amountOfOre).Text = oreObject.OreCount.ToString();
            view.FindViewById<TextView>(Resource.Id.oreName).Text = oreObject.Name;
            view.FindViewById<ImageView>(Resource.Id.listViewOreImage).SetImageResource(imageResources[oreObject.Name]);
            

            return view;
        }
    }
}
