using Godot;
using System;

public partial class Startscreen : Node2D
{

    [Export] Marker2D spawnpoint;
    [Export] PackedScene skier;
    [Export] PackedScene ball;
    private void _on_startbutton_pressed(){
        GetTree().ChangeSceneToFile("res://scenes/world.tscn");
    }

    private void _on_spawntimer_timeout(){
        Random rand = new Random();
        string objname = "";

        switch (rand.Next(0,2)){
            case 0:
                objname = "IceBall";
                break;
            case 1:
                objname = "SkatMan";
                break;
        }

        Node2D obj = spawnObject(objname);

        AddChild(obj);
        obj.GlobalPosition = spawnpoint.GlobalPosition;


    }

    private Node2D spawnObject(string Name){
        Random rand = new Random();
        switch (Name){
            case "IceBall":
                RigidBody2D balls = ball.Instantiate<RigidBody2D>();
                return balls;
            case "SkatMan":
                RigidBody2D skiman = skier.Instantiate<RigidBody2D>();
                return skiman;
        }

        return null;
    }

}
