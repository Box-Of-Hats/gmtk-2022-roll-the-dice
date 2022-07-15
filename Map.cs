using Godot;
using System;

public class Map : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export(PropertyHint.Range, "0, 500")]
	public int MoveSpeed { get; set; } = 200;

	public Path2D EnemyPath { get; set; }
	public Enemy Enemy { get; set; }
	public PathFollow2D EnemyFollowPath { get; set; }
	public Timer SpawnTimer { get; set; }

	public PackedScene EnemyScene { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EnemyPath = GetNode<Path2D>("EnemyPath");
		//Enemy = GetNode<Enemy>("Enemy");
		EnemyFollowPath = GetNode<PathFollow2D>("EnemyPath/PathFollow2D");
		SpawnTimer = GetNode<Timer>("SpawnTimer");

		EnemyScene = GD.Load<PackedScene>("res://Enemy.tscn");


		SpawnTimer.Connect("timeout", this, nameof(SpawnTimer_Timeout));


		SpawnEnemy();

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// Move the enemy along the path
		var followPaths =  EnemyPath.GetChildren();
		
		foreach (PathFollow2D path in followPaths)
		{
			path.Offset += MoveSpeed * delta;
		}


	}

	public void SpawnTimer_Timeout()
	{
		SpawnEnemy();
	}


	public void SpawnEnemy()
	{
		GD.Print("Spawning enemy");

		// Create new enemy instance
		var enemy = EnemyScene.Instance<Enemy>();

		// Create new follow path and add enemy
		var followPath = new PathFollow2D();
		followPath.AddChild(enemy);

		// Add enemy with path to the main node
		EnemyPath.AddChild(followPath);
	}


}
