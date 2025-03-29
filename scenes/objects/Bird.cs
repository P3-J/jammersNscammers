using System;
using Godot;

public partial class Bird : Area2D
{
    [Export]
    AudioStreamPlayer caaw;

    private void _on_body_entered(Node2D body)
    {
        if (body is RigidBody2D && body.IsInGroup("player"))
        {
            Snowball ball = (Snowball)body;
            caaw.Play();
            float oldX = ball.LinearVelocity.X;
            float newX = oldX < 0 ? oldX * -1 : oldX;
            newX += 500f;
            ball.LinearVelocity = new Vector2(newX, -2000);
        }
    }

    private void _on_caaw_finished()
    {
        CallDeferred("queue_free");
    }
}
