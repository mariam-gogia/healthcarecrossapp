using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace UniversalCarePlan
{
    //class mainly used to generate frames for each guideline
    public class frameGenerator
    {
        //these lists will be loaded as guidelines are selected
        public static List<Label> listOfSelectedOsteo = new List<Label>();
        public static List<Label> listOfSelectedTension = new List<Label>();
        public static List<Label> listOfSelectedOpioid = new List<Label>();
        public static List<Result> carePlanResults = new List<Result>(); //collection of care plan results

        //constructor takes in guideline code as a parameter
        public frameGenerator(string code)
        {
            GenerateFrame(code);
        }

        //method generates list of frames for each guideline with given code and returns frame back
        public List<Frame> GenerateFrame(string code)
        {
            //instantiating the local list of frames that will be returned
            List<Frame> returnableFramelist = new List<Frame>();

            foreach (Result r in App.GuidelinesInstance.results)
            {
                if (r.code == code)
                {
                    Label urlLabel = new Label //label for guideline URL
                    {
                        Text = "More...",
                        Margin = new Thickness(0, 0, 0, 6),
                        TextColor = Color.Blue,
                        FontSize = 18,
                        VerticalOptions = LayoutOptions.Center,

                        TextDecorations = TextDecorations.Underline
                    };

                    //create gesture recognizer for the urlLabel to redirect to given url once clicked
                    var tapGestureRecognizer = new TapGestureRecognizer();

                    tapGestureRecognizer.Command = ClickCommand;
                    tapGestureRecognizer.CommandParameter = r.URL;
                    urlLabel.GestureRecognizers.Add(tapGestureRecognizer);

                    //create label where recommendations are loaded
                    Label recommendations = new Label
                    {
                        Text = r.recommendation,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };

                    //extra copy of recommendation label but with classID set to code (condition it belongs to)
                    Label recommDouble = new Label
                    {
                        Text = r.recommendation,
                        ClassId = code
                    };

                    var section = new Span
                    {
                        Text = "\n|" + r.section + "| " + r.date,
                        TextColor = Color.Gray

                    };

                    //creating formatted string to display title and rank below in color coded manner 
                    var formattedString = new FormattedString();
                    var priorityString = new Span
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 16
                    };
                    var titleString = new Span
                    {
                        Text = (r.title).ToUpper(),
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.DarkMagenta,
                        FontSize = 18
                    };

                    switch (r.priority)
                    {
                        case "high":
                            priorityString.TextColor = Color.Crimson;
                            priorityString.Text = "1 (" + r.priority + ")";
                            break;
                        case "moderate":
                            priorityString.TextColor = Color.DarkOrange;
                            priorityString.Text = "2 (" + r.priority + ")";
                            break;
                        case "low":
                            priorityString.TextColor = Color.Goldenrod;
                            priorityString.Text = "3 (" + r.priority + ")";
                            break;
                    }

                    formattedString.Spans.Add(titleString);
                    formattedString.Spans.Add(new Span { Text = "\nRank: ", FontAttributes = FontAttributes.Bold, FontSize = 15 });
                    formattedString.Spans.Add(priorityString);
                    formattedString.Spans.Add(section);

                    //create separator
                    BoxView separator = new BoxView
                    {
                        Color = Color.Black,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 1
                    };
                    //select button
                    Button selectBtn = new Button
                    {
                        Text = " SELECT ",
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.End,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 18,
                        TextColor = Color.White,
                        BackgroundColor = Color.Green,
                        WidthRequest = 105

                    };
                    //select button event handler takes in button, ClassID assigned recommendation and result r
                    selectBtn.Clicked += delegate (object sender, EventArgs e) { button_Click(sender, e, selectBtn, recommDouble, r); };

                    var layoutText = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children = { new Label { FormattedText = formattedString }, separator, recommendations }

                    };

                    //creating layout of the frame
                    var layoutSelect = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.End,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        Children = { selectBtn }
                    };

                    var layoutURL = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.End,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        Children = { urlLabel }
                    };

                    var layoutButtons = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children = { layoutURL, layoutSelect }
                    };

                    var layoutMain = new StackLayout
                    {
                        Children = { layoutText, layoutButtons }
                    };

                    //creating frame for each recommendation
                    Frame guideFrames = new Frame
                    {
                        IsClippedToBounds = true,
                        BorderColor = Color.Black,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 209,
                        HasShadow = false,
                        Margin = 5,
                        Content = layoutMain,
                    };
                    //add each frame to the list
                    returnableFramelist.Add(guideFrames);
                }
            } //exiting foreach loop

            void button_Click(object sender, EventArgs e, Button btn, Label recommendation, Result r)
            {
                if (btn.Text == " SELECT ")
                {
                    btn.BackgroundColor = Color.Red;
                    btn.Text = " UNSELECT ";
                    btn.TextColor = Color.White;
                    SelectedGuidelines(recommendation); //method adds guidlines to appropriate condition's list
                    carePlanResults.Add(r); //adds result to care plan list
                }

                else if (btn.Text == " UNSELECT ")
                {
                    btn.BackgroundColor = Color.Green;
                    btn.Text = " SELECT ";
                    btn.TextColor = Color.White;
                    carePlanResults.Remove(r);
                    //removes recommendation from appropriate condition list
                    switch (recommendation.ClassId)
                    {
                        case "M19":
                            listOfSelectedOsteo.Remove(recommendation);
                            break;
                        case "V70":
                            listOfSelectedTension.Remove(recommendation);
                            break;
                        case "F11":
                            listOfSelectedOpioid.Remove(recommendation);
                            break;
                    }
                }
            }

            return returnableFramelist;
        }

        //to open URL
        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Device.OpenUri(new System.Uri(url));
        });

        //counts and returns guidelines in specified(code) condition
        public int CountGuidelines(string code)
        {
            int countG = 0;
            foreach (Result r in App.GuidelinesInstance.results)
            {
                if (r.code == code)
                {
                    countG += 1;
                }
            }
            return countG;
        }

        //adds selected guidelines to appropriate lists
        public void SelectedGuidelines(Label l)
        {

            if (l.ClassId == "M19" && !listOfSelectedOsteo.Contains(l))
            {
                listOfSelectedOsteo.Add(l);
            }
            else if (l.ClassId == "V70" && !listOfSelectedTension.Contains(l))
            {
                listOfSelectedTension.Add(l);
            }

            else if (l.ClassId == "F11" && !listOfSelectedOpioid.Contains(l))
            {
                listOfSelectedOpioid.Add(l);
            }
        }
    }
}

       
    



