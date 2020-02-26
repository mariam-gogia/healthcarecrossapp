using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Syncfusion.Pdf;
using Syncfusion.Compression;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using Xamarin.Forms;
using Syncfusion.SfDataGrid.XForms;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Color = Xamarin.Forms.Color;
using System.Diagnostics;
using System.Text;

namespace UniversalCarePlan
{

    public partial class Plan : ContentPage
    {
        public Plan()
        {
            InitializeComponent();

            CarePlanRootobject cproot = new CarePlanRootobject();

            cproot.Patient = App.PatientDataInstance.Patient;
            cproot.results = new Result[frameGenerator.carePlanResults.Count];

            int i = 0;
            foreach (Result r in frameGenerator.carePlanResults)
            {
                cproot.results[i] = frameGenerator.carePlanResults[i];
                i++;
            }
            string jsonExportString = JsonConvert.SerializeObject(cproot);


            exportJSON.Clicked += exportJSONClicked;
            exportPDF.Clicked += exportPDFClicked;

            void exportJSONClicked(object sender, System.EventArgs e)
            {

                MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(jsonExportString));
                Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("jsonExport.json", jsonExportString, memoryStream);

            }

            //export PDF button event handler
            void exportPDFClicked(object sender, System.EventArgs e)
            {
                //creating pdf document and its first page
                PdfDocument carePlanDoc = new PdfDocument();
                PdfPage pageOne = carePlanDoc.Pages.Add();

                //counting elements in care plan
                int countElements = frameGenerator.carePlanResults.Count;

                //formatting string to wrap at the end of the page
                PdfStringFormat pdfStringFormat = new PdfStringFormat
                {
                    WordWrap = PdfWordWrapType.Word,
                    Alignment = PdfTextAlignment.Justify,
                    LineAlignment = PdfVerticalAlignment.Top
                };

                //formating PDf Output
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

                carePlanDoc.PageSettings.Margins.Top = 40;
                carePlanDoc.PageSettings.Margins.Left = 40;
                carePlanDoc.PageSettings.Margins.Right = 40;

                PdfFont fontPatientData = new PdfStandardFont(PdfFontFamily.Courier, 14, PdfFontStyle.Bold);
                PdfFont fontHeader = new PdfStandardFont(PdfFontFamily.Courier, 18, PdfFontStyle.Bold);

                Syncfusion.Drawing.RectangleF boundsPageOneRecommendations = new Syncfusion.Drawing.RectangleF(new Syncfusion.Drawing.PointF(10, 85), new Syncfusion.Drawing.SizeF(pageOne.Graphics.ClientSize.Width - 30, pageOne.Graphics.ClientSize.Height - 20));
                Syncfusion.Drawing.RectangleF bounds1PatientInfo = new Syncfusion.Drawing.RectangleF(new Syncfusion.Drawing.PointF(10, 35), new Syncfusion.Drawing.SizeF(pageOne.Graphics.ClientSize.Width - 30, pageOne.Graphics.ClientSize.Height - 20));

                //Heading's bounds is later used as bounds strings in rest of the dynamically created pages
                Syncfusion.Drawing.RectangleF boundsHeading = new Syncfusion.Drawing.RectangleF(new Syncfusion.Drawing.PointF(10, 10), new Syncfusion.Drawing.SizeF(pageOne.Graphics.ClientSize.Width - 30, pageOne.Graphics.ClientSize.Height - 20));

                //setting and loading strings for header and patient data info
                string header = "CARE PLAN FOR: \n";
                string patientData = App.PatientDataInstance.Patient.GivenName.ToUpper() + "    " + App.PatientDataInstance.Patient.FamilyName.ToUpper() + "\n" +
                              "SEX:" + App.PatientDataInstance.Patient.Sex + "  AGE:" + App.PatientDataInstance.Patient.Age +
                              "\n_________________________________________________________";

                pageOne.Graphics.DrawString(header, fontHeader, PdfBrushes.Crimson, boundsHeading, pdfStringFormat);
                pageOne.Graphics.DrawString(patientData, fontPatientData, PdfBrushes.Black, bounds1PatientInfo, pdfStringFormat);


                //accumulate and load first five results 
                string accumulate = "";
                for (int r = 0; r < countElements; r++)
                {
                    if (r < 5)
                    {

                        accumulate += "CODE: " + frameGenerator.carePlanResults[r].code + "   SECTION: " +
                                frameGenerator.carePlanResults[r].section + "   Date: " + frameGenerator.carePlanResults[r].date
                                + "\n" + "------------------------------------------------------------------------\n" +
                            "TITLE:  " + frameGenerator.carePlanResults[r].title + " \n" +
                                "PRIORITY: " + frameGenerator.carePlanResults[r].priority + "\n" +
                                frameGenerator.carePlanResults[r].recommendation + "\n\n";
                    }
                }

                pageOne.Graphics.DrawString(accumulate, font, PdfBrushes.Black, boundsPageOneRecommendations, pdfStringFormat);


                //putting 5 results on first and 7 on the rest of the pages
                int pagesNeeded = (countElements / 6); //calculate # of pages needed given the care plan elements
                string[] accumulatedStrings = new string[pagesNeeded]; //create array of strings size of pages needed

                //load array from generateString method that generates one accumulated string per 7 results
                int lowBound = 5;
                for (int pages = 0; pages < pagesNeeded; pages++)
                {
                    accumulatedStrings[pages] = GenerateString(lowBound);
                    lowBound += 7;
                }

                //for each 7 element that each position contains as one string generate and load pages with those strings
                for (int index = 0; index < accumulatedStrings.Length; index++)
                {
                    carePlanDoc.Pages.Add().Graphics.DrawString(accumulatedStrings[index], font, PdfBrushes.Black, boundsHeading, pdfStringFormat);
                }

                MemoryStream stream = new MemoryStream();
                carePlanDoc.Save(stream);

                carePlanDoc.Close(true);
                //find references to ISave in helper class
                Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("CarePlan.pdf", "application / pdf", stream);
            }

