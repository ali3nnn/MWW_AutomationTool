using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

namespace MWW_test
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnMontare;
        Button btnDemontare;
        Button btnInfo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btnMontare = FindViewById<Button>(Resource.Id.buttonMontare);
            btnMontare.Click += btnMontare_click;

            btnDemontare = FindViewById<Button>(Resource.Id.buttonDemontare);
            btnDemontare.Click += btnDemontare_click;

            btnInfo = FindViewById<Button>(Resource.Id.buttonInfo);
            btnInfo.Click += btnInfo_click;
        }

        private void btnMontare_click(object sender, EventArgs e)
        {
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(MontareActivity));
            this.StartActivity(intent); 
        }

        private void btnDemontare_click(object sender, EventArgs e)
        {
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(DemontareActivity));
            this.StartActivity(intent);
        }

        private void btnInfo_click(object sender, EventArgs e)
        {
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(InfoActivity));
            this.StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}