using gmtkjame2022rollthedice.Helpers;
using gmtkjame2022rollthedice.Models;
using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Area2D
{
    public Sprite Sprite { get; set; }
    public AudioStreamPlayer DeathSoundAudioPlayer { get; private set; }
    [Export(PropertyHint.Range, "0, 500")]
    public int MoveSpeed { get; set; } = 150;

    [Export(PropertyHint.Range, "0, 1000")]
    public int InitialHealth { private get; set; } = 5;

    /// <summary>
    /// The amount of money to reward on death
    /// </summary>
    [Export]
    public int Reward { get; set; } = 10;

    public HealthBar HealthBar { get; set; }

    public readonly HashSet<ulong> CollidedBullets = new HashSet<ulong>();


    [Signal]
    public delegate void EnemyDied(Enemy enemy);

    public AnimationPlayer AnimationPlayer { get; set; }

    public EnemyModel EnemyModel { get; set; }

    public override void _Ready()
    {
        // Nodes
        Sprite = GetNode<Sprite>("Sprite");
        DeathSoundAudioPlayer = GetNode<AudioStreamPlayer>("DeathSoundAudioPlayer");
        HealthBar = GetNode<HealthBar>("HealthBar");
        HealthBar.MaxHealth = InitialHealth;
        HealthBar.CurrentHealth = InitialHealth;

        Sprite.Texture = EnemyModel.SpriteTexture;

        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        HealthBar.UpdateHealth(InitialHealth);

    }

    public void LoadEnemyModel(EnemyModel enemyModel)
    {
        EnemyModel = enemyModel;
    }

    public override void _Process(float delta)
    {
        var hasTakenDamage = false;

        // Check if the enemy has been shot
        foreach (PhysicsBody2D body in GetOverlappingBodies())
        {
            if (HealthBar.CurrentHealth <= 0)
            {
                // Enemy is already dead
                continue;
            }

            if (!body.IsInGroup("bullet"))
            {
                // Object is not a bullet
                continue;
            }

            var bulletId = body.GetInstanceId();
            if (CollidedBullets.Contains(bulletId))
            {
                // Bullet has already been used
                continue;
            }

            // Reduce health by bullet damage amount
            var bullet = body as Bullet;
            HealthBar.UpdateHealth(HealthBar.CurrentHealth - bullet.Damage);

            // Save the ID of the bullet so we don't count it for collision any more
            CollidedBullets.Add(bulletId);

            // Notify the bullet it has collided
            bullet.UpdateCollisionCount();
            hasTakenDamage = true;

        }

        if (HealthBar.CurrentHealth <= 0)
        {
            // Enemy died
            AnimationPlayer.Play("die");
            if (!DeathSoundAudioPlayer.Playing)
            {
                DeathSoundAudioPlayer.Play();
            }

        }
        else if (hasTakenDamage)
        {
            AnimationPlayer.Play("damage");
        }
        else
        {
            if (!AnimationPlayer.IsPlaying())
            {
                AnimationPlayer.Play("walk");
            }
        }


    }

    public void Die()
    {
        EmitSignal(nameof(EnemyDied), this);
    }
}
