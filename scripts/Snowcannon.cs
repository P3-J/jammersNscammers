using Godot;
using System;

public partial class Snowcannon : Node2D
{

    [Export] Marker2D target;
    [Export] Marker2D launchspot;
    [Export] Node2D launcherparent;
    [Export] PackedScene snowball;
    [Export] TextureProgressBar launchbar;
    [Export] Timer randomSkyItemSpawnTimer;

    private enum cannonState {aiming, shooting, locked}
    private cannonState currentCannonState;
    bool launchedBall;


    public override void _Ready()
    {
        base._Ready();
        launchbar.Visible = false;
        launchbar.Value = 0;
        currentCannonState = cannonState.aiming;

    }


    public override void _Process(double delta)
    {
        base._Process(delta);


        if (currentCannonState == cannonState.aiming){
            var mousePos = GetGlobalMousePosition();
            launcherparent.LookAt(mousePos);
            float newrot = launcherparent.RotationDegrees;
            newrot = Mathf.Clamp(newrot, -85, 10);
            launcherparent.RotationDegrees = newrot;
        }

    }


    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionJustPressed("lmb")){
            launchbar.Visible = true;
            StartChargingCannonRight();
        }

        if (Input.IsActionJustReleased("lmb")){
            if (!launchedBall){
                launchSnowBall();
                launchedBall = true;
                randomSkyItemSpawnTimer.Start();
            }
        }
    }

    private void StartChargingCannonRight(){
        if (launchedBall){
            launchbar.Visible = false;
            return;
        }
        Tween tween = GetTree().CreateTween();
		tween.TweenProperty(launchbar, "value", 100, 0.5)
			.SetTrans(Tween.TransitionType.Linear);
		tween.Finished += StartChargingCannonLeft;
    }

	private void StartChargingCannonLeft()
	{
        if (launchedBall){
            launchbar.Visible = false;
            return;
        }
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(launchbar, "value", 0, 0.5)
			.SetTrans(Tween.TransitionType.Linear);
		tween.Finished += StartChargingCannonRight;
	}

    private void launchSnowBall()
    {
        snowballmanager sballscene = snowball.Instantiate<snowballmanager>();
        GetTree().CurrentScene.AddChild(sballscene);
        sballscene.GlobalPosition = launchspot.GlobalPosition;

        Vector2 direction = (target.GlobalPosition - launchspot.GlobalPosition).Normalized();
        sballscene.ApplyForce(direction * GetPowerBasedOnLaunchBar());

    }

    private int GetPowerBasedOnLaunchBar(){

        if (launchbar.Value < 33){
            return 1000;
        }
        if (launchbar.Value < 79){
            return 2000;
        }
        return 6000;
    }

}
