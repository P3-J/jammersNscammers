using System;
using Godot;

public partial class Globals : Node2D
{
    public int highScore;

    [Signal]
    public delegate void microsoftEventHandler();
}
