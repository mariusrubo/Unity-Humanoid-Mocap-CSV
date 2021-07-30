# Purpose
These scripts allow you to record mocap data to, and play from .csv files. This approach facilitates parsing of body movement data in statistical software such as MATLAB or R.

![alt tag](https://github.com/mariusrubo/Unity-Humanoid-Mocap-CSV/blob/master/mocap.jpg)

# Installation
* Attach the script "RecordAnimation.cs" to the character that is being tracked.
* Press play, choose an animation number, press 'Start Rec' and "Stop Rec" to track the character's movement and "Save Anim" to store the data on your harddrive. A folder "Assets/Animations" will be created, and the data will be stored in there as a .csv file.
* Press 'Play Anim' to scroll through the recorded animation using a slider.
* You can load the recorded data into R using "MocapData.R".

# Limitations
* The script only captures movements that are completed inside the Update()-loop. This should work well for most motion capturing devices. If you capture movements processed in LateUpdate(), you will need to modify the scripts. For instance, if you use the Final IK package, the best option would be to use the solver's OnPostUpdate delegate. 
* While storing mocap data as .csv file facilitates statistical analyses, recorded animations can not easily be integrated and blended in a Unity controller component, as you can with .anim files.

# License
These scripts run under the GPLv3 license.
