using Godot;
using System;

public partial class snowballmanager : Node2D
{

    [Export] Snowball sball;
    [Export] Node2D ballui;
    [Export] Label radiuslabel;

    public void ApplyForce(Vector2 dir){

        sball.SetSnowballCameraAsCurrent();
        sball.LinearVelocity = dir;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        ballui.GlobalPosition = sball.GlobalPosition;
        radiuslabel.Text = $"{Mathf.RoundToInt(sball.colradiuss)}";
    }

}
