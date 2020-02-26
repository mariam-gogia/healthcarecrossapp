using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace UniversalCarePlan
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        //instances of conditions pages
        Osteoarthritis osteoPage = new Osteoarthritis();
        Primary_Hypertension hypertensionPage = new Primary_Hypertension();
        Opioid_use_disorder opioidPage = new Opioid_use_disorder();

        //checkboxes
        public static CheckBox one = new CheckBox
        {
            Color = Color.Green,
            HorizontalOptions = LayoutOptions.Center

        };
        public static CheckBox two = new CheckBox
        {
            Color = Color.Green,
            HorizontalOptions = LayoutOptions.Center
        };
        public static CheckBox three = new CheckBox
        {
            Color = Color.Green,
            HorizontalOptions = LayoutOptions.Center
        };

        public MainPage()
        {

            InitializeComponent();

            string givenName = "       Given Name:              " + (App.PatientDataInstance.Patient.GivenName);
            string lastName = "       Family Name:             " + (App.PatientDataInstance.Patient.FamilyName);
            string gender = "       Gender:                        " + (App.PatientDataInstance.Patient.Sex);
            string age = "        Age:                              " + (App.PatientDataInstance.Patient.Age).ToString();

            //method styles and adds each string to PatientInfoStack
            CreateStack(givenName);
            CreateStack(lastName);
            CreateStack(gender);
            CreateStack(age);

            //dynamically generate buttons for each condition set them to grid
            int row = 0;
            foreach (Condition c in App.PatientDataInstance.Condition)
            {
                Button conditionsBtns = new Button
                {
                    Text = c.DisplayName.ToUpper(),
                    FontFamily = "Montserrat",
                    FontSize = 22,
                    TextColor = Color.DarkViolet,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = 18,

                };

                //silver frame to hold buttons and checkboxes
                CreateBoxView(row);
                conditionsGrid.Children.Add(conditionsBtns);
                conditionsBtns.SetValue(Grid.RowProperty, row);
                row++;

                if (conditionsBtns.Text == "OSTEOARTHRITIS")
                {
                    conditionsBtns.Clicked += this.Handle_Clicked;
                }

                if (conditionsBtns.Text == "PRIMARY HYPERTENSION")
                {
                    conditionsBtns.Clicked += this.Handle_Clicked_1;
                }

                if (conditionsBtns.Text == "OPIOID USE DISORDER")
                {
                    conditionsBtns.Clicked += this.Handle_Clicked_2;
                }
            }
            //add checkboxes across buttons
            conditionsGrid.Children.Add(one);
            conditionsGrid.Children.Add(two);
            conditionsGrid.Children.Add(three);

            one.SetValue(Grid.RowProperty, 0);
            one.SetValue(Grid.ColumnProperty, 1);

            two.SetValue(Grid.RowProperty, 1);
            two.SetValue(Grid.ColumnProperty, 1);

            three.SetValue(Grid.RowProperty, 2);
            three.SetValue(Grid.ColumnProperty, 1);

            Button carePlanbtn = new Button
            {
                Text = "Click for Care Plan ",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.DarkRed,
                TextColor = Color.White,
                FontSize = 28,
                HeightRequest = 90,
                WidthRequest = 90,
                BorderColor = Color.Black,
                Margin = 5
            };

            carePlanbtn.Clicked += this.Handle_Clicked_3;
            carePlanStack.Children.Add(carePlanbtn);
        }

        //separator
        private void AddSeparator()
        {
            BoxView separator = new BoxView
            {
                Color = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 4,
                WidthRequest = 400
            };

            PatientInfoStack.Children.Add(separator);
        }

        //create stack for patient info
        private void CreateStack(string patientInfoLbls)
        {
            Label l = new Label
            {
                Text = patientInfoLbls,
                FontSize = 24
            };
            this.PatientInfoStack.Children.Add(l);
            AddSeparator();
        }

        //create silver frames with btns and chkboxes in them
        private void CreateBoxView(int rowBox)
        {
            BoxView color = new BoxView
            {
                BackgroundColor = Color.Silver,
                Margin = 8,
                Opacity = 0.3,
                CornerRadius = 15,
                MinimumHeightRequest = 2
            };
            conditionsGrid.Children.Add(color);
            color.SetValue(Grid.RowProperty, rowBox);
            color.SetValue(Grid.ColumnSpanProperty, 2);
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(osteoPage);
        }

        async void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(hypertensionPage);
        }

        async void Handle_Clicked_2(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(opioidPage);
        }

        async void Handle_Clicked_3(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Plan());
        }
    }
}




