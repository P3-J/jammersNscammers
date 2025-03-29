using Godot;
using System;

public partial class Springtrap : Area2D
{
    [Export] AudioStreamPlayer boing;

    private void _on_body_entered(Node2D body){
        if (body is RigidBody2D && body.IsInGroup("player")){
          Snowball ball = (Snowball)body;
          boing.Play();
          ball.LinearVelocity = new Vector2(500, -1600);
          
        }
    }

    private void _on_boing_finished(){
        CallDeferred("queue_free");
    }

}


