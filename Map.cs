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


	// Spawners
	public PackedScene EnemyScene { get; private set; }
	public PackedScene BulletScene { get; private set; }

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

		// Load scenes
		EnemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
		BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");

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

				//TODO: Make sure path is also deleted
				EnemyPath.RemoveChild(enemyPath);

				//TODO: Damage the player
			}
		}

		var firstEnemy = allEnemies.FirstOrDefault();



		foreach (var turret in GetTurrets())
		{
			//TODO: rotate turrets to the closest enemy
			if (firstEnemy == null)
			{
				// No first enemy to target
				turret.Cannon.Rotation = 0;
				continue;
			}

			turret.Cannon.Rotation = turret.GetAngleTo(firstEnemy.Position) + CannonRotationOffset;
		}


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
		EnemyPath.RemoveChild(enemyParent);
	}
}
