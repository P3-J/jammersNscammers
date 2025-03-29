using System;
using Godot;

public partial class Snowflake : Area2D
{
    private void _on_body_entered(Node2D body)
    {
        if (body is RigidBody2D && body.IsInGroup("player"))
        {
            Snowball ball = (Snowball)body;
            ball.GrowBall(1.2f);
            CallDeferred("queue_free");
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Vector2 newglob = GlobalPosition;
        newglob.Y += 1;
        GlobalPosition = newglob;
    }

}
