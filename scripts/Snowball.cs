using Godot;
using System;

public partial class Snowball : RigidBody2D
{


    [Export] Camera2D sballCamera;
    [Export] RayCast2D groundcheck;
    [Export] Timer growtimer;
    [Export] CollisionShape2D snowballcol;
    [Export] Sprite2D snowballsprite;



    public void SetSnowballCameraAsCurrent(){
        sballCamera.MakeCurrent();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        GD.Print(Scale);
        GodotObject collider = groundcheck.GetCollider();

        if (collider is StaticBody2D && growtimer.IsStopped()){
            growtimer.Start();
        }

    }


    private void _on_grow_snoball_timeout(){
        float growthFactor = 1.1f;
        snowballsprite.Scale = new Vector2(snowballsprite.Scale.X * growthFactor, snowballsprite.Scale.Y * growthFactor);

        
        float colRadius = (float)snowballcol.Shape.Get("radius");

        //snowballcol.Shape.Set("radius", colRadius * growthFactor);

        Tween tween1 = GetTree().CreateTween();
		tween1.TweenProperty(snowballcol.Shape, "radius", colRadius * growthFactor, 0.5)
			.SetTrans(Tween.TransitionType.Linear);
        
        float radiusChange = (colRadius * growthFactor) - colRadius;
        
        GlobalPosition += new Vector2(0, -radiusChange); 

        Tween tween2 = GetTree().CreateTween();
		tween2.TweenProperty(sballCamera, "zoom", sballCamera.Zoom *= 0.9f, 1)
			.SetTrans(Tween.TransitionType.Linear);

        groundcheck.Scale *= growthFactor;
        LinearVelocity = new Vector2(LinearVelocity.X - 100f, LinearVelocity.Y + 100);
    }
        
        
    

}
