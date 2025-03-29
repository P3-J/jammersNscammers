using System;
using Godot;

public partial class Snowball : RigidBody2D
{
    [Export]
    Camera2D sballCamera;

    [Export]
    RayCast2D groundcheck;

    [Export]
    Timer growtimer;

    [Export]
    CollisionShape2D snowballcol;

    [Export]
    Sprite2D snowballsprite;

    [Export]
    Timer stucktimer;

    [Export]
    GameOver gameOver;

    [Export]
    TextureProgressBar jumpChargeBar;

    public float colradiuss;
    bool boostedDown = false;
    Vector2 lastpos;
    Godot.Collections.Array<Node2D> colbodies;

    /// <summary>
    /// ignore
    /// </summary>
    Vector2 ogSpriteScale;
    float ogRadius;
    Vector2 ogZoom;
    float ogMass;
    Vector2 ogGroundCheck;
    Globals globals;
    public int jumpCharges = 2;

    public void SetSnowballCameraAsCurrent()
    {
        sballCamera.MakeCurrent();
    }

    public override void _Ready()
    {
        base._Ready();
        colradiuss = (float)snowballcol.Shape.Get("radius");

        ogSpriteScale = snowballsprite.Scale;
        ogRadius = colradiuss;
        ogZoom = sballCamera.Zoom;
        ogMass = Mass;
        ogGroundCheck = groundcheck.Scale;
        globals = GetNode<Globals>("/root/Globals");

        setJumpChargeBar();
    }

    public void setJumpChargeBar()
    {
        jumpChargeBar.Value = jumpCharges;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        GodotObject collider = groundcheck.GetCollider();
        colbodies = GetCollidingBodies();

        if (stucktimer.IsStopped())
        {
            stucktimer.Start();
            lastpos = GlobalPosition;
        }

        if (colbodies.Count >= 1)
        {
            boostedDown = false;
        }

        if (collider is StaticBody2D && growtimer.IsStopped())
        {
            StaticBody2D col = (StaticBody2D)collider;
            if (col.IsInGroup("hill"))
            {
                groundcheck.Rotation += 45f;
                if (groundcheck.Rotation > 360f)
                {
                    groundcheck.Rotation = 0;
                }
                GrowBall();
                growtimer.Start();
            }
        }
    }

    private void _on_stucktimer_timeout()
    {
        if (lastpos == GlobalPosition)
        {
            int currentScore = (int)Math.Round(colradiuss);
            gameOver.Visible = true;
            gameOver.snowBallSizeLabel.Text = $"{currentScore}";

            if (currentScore > globals.highScore)
            {
                globals.highScore = currentScore;
            }
            gameOver.setHighScore();
        }
    }

    public void ResetSceneStuff()
    {
        snowballsprite.Scale = ogSpriteScale;
        snowballcol.Shape.Set("radius", ogRadius);
        sballCamera.Zoom = ogZoom;
        Mass = ogMass;
        groundcheck.Scale = ogGroundCheck;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("jump"))
        {
            // handle charge to ground
            if (colbodies.Count == 0 && !boostedDown)
            {
                LinearVelocity = new Vector2(LinearVelocity.X + 400, LinearVelocity.Y + 700);
                boostedDown = true;
            }
            // handle jump up
            else if (jumpCharges > 0)
            {
                jumpCharges--;
                LinearVelocity = new Vector2(LinearVelocity.X + -200f, -700f);
                setJumpChargeBar();
            }
        }
    }

    public void GrowBall(float growth = 1.05f)
    {
        float growthFactor = growth;
        snowballsprite.Scale = new Vector2(
            snowballsprite.Scale.X * growthFactor,
            snowballsprite.Scale.Y * growthFactor
        );

        colradiuss = (float)snowballcol.Shape.Get("radius");

        //snowballcol.Shape.Set("radius", colRadius * growthFactor);


        Tween tween1 = GetTree().CreateTween();
        tween1
            .TweenProperty(snowballcol.Shape, "radius", colradiuss * growthFactor, 0.5)
            .SetTrans(Tween.TransitionType.Linear);

        float radiusChange = (colradiuss * growthFactor) - colradiuss;
        GlobalPosition += new Vector2(0, -radiusChange);

        Tween tween2 = GetTree().CreateTween();
        tween2
            .TweenProperty(sballCamera, "zoom", sballCamera.Zoom *= 0.98f, 1)
            .SetTrans(Tween.TransitionType.Linear);

        Mass += 0.5f;

        groundcheck.Scale *= growthFactor;
        LinearVelocity = new Vector2(LinearVelocity.X - 100f, LinearVelocity.Y + 100);
    }
}
