using System;
using Godot;

public partial class worldcontroller : Node2D
{
    Globals glob;
    [Export] CollisionPolygon2D hillcollision;
    [Export] PackedScene objectRamp;
    [Export] PackedScene objectWall;
    [Export] PackedScene objectSpring;
    [Export] PackedScene objectIceBall;
    [Export] PackedScene objectSkiMan;
    [Export] PackedScene objectLagoon;
    [Export] PackedScene objectBird;
    [Export] PackedScene objectSnowflake;
    [Export] Snowcannon snowcannon;
    [Export] AnimationPlayer startanim;
    [Export] PackedScene objectRocket;
    [Export] bool skipintro = false;

    private enum groundObjects {Ramp, Wall, Spring, Lagoon}

    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");

        glob.playsound("globalmusic");

        spawnHillObjects();
        SpawnStuffInAir();

        if (glob.firstboot && !skipintro){
            startanim.Play("startanim");
        }
        if (skipintro){
            glob.firstboot = false;
        }

    }

    private void TestPrint()
    {

        //GetTree().Paused = true;
    }

    private void spawnHillObjects()
    {
        float ratio = 0.9f;
        // ratio = 0.8889   x * 0.8889

        Random rand = new Random();
        int lastx = 500;
        string lastobjName = "";
        int objCount = 100;
        for (int i = 1; i <  objCount; i++)
        {

            int selection = rand.Next(0, Enum.GetNames(typeof(groundObjects)).Length);
            string[] names = Enum.GetNames(typeof(groundObjects));
            string nameofobj = names[selection];

            if (nameofobj == "Lagoon" || nameofobj == "Spring"){

                // reroll
                if (lastobjName == "Lagoon" && nameofobj != "Spring"){
                    selection = rand.Next(0, Enum.GetNames(typeof(groundObjects)).Length);
                    nameofobj = names[selection];
                }

                if (lastobjName == "Lagoon" || lastobjName == "Spring"){
                    // if 2 in a row skip
                    objCount++;
                    continue;
                }                
            }
            Node2D obj = spawnObject(nameofobj);
            GetTree().CurrentScene.AddChild(obj);
             
            int distancebetween = lastx + rand.Next(200,1000);
            int randomizedYdiff = 0;

            if (nameofobj != "Spring" && nameofobj != "Lagoon"){
                randomizedYdiff  = rand.Next(-90, 100);
            } else {
                randomizedYdiff = 0;
                distancebetween = lastx + rand.Next(200,400);  
            }

            if (lastobjName == "Lagoon"){
                distancebetween = lastx + rand.Next(1200,1500); 
            }
            
            lastobjName = nameofobj;
            lastx = distancebetween;
            obj.GlobalPosition = new Vector2(distancebetween, -(distancebetween * ratio) + randomizedYdiff);
            
            if (rand.Next(1, 5) == 1)
            {
                Area2D rocket = objectRocket.Instantiate<Area2D>();
                GetTree().CurrentScene.AddChild(rocket);
                rocket.GlobalPosition = new Vector2(obj.GlobalPosition.X, obj.GlobalPosition.Y - 200f);
            }

        }
    
    }

    private Node2D spawnObject(string Name){
        Random rand = new Random();
        switch (Name){
            case "Ramp":
                Node2D ramp = objectRamp.Instantiate<Node2D>();
                return ramp;
            case "Wall":
                Node2D wall = objectWall.Instantiate<Node2D>();
                return wall;
            case "Spring":
                Area2D spring = objectSpring.Instantiate<Area2D>();
                return spring;
            case "IceBall":
                RigidBody2D ball = objectIceBall.Instantiate<RigidBody2D>();
                return ball;
            case "SkatMan":
                RigidBody2D skiman = objectSkiMan.Instantiate<RigidBody2D>();
                return skiman;
            case "Lagoon":
                Node2D lagoon = objectLagoon.Instantiate<Node2D>();
                return lagoon;
            case "SnowFlake":
                Area2D snowflake = objectSnowflake.Instantiate<Area2D>();
                return snowflake;
            case "Eagle":
                Area2D eagle = objectBird.Instantiate<Area2D>();
                return eagle;
        }

        return null;
    }

    private void SpawnStuffThatFallsDown(){
        Random rand = new Random();

        int Selection = rand.Next(0,3);
        string objName = "";

        switch (Selection){
            case 0:
                objName = "SkatMan";
                break;
            case 1:
            case 2:
                objName = "IceBall";
                break;
        }

        
        Node2D obj = spawnObject(objName);
        int randomx = rand.Next(1000, 40000);
        
        AddChild(obj);
        RigidBody2D rigidObj = (RigidBody2D)obj;
        rigidObj.LinearVelocity = new Vector2(rigidObj.LinearVelocity.X - 100, rigidObj.LinearVelocity.Y);
        
        obj.GlobalPosition = new Vector2(randomx, -(randomx * 0.9f) - 2000);
    }

    private void _on_world_item_spawn_timer_timeout(){
        SpawnStuffThatFallsDown();
    }

    private void SpawnStuffInAir(){
        Random rand = new Random();
        for (int i = 1; i <  40; i++)
        {
            float x = rand.Next(3000, 40000);

            float y = -(x * 0.9f) - (rand.Next(900,5000));
            string objName = "";
            
            switch (rand.Next(0,2)){
                case 0:
                    objName = "SnowFlake";
                    break;
                case 1:
                    objName = "Eagle";
                    break;
            }

            Node2D obj = spawnObject(objName);
            AddChild(obj);
            obj.GlobalPosition = new Vector2(x,  y);

        }
    }

    private void _on_animation_player_animation_finished(string animname){
        glob.firstboot = false;
        snowcannon.currentCannonState = Snowcannon.cannonState.aiming;
    }

    private void _on_assetcleararea_body_entered(Node2D body){
        if (body.IsInGroup("player")){
            return;
        }
        body.CallDeferred("queue_free");
    }
    

}
