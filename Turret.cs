using Godot;
using System;

public class Turret : Node2D
{
    // Signals
    [Signal]
    public delegate void TurretFired(Turret turret);

    /// <summary>
    /// The number of seconds between each shot being fired
    /// </summary>
    [Export]
    public int RateOfFire = 2;

    [Export(PropertyHint.Range, "300, 1000")]
    public int BulletSpeed { get; set; } = 700;

    // Nodes
    public AnimationPlayer AnimationPlayer { get; set; }
    public Sprite Cannon { get; set; }
    public Timer ShootTimer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Nodes
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Cannon = GetNode<Sprite>("Top");
        ShootTimer = GetNode<Timer>("ShootTimer");

        // Connections
        ShootTimer.Connect("timeout", this, nameof(ShootTimer_Timeout));

        // Setup
        ShootTimer.WaitTime = RateOfFire;
    }
    public override void _Process(float delta)
    {
        Cannon.Rotation = Cannon.Rotation + 1f * delta;
    }

    /// <summary>
    /// Play a shooting animation. 
    /// </summary>
    public void Shoot()
    {
        if (AnimationPlayer.IsPlaying())
        {
            // Animation is already playing so cannot shoot again
            GD.Print("Cannot shoot cannon - animation is playing ");
            return;
        }

        AnimationPlayer.Play("Shoot");

        EmitSignal(nameof(TurretFired), this);
    }

    public void ShootTimer_Timeout()
    {
        Shoot();
    }

}
