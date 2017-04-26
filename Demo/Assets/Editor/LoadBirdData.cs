using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEditor;



public class LoadBirdData : EditorWindow {
	// Serialzed Fields
	[SerializeField]
	TextAsset csvFile;

	// Non-Serialized Fields
	string buttonText, status = null;
	bool loaded, running, finished = false;
	int pointNumber;
	float percent;
	string[,] grid;
	DataPoint[] points;

	// Methods

	// Creates the menu item to open the window
	[MenuItem("CSC 450/Load Bird Data")]
	static void Init() {
		// Get existing open window or if none, make a new one:
		Debug.Log("Opening \"Load Bird Data\" window");
		LoadBirdData window = (LoadBirdData)EditorWindow.GetWindow(typeof(LoadBirdData));
		window.Show();
	}

	// Executes when the window opens and is in focus
	void OnGUI() {
		// Updates states for the button text and status indicator based on progress
		if (!loaded) {
			buttonText = "Load CSV File";
			status = "Idle";
		} else if (loaded && !running) {
			buttonText = "Populate Data Points";
		} else if (finished) {
			buttonText = "";
		}

		// Button to initiate loading the data from the csv file into a 2D string array
		if (!finished) {
			if (GUILayout.Button (buttonText)) {
				Debug.Log ("Button clicked");
				if (!finished) {
					if (loaded && !running) {
						Debug.Log ("Loading data into data points");
						running = true;
						status = "Setting point data...";
						LoadData (); // Method to populate the data into the data points
						Debug.Log ("Data loaded into data points");
						status = "Complete";
					} else {
						Debug.Log ("Loading CSV file into 2D string array)");
						loaded = true;
						status = "Loading CSV data...";
						LoadCSV (); // Method to read the csv file into a 2D string array
						status = "CSV data successfully loaded. Ready to set data.";
						Debug.Log ("Finished load CSV data");
					}
				}
			}
		}

		// Displays progress and status information
		GUILayout.Label ("Status: " + status);
		// Only displays when data is loaded and being populated into the data points
		if (loaded && running) {
			GUILayout.Label ("Point Number: " + pointNumber);
		}
	}

	// Load the point and bird data from the 2D string array into each point
	void LoadData() {
		// Iterate through all data points in the 2d array
		for (int y = 1; y < grid.GetUpperBound(1); y++) {
			Debug.Log ("Processing data on point: " + y); 

			// Set point number
			if (grid[0, y] != null) {
				pointNumber =  Int32.Parse(grid [0, y]);
			}
				
			Debug.Log ("Point Number: " + pointNumber);

			// Locate and reference the data point object in the scene

			GameObject pointObj = GameObject.Find("GIS_Point (" + pointNumber + ")");
			
			// Find the number of positive counts for birds
			int size = 0;
			for (int j = 4; j < grid.GetUpperBound (0); j++) {
				string sNum = grid [j, y]; // Stores the bird count as a string
				if (sNum != null && sNum.Length > 0) {
					Debug.Log ("Count (string): " + sNum);
					int num = Int32.Parse (sNum); // Converts the string to an Int32
					if (num > 0) { // Increases the size when the count is > 0
						size++;
					}
				}
			}
			Debug.Log ("Point " + pointNumber + " Bird Array Size: " + size);

			// Add the DataPoint script component to the object and reference it
			DataPoint p = pointObj.AddComponent<DataPoint>();
			p.sizeArrays (size);
			p.id = pointNumber; // Set the point ID
			p.coordinates = grid[2, y] + ", " + grid[3, y]; // Set the point coordinates


			int i = 0; // Stores the current position in the Entry array for writing new Entry objects
			// Iterate through the bird data for the point
			for (int x = 4; x < grid.GetUpperBound(0); x++) {
				//Get the bird count for the bird code
				string sNum = grid[x, y];
				if (sNum != null && sNum.Length > 0) {
					int num = Int32.Parse (sNum);
					if (num > 0) { // If the count is > 0
						string code = grid [x, 0]; // Get the code that coresponds with the count
						//birds[entryIndex].setEntry(code, num); // Create an Entry for the point with the code and number seen
						p.SetBirdCode (i, code);
						p.SetBirdCount (i, num);
						i++; // Increment the Entry array index
					}
				}
			}
		}

		// Set finished to true
		finished = true;
	}


	// Loads the contents of the csv file into a 2D string array
	void LoadCSV()
	{
		grid = SplitCsvGrid(csvFile.text);
	}

	static string[,] SplitCsvGrid(string csvText) {
		string[] lines = csvText.Split("\n"[0]); 

		// finds the max width of row
		int width = 0; 
		for (int i = 0; i < lines.Length; i++) {
			string[] row = SplitCsvLine( lines[i] ); 
			width = Mathf.Max(width, row.Length); 
		}

		// creates new 2D string grid to output to
		string[,] outputGrid = new string[width + 1, lines.Length + 1]; 
		for (int y = 0; y < lines.Length; y++) 	{
			string[] row = SplitCsvLine( lines[y] ); 
			for (int x = 0; x < row.Length; x++) {
				outputGrid[x,y] = row[x]; 

				// This line was to replace "" with " in my output. 
				// Include or edit it as you wish.
				outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\"");
			}
		}

		return outputGrid; 
	}


	// splits a CSV row
	static string[] SplitCsvLine(string line)
	{
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
			@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", 
			System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
			select m.Groups[1].Value).ToArray();
	}
}
