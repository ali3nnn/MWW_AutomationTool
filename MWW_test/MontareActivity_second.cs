
using System;
using System.Collections.Generic;
using System.Linq;
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
using AlertDialog = Android.App.AlertDialog;
using Xamarin.Forms.Internals;
using Android.Nfc.Tech;

namespace MWW_test
{
    [Activity(Label = "MontareActivity_second", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered, NfcAdapter.ActionTagDiscovered, Intent.CategoryDefault })]

    public class MontareActivity_second : Activity
    {

        private NfcAdapter _nfcAdapter;
        private TextView mTextView;
        private TextView mTextViewEMM;
        string codEMM;

    protected override void OnCreate(Bundle savedInstanceState)
        {
            BackgroundAggregator.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.montareAct_second);

            mTextView = (TextView)FindViewById(Resource.Id.textView2);
            mTextViewEMM = (TextView)FindViewById(Resource.Id.textView3);

            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            string text = Intent.GetStringExtra("key");
            //Console.WriteLine(text);
            mTextView.Text += ": "+text;

        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_nfcAdapter == null)
            {
                var alert = new AlertDialog.Builder(this).Create();
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

        protected override void OnPause()
        {
            base.OnPause();

            if (_nfcAdapter != null)
                _nfcAdapter.DisableForegroundDispatch(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent.Action == NfcAdapter.ActionTagDiscovered)
            {

                List<string> tags = new List<string>();

                var id = intent.GetByteArrayExtra(NfcAdapter.ExtraId);

                if (id != null)
                {
                    string data = "";
                    for (int ii = 0; ii < id.Length; ii++)
                    {
                        if (!string.IsNullOrEmpty(data))
                            data += "-";
                        data += id[ii].ToString("X2");
                    }

                    tags.Add(data);

                }
                else
                    tags.Add(null);

                var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
                if (tag != null)
                {

                    var rawTagMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraTag);

                    // First get all the NdefMessage
                    var rawMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                    if (rawMessages != null)
                    {

                        // https://medium.com/@ssaurel/create-a-nfc-reader-application-for-android-74cf24f38a6f

                        foreach (var message in rawMessages)
                        {

                            foreach (var r in NdefMessageParser.GetInstance().Parse((NdefMessage)message))
                            {
                                //System.Diagnostics.Debug.WriteLine("TAG: " + r.Str());
                                codEMM = r.Str();
                                mTextViewEMM.Text += ": " + codEMM;

                                //change activity after scanning LF
                                //Intent intent2 = new Android.Content.Intent(this, typeof(MontareActivity_second));
                                //this.StartActivity(intent2);

                            }

                        }
                    }
                }

                //MessagingCenter.Send<App, List<string>>((App)Xamarin.Forms.Application.Current, "Tag", tags);
                //MessagingCenter.Send((App)Xamarin.Forms.Application.Current, "Tag", tags);

            }
            else if (intent.Action == NfcAdapter.ActionNdefDiscovered)
            {
                System.Diagnostics.Debug.WriteLine("ActionNdefDiscovered");
            }
        }

    }
}
