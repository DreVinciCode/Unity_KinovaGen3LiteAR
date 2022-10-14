# KinovaAR

Welcome to project KinovaAR! (Figuring out a better name still) 
In this project, I am communicating robotic arm information to an augmented reality device and visualize that information to improve human-robot collaborative tasks. Users can provide input information to update the robot's motion plan, perceptions, and contraints with mixed-reality.

Features in this project:
- Voice commands to execute planned trajectories.
- Eye gaze control to teleop robot end effector to target areas.
- Hand Gesture control to telop robot end effector to mirror user's hand location and pinch gesture to control robotic gripper.
- Visualizing current and future planned robotic joint states.
- Visualizing Robot's working space to convey full reach of the arm. 
- Interactive virtual marker for target goal pose.  


Future items to implement in this project include:
- Enable users to add and control boundary zones to constrain the arm to a specified working area.
- Visualizing force/torque sensors 
- 

This project is tied to its ROS component linked here, [ROS_KinovaGen3LiteAR](https://github.com/DreVinciCode/ROS_KinovaGen3LiteAR)

Tools used in this project:
- Unity 2020.3.32f https://unity3d.com/unity/whats-new/2020.3.32
- Vuforia Engine 10.4.4 https://developer.vuforia.com/downloads/sdk 
- MRTK 2.7.3 https://github.com/microsoft/MixedRealityToolkit-Unity/releases/tag/v2.7.3
- ROS-Sharp for UWP/HoloLens https://github.com/EricVoll/ros-sharp

Hardware in this project:
- Kinova Gen3 Lite https://www.kinovarobotics.com/product/gen3-lite-robots
- Hololens 2 https://www.microsoft.com/en-us/hololens/hardware




## Demos

![Alt text](Demos/interactive_marker.gif)
<br/> Controlling robot target goal pose with a virtual interactive marker (blue arrow) and visualize the resulting planned trajectory. 

![Alt text](Demos/cursive_demo.gif)
<br/> Controlling robot end effector for writing purposes using hand control

![Alt text](Demos/SafetyZoneKinova.gif)
<br/> Visualizing the working range of the arm

![Alt text](Demos/currentfuture.gif)
<br/> Visualizing the current and future arm trajectory 

![Alt text](Demos/handgesturecontrol.gif)
<br/> Teleop the arms end effector with hand gestures

