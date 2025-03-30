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
    [Export] Timer slowmotimer;
    [Export] Camera2D camundercannon;

    public enum cannonState {aiming, shooting, locked}
    public cannonState currentCannonState;
    bool launchedBall;
    Globals glob;


    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");
        launchbar.Visible = false;
        launchbar.Value = 0;
        camundercannon.Zoom = new Vector2(1,1);
         
        currentCannonState = cannonState.aiming;
        camundercannon.MakeCurrent();
        


    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        if (glob.firstboot){
            currentCannonState = cannonState.locked;
        }

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

        if (currentCannonState == cannonState.locked){
            return;
        }

        if (Input.IsActionJustPressed("lmb")){
            launchbar.Visible = true;
            StartChargingCannonRight();
        }

        if (Input.IsActionJustReleased("lmb")){
            currentCannonState = cannonState.locked;
            if (!launchedBall){
                glob.playsound("boom");
                Engine.TimeScale = 0.1f;
                launchSnowBall();
                launchedBall = true;
                randomSkyItemSpawnTimer.Start(); 
                slowmotimer.Start();
                
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

        if (launchbar.Value < 50){
            return 2500;
        }
        if (launchbar.Value > 64){
            return 4500;
        }
        return 7500 ;
    }

    private void _on_slowmotimer_timeout(){
        GD.Print("time back");
        Engine.TimeScale = 1f;
        this.Visible = false;
    }

}
