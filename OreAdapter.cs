using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;

namespace minefine
{
    public class OreAdapter : BaseAdapter<Ore>
    {
        ObservableCollection<Ore> ores;
        Activity context;

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
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            }

            var oreObject = ores[position];

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = oreObject.Name;
            ImageView img = view.FindViewById<ImageView>(Android.Resource.Id.Icon);

            return view;
        }
    }
}
