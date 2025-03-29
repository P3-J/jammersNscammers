using System;
using Godot;

public partial class worldcontroller : Node2D
{
    Globals glob;

    [Export]
    CollisionPolygon2D hillcollision;

    [Export]
    PackedScene objectRamp;

    [Export]
    PackedScene objectWall;

    [Export]
    PackedScene objectSpring;

    [Export]
    PackedScene objectSnowflake;

    [Export]
    PackedScene objectFire;

    private enum groundObjects
    {
        Ramp,
        Wall,
        Spring,
        Snowflake,
        Fire,
    }

    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");

        glob.Connect("microsoft", new Callable(this, nameof(TestPrint)));
        glob.EmitSignal("microsoft");
        spawnHillObjects();
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
        int lastx = 0;
        for (int i = 1; i < 40; i++)
        {
            int selection = rand.Next(0, Enum.GetNames(typeof(groundObjects)).Length);
            string[] names = Enum.GetNames(typeof(groundObjects));

            string nameofobj = names[selection];

            Node2D obj = spawnObject(nameofobj);
            GetTree().CurrentScene.AddChild(obj);

            int distancebetween = lastx + rand.Next(200, 1000);
            int randomizedYdiff = 0;

            if (nameofobj != "Spring")
            {
                randomizedYdiff = rand.Next(-90, 100);
            }
            else
            {
                distancebetween = lastx + rand.Next(200, 400);
            }

            lastx = distancebetween;
            obj.GlobalPosition = new Vector2(
                distancebetween,
                -(distancebetween * ratio) + randomizedYdiff
            );
        }
    }

    private Node2D spawnObject(string Name)
    {
        Random rand = new Random();
        switch (Name)
        {
            case "Ramp":
                Node2D ramp = objectRamp.Instantiate<Node2D>();
                return ramp;
            case "Wall":
                Node2D wall = objectWall.Instantiate<Node2D>();
                return wall;
            case "Spring":
                Area2D spring = objectSpring.Instantiate<Area2D>();
                return spring;
            case "Snowflake":
                Area2D snowflake = objectSnowflake.Instantiate<Area2D>();
                return snowflake;
            case "Fire":
                Area2D fire = objectFire.Instantiate<Area2D>();
                return fire;
        }

        return null;
    }
}
