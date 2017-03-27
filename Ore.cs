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
    public class Ore
    {
        public string name { get; set; }
        public string image { get; set; }

        public Ore(string name, string image)
        {
            this.name = name;
            this.image = name;
        }
    }
}