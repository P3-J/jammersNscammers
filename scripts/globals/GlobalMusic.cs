using Godot;

public partial class GlobalMusic : Node
{
    [Export] public AudioStreamPlayer musicPlayer;

    private float basePitch = 1.0f;  // Default pitch scale

    public override void _Ready()
    {
        if (musicPlayer != null)
        {
            musicPlayer.Play();
        }
    }

    // Function to lower the pitch
    public void DropPitch()
    {
        if (musicPlayer != null)
        {
            basePitch -= 0.10f;
            basePitch = Mathf.Max(0.5f, basePitch);  // Prevent pitch from going too low
            musicPlayer.PitchScale = basePitch;
        }
    }
}
