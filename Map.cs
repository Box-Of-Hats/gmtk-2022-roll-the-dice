using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Data;
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
    public Control SpawnBlocker { get; set; }
    public Label LifeLabel { get; set; }
    public TowerRandomiser TowerRandomiser { get; set; }
    public Button ContinueButton { get; set; }
    public Label WaveLabel { get; set; }
    public Label WaveBonusLabel { get; set; }
    public Sprite RangeIndicator { get; set; }
    public Sprite TowerPreview { get; set; }
    public AnimationPlayer AnimationPlayer { get; set; }
    public AudioStreamPlayer HurtSoundAudioPlayer { get; private set; }
    public GameOverMenu GameOverMenu { get; set; }

    // Spawners
    public PackedScene EnemyScene { get; private set; }
    public PackedScene BulletScene { get; private set; }
    public PackedScene TurretScene { get; private set; }

    public ITurret SelectedTurret { get; set; }

    private int _lives;

    [Export]
    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            if (LifeLabel != null)
            {
                LifeLabel.Text = $"Lives: {value}";
            }
            _lives = value;
        }
    }


    // Rotation offset for all cannon top math
    const float CannonRotationOffset = (float)Math.PI / 2f;


    public Waves Waves = new Waves();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Set random seed
        GD.Randomize();

        // Get nodes
        EnemyPath = GetNode<Path2D>("EnemyPath");
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        Turrets = GetNode<Node2D>("Turrets");
        Bullets = GetNode<Node2D>("Bullets");
        Shop = GetNode<Shop>("Shop");
        SpawnBlocker = GetNode<Control>("SpawnBlocker");
        LifeLabel = GetNode<Label>("HudContainer/LifeLabel");
        TowerRandomiser = GetNode<TowerRandomiser>("TowerRandomiser");
        ContinueButton = GetNode<Button>("ContinueButton");
        WaveLabel = GetNode<Label>("HudContainer/WaveLabel");
        WaveBonusLabel = GetNode<Label>("HudContainer/WaveBonusLabel");
        RangeIndicator = GetNode<Sprite>("RangeIndicator");
        TowerPreview = GetNode<Sprite>("TowerPreview");
        GameOverMenu = GetNode<GameOverMenu>("GameOverMenu");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        HurtSoundAudioPlayer = GetNode<AudioStreamPlayer>("Sounds/PlayerDamage");
        // Load scenes
        EnemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
        BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        TurretScene = GD.Load<PackedScene>("res://Turret.tscn");

        // Add connections
        SpawnTimer.Connect("timeout", this, nameof(SpawnTimer_Timeout));
        SpawnTimer.Paused = true;

        RangeIndicator.Visible = false;
        TowerPreview.Visible = false;

        WaveLabel.Text = $"Wave {Waves.WaveDisplayNumber}/{Waves.TotalWaveCount}";
        TowerRandomiser.Visible = true;

        // Continue button
        ContinueButton.Connect("pressed", this, nameof(ContinueButton_Pressed));

        // Turret connections
        foreach (var turret in GetTurrets())
        {
            turret.Connect(nameof(Turret.TurretFired), this, nameof(Turret_Shoot));
        }

        // Shop connection
        Shop.Connect(nameof(Shop.TurretSelected), this, nameof(Shop_TurretSelected));

        // Randomiser connection
        TowerRandomiser.Connect(nameof(TowerRandomiser.TowerAccepted), this, nameof(TowerRandomiser_TowerRolled));


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

                enemyPath.QueueFree();

                Lives -= 1;
                AnimationPlayer.Play("lose-life");
                HurtSoundAudioPlayer.Play();

                if (Lives <= 0)
                {
                    GameOver();
                }
            }
        }


        var firstEnemy = allEnemies.FirstOrDefault();
        var allEnemyLocations = allEnemies.Select(o => o.Position);

        foreach (var turret in GetTurrets())
        {
            if (firstEnemy == null)
            {
                // No first enemy to target
                continue;
            }
            var enemyToTarget = firstEnemy;

            if (enemyToTarget.Position.DistanceSquaredTo(turret.Position) > turret.Range)
            {
                // First Enemy is out of range for this turret. Fallback to the closest enemy
                enemyToTarget = allEnemies.OrderBy(o => o.Position.DistanceTo(turret.Position))
                    .FirstOrDefault();

                if (enemyToTarget.Position.DistanceTo(turret.Position) > turret.Range)
                {
                    // All targets are out of range
                    continue;
                }
            }

            const float tau = (float)(Math.PI * 2f);

            // Get the current tower rotation
            var absoluteTurretRotation = turret.Cannon.Rotation % tau;

            // Get the ideal rotation
            var desiredRotation = (turret.GetAngleTo(enemyToTarget.Position) + CannonRotationOffset) % tau;

            // Point the cannon at the enemy
            turret.Cannon.Rotation = desiredRotation;
        }

        if (SelectedTurret is null)
        {
            // No turret selected
            return;
        }


        var mousePosition = GetLocalMousePosition();
        if (Input.IsActionJustReleased("place_tower"))
        {
            //User has clicked - attempt to spawn tower
            RangeIndicator.Visible = false;
            TowerPreview.Visible = false;

            if (!IsValidTurretPosition(mousePosition))
            {
                // Invalid tower position
                return;
            }


            // Create a tower
            SpawnTurret(mousePosition, SelectedTurret);
            return;
        }

        drawTurretPreview(mousePosition);
    }

    /// <summary>
    /// Render the turret preview to the scrren
    /// </summary>
    private void drawTurretPreview(Vector2 position)
    {
        TowerPreview.Visible = true;

        if (!IsValidTurretPosition(position))
        {
            TowerPreview.SelfModulate = new Color("#ee000011");
            RangeIndicator.Visible = false;

        }
        else
        {
            TowerPreview.SelfModulate = new Color("#ffffff11");
            RangeIndicator.Visible = true;
        }

        RangeIndicator.Position = position;
        TowerPreview.Position = position;
        var scale = (SelectedTurret.Range / 64f) * 2f;
        RangeIndicator.Scale = new Vector2(scale, scale);
    }

    public bool IsValidTurretPosition(Vector2 position)
    {
        var mousePosition = GetLocalMousePosition();

        if (SpawnBlocker.GetRect().HasPoint(mousePosition))
        {
            // Click is within the spawn blocker
            return false;
        }

        // Check position isn't on the path
        const float pathWidth = 30;
        const float turretWidth = 128;
        const float pathDeadZone = turretWidth / 2 + pathWidth;

        var closestPathPoint = EnemyPath.Curve.GetClosestPoint(mousePosition);

        var distanceToPath = mousePosition.DistanceTo(closestPathPoint);

        if (distanceToPath < pathDeadZone)
        {
            return false;
        }

        // Check position doesnt overlap any other towers
        var (closestTurret, turretDistance) = GetNearestTurret(mousePosition);
        if (turretDistance < turretWidth)
        {
            return false; ;
        }

        return true;
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

        // Get the next enemy from the wave
        var nextWaveEnemy = Waves.GetCurrentWave().GetNextEnemy();
        if (nextWaveEnemy is null)
        {
            if (GetEnemies().Count() == 0)
            {
                // No more enemies to spawn and all enemies are dead
                EndCurrentWave();
            }

            // No more enemies to spawn but some are still onscreen
            return;
        }


        // Create new enemy instance
        var enemy = EnemyScene.Instance<Enemy>();
        enemy.EnemyModel = nextWaveEnemy;

        enemy.InitialHealth = (int)(nextWaveEnemy.HealthMultiplier * Waves.WaveDisplayNumber);
        enemy.MoveSpeed = nextWaveEnemy.MoveSpeed;

        // Create new follow path and add enemy
        var followPath = new PathFollow2D();
        followPath.AddChild(enemy);

        // Add connections
        enemy.Connect(nameof(Enemy.EnemyDied), this, nameof(Enemy_EnemyDied));

        // Add enemy with path to the main node
        EnemyPath.AddChild(followPath);
    }

    /// <summary>
    /// End the current wave and adance to the next wave
    /// </summary>
    private void EndCurrentWave()
    {
        SelectedTurret = null;
        Shop.Money += Waves.WaveDisplayNumber * 10;

        var currentWave = Waves.GetCurrentWave();
        if (currentWave.DiceReward)
        {
            TowerRandomiser.TowerRandomiserShown();
            TowerRandomiser.Visible = true;
        }

        // Clear screen of bullets
        foreach (var bullet in Helpers.GetChildrenOfType<Bullet>(Bullets))
        {
            bullet.QueueFree();
        }

        // Hide ui elements
        RangeIndicator.Visible = false;
        TowerPreview.Visible = false;


        // Advance to the next wave of enemies
        Waves.AdvanceWave();
        var nextWave = Waves.GetCurrentWave();

        if (nextWave is null)
        {
            // All waves completed!
            GameOverVictory();
            return;
        }

        SpawnTimer.WaitTime = nextWave.SpawnDelay;


        var waveBonus = nextWave.DiceReward
            ? "Tower dice"
            : $"${Waves.WaveDisplayNumber * 10}";

        WaveBonusLabel.Text = $"Wave bonus: {waveBonus}";

        GetTree().Paused = true;
        ContinueButton.Visible = true;
        WaveLabel.Text = $"Wave {Waves.WaveDisplayNumber}/{Waves.TotalWaveCount}";
    }

    public IEnumerable<Turret> GetTurrets() => Helpers.GetChildrenOfType<Turret>(Turrets);

    public IEnumerable<PathFollow2D> GetEnemies() => Helpers.GetChildrenOfType<PathFollow2D>(EnemyPath);

    public void Turret_Shoot(Turret turret)
    {
        // Spawn bullets for the firing turret
        var newBullet = BulletScene.Instance<Bullet>();

        newBullet.Damage = turret.Damage;
        newBullet.MaxCollisions = turret.MaxCollisions;
        //TODO: set scale permanently

        var turretRoation = turret.Cannon.Rotation - CannonRotationOffset;

        // Velocity
        var direction = new Vector2((float)Math.Cos(turretRoation) * turret.BulletSpeed, (float)Math.Sin(turretRoation) * turret.BulletSpeed);
        newBullet.LinearVelocity = direction;

        // Position
        const int towerWidth = 128;
        const int bulletOffset = towerWidth / 2;
        var positionOffset = new Vector2((float)Math.Cos(turretRoation) * bulletOffset, (float)Math.Sin(turretRoation) * bulletOffset);

        newBullet.Position = turret.Position + positionOffset;

        Bullets.AddChild(newBullet);
    }


    public void Enemy_EnemyDied(Enemy enemy)
    {
        // Need to get the parent of the enemy because they are nested under a path follower
        var enemyParent = enemy.GetParentOrNull<PathFollow2D>();

        Shop.Money += enemy.Reward;

        // Remove the enemy from the scene
        enemyParent.QueueFree();
    }

    public void SpawnTurret(Vector2 position, ITurret turret)
    {
        if (Shop.Money < turret.Cost)
        {
            // Player doesn't have enough money
            GD.Print("Cannot spawn turret - cost is too high!");
            return;
        }

        // Take the cost of the turret
        Shop.Money -= turret.Cost;

        // Create the new turret
        var newTurret = TurretScene.Instance<Turret>();
        newTurret.Position = position;
        newTurret.BulletSpeed = turret.BulletSpeed;
        newTurret.RateOfFire = turret.RateOfFire;
        newTurret.TopSprite = turret.TopSprite;
        newTurret.BottomSprite = turret.BottomSprite;
        newTurret.RotateSpeed = turret.RotateSpeed;
        newTurret.Cost = turret.Cost;
        newTurret.Damage = turret.Damage;
        newTurret.Range = turret.Range;
        newTurret.MaxCollisions = turret.MaxCollisions;

        newTurret.Connect(nameof(Turret.TurretFired), this, nameof(Turret_Shoot));

        Turrets.AddChild(newTurret);

        SelectedTurret = null;
    }

    public void Shop_TurretSelected(TurretModel turret)
    {
        SelectedTurret = turret;
        TowerPreview.Texture = Helpers.TextureFromImagePath(turret.BottomSprite);
    }

    public void GameOver()
    {
        GetTree().Paused = true;
        GameOverMenu.Visible = true;
        GameOverMenu.SetScoreLabel(Waves.WaveDisplayNumber);

        GD.Print("Game over!");
    }

    public void GameOverVictory()
    {
        GetTree().Paused = true;
        GameOverMenu.Visible = true;
        GameOverMenu.ShowVictory(Waves.TotalWaveCount);

        GD.Print("Game over! - You win!");
    }

    public void TowerRandomiser_TowerRolled(TurretModel turret)
    {
        Shop.AddButton(turret);
    }

    public void ContinueButton_Pressed()
    {
        GetTree().Paused = false;
        SpawnTimer.Paused = false;
        ContinueButton.Visible = false;
    }


}
