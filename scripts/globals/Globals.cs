using System;
using Godot;

public partial class Globals : Node2D
{
    public int highScore;
    public bool firstboot = true;
    [Export] AudioStreamPlayer boom;
    [Export] AudioStreamPlayer globalmusic;
    [Export] AudioStreamPlayer helveke;
    [Export] AudioStreamPlayer boost;
    [Export] AudioStreamPlayer hit;


    public void playsound(string name){
        switch (name){

            case "boom":
                boom.Play();
                break;
            case "globalmusic":
                globalmusic.Play();
                break;
            case "helveke":
                helveke.Play();
                break;
            case "boost":
                boost.Play();
                break;
            case "hit":
                hit.Play();
                break;


        }
    }

    public void DropPitch()
    {
        float newpitch = globalmusic.PitchScale;
        newpitch -= 0.005f;
        newpitch = Mathf.Max(0.5f, newpitch);  // Prevent pitch from going too low
        globalmusic.PitchScale = newpitch;
        
    }

    public void ResetPitch(){
        globalmusic.PitchScale = 1f;
    }

}
