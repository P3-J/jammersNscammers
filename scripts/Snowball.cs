using Godot;
using System;

public partial class Snowball : RigidBody2D
{


    [Export] Camera2D sballCamera;



    public void SetSnowballCameraAsCurrent(){
        sballCamera.MakeCurrent();
    }

}
