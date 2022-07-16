using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using Godot;
using System;

public class Turret : Node2D, ITurret
{
    // Signals
    [Signal]
    public delegate void TurretFired(Turret turret);

    [Export]
    public float RateOfFire { get; set; } = 2f;

    [Export(PropertyHint.Range, "300, 1000")]
    public int BulletSpeed { get; set; } = 700;

    /// <summary>
    /// The speed that the turret can rotate/target
    /// </summary>
    [Export()]
    public float RotateSpeed { get; set; } = 0.01f;

    // Nodes
    public AnimationPlayer AnimationPlayer { get; set; }
    public Sprite Cannon { get; set; }
    public Sprite CannonBase { get; set; }
    public Timer ShootTimer { get; set; }


    public string TopSprite { get; set; }
    public string BottomSprite { get; set; }
    public int Cost { get; set; }
    public int Range { get; set; }
    public int Damage { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Nodes
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Cannon = GetNode<Sprite>("Top");
        CannonBase = GetNode<Sprite>("Bottom");
        ShootTimer = GetNode<Timer>("ShootTimer");


        Cannon.Texture = Helpers.TextureFromImagePath(TopSprite);

        CannonBase.Texture = Helpers.TextureFromImagePath(BottomSprite);


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
