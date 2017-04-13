using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;

namespace MineFine
{
    
    public class DialogFragmentCustom : DialogFragment
    {
        DatabaseDataHandler databaseDataHandler = DatabaseDataHandler.Instance;
        public static DialogFragmentCustom NewInstance(Bundle bundle)
        {
            DialogFragmentCustom fragment = new DialogFragmentCustom();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            string oreName = Arguments.GetString("oreName");
            string oreCount = Arguments.GetString("oreCount");
            int index = databaseDataHandler.getObservable().Select((p, i) => new { Ore = p, Index = i}).Where(p => p.Ore.Name == oreName).First().Index;
            ObservableCollection<Ore> oreList = databaseDataHandler.getObservable();

            //custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.CustomDialog, container, false);
            view.FindViewById<TextView>(Resource.Id.topText).Text = oreName;
            view.FindViewById<TextView>(Resource.Id.midText).Text = oreCount;

            Button button = view.FindViewById<Button>(Resource.Id.btnLeft);
            button.Click += delegate {
                databaseDataHandler.CurrentOre = oreName;
                Dismiss();
                
            };

            Button button2 = view.FindViewById<Button>(Resource.Id.btnRight);
            button2.Click += delegate {
                
                databaseDataHandler.Currency += oreList[index].OreCurrencyValue * oreList[index].OreCount;
                oreList[index].OreCount = 0;

                Dismissed?.Invoke(this, new DialogEventArgs { Text = databaseDataHandler.Currency.ToString() });

            };

            return view;
        }
        public event DialogEventHandler Dismissed;
    }
 
}
