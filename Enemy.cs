using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Area2D
{
    [Export(PropertyHint.Range, "0, 500")]
    public int MoveSpeed { get; set; } = 150;

    [Export(PropertyHint.Range, "0, 1000")]
    public int InitialHealth { private get; set; } = 5;

    public HealthBar HealthBar { get; set; }

    public readonly HashSet<ulong> CollidedBullets = new HashSet<ulong>();


    [Signal]
    public delegate void EnemyDied(Enemy enemy);

    public AnimationPlayer AnimationPlayer { get; set; }

    public override void _Ready()
    {
        // Nodes
        HealthBar = GetNode<HealthBar>("HealthBar");
        HealthBar.MaxHealth = InitialHealth;
        HealthBar.CurrentHealth = InitialHealth;

        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        HealthBar.UpdateHealth(InitialHealth);

    }

    public override void _Process(float delta)
    {
        // Check if the enemy has been shot
        foreach (PhysicsBody2D body in GetOverlappingBodies())
        {
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

            if (HealthBar.CurrentHealth <= 0)
            {
                // Enemy died
                Die();
            }

        }
    }

    public void Die()
    {
        AnimationPlayer.Play("die");
        EmitSignal(nameof(EnemyDied), this);
    }
}
