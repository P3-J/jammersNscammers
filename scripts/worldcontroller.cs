using Godot;
using System;

public partial class worldcontroller : Node2D
{

    Globals glob;
    public override void _Ready()
    {
        base._Ready();
        glob = GetNode<Globals>("/root/Globals");

        glob.Connect("microsoft", new Callable(this, nameof(TestPrint)));
        glob.EmitSignal("microsoft");
       
    }

    private void TestPrint()
    {
        GD.Print("log");
        //GetTree().Paused = true;
    }


}