            bool osteo = false;
            bool tension = false;
            bool opioid = false;

            //if care plan contains guidelines from conditions boolian is set to true
            foreach (Result r in frameGenerator.carePlanResults)
            {
                osteo |= r.code == "M19";
                tension |= r.code == "V70";
                opioid |= r.code == "F11";
            }
            //if care plan contains specific condition guidelines send the code and count to Generate care plan
            if (osteo == true)
            {
                int count1 = frameGenerator.listOfSelectedOsteo.Count;

                GenerateCarePlan("M19", count1);
            }

            if (tension == true)
            {
                int count2 = frameGenerator.listOfSelectedTension.Count;
                GenerateCarePlan("V70", count2);
            }

            if (opioid == true)
            {
                int count3 = frameGenerator.listOfSelectedOpioid.Count;
                GenerateCarePlan("F11", count3);
            }
        }

        //creating separator for each recommendation
        public void AddSeparator(StackLayout stack)
        {
            BoxView separator = new BoxView
            {
                Color = Color.Black,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 2
            };

            stack.Children.Add(separator);
        }

        public void GenerateCarePlan(string code, int count)
        {
            if (frameGenerator.carePlanResults.Count != 0)
            {
                Frame holder = new Frame();
                StackLayout holderStack = new StackLayout();
                Label condition = new Label();
                condition.Text = code + " | SELECTED GUIDELINES: " + count;
                condition.FontAttributes = FontAttributes.Bold;
                holderStack.Children.Add(condition);
                holderStack.Spacing = 2;
                AddSeparator(holderStack);

                //for each element in care plan create title label and details button with read only editor
                foreach (Result guideLine in frameGenerator.carePlanResults)
                {
                    if (guideLine.code == code)
                    {
                        var editor = new Editor { IsReadOnly = true };
                        editor.Text = null;
                        editor.HeightRequest = 1;

                        Label guideLineLabel = new Label
                        {

                            Text = " \n " + (guideLine.title).ToUpper(),
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 16,
                            BackgroundColor = Color.LavenderBlush,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            WidthRequest = 800,
                            HeightRequest = 55,
                        };

                        Button guidelineButton = new Button
                        {
                            Text = char.ConvertFromUtf32(0x2193) + "DETAILS  ",
                            FontSize = 16,
                            BackgroundColor = Color.LavenderBlush,
                            CornerRadius = 0,
                            HorizontalOptions = LayoutOptions.EndAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            HeightRequest = 55

                        };
                        
                        //set label and button side to side and then to vertical stack
                        var titleStack = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal
                        };

                        titleStack.Children.Add(guideLineLabel);
                        titleStack.Children.Add(guidelineButton);

                        holderStack.Children.Add(titleStack);
                        holderStack.Children.Add(editor);

                        //handle click on down arrow details button
                        guidelineButton.Clicked += delegate (object sender, EventArgs e) { guidelineButton_Clicked(sender, e, guidelineButton, editor, char.ConvertFromUtf32(0x2193) + "DETAILS  ", guideLine.recommendation); };
                    }
                }

                //set frames to main stack
                holder.Content = holderStack;
                RecomStack.HorizontalOptions = LayoutOptions.StartAndExpand;
                RecomStack.VerticalOptions = LayoutOptions.StartAndExpand;
                RecomStack.WidthRequest = 1000;
                RecomStack.Children.Add(holder);
            }

            //on down arrow clicked - display recommendation in editor - change the arrow direction
            void guidelineButton_Clicked(object sender, System.EventArgs e, Button btn, Editor editor1, string title, string recommendation)
            {
                if (btn.Text == char.ConvertFromUtf32(0x2193) + "DETAILS  ")
                {
                    editor1.HeightRequest = 120;
                    editor1.FontSize = 18;
                    editor1.Text = recommendation;
                    btn.Text = char.ConvertFromUtf32(0x2191) + "DETAILS  ";
                }

                else if (btn.Text == char.ConvertFromUtf32(0x2191) + "DETAILS  ")
                {
                    editor1.Text = null;
                    editor1.HeightRequest = 1;
                    btn.Text = char.ConvertFromUtf32(0x2193) + "DETAILS  ";

                }
            }
        }

        //used for the pdf file
        public string GenerateString(int lowBound)
        {
            string accumulated = "";

            for (int r = lowBound; r < frameGenerator.carePlanResults.Count; r++)
            {
                if (r >= lowBound && r < lowBound + 7)
                {
                    accumulated += "CODE: " + frameGenerator.carePlanResults[r].code + "   SECTION: " +
                        frameGenerator.carePlanResults[r].section + "   Date: " + frameGenerator.carePlanResults[r].date
                        + "\n" + "------------------------------------------------------------------------\n" +
                        "TITLE:  " + frameGenerator.carePlanResults[r].title + " \n" +
                        "PRIORITY: " + frameGenerator.carePlanResults[r].priority + "\n" +
                        frameGenerator.carePlanResults[r].recommendation + "\n\n";
                }
            }
            return accumulated;
        }
    }
}
        
    


