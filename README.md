# healthcarecrossapp
Project 

**Cross-Platform App Universal Care Plan**

Description: The app is developed as a prototype for a healthcare organization
to help their practitioners generate care plan for patients with multiple conditions.

The objective of this organization is to have a tool that will generate one, universal care plan for their patients combining treatments/recommendations for them in one document.

**Main Page:**
<pre>
-	The app displays patient’s information along with the list of conditions she suffers from. 
	(This data is read from the provided JSON file). 

-	Each condition has the radio button across for practitioner to keep track of which conditions she/he visited. 

-	At the end of the page, there is a button “generate care plan” which displays chosen plan and then generates PDF and JSON files for distribution

</pre>

**Condition Pages:**

On the main page practitioner must visit each condition by clicking on them. The number of condition pages depends on the number of conditions the patient suffers from; these pages work the same way.

Once clicked on Condition 1 the practitioner will see the fixed frame with patient’s info on top and below it - multiple frames aligned vertically in a scroll view. These frames contain information about recommendation/treatment for patient; in other words, what is recommended to do if the patient has X condition. The frame contains information about the rank of this recommendation (high, medium, low), the date it has been published on cdc.gov, the disease code and more… button containing the link that redirects the user cdc.gov website to find out more. The frame also contains SELECT/UNSELECT buttons. If SELECT is pressed the treatment/recommendation is added to the care plan.

After choosing treatments/recommendations user must go back, move on to next condition and repeat the process.

(The data about conditions are also read from JSON file).

Care Plan Page:

Care plan page displays the patient information and chosen recommendations/treatments in one document organized by the disease name and code.

**Framework**

The project is written in Visual Studio, using Xamarin.Forms, C# and XAML files.
