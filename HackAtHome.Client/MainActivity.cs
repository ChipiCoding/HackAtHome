namespace HackAtHome.Client
{
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Widget;
    using HackAtHome.Entities;
    using HackAtHome.SAL;
    using System;

    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //UserInfo userInfo;
        ServiceClient service;
        ResultInfo result;
        
        private string[] dataStudent;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);            
            SetContentView(Resource.Layout.Main);
            if (bundle != null)                     
                dataStudent = bundle.GetStringArray("dataStudent");

           
            
            Button button = FindViewById<Button>(Resource.Id.buttonValidate);
            button.Click += delegate
            {
                Validate();
            };
        }

        private async void Validate()
        {          
            service = new ServiceClient();            
            dataStudent = Intent.GetStringArrayExtra("dataStudent");
            if (dataStudent == null)
            {
                result = await service.AutenticateAsync(
                FindViewById<EditText>(Resource.Id.etEmail).Text,
                FindViewById<EditText>(Resource.Id.etPassword).Text);

                var microsoftEvidence = new LabItem()
                {
                    Email = FindViewById<EditText>(Resource.Id.etEmail).Text,
                    Lab = "Hack@Home",
                    DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)
                };

                MicrosoftServiceClient microsoftClient = new MicrosoftServiceClient();
                await microsoftClient.SendEvidence(microsoftEvidence);
            }

            if ((result != null && result.Status == Status.Success) || dataStudent != null)
            {
                dataStudent = new string[3];
                dataStudent[0] = result != null ? result.Token : dataStudent[0];
                dataStudent[1] = result != null ? result.FullName : dataStudent[1];
                dataStudent[2] = DateTime.Now.ToString();    
                Intent intent = new Intent(this, typeof(listItemActivity));                
                intent.PutExtra("dataStudent", dataStudent);
                StartActivity(intent);
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {            
            outState.PutStringArray("dataStudent", dataStudent);
            base.OnSaveInstanceState(outState);
        }
    }
}

