# CSC450_ProjectCardboard Readme
Data Visualization via Google Cardboard

### Links to run Unity VR:

[Unity 5.6.0](https://store.unity.com/download/thank-you?thank-you=personal&os=win&nid=237)

[Android SDK Tools](https://dl.google.com/android/repository/tools_r25.2.3-windows.zip)

[Google VR SDK for Unity](https://github.com/googlevr/gvr-unity-sdk/raw/master/GoogleVRForUnity.unitypackage)


### Instructions:

### Install Unity 
  *(Make Sure proper target builds are selected on install)*
   * You will need to make a Unity account and select a license (Personal)

### Extract Android Tools from zip
   * Extract the tools to their own folder
   * Make sure to remember where you extract these tools, we'll be using them to install APKs

### Importing Google VR to unity:
   * Open Unity
   * Create a new project
   * Go to Assets -> Import Package -> Custom Package -> 
   * Browse to where you stored GoogleVRForUnity.unitypackage
   
### Note:
*A dialog box will pop up with the install, once it’s done make sure to click into Unity, that should bring up a selection tool with options on which builds you would like to install with the Google SDK.*

## Steps after Downloading the Repo

### Opening the project
 * Open Unity 5.6.0f3
 * Select Open (at the top)
 * Navigate to where you stored the repo on your file system
 * Click on the Demo folder once and hit "Select Folder"
 * Unity is Going to install all of the packages for the project
 * Once the Project loads there may be Errors, if they are Google asset related you will need to import the enitre GoogleVR.unity package again. 
 * There will also be an error related to the code, double clicking on it will bring you to Visual Studios editor, there is a line of cade that is trying to return a false value, comment the return out and save. - This happens often and the saved file somehow doesn't stick
 * after all that should be good to run
 
## Unity Extensions utilized in this project:
 * Terralands 2.0
 * GazeClick 1.3.0
