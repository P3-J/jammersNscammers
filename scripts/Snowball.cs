using Godot;
using System;

public partial class Snowball : RigidBody2D
{


    [Export] Camera2D sballCamera;
    [Export] RayCast2D groundcheck;
    [Export] Timer growtimer;
    [Export] CollisionShape2D snowballcol;
    [Export] Sprite2D snowballsprite;
    [Export] Timer stucktimer;

    public float colradiuss;
    bool boostedDown = false;
    Vector2 lastpos;
    Godot.Collections.Array<Node2D> colbodies;


    public void SetSnowballCameraAsCurrent(){
        sballCamera.MakeCurrent();
    }

    public override void _Ready()
    {
        base._Ready();
        colradiuss = (float)snowballcol.Shape.Get("radius");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        GodotObject collider = groundcheck.GetCollider();
        colbodies = GetCollidingBodies(); 

        if (stucktimer.IsStopped()){
            GD.Print("started");
            stucktimer.Start();
            lastpos = GlobalPosition;
        }

        if (colbodies.Count >= 1){
            boostedDown = false;
        }
        
        if (collider is StaticBody2D &&growtimer.IsStopped()){
            StaticBody2D col = (StaticBody2D)collider;
            if (col.IsInGroup("hill")){
                GrowBall();
                growtimer.Start();
            }
        }

    }

    private void _on_stucktimer_timeout(){
        GD.Print(lastpos, GlobalPosition);
        if (lastpos == GlobalPosition){
            GetTree().ChangeSceneToFile("res://scenes/world.tscn");
        }
    }

    public override void _Input(InputEvent @event)
    {
        
        if (Input.IsActionJustPressed("jump") && colbodies.Count == 0 && !boostedDown){
             LinearVelocity = new Vector2(LinearVelocity.X + 400, LinearVelocity.Y + 700);
             boostedDown = true;
        }
    }

    void GrowBall(){
        float growthFactor = 1.05f;
        snowballsprite.Scale = new Vector2(snowballsprite.Scale.X * growthFactor, snowballsprite.Scale.Y * growthFactor);

        
        colradiuss = (float)snowballcol.Shape.Get("radius");

        //snowballcol.Shape.Set("radius", colRadius * growthFactor);

        Tween tween1 = GetTree().CreateTween();
		tween1.TweenProperty(snowballcol.Shape, "radius", colradiuss * growthFactor, 0.5)
			.SetTrans(Tween.TransitionType.Linear);
        
        float radiusChange = (colradiuss * growthFactor) - colradiuss;
        GlobalPosition += new Vector2(0, -radiusChange); 

        Tween tween2 = GetTree().CreateTween();
		tween2.TweenProperty(sballCamera, "zoom", sballCamera.Zoom *= 0.98f, 1)
			.SetTrans(Tween.TransitionType.Linear);

        Mass += 0.5f;

        groundcheck.Scale *= growthFactor;
        LinearVelocity = new Vector2(LinearVelocity.X - 100f, LinearVelocity.Y + 100);
    }
        
}
