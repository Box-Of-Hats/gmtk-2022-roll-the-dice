using gmtkjame2022rollthedice.Helpers;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Map : Node2D
{

    [Export(PropertyHint.Range, "0, 100")]
    public int MaxEnemyCount { get; set; } = 3;

    public Path2D EnemyPath { get; set; }
    public Timer SpawnTimer { get; set; }
    public Node2D Turrets { get; set; }
    public Node2D Bullets { get; set; }
    public Shop Shop { get; set; }


    // Spawners
    public PackedScene EnemyScene { get; private set; }
    public PackedScene BulletScene { get; private set; }
    public PackedScene TurretScene { get; private set; }

    // Rotation offset for all cannon top math
    const float CannonRotationOffset = (float)Math.PI / 2f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get nodes
        EnemyPath = GetNode<Path2D>("EnemyPath");
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        Turrets = GetNode<Node2D>("Turrets");
        Bullets = GetNode<Node2D>("Bullets");
        Shop = GetNode<Shop>("Shop");

        // Load scenes
        EnemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
        BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        TurretScene = GD.Load<PackedScene>("res://Turret.tscn");

        // Add connections
        SpawnTimer.Connect("timeout", this, nameof(SpawnTimer_Timeout));

        foreach (var turret in GetTurrets())
        {
            turret.Connect(nameof(Turret.TurretFired), this, nameof(Turret_Shoot));
        }


    }

    public override void _Process(float delta)
    {
        //TODO: move total length to property and set only on ready
        var totalLength = EnemyPath.Curve.GetBakedLength();

        const int endPadding = 20;


        // Move enemies along their paths
        var allEnemies = GetEnemies().ToList();
        foreach (var enemyPath in allEnemies)
        {
            var enemyNode = enemyPath.GetNode<Enemy>("Enemy");

            // Move along path
            enemyPath.Offset += enemyNode.MoveSpeed * delta;

            //Remove path if enemy has reached the end
            if (enemyPath.Offset >= (totalLength - endPadding))
            {
                GD.Print("Enemy reached the end!");

                enemyPath.QueueFree();

                //TODO: Damage the player
            }
        }

        var firstEnemy = allEnemies.FirstOrDefault();

        const float onTargetPadding = 0.5f;

        foreach (var turret in GetTurrets())
        {
            //TODO: rotate turrets to the closest enemy instead of the first?
            if (firstEnemy == null)
            {
                // No first enemy to target
                //turret.Cannon.Rotation = turret.Cannon.Rotation + turret.RotateSpeed;
                continue;
            }

            const float tau = (float)(Math.PI * 2f);

            // Get the current tower rotation
            var absoluteTurretRotation = turret.Cannon.Rotation % tau;

            // Get the ideal rotation
            var desiredRotation = (turret.GetAngleTo(firstEnemy.Position) + CannonRotationOffset) % tau;


            // Point the cannon at the enemy
            turret.Cannon.Rotation = desiredRotation;

            //TODO: See if I can get turret turning to be smooth
            //if (absoluteTurretRotation - desiredRotation <= onTargetPadding)
            //{
            //    turret.Cannon.Rotation = absoluteTurretRotation - turret.RotateSpeed;
            //}
            //else if (absoluteTurretRotation < desiredRotation)
            //{
            //    turret.Cannon.Rotation = absoluteTurretRotation + turret.RotateSpeed;

            //}
            //else
            //{
            //    // Already pointing at the enemy, do nothing
            //}

        }

        if (Input.IsActionJustPressed("place_tower"))
        {
            // Check position isn't on the path
            const float pathWidth = 30;
            const float turretWidth = 128;
            const float pathDeadZone = turretWidth / 2 + pathWidth;

            var mousePosition = GetLocalMousePosition();
            var closestPathPoint = EnemyPath.Curve.GetClosestPoint(mousePosition);

            var distanceToPath = mousePosition.DistanceTo(closestPathPoint);

            if (distanceToPath < pathDeadZone)
            {
                GD.Print($"Cannot place tower - too close to path ({distanceToPath})");
                return;
            }

            // Check position doesnt overlap any other towers
            var (closestTurret, turretDistance) = GetNearestTurret(mousePosition);
            GD.Print(turretDistance);
            if (turretDistance < turretWidth)
            {
                GD.Print($"Cannot place tower - too close to tower ({turretDistance})");
                return;
            }

            //TODO: Check a tower is selected?

            // Create a tower
            SpawnTurret(mousePosition);
        }

    }

    /// <summary>
    /// Get the nearest turret to a given position
    /// </summary>
    /// <param name="position"></param>
    /// <returns>Tuple of turret and distance</returns>
    public Tuple<Turret, float> GetNearestTurret(Vector2 position)
    {
        Turret closestTurret = null;
        var closestDistance = float.MaxValue;
        foreach (var turret in GetTurrets())
        {
            var distanceToTurret = turret.Position.DistanceTo(position);
            if (distanceToTurret < closestDistance)
            {
                closestTurret = turret;
                closestDistance = distanceToTurret;
            }
        }

        return new Tuple<Turret, float>(closestTurret, closestDistance);
    }

    public void SpawnTimer_Timeout()
    {
        SpawnEnemy();
    }


    public void SpawnEnemy()
    {
        var enemyCount = EnemyPath.GetChildren().Count;
        if (enemyCount >= MaxEnemyCount)
        {
            GD.Print($"Cannot spawn enemy. Enemy count is at maximum {enemyCount}/{MaxEnemyCount}");

            return;
        }

        GD.Print("Spawning enemy");

        // Create new enemy instance
        var enemy = EnemyScene.Instance<Enemy>();

        // Create new follow path and add enemy
        var followPath = new PathFollow2D();
        followPath.AddChild(enemy);

        // Add connections
        enemy.Connect(nameof(Enemy.EnemyDied), this, nameof(Enemy_EnemyDied));

        // Add enemy with path to the main node
        EnemyPath.AddChild(followPath);
    }

    public IEnumerable<Turret> GetTurrets() => Helpers.GetChildrenOfType<Turret>(Turrets);

    public IEnumerable<PathFollow2D> GetEnemies() => Helpers.GetChildrenOfType<PathFollow2D>(EnemyPath);

    public void Turret_Shoot(Turret turret)
    {
        // Spawn bullets for the firing turret
        var newBullet = BulletScene.Instance<Bullet>();

        var turretRoation = turret.Cannon.Rotation - CannonRotationOffset;

        var direction = new Vector2((float)Math.Cos(turretRoation) * turret.BulletSpeed, (float)Math.Sin(turretRoation) * turret.BulletSpeed);

        newBullet.Position = turret.Position;
        newBullet.LinearVelocity = direction;

        Bullets.AddChild(newBullet);
    }


    public void Enemy_EnemyDied(Enemy enemy)
    {
        // Need to get the parent of the enemy because they are nested under a path follower
        var enemyParent = enemy.GetParentOrNull<PathFollow2D>();

        // Remove the enemy from the scene
        enemyParent.QueueFree();
    }

    public void SpawnTurret(Vector2 position)
    {
        var newTurret = TurretScene.Instance<Turret>();
        newTurret.Position = position;

        newTurret.Connect(nameof(Turret.TurretFired), this, nameof(Turret_Shoot));

        //TODO: remove randomiser
        // Randomise rotation speed
        newTurret.RotateSpeed = GD.Randf() / 10;

        Turrets.AddChild(newTurret);
    }
}