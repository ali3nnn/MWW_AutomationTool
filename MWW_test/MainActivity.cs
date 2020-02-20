using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content.PM;
using Android.Views;
using Android.Nfc;
using Android.Content;
using System.Text;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections;
using Android.Support.V4.Util;
using Java.Util;
using Button = Android.Widget.Button;
using Matcha.BackgroundService.Droid;

namespace MWW_test
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    //Icon = "@mipmap/icon"

    [Activity(Label = "MWW_test", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered, NfcAdapter.ActionTagDiscovered, Intent.CategoryDefault })]

    //[Activity(Label = "MontareActivity", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //[IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered, NfcAdapter.ActionTagDiscovered, Intent.CategoryDefault })]

    public class MainActivity : AppCompatActivity
    {
        Button btnMontare;
        Button btnDemontare;
        Button btnInfo;

        private NfcAdapter _nfcAdapter;

        private TextView _infoMsg;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _infoMsg = FindViewById<TextView>(Resource.Id.infoMsg);

            btnMontare = FindViewById<Button>(Resource.Id.buttonMontare);
            btnMontare.Click += btnMontare_click;

            btnDemontare = FindViewById<Button>(Resource.Id.buttonDemontare);
            btnDemontare.Click += btnDemontare_click;

            btnInfo = FindViewById<Button>(Resource.Id.buttonInfo);
            btnInfo.Click += btnInfo_click;

            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

        }

        protected override void OnPause()
        {
            base.OnPause();

            if (_nfcAdapter != null)
                _nfcAdapter.DisableForegroundDispatch(this);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_nfcAdapter == null)
            {
                var alert = new Android.App.AlertDialog.Builder(this).Create();
                alert.SetMessage("NFC is not supported on this device.");
                alert.SetTitle("NFC Unavailable");
                alert.Show();
            }
            else
            {
                var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
                var ndefDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
                var techDetected = new IntentFilter(NfcAdapter.ActionTechDiscovered);

                var filters = new[] { ndefDetected, tagDetected, techDetected };

                var intent = new Intent(this, this.GetType()).AddFlags(ActivityFlags.SingleTop);

                var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

                _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent.Action == NfcAdapter.ActionTagDiscovered)
            {
                _infoMsg.Text = "Selectează o activitate înainte de scanare";
            }
            else if (intent.Action == NfcAdapter.ActionNdefDiscovered)
            {
                System.Diagnostics.Debug.WriteLine("ActionNdefDiscovered");
            }
        }

        private void btnMontare_click(object sender, EventArgs e)
        {
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(MontareActivity));
            _infoMsg.Text = "";
            this.StartActivity(intent); 
        }

        private void btnDemontare_click(object sender, EventArgs e)
        {
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(DemontareActivity));
            _infoMsg.Text = "";
            this.StartActivity(intent);
        }

        private void btnInfo_click(object sender, EventArgs e)
        {
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(InfoActivity));
            _infoMsg.Text = "";
            this.StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}