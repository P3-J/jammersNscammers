using System;
using Godot;

public partial class GameOver : CanvasLayer
{
    [Export]
    public RichTextLabel snowBallSizeLabel;

    [Export]
    RichTextLabel highScoreText;

    [Export]
    RichTextLabel highScoreValue;

    [Export]
    Button tryAgainButton;

    [Export]
    Snowball snowball;

    Globals globals;

    public override void _Ready()
    {
        base._Ready();
        globals = GetNode<Globals>("/root/Globals");
    }

    public void setHighScore()
    {
        if (globals.highScore > 0)
        {
            highScoreValue.Text = $"{globals.highScore}";
            highScoreText.Visible = true;
        }
    }

    void _on_try_again_button_pressed()
    {
        snowball.ResetSceneStuff();
        GetTree().ChangeSceneToFile("res://scenes/world.tscn");
    }
}
