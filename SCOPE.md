# CSC450_ProjectCardboard
* *Data Visualization via Google Cardboard* *

### Introduction

Current applications for Google Cardboard are limited given the newness of the technology and the hobbyist approach to application development. There are a few options for general GIS information, namely maps and images such as Google Streetview, but even these are very limited.

### Goals

Establish a framework for importing data and creating meshes on a given set of data
Render an interactive diorama model of the given data usable in the Google Cardboard environment
Allow a user to scale between diorama view and world view to explore the environment at data points provided from GIS import

### Objective

To establish a framework for GIS data visualization in a VR environment.

### Scope

The established framework can be used for further application in scientific research, education and game design providing a 3D visualization of any provided GIS data. Implementation may provide low cost solutions for researchers and educators and create a baseline product for further game development.
It is not within the scope of this project for any additional data sets to be implemented or tested. As a baseline, the team will work within one data set and a follow-up project may be created to allow for multiple data sets and expansion.
The application will run on Android (KitKat 4.4 and up) for Google Cardboard. 

### *Baseline Scope: (Epic I)* – 
At baseline, the team will utilize Unity to create a 3D mesh of the environment as a top-down diorama that a user will be able to navigate around as if walking around a table. We will then load data points from the provided GIS data into the mesh.  This map will be textured and colored appropriately based on the given GIS data.  Data points will be loaded into their appropriate locations based on the mesh and be capable of basic interaction.
Before moving on, load times should be minimal (dependent on hardware and internet/data connection) and interaction within the environment will be fully functional.

### *Level 2 Scope: (Epic II)* –
Upon completion, data points will be visitable in the world scale with the user having the ability to return to diorama view when they like. Appropriate contextual content (based on the given data set) will be located at each data point and a HUD will provide the user with feedback. The world will be fully textured with world objects placed in their proper place. 

### *Level 3 Scope: (Epic III)* -
Before finalizing, 3D models of data point objects will be added to each location in “hidden” places. Game-like controls will be added to the game allowing the user to take photos/screenshots of the birds/contextual data things in-game. Photos taken by user will be subject to a basic scoring algorithm and the HUD will provide the user with proper feedback. The photos taken by the user will be stored locally with the ability to share via common social media platforms.
