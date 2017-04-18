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
        string btnText = "Buy";
        public static DialogFragmentCustom NewInstance(Bundle bundle)
        {
            DialogFragmentCustom fragment = new DialogFragmentCustom();
            fragment.Arguments = bundle;
            return fragment;
            
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            base.SetStyle(DialogFragmentStyle.NoTitle, 0);
        
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            
            string oreName = Arguments.GetString("oreName");
            string oreCount = Arguments.GetString("oreCount");
            btnText = Arguments.GetString("leftBtnText");
            int index = databaseDataHandler.OreObservableList.Select((p, i) => new { Ore = p, Index = i }).Where(p => p.Ore.Name == oreName).First().Index;


            //custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.CustomDialog, container, false);
            view.FindViewById<TextView>(Resource.Id.topText).Text = oreName;
            view.FindViewById<TextView>(Resource.Id.midText).Text = oreCount;

            Button button = view.FindViewById<Button>(Resource.Id.btnLeft);
            button.Text = btnText;
            button.Click += delegate {
                if(btnText == "Buy")
                {
                    if(databaseDataHandler.UserData.Currency >= databaseDataHandler.OreObservableList[index].OreCurrencyValue * 1500 * index)
                    {
                        databaseDataHandler.UserData.CurrentOre = oreName;
                        databaseDataHandler.OreObservableList[index].IsOreUnlockedByUser = true;
                    }
                    else
                    {
                        Toast.MakeText(Activity, "You need " +( databaseDataHandler.OreObservableList[index].OreCurrencyValue * 1500 * index) + " gold coins to buy this item", ToastLength.Short).Show();
                    }
                }
                else
                {
                    databaseDataHandler.UserData.CurrentOre = oreName;
                }
                
                Dismiss();

            };

            Button button2 = view.FindViewById<Button>(Resource.Id.btnRight);
            button2.Click += delegate {

                databaseDataHandler.UserData.Currency += databaseDataHandler.OreObservableList[index].OreCurrencyValue * databaseDataHandler.OreObservableList[index].OreCount;
                databaseDataHandler.OreObservableList[index].OreCount = 0;

                Dismissed?.Invoke(this, new DialogEventArgs { Text = databaseDataHandler.UserData.Currency.ToString() });

            };

            return view;
        }
        public event DialogEventHandler Dismissed;
    }

}
