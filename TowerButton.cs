using Godot;
using System;

public class TowerButton : Button
{
    public Turret Turret { get; set; }

    public Sprite TopSprite { get; set; }
    public Sprite BottomSprite { get; set; }
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //TODO: set turret in code

        TopSprite = GetNode<Sprite>("Icon/Top");
        BottomSprite = GetNode<Sprite>("Icon/Bottom");

        if (Turret != null)
        {
            TopSprite.Texture = Turret.Cannon.Texture;
        }

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
