using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using gmtkjame2022rollthedice.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class TowerRandomiser : Node2D
{
    [Signal()]
    public delegate void TowerAccepted(TurretModel turret);

    [Signal()]
    public delegate void TowerRolled(TurretModel turret);

    public VisibilityNotifier2D VisibilityNotifier2D { get; set; }
    public Label InfoLabel { get; set; }

    public Turret TurretPreview { get; set; }

    public static List<Cannon> Cannons = new List<Cannon>()
    {
        // Basic
        new Cannon()
        {
            SpritePath = "res://sprites/top.png",
            Cost = 100,
            Damage = 1,
            RateOfFire = 1f

        },
        // Rapid fire
        new Cannon()
        {
            SpritePath = "res://sprites/top-2.png",
            Cost = 150,
            Damage = 5,
            RateOfFire = 5f,

        },
        // Sniper
        new Cannon()
        {
            SpritePath = "res://sprites/top-3.png",
            Cost = 150,
            BulletSpeed = 2000,
            Damage = 5,
            RateOfFire = 2f,
            BulletSize =  1.2f
        },
        // Slow pulse cannon
        new Cannon()
        {
            SpritePath = "res://sprites/top-4.png",
            Cost = 150,
            BulletSpeed = 200,
            Damage = 3,
            RateOfFire = 0.6f,
            BulletSize = 1.5f
        },
        // Railgun
        new Cannon()
        {
            SpritePath = "res://sprites/top-5.png",
            Cost = 150,
            BulletSpeed = 800,
            Damage = 1,
            RateOfFire = 0.6f,
            BulletSize = 3f,
            MaxCollisions = 3
        },
    };

    public static List<CannonBase> CannonBases = new List<CannonBase>()
    {
        new CannonBase()
        {
            SpritePath = "res://sprites/base.png",
            RotateSpeed = 0.05f,
            Cost = 20
        },
        new CannonBase()
        {
            SpritePath = "res://sprites/base-2.png",
            RotateSpeed = 0.4f,
            Cost = 50,
            Range = 300
        },
        new CannonBase()
        {
            SpritePath = "res://sprites/base-3.png",
            RotateSpeed = 0.4f,
            Cost = 50,
            Range = 500,
            Damage = 100
        }
    };


    public Button RollButton { get; set; }
    public Button ReRollButton { get; set; }
    public Button AcceptButton { get; set; }

    public Cannon CannonTop { get; set; }
    public CannonBase CannonBase { get; set; }

    public Dice TopDice { get; set; }
    public Dice BaseDice { get; set; }

    public TurretStats TurretStats { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        // Nodes 
        RollButton = GetNode<Button>("RollButton");
        AcceptButton = GetNode<Button>("AcceptButton");
        ReRollButton = GetNode<Button>("ReRollButton");
        VisibilityNotifier2D = GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
        TopDice = GetNode<Dice>("TopDiceWrapper/TopDice");
        BaseDice = GetNode<Dice>("BaseDiceWrapper/BaseDice");
        InfoLabel = GetNode<Label>("InfoLabel");
        TurretPreview = GetNode<Turret>("TurretPreview");
        TurretStats = GetNode<TurretStats>("TurretStats");


        RollButton.Connect("pressed", this, nameof(RollDice));

        //TODO: Implement!
        AcceptButton.Connect("pressed", this, nameof(AcceptDiceRoll));
        ReRollButton.Connect("pressed", this, nameof(RollDice));

        // Whenever the tower randomiser is shown, pause the game
        VisibilityNotifier2D.Connect("screen_entered", this, nameof(TowerRandomiserShown));

        TopDice.Connect(nameof(Dice.RollFinished), this, nameof(TopDice_RollFinished));

        BaseDice.Connect(nameof(Dice.RollFinished), this, nameof(BaseDice_RollFinished));

    }

    public void TowerRandomiserShown()
    {
        // Pause game and hide appropriate controls
        GetTree().Paused = true;
        RollButton.Visible = true;
        AcceptButton.Visible = false;
        InfoLabel.Visible = false;
        TurretStats.Visible = false;
    }


    public void RollDice()
    {
        // Randomise order of sprite list
        TopDice.Sprites = Cannons.Select(c => c.SpritePath)
                                .OrderBy(a => Guid.NewGuid())
                                .ToList();

        BaseDice.Sprites = CannonBases.Select(c => c.SpritePath)
                        .OrderBy(a => Guid.NewGuid())
                        .ToList();

        RollButton.Visible = false;
        TopDice.Roll();
        BaseDice.Roll();
    }

    /// <summary>
    /// Called after dice have finished roll animation
    /// </summary>
    public void TopDice_RollFinished(string spritePath)
    {
        //TODO: Show re-roll button
        AcceptButton.Visible = true;
        CannonTop = Cannons.FirstOrDefault(o => o.SpritePath == spritePath);

        EmitSignal(nameof(TowerRolled), TopDice.CurrentSprite());
    }

    /// <summary>
    /// Called after dice have finished roll animation
    /// </summary>
    public void BaseDice_RollFinished(string spritePath)
    {
        //TODO: Show re-roll button???
        AcceptButton.Visible = true;
        CannonBase = CannonBases.FirstOrDefault(o => o.SpritePath == spritePath);

        EmitSignal(nameof(TowerRolled), BaseDice.CurrentSprite());
        ShowPreview();
    }

    public void ShowPreview()
    {
        // Update preview image
        TurretPreview.CannonBase.Texture = Helpers.TextureFromImagePath(CannonBase.SpritePath);
        TurretPreview.Cannon.Texture = Helpers.TextureFromImagePath(CannonTop.SpritePath);


        InfoLabel.Visible = true;
        TurretPreview.Visible = true;
        TurretStats.Visible = true;
        TurretStats.LoadTurret(MergeTowerParts(CannonBase, CannonTop));
    }



    public void AcceptDiceRoll()
    {
        var tower = MergeTowerParts(CannonBase, CannonTop);

        EmitSignal(nameof(TowerAccepted), tower);
        Visible = false;
        GetTree().Paused = false;
    }

    /// <summary>
    /// Merge parts of a cannon into a single turret
    /// </summary>
    /// <param name="cannonBase"></param>
    /// <param name="cannon"></param>
    /// <returns></returns>
    public static TurretModel MergeTowerParts(CannonBase cannonBase, Cannon cannon)
    {
        var turret = new TurretModel()
        {
            // Cannon
            BulletSpeed = cannon.BulletSpeed,
            Damage = cannon.Damage + cannonBase.Damage,
            RateOfFire = cannon.RateOfFire,
            TopSprite = cannon.SpritePath,
            Cost = cannon.Cost + cannonBase.Cost,
            Range = cannon.Range + cannonBase.Range,
            BulletSize = cannon.BulletSize,
            MaxCollisions = cannon.MaxCollisions,


            // Base
            RotateSpeed = cannonBase.RotateSpeed,
            BottomSprite = cannonBase.SpritePath,

        };
        return turret;
    }

    public static TurretModel GetRandomTower()
    {
        var cannon = GetRandomCannon();
        var cannonBase = CannonBases[(int)GD.RandRange(0, CannonBases.Count)];

        return MergeTowerParts(cannonBase, cannon);
    }

    public static Cannon GetRandomCannon()
    {
        return Cannons[(int)GD.RandRange(0, Cannons.Count)];
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
