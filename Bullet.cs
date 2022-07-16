using Godot;
using System;

public class Bullet : RigidBody2D
{
    [Export(PropertyHint.Range, "1,10")]
    public int Damage { get; set; } = 1;

    [Export(PropertyHint.Range, "1, 100")]
    public int MaxCollisions = 1;

    private int CollisionCount { get; set; } = 0;

    public VisibilityNotifier2D VisibilityNotifier { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        VisibilityNotifier = GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");

        //Friction = 0;
        LinearDamp = 0;

        VisibilityNotifier.Connect("screen_exited", this, nameof(VisibilityNotifier_ScreenExited));
    }

    public void VisibilityNotifier_ScreenExited()
    {
        // Remove bullet from tree 
        QueueFree();
    }

    public void UpdateCollisionCount()
    {

        CollisionCount += 1;
        if (CollisionCount >= MaxCollisions)
        {
            // Bullet has reached its collision limit
            Visible = false;
            QueueFree();

        }

    }
}
