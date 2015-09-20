Ground Running - Visual Studio & Stash Repo Automation
======================================================

Automates the tasks of creating a VisualStudio solution (including Test Project), Nuspec, Build files (PoshBuild), and Stash Repository.  

Hit the "Ground Running" :-)


Getting Started
---------------
* Requires Visual Studio 2013 (requires System.Windows.Interactivity version 4.5  and Visual Studio Automation dlls)
- Clone the Ground-Running repo and build in Visual Studio.
- Set the GroundRunning.GUI as the start-up project and run the app.  (You can also grab the files from the bin folder and run the "GroundRunning.GUI.exe" from there.  I'll get to packaging them up eventually).
- Enter your required project name and local repo path (a folder will be created within the repo path based on the project name).
- Select the optional extras, eg. Nuspec, Poshbuild, etc.
- If creating a Stash Repo you will need to enter the Stash URLs and the required Project Key. You can get this by going into Stash and looking at the Project Settings for the project you want to use (or in the URL).
- Enter your user name and password for Stash
- Click "Create" and wait for things to happen.  When the spinner has stopped everything should be created (Unless they weren't.  Error handling still to come!).  


Configuration
-------------
- The App.config file contains the defaults for Folder Locations, Templates, and Stash settings.
- The "Resources" folder contains the templates for Nuspecs, PoshBuild and Git Ignore files.


Limitations/Issues
------------------
- Currently only creates a Class Library project and an NUnit based test project.  Optional templates to come in future versions.
- Only basic error handling. Error messages are reported in UI.  All others logged to bin\Logs\. 
- Nuget package restore not working correctly on initial Visual Studio Build after creation.  Poshbuild/command-line build should be OK.  
- Need to install NUnit Runners  Manually (if you have a test project).
- Requires user to enter Stash credentials.  
- Console application "GroundRunning.exe" does not create Stash Repo yet.

Known Issues/Bugs
-----------------
- Cannot handle a Project Name with 4 or more dots in it which ends in .Web eg. My.New.Project.Web  


Coming Soon (Backlog)
--------------------
- Select a template based on installed Visual Studio templates (eg. Web.API project)
- Install the NUnit Runners Nuget Package if Test Project selected. 
- Add the "Run-Tests" task to PoshBuild build.ps1 (if Test Project selected).
- Better handling of Stash credentials. Should be possible to use SSH Key. Further investigation required.
- Finish work on Console app.  so things can be scripted, ie. Automate the Automation!
- Package up the app for better distribution.


Other aspirational features
---------------------------
- Automate Bamboo build creation
- Automate Octopus things
- More options around Nuspec creation (eg. configure default authors, specify dependencies, etc.)
- Better PoshBuild automation (get latest version automatically).
- Web interface. ie. do the automation stuff on a server and the end-user just clone the generated repo from Stash.
