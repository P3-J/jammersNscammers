using System;
using Godot;

public partial class Fire : Area2D
{
    private void _on_body_entered(Node2D body)
    {
        if (body is RigidBody2D && body.IsInGroup("player"))
        {
            Snowball ball = (Snowball)body;
            ball.GrowBall(0.8f);
            CallDeferred("queue_free");
        }
    }
}
