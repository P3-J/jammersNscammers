using System;
using Godot;

public partial class Snowflake : Area2D
{
    Globals glob;
    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");
    }

    private void _on_body_entered(Node2D body)
    {
        if (body is RigidBody2D && body.IsInGroup("player"))
        {
            glob.playsound("helveke");
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
