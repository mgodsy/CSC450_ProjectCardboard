# CSC450_ProjectCardboard Readme
Data Visualization via Google Cardboard

### Links to run Unity VR:

[Unity 5.5.0](https://unity3d.com/get-unity/download?thank-you=update&download_nid=44093&os=Win)

[Android Studio](https://dl.google.com/dl/android/studio/install/2.2.3.0/android-studio-bundle-145.3537739-windows.exe)

[Google VR SDK for Unity (ver. 1.0.3)](https://github.com/googlevr/gvr-unity-sdk/archive/f391c2436426857899af1c37f0720b3985631eb3.zip)


### Instructions:

### Install Unity 
  *(Make Sure proper target builds are selected on install)*
   * You will need to make a Unity account and select a license (Personal)

### Install Android Studio

### Extract Zip 
   * The key file in this zip is the GoogleVRForUnity.unitypackage file. 
   * We’re going to add that to Unity later, so remember where you extract it.

### Importing Google VR to unity:
   * Open Unity
   * Create a new project
   * Go to Assets -> Import Package -> Custom Package -> 
   * Browse to where you stored GoogleVRForUnity.unitypackage
   
### Note:
*A dialog box will pop up with the install, once it’s done make sure to click into Unity, that should bring up a selection tool with options on which builds you would like to install with the Google SDK.*

## Steps after Downloading the Repo

### Opening the project
 * Open Unity 5.5.0f3
 * Select Open (at the top)
 * Navigate to where you stored the repo on your file system
 * Click on the Demo folder once and hit "Select Folder"
 * Unity is Going to install all of the packages for the project
 * Once the Project loads there may be Errors, if they are Google asset related you will need to import the enitre GoogleVR.unity package again. 
 * There will also be an error related to the code, double clicking on it will bring you to Visual Studios editor, there is a line of cade that is trying to return a false value, comment the return out and save. - This happens often and the saved file somehow doesn't stick
 * after all that should be good to run
 
