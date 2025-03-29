using System;
using Godot;

public partial class Rocket : Area2D
{
    private void _on_body_entered(Node2D body)
    {
        if (body is RigidBody2D && body.IsInGroup("player"))
        {
            Snowball ball = (Snowball)body;
            // only add charges if less than 2
            int currentCharges = ball.jumpCharges;
            if (currentCharges < 4)
            {
                ball.jumpCharges++;
                ball.setJumpChargeBar();
            }
            CallDeferred("queue_free");
        }
    }
}
