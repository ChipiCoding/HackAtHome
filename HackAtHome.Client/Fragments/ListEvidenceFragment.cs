namespace HackAtHome.Client.Fragments
{
    using Android.App;
    using Android.OS;
    using HackAtHome.Entities;
    using System.Collections.Generic;

    public class ListEvidenceFragment : Fragment
    {
        public List<Evidence> ListEvidence { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            RetainInstance = true;
            base.OnCreate(savedInstanceState);
        }
    }
}