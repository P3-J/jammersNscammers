using Godot;
using System;

public partial class Snowball : RigidBody2D
{


    [Export] Camera2D sballCamera;
    [Export] RayCast2D groundcheck;
    [Export] Timer growtimer;
    [Export] CollisionShape2D snowballcol;
    [Export] Sprite2D snowballsprite;

    public float colradiuss;


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

        if (collider is StaticBody2D && growtimer.IsStopped()){
            growtimer.Start();
        }

    }

    public override void _Input(InputEvent @event)
    {
        Godot.Collections.Array<Node2D> colbodies = GetCollidingBodies(); 
        GD.Print(colbodies.Count );

        if (Input.IsActionJustPressed("jump") && colbodies.Count != 0){
             LinearVelocity = new Vector2(LinearVelocity.X - 10f, LinearVelocity.Y - 700);
        }
    }


    private void _on_grow_snoball_timeout(){
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

        groundcheck.Scale *= growthFactor;
        LinearVelocity = new Vector2(LinearVelocity.X - 100f, LinearVelocity.Y + 100);
    }
        
        
    

}
