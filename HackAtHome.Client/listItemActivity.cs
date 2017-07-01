namespace HackAtHome.Client
{
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Widget;
    using HackAtHome.Client.Fragments;
    using HackAtHome.CustomAdapters;
    using HackAtHome.Entities;
    using HackAtHome.SAL;
    using System;
    using System.Linq;
    using static Android.Widget.AdapterView;

    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon")]
    public class listItemActivity : Activity
    {

        /// <summary>
        /// 0 - Token ||
        /// 1 - FullName ||
        /// 2 - DateAutentication 
        /// </summary>
        private string[] dataStudent;

        /// <summary>
        /// 0 - URL ||
        /// 1 - Description ||
        /// 2 - FullName ||
        /// 3 - TittleLab ||
        /// 4 - Status
        /// </summary>
        private string[] dataEvidence;

        ListEvidenceFragment listEvidence;       
        ListView listViewEvidence;
        ServiceClient service;
        Evidence evidenceSelected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listEvidence);

            dataStudent = Intent.GetStringArrayExtra("dataStudent");

            if (dataStudent != null)
            {
                if (savedInstanceState != null)
                    dataStudent = savedInstanceState.GetStringArray("dataStudent");

                FindViewById<TextView>(Resource.Id.tvFullName).Text = dataStudent[1];

                if (dataStudent != null && !string.IsNullOrEmpty(dataStudent[0]))
                    GetEvidencesAsync();
            }
        }

        private async void GetEvidencesAsync()
        {
            service = new ServiceClient();
            listEvidence = (ListEvidenceFragment)FragmentManager.FindFragmentByTag("listEvidence");
            if (listEvidence == null)
            {
                listEvidence = new ListEvidenceFragment();
                FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
                fragmentTransaction.Add(listEvidence, "listEvidence");
                fragmentTransaction.Commit();
                listEvidence.ListEvidence = await service.GetEvidencesAsync(dataStudent[0]);
            }
            listViewEvidence = FindViewById<ListView>(Resource.Id.listView1);
            listViewEvidence.Adapter = new EvidencesAdapter(this, listEvidence.ListEvidence, Resource.Layout.ListItem, Resource.Id.textView1, Resource.Id.textView2);

            listViewEvidence.ItemClick += (object sender, ItemClickEventArgs e) =>
            {
                long evidendeID = listViewEvidence.Adapter.GetItemId(e.Position);
                evidenceSelected = listEvidence.ListEvidence.Where(x => x.EvidenceID == evidendeID).FirstOrDefault();
                GetEvidenceByIDAsync(dataStudent[0], Convert.ToInt32(evidendeID));
            };
        }

        private async void GetEvidenceByIDAsync(string token, int evidendeID)
        {
            EvidenceDetail evidenceDetail = new EvidenceDetail();
            dataStudent = Intent.GetStringArrayExtra("dataStudent");
            evidenceDetail = await service.GetEvidenceByIDAsync(token, evidendeID);
            Intent intent = new Intent(this, typeof(DetailEvidenceActivity));
            intent.PutExtra("dataStudent", dataStudent);
            dataEvidence = new string[5];
            dataEvidence[0] = evidenceDetail.Url;
            dataEvidence[1] = evidenceDetail.Description;
            dataEvidence[2] = dataStudent[1];
            dataEvidence[3] = evidenceSelected.Title;
            dataEvidence[4] = evidenceSelected.Status;
            intent.PutExtra("dataEvidence", dataEvidence);
            StartActivity(intent);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutStringArray("dataStudent", dataStudent);
            outState.PutStringArray("dataEvidence", dataEvidence);
            base.OnSaveInstanceState(outState);
        }
    }
}
