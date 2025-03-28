using System;
using Godot;

public partial class Spawner : Node2D { }


// have a window that is larger than the player viewport both horizontally and vertically
// elements are spawned out of view and are deleted once they go out of bounds
// when a delete is triggered new element is spawned in the opposite direction
// account for movement in all directions
