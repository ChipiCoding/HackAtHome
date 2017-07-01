namespace HackAtHome.Entities
{
    using Android.App;
    using Android.OS;

    public class ResultFragment : Fragment
    {
        public string Token { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
    }
}