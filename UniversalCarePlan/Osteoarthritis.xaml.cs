using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows;
using Xamarin.Forms;

namespace UniversalCarePlan
{
    public partial class Osteoarthritis : ContentPage
    {
        public Osteoarthritis()
        {
            InitializeComponent();

            if (MainPage.one.IsChecked == true)
            {
                OsteoDone.IsChecked = true;
            }

            string givenName = " Given Name:              " + (App.PatientDataInstance.Patient.GivenName);
            string lastName = " Family Name:             " + (App.PatientDataInstance.Patient.FamilyName);
            string gender = " Gender:                        " + (App.PatientDataInstance.Patient.Sex);
            string age = " Age:                              " + (App.PatientDataInstance.Patient.Age).ToString();

            CreateStack(givenName);
            CreateStack(lastName);
            CreateStack(gender);
            CreateStack(age);

            frameGenerator frame = new frameGenerator("M19");
            List<Frame> listOfguidelines = new List<Frame>();


            listOfguidelines = frame.GenerateFrame("M19");


            int catchCount = frame.CountGuidelines("M19");
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
            Label l = new Label();
            l.Text = patientLbls;
            l.FontSize = 24;
            this.PatientInfoStack.Children.Add(l);
            AddSeparator();
        }


        void Handle_CheckedChanged(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            MainPage.one.IsChecked = true;

            if (OsteoDone.IsChecked == false)
            {
                MainPage.one.IsChecked = false;
            }
        }

        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            OsteoDone.IsChecked = true;
        }
    }
}
