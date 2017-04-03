# this script is designed to parse through each Unity HumanPose.muscle individually
# written by Marius Rubo

rm(list=ls())
animationNumber = 5
projectPath <- 'C:/Users/Public/Documents/Unity Projects/ERC WPC 2017-04-03'
filePath <- paste(projectPath, '/Assets/Animations/Animation', animationNumber, '.csv', sep="")

mocapData <- read.table(filePath,skip=1,dec=",", sep=";",na.strings="NA") # leave out first row, which stores data on position and rotation at program start
names(mocapData) <- c("bodyPosition_x", "bodyPosition_y", "bodyPosition_z", "bodyRotation_x", "bodyRotation_y", "bodyRotation_z", 
                      "bodyRotation_w", "bodyRotationEuler_x", "bodyRotationEuler_y","bodyRotationEuler_z",
"Spine_Front_Back","Spine_Left_Right","Spine_Twist_Left_Right","Chest_Front_Back","Chest_Left_Right","Chest_Twist_Left_Right","Neck_Nod_Down_Up",
"Neck_Tilt_Left_Right","Neck_Turn_Left_Right","Head_Nod_Down_Up","Head_Tilt_Left_Right","Head_Turn_Left_Right","Left_Eye_Down_Up","Left_Eye_In_Out",
"Right_Eye_Down_Up","Right_Eye_In_Out","Jaw_Close","Jaw_Left_Right","Left_Upper_Leg_Front_Back","Left_Upper_Leg_In_Out","Left_Upper_Leg_Twist_In_Out",
"Left_Lower_Leg_Stretch","Left_Lower_Leg_Twist_In_Out","Left_Foot_Up_Down","Left_Foot_Twist_In_Out","Left_Toes_Up_Down","Right_Upper_Leg_Front_Back",
"Right_Upper_Leg_In_Out","Right_Upper_Leg_Twist_In_Out","Right_Lower_Leg_Stretch","Right_Lower_Leg_Twist_In_Out","Right_Foot_Up_Down",
"Right_Foot_Twist_In_Out","Right_Toes_Up_Down","Left_Shoulder_Down_Up","Left_Shoulder_Front_Back","Left_Arm_Down_Up","Left_Arm_Front_Back",
"Left_Arm_Twist_In_Out","Left_Forearm_Stretch","Left_Forearm_Twist_In_Out","Left_Hand_Down_Up","Left_Hand_In_Out","Right_Shoulder_Down_Up",
"Right_Shoulder_Front_Back","Right_Arm_Down_Up","Right_Arm_Front_Back","Right_Arm_Twist_In_Out","Right_Forearm_Stretch","Right_Forearm_Twist_In_Out",
"Right_Hand_Down_Up","Right_Hand_In_Out","Left_Thumb_1_Stretched","Left_Thumb_Spread","Left_Thumb_2_Stretched","Left_Thumb_3_Stretched",
"Left_Index_1_Stretched","Left_Index_Spread","Left_Index_2_Stretched","Left_Index_3_Stretched","Left_Middle_1_Stretched","Left_Middle_Spread",
"Left_Middle_2_Stretched","Left_Middle_3_Stretched","Left_Ring_1_Stretched","Left_Ring_Spread","Left_Ring_2_Stretched","Left_Ring_3_Stretched",
"Left_Little_1_Stretched","Left_Little_Spread","Left_Little_2_Stretched","Left_Little_3_Stretched","Right_Thumb_1_Stretched","Right_Thumb_Spread",
"Right_Thumb_2_Stretched","Right_Thumb_3_Stretched","Right_Index_1_Stretched","Right_Index_Spread","Right_Index_2_Stretched","Right_Index_3_Stretched",
"Right_Middle_1_Stretched","Right_Middle_Spread","Right_Middle_2_Stretched","Right_Middle_3_Stretched","Right_Ring_1_Stretched","Right_Ring_Spread",
"Right_Ring_2_Stretched","Right_Ring_3_Stretched","Right_Little_1_Stretched","Right_Little_Spread","Right_Little_2_Stretched",
"Right_Little_3_Stretched")

# Make all variables numeric
for (var in 1:length(mocapData)){
mocapData[,var] <- as.numeric(as.character(mocapData[,var]))
}

# some interesting variables
plot(mocapData$bodyPosition_z) 
plot(mocapData$bodyRotationEuler_y)
plot(mocapData$Right_Forearm_Stretch[0:200])
plot(mocapData$Spine_Left_Right[0:200])
plot(mocapData$Left_Foot_Up_Down[0:200])




 