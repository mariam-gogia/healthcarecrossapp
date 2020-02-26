using System;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversalCarePlan
{
    public partial class App : Application
    {

        public static PatientData PatientDataInstance { get; private set;}
        public static GuidelinesData GuidelinesInstance { get; private set; }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTI3MzkwQDMxMzcyZTMyMmUzMEhxRHRWYTF5QndYZXAwVDJBbzZ4eFhLTnZCNmdwNjF3d1RZb0hYb0pQL0U9");


            //handling json files
            var assembly = this.GetType().Assembly;
            var stream = assembly.GetManifestResourceStream("UniversalCarePlan.data.json");

            var assembly2 = this.GetType().Assembly;
            var stream2 = assembly2.GetManifestResourceStream("UniversalCarePlan.Guidelines.json");

            StreamReader sr = new StreamReader(stream);
            string text = sr.ReadToEnd();

            StreamReader sr2 = new StreamReader(stream2);
            string text2 = sr2.ReadToEnd();

            JsonTextReader tr = new JsonTextReader(new StringReader(text));
            JsonSerializer serializer = new JsonSerializer();

            JsonTextReader tr2 = new JsonTextReader(new StringReader(text2));
            JsonSerializer serializer2 = new JsonSerializer();

            PatientData o = (PatientData)serializer.Deserialize(tr, typeof(PatientData));
            GuidelinesData g = (GuidelinesData)serializer2.Deserialize(tr2, typeof(GuidelinesData));
            
            
            App.PatientDataInstance = o;
            App.GuidelinesInstance = g;


            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
