using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace UniversalCarePlan
{
    public partial class Primary_Hypertension : ContentPage
    {

        public Primary_Hypertension()
        {
            InitializeComponent();

            if (MainPage.two.IsChecked == true)
            {
                TensionDone.IsChecked = true;
            }

            string givenName = " Given Name:              " + (App.PatientDataInstance.Patient.GivenName);
            string lastName = " Family Name:             " + (App.PatientDataInstance.Patient.FamilyName);
            string gender = " Gender:                        " + (App.PatientDataInstance.Patient.Sex);
            string age = " Age:                              " + (App.PatientDataInstance.Patient.Age).ToString();

            CreateStack(givenName);
            CreateStack(lastName);
            CreateStack(gender);
            CreateStack(age);
            
           
            frameGenerator frame = new frameGenerator("V70");
            List<Frame> listOfguidelines = new List<Frame>();

            listOfguidelines = frame.GenerateFrame("V70");

            int catchCount = frame.CountGuidelines("V70");
            guidlinesHeader.Text = "  Available Guidelines: " + catchCount;

            foreach (Frame f in listOfguidelines)
            {
                guidelinesStack.Children.Add(f);
            }
        }

        private void AddSeparator()
        {
            BoxView separator = new BoxView
            {
                Color = Color.White,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 4,
                WidthRequest = 320
            };

            PatientInfoStack.Children.Add(separator);
        }

        private void CreateStack(string patientLbls)
        {
            Label l = new Label
            {
                Text = patientLbls,
                FontSize = 24
            };
            this.PatientInfoStack.Children.Add(l);
            AddSeparator();
        }

        void Handle_CheckedChanged(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            MainPage.two.IsChecked = true;

            if (TensionDone.IsChecked == false)
            {
                MainPage.two.IsChecked = false;
            }
        }

        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            TensionDone.IsChecked = true;
        }
    }
}

    
    

