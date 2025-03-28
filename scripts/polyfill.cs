using Godot;
using System;

public partial class polyfill : Polygon2D
{

    [Export] CollisionPolygon2D parentpoly;

    public override void _Ready()
    {
        base._Ready();
        Set("polygon", parentpoly.Polygon);
        GlobalPosition = parentpoly.GlobalPosition;
    }
}
