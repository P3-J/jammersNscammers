using System;
using Godot;

public partial class Springtrap : Area2D
{
    [Export]
    AudioStreamPlayer boing;

    private void _on_body_entered(Node2D body)
    {
        Random rand = new Random();
        if (body is RigidBody2D && body.IsInGroup("player"))
        {
            Snowball ball = (Snowball)body;
            boing.Play();
            ball.LinearVelocity = new Vector2(500, rand.Next(-1600, -1000));
        }
    }

    private void _on_boing_finished()
    {
        CallDeferred("queue_free");
    }
}
