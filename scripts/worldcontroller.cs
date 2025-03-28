using System;
using Godot;

public partial class worldcontroller : Node2D
{
    Globals glob;
    [Export] CollisionPolygon2D hillcollision;
    [Export] PackedScene objectRamp;
    [Export] PackedScene objectWall;

    private enum groundObjects {Ramp, Wall}

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

        for (int i = 1; i < 40; i++)
        {

            int selection = rand.Next(0, Enum.GetNames(typeof(groundObjects)).Length);
            string[] names = Enum.GetNames(typeof(groundObjects));

            string nameofobj = names[selection];
            Node2D obj = spawnObject(nameofobj);
            GetTree().CurrentScene.AddChild(obj);
            
            int distancebetween = rand.Next(600,1200) * i;
            obj.GlobalPosition = new Vector2(distancebetween, -(distancebetween * ratio));

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
        }

        return null;
    }


    private void _on_area_2d_body_entered(Node2D body){
        GD.Print(body);
    }

}
