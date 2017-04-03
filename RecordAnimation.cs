using UnityEngine;
using System.Collections;
using System.IO; // needed for reading and writing .csv
using System.Text; // for csv 

[RequireComponent(typeof(Animator))]
public class RecordAnimation : MonoBehaviour
{
    //public // skeletonRoot does not need to be inserted - it is simply the main transform
    Transform skeletonRoot;
    HumanPose currentPose = new HumanPose(); // keeps track of currentPose while animated
    HumanPose poseToSet; // reassembles Pose from .csv data
    Animator animator;
    HumanPoseHandler poseHandler; // to record entire animation

    int nframes = 108000; // max number of frames until unity stops recording automatically: currently set to 30min (30*60*60 = 108000)
    float[] currentPoseFloats; // sort current pose values as one array simply containing floats
    float[,] animationFloats; // = new float[nframes, 102]; // stack all currentPoseFloats in one array
    int AnimationNumber;
    int counterRec = 0; // count number of frames

    Vector3 PositionAtStart;
    Vector3 PoseAtStart;
    Quaternion RotationAtStart;

    // for setting a pose
    Vector3 posePosition;
    Vector3 poseRotationEuler;
    float[] poseMuscles;
    bool recordPoses = false;
    bool Reapply; // the recorded animation
    int counterPlay; // like counter, but counts animation playback frames
    float[] musclesPlayback; // zwischenspeichert values for muscles, cause they cannot be inserted directly

    void Start()
    {
        animationFloats = new float[nframes, 102];
        currentPoseFloats = new float[102];

        animator = GetComponent<Animator>();
        skeletonRoot = transform; // I take main transform as root (not "Hips" as I first guessed). Not sure if this works for all agents or only Autodesk.
        poseHandler = new HumanPoseHandler(animator.avatar, skeletonRoot);

        poseMuscles = new float[92];
        musclesPlayback = new float[92];
    }

    // Update is called once per frame
    // even running this in LateUpdate does not capture IK
    void LateUpdate()
    {
        if (recordPoses) { RecordPoses(); } 
        if (Reapply) { ReapplyAnimation(); }
    }

    void OnGUI()
    {
        // change animation number using buttons (text field looked less elegant and I could not set it up via code)
        GUILayout.BeginArea(new Rect(240, 10, 100, 200));
        if (GUILayout.Button("+")) { AnimationNumber++; } 
        if (GUILayout.Button("Animation "+AnimationNumber.ToString())) { }
        if (GUILayout.Button("- ")) { AnimationNumber--; }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(350, 10, 100, 200));
        if (recordPoses == false) { if (GUILayout.Button("Start Rec")) { recordPoses = true; counterRec = 0; }  }
        if (recordPoses == true) { if (GUILayout.Button("Stop Rec")) { recordPoses = false; } }
        if (GUILayout.Button("Play Anim")) { Reapply = true; counterPlay = 0; }
        if (GUILayout.Button("Save Anim")) { SaveAnimation(); }
        if (GUILayout.Button("Load Anim")) { LoadAnimation(); }
        //if (GUILayout.Button("Note Muscles")) { NoteMuscles(); }
        GUILayout.EndArea();
        if (Reapply) { counterPlay = (int)GUI.HorizontalSlider(new Rect(30, 120, 400, 30), counterPlay, 1.0F, counterRec-1); } // start at 1 -> line 0 just sets position
    }

    // to get a simple file containing the names of all muscles
    void NoteMuscles()
    {
        string[] musclename = HumanTrait.MuscleName; // show names of muscles
        string path = Directory.GetCurrentDirectory() + "/Assets/Animations/MuscleNames.csv";
        TextWriter sw = new StreamWriter(path);
        string Text = "";
        for (int i = 0; i < HumanTrait.MuscleCount; i++) {Text = Text + musclename[i].ToString() + ";";}
        sw.WriteLine(Text);
        sw.Close();
    }
    
    // Save entire animation as .csv (note: Streamwriter is much faster than "File.WriteAllText(path, Text);")
    public void SaveAnimation()
    {
        string path = Directory.GetCurrentDirectory();
        path = path + "/Assets/Animations";
        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); } // create "Animations" folder if it does not exist
        path = path + "/Animation" + AnimationNumber + ".csv";
        TextWriter sw = new StreamWriter(path);
        string Line;

        for (int frame = 0; frame < counterRec; frame++) // run through all frames 
        {
            Line = "";
            for (int i = 0; i < currentPoseFloats.Length; i++) // and all values composing one Pose
            {
                Line = Line + animationFloats[frame, i].ToString() + ";";
            }
            sw.WriteLine(Line);
        }
        sw.Close();
    }

    public void LoadAnimation() // refill animationFloats with values from .csv-Files
    {
        string path = Directory.GetCurrentDirectory();
        path = path + "/Assets/Animations";
        path = path + "/Animation" + AnimationNumber + ".csv";
        Debug.Log("Loading " + path.ToString());

        if (File.Exists(path))
        {
            animationFloats = new float[nframes, 102];
            StreamReader sr = new StreamReader(path);
            int frame = 0;
            while (!sr.EndOfStream)
            {
                string[] Line = sr.ReadLine().Split(';');
                for (int column = 0; column < Line.Length - 1; column++)
                {
                    animationFloats[frame, column] = float.Parse(Line[column]);
                }
                frame++;
                counterRec = frame; // remember length of data, to correctly display slider
            }
        }
        else { Debug.Log("File not found"); }
    }

    public void RecordPoses()
    {
        poseHandler.GetHumanPose(ref currentPose);

        if (counterRec == 0) // first note down position and rotation at start
        {
            PoseAtStart = currentPose.bodyPosition; // somehow need to note down pose at start, reference all other poses to this
            PositionAtStart = transform.position;
            RotationAtStart = transform.rotation;

            // note down all values defining a pose
            currentPoseFloats[0] = PositionAtStart.x;
            currentPoseFloats[1] = PositionAtStart.y;
            currentPoseFloats[2] = PositionAtStart.z;

            currentPoseFloats[3] = RotationAtStart.x;
            currentPoseFloats[4] = RotationAtStart.y;
            currentPoseFloats[5] = RotationAtStart.z;
            currentPoseFloats[6] = RotationAtStart.w;

            currentPoseFloats[7] = RotationAtStart.eulerAngles.x; // also save rotation as Vector3
            currentPoseFloats[8] = RotationAtStart.eulerAngles.y;
            currentPoseFloats[9] = RotationAtStart.eulerAngles.z;

            for (int i = 0; i < 102; i++) { animationFloats[counterRec, i] = currentPoseFloats[i]; }
            counterRec++;
        }

        if (counterRec > 0 && counterRec < nframes) // then note down poses
        {
            // note down all values defining a pose
            currentPoseFloats[0] = currentPose.bodyPosition.x - PoseAtStart.x; 
            currentPoseFloats[1] = currentPose.bodyPosition.y; // - PositionAtStart.y; // no need to correct y component. I don't know why.
            currentPoseFloats[2] = currentPose.bodyPosition.z - PoseAtStart.z;

            currentPoseFloats[3] = currentPose.bodyRotation.x;
            currentPoseFloats[4] = currentPose.bodyRotation.y;
            currentPoseFloats[5] = currentPose.bodyRotation.z;
            currentPoseFloats[6] = currentPose.bodyRotation.w;

            currentPoseFloats[7] = currentPose.bodyRotation.eulerAngles.x; // also save rotation as Vector3
            currentPoseFloats[8] = currentPose.bodyRotation.eulerAngles.y;
            currentPoseFloats[9] = currentPose.bodyRotation.eulerAngles.z;

            // note down all muscles
            for (int i = 0; i < 92; i++) { currentPoseFloats[i + 10] = currentPose.muscles[i]; }

            for (int i = 0; i < 102; i++) { animationFloats[counterRec, i] = currentPoseFloats[i]; }
            counterRec++;
        }

    }

    // loop through array and apply poses one after another. 
    public void ReapplyAnimation()
    {
        if (counterPlay == 0) // first set position and rotation
        {
            animator.enabled = false; // first set off animator: otherwise, it will rotate towards goal again
            Vector3 PositionAtStart = new Vector3(animationFloats[0, 0], animationFloats[0, 1], animationFloats[0, 2]);
            transform.position = PositionAtStart;
            Quaternion RotationAtStart = new Quaternion(animationFloats[0, 3], animationFloats[0, 4], animationFloats[0, 5], animationFloats[0, 6]);
            //transform.rotation = RotationAtStart;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // actually no need to reference to initial rotation

            counterPlay++;
        }

        if (counterPlay < nframes)
        {
            poseToSet = new HumanPose();
            float[] currentPose = new float[102];
            for (int i = 0; i < 102; i++) { currentPose[i] = animationFloats[counterPlay, i]; } // get only single pose from animation data
                                                                                          
            poseToSet.bodyPosition = new Vector3(currentPose[0], currentPose[1], currentPose[2]); // retrieve position from stored data
            
            poseToSet.bodyRotation = new Quaternion(currentPose[3], currentPose[4], currentPose[5], currentPose[6]); // retrieve position

            //for (int i = 0; i < 92; i++) { poseToSet.muscles[i] = currentPose[i + 10]; } // does not work
            for (int i = 0; i < 92; i++) { musclesPlayback[i] = currentPose[i + 10]; } // retrieve muscles
            poseToSet.muscles = musclesPlayback;
            poseHandler.SetHumanPose(ref poseToSet);

            //counterPlay++; // enable this to let animation run at original speed
        }
    }
}


