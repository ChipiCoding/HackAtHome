namespace HackAtHome.Client
{
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Webkit;
    using Android.Widget;

    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon")]
    public class DetailEvidenceActivity : Activity
    {
        /// <summary>
        /// 0 - URL ||
        /// 1 - Description ||
        /// 2 - FullName ||
        /// 3 - TittleLab ||
        /// 4 - Status
        /// </summary>
        private string[] dataEvidence;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            dataEvidence = Intent.GetStringArrayExtra("dataEvidence");
            SetContentView(Resource.Layout.EvidenceDetail);

            Koush.UrlImageViewHelper.SetUrlDrawable(FindViewById<ImageView>(Resource.Id.imageView1), dataEvidence[0]);
            FindViewById<WebView>(Resource.Id.webView1).LoadDataWithBaseURL(null, $"<html><head><style type='text/css'>body{{color:#fff}}</style></head><body>{dataEvidence[1]}</body></html>", "text/html", "utf-8", null);
            FindViewById<TextView>(Resource.Id.tvFullName).Text = dataEvidence[2];
            FindViewById<TextView>(Resource.Id.tvTittleLab).Text = dataEvidence[3];
            FindViewById<TextView>(Resource.Id.tvStatus).Text = dataEvidence[4];
            FindViewById<WebView>(Resource.Id.webView1).SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}