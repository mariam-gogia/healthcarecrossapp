using System;
using System.Collections.Generic;
using Xamarin.Forms;
namespace UniversalCarePlan
{
    public partial class Opioid_use_disorder : ContentPage
    {

        public Opioid_use_disorder()
        {
            InitializeComponent();

            if(MainPage.three.IsChecked == true)
            {
                OpioidDone.IsChecked = true;
            }
            else if(MainPage.three.IsChecked == false)
            {
                OpioidDone.IsChecked = false;
            }

            //creating patient info stack
            string givenName = " Given Name:              " + (App.PatientDataInstance.Patient.GivenName);
            string lastName = " Family Name:             " + (App.PatientDataInstance.Patient.FamilyName);
            string gender = " Gender:                        " + (App.PatientDataInstance.Patient.Sex);
            string age = " Age:                              " + (App.PatientDataInstance.Patient.Age).ToString();

            CreateStack(givenName);
            CreateStack(lastName);
            CreateStack(gender);
            CreateStack(age);

            
            //create an instance of frameGenerator class 
            frameGenerator frame = new frameGenerator("F11");
            List<Frame> listOfguidelines = new List<Frame>();
            listOfguidelines = frame.GenerateFrame("F11");

            //displays amount of guidlines available for this condition
            int catchCount = frame.CountGuidelines("F11"); 
            guidlinesHeader.Text = "  Available Guidelines: " + catchCount;

            //add guideline frames to stack
            foreach (Frame f in listOfguidelines)
            {
                guidelinesStack.Children.Add(f);
            }
        }

        //separator
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

        //creates patient info stack
        public void CreateStack(string patientLbls)
        {
            Label l = new Label();
            l.Text = patientLbls;
            l.FontSize = 24;
            this.PatientInfoStack.Children.Add(l);
            AddSeparator();
        }

        //checkbox state changed
        void Handle_CheckedChanged(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            MainPage.three.IsChecked = true;

            if (OpioidDone.IsChecked == false)
            {
                MainPage.three.IsChecked = false;
            }
        }

        //if done button is clicked change the state of the checkbox
        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            OpioidDone.IsChecked = true;
        }
    }
}
