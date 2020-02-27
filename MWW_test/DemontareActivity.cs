
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

namespace MWW_test
{
    [Activity(Label = "DemontareActivity")]
    public class DemontareActivity : Activity
    {

        private Button goToMainMenuBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.demontareAct);

            goToMainMenuBtn = FindViewById<Button>(Resource.Id.gotomainmenu);
            goToMainMenuBtn.Click += goToMainMenuBtn_click;
        }

        public override void OnBackPressed()
        {
            //Toast.MakeText(Application.Context, "Back button pressed", ToastLength.Short).Show();
            Intent intent2 = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent2);
            Finish();
            //return true // if you want to disable the back button
        }

        private void goToMainMenuBtn_click(object sender, EventArgs e)
        {
            Intent intent2 = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent2);
            Finish();

        }

    }
}
