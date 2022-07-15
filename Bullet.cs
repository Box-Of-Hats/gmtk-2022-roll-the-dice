using Godot;
using System;

public class Bullet : RigidBody2D
{
    [Export(PropertyHint.Range, "1,10")]
    public int Damage { get; set; } = 1;

    public VisibilityNotifier2D VisibilityNotifier { get; set; }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        VisibilityNotifier = GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");

        Friction = 0;
        LinearDamp = 0;

        VisibilityNotifier.Connect("screen_exited", this, nameof(VisibilityNotifier_ScreenExited));
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void VisibilityNotifier_ScreenExited()
    {
        // Remove bullet from tree 
        GetParent().RemoveChild(this);
    }
}
