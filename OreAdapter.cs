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
    public class OreAdapter : BaseAdapter<Ore>
    {
        Ore[] ores;
        Activity context;

        public OreAdapter(Activity context, Ore[] ores) : base()
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
            get { return ores.Length; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            { 
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            }

            var oreObject = ores[position];

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = oreObject.name;
            ImageView img = view.FindViewById<ImageView>(Android.Resource.Id.Icon);

            return view;
        }
    }
}