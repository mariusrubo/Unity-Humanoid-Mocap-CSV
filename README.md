# Purpose
With these scripts building on Final IK, you can make your character walk to an object, pick it up with both hands, transport it to a specified destination and place it there. The character will:
* grab the object in a specified manner, and hold it at a specified point in front of it
* smoothly avoid obstacles while walking to the object and to its destination


# Installation
## Prepare your character
* Download and import Final IK from Unity's AssetStore (90$)
* Attach the script "WalkToGoal.cs" from my repository "Unity-Humanoid-Walk-To-Goal" to your character. Make sure the character has an animator and the Locomotion controller.
* Attach scripts "Full Body Biped IK" and "Interaction System" to your character (drag character in?)
* Attach the script "PickUp.cs" to your character, and insert ...
* Find both hands from your character in the hierarchy, and attach "Hand Poser.cs" to both of them. You do not need to fill in a transform for "PoseRoot".
* Create two empty transforms, name them "HoldPoint" and "HoldPointOriginal", make them children of your character and place them in front of the character where it should hold objects (e.g. at (0, 1.0, 0.4)).

## Prepare Objects
* Make sure your object has a Rigidbody component and a box collider
* Attach the script "Interaction Object" to your object. Add a weight curve for "Position Weight" that starts at 0, reaches 1 at 0.6sec and returns to 0 at 1.2sec. This means that the character will need 0.6sec to move its hands to the correct positions on the objects. You can change these values, if this is too fast or slow for you.
* Still on your object's "Interaction Object", also add 3 multipliers, all computing from Position Weight: One for "Rotation Weight" (value here: 1), one for "Poser Weight" (value: 1), one for "Reach" (value: 0.2). You do not need to insert anything in "Other Look At Target", "Other Targets" and "Position Offset".
* Still on your object's "Interaction Object", also set an Event "Pause" (not "Pickup"!), and set its time to half of the weight curve length you specified above (0.6sec in the example above).
* Copy both of your character's hands, make these copies children of your object (they will become invisible). Remove "Hand Poser" from both hands, attach "Interaction Target" (you will now see their bones again) and select the corresponding effector type. Move these hands' copies in the right position, and manipulate the bones' angles until they resemble the posture in which your character should hold the object.
* Copy your object and name it something like "Object1Destination". This serves to visually indicate where your character should place the object. On this copy, delete everything you previously added to your object and has now been copied, too. Switch off the Mesh Renderer to make the copy invisible, but keep its box collider.  
* Don't specify your object or the transform it is placed on are as obstacles in the Navigation Mesh, because this will keep the character from walking close enough to the object to grab it.  

## Controll Transportation behavior
* Place the script "TransportInterface.cs" to any object in your scene. Insert the gaps accordingly.
* You can repeat the procedure described above for several objects, and reference them all in "TransportInterface.cs".
* Press Play, click on the buttons in Game View.

## Fine-tune grasping posture
* You may have noticed that defining the hand posture with high precision is difficult, because when clicking on hands copies' attached to the object, you only see their bones.
* To fine-tune the posture, first open TransportInterface.cs, find the line "if (cascade == 3) // walk to goal" and replace 3 with a larger number (e.g. 30). This will keep the character from walking towards the goal after it has grabbed the object.
* Play the scene and make the character grab the object by clicking on the according button. Then find the grabbed object in the hierarchy (it is now a child of the character!), click on its hand copies, and manipulate their bones. Your character will now move its actual hand accordingly.
* When you are satisfied with the posture, copy the hand copies while still in play mode.
* Stop play mode, paste the hand copies in the hierarchy and use these copies to replace the old hand copies that were attached to the object.
* In "TransportInterface.cs", reset the line "if (cascade == 3) // walk to goal" to what is was originally.

# Limitations

* The fingers do not move in the right positions if the scripts from my repository "Unity-Humanoid-Gestures" are being used in parallel. It is therefore best to switch these off.
* The picking-up procedure looks best if the object is positioned a little below the HoldPoint (e.g. o top of a table). If your character has to lift objects from the ground, some adjustments may be needed to make this look natural. If your character has to get objects from a heightened position, adjust the Weight Curves on your object's "Interaction Object" to not make the object move through the table or shelf it is positioned on.
* When placing the object to its destination, it is simply moved along a line from its HoldPoint in front of the character to its destination. Therefore, putting objects onto a heightened place may cause problems (e.g. if your character puts it on shelf above its head, the object will have to move through the shelf.)


# License
These scripts run under the GPLv3 license. See the comments inside the scripts for more details and ideas on how to adapt them for your own projects.
