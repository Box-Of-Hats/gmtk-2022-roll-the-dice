using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using gmtkjame2022rollthedice.Models;
using gmtkjame2022rollthedice.Data;
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


    public Button RollButton { get; set; }
    public Button ReRollButton { get; set; }
    public Button AcceptButton { get; set; }
    public ColorRect TurretStatsBg { get; set; }

    public Cannon CannonTop { get; set; }
    public CannonBase CannonBase { get; set; }

    public Dice TopDice { get; set; }
    public Dice BaseDice { get; set; }

    public TurretStats TurretStats { get; set; }

    public Node2D DiceImage { get; set; }

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
        TurretStatsBg = GetNode<ColorRect>("TurretStatsBg");
        DiceImage = GetNode<Node2D>("DiceImage");


        RollButton.Connect("pressed", this, nameof(RollDice));

        //TODO: Implement!
        AcceptButton.Connect("pressed", this, nameof(AcceptDiceRoll));
        ReRollButton.Connect("pressed", this, nameof(RollDice));

        // Whenever the tower randomiser is shown, pause the game
        VisibilityNotifier2D.Connect("screen_entered", this, nameof(TowerRandomiserShown));

        TopDice.Connect(nameof(Dice.RollFinished), this, nameof(TopDice_RollFinished));

        BaseDice.Connect(nameof(Dice.RollFinished), this, nameof(BaseDice_RollFinished));


        // Set sprites of the dice image
        var initialRandomTower = TowerRandomiser.GetRandomTower();
        GetNode<Sprite>("DiceImage/Base/Fg").Texture = initialRandomTower.BottomSprite;
        GetNode<Sprite>("DiceImage/Top/Fg").Texture = initialRandomTower.TopSprite;

        TowerRandomiserShown();

    }

    public void TowerRandomiserShown()
    {
        InfoLabel.Text = "You got some new dice!";

        // Pause game and hide appropriate controls
        GetTree().Paused = true;
        RollButton.Visible = true;
        AcceptButton.Visible = false;
        TurretStats.Visible = false;
        TurretStatsBg.Visible = false;
        TurretPreview.Visible = false;

        TopDice.Visible = false;
        TopDice.Scale = new Vector2(4f, 4f);

        BaseDice.Visible = false;
        BaseDice.Scale = new Vector2(4f, 4f);

        DiceImage.Visible = true;
    }


    public void RollDice()
    {
        DiceImage.Visible = false;
        TopDice.Visible = true;
        BaseDice.Visible = true;

        // Randomise order of sprite list
        TopDice.Sprites = CannonData.Cannons.Select(c => c.SpriteTexture)
                                .OrderBy(a => Guid.NewGuid())
                                .ToList();

        BaseDice.Sprites = CannonData.CannonBases.Select(c => c.SpriteTexture)
                        .OrderBy(a => Guid.NewGuid())
                        .ToList();

        RollButton.Visible = false;
        TopDice.Roll();
        BaseDice.Roll();
    }

    /// <summary>
    /// Called after dice have finished roll animation
    /// </summary>
    public void TopDice_RollFinished(Texture spritePath)
    {
        //TODO: Show re-roll button
        AcceptButton.Visible = true;
        CannonTop = CannonData.Cannons.FirstOrDefault(o => o.SpriteTexture == spritePath);

        EmitSignal(nameof(TowerRolled), TopDice.CurrentSprite());
    }

    /// <summary>
    /// Called after dice have finished roll animation
    /// </summary>
    public void BaseDice_RollFinished(Texture spritePath)
    {
        //TODO: Show re-roll button???
        AcceptButton.Visible = true;
        CannonBase = CannonData.CannonBases.FirstOrDefault(o => o.SpriteTexture == spritePath);

        EmitSignal(nameof(TowerRolled), BaseDice.CurrentSprite());
        ShowPreview();
    }

    public void ShowPreview()
    {
        // Update preview image
        TurretPreview.CannonBase.Texture = CannonBase.SpriteTexture;
        TurretPreview.Cannon.Texture = CannonTop.SpriteTexture;

        InfoLabel.Text = "New tower unlocked!";


        InfoLabel.Visible = true;
        TurretPreview.Visible = true;
        TurretStats.Visible = true;
        TurretStatsBg.Visible = true;
        TurretStats.LoadTurret(MergeTowerParts(CannonBase, CannonTop));
    }



    public void AcceptDiceRoll()
    {
        var tower = MergeTowerParts(CannonBase, CannonTop);

        EmitSignal(nameof(TowerAccepted), tower);
        Visible = false;
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
            RateOfFire = cannon.RateOfFire,
            BulletSize = cannon.BulletSize,
            MaxCollisions = cannon.MaxCollisions,

            // Base
            RotateSpeed = cannonBase.RotateSpeed,

            // Combo
            Damage = cannon.Damage + cannonBase.Damage,
            Cost = cannon.Cost + cannonBase.Cost,
            Range = cannon.Range + cannonBase.Range,

            // Sprites
            TopSprite = cannon.SpriteTexture,
            BottomSprite = cannonBase.SpriteTexture,

        };
        return turret;
    }

    public static TurretModel GetRandomTower()
    {
        var cannon = GetRandomCannon();
        var cannonBase = CannonData.CannonBases[(int)GD.RandRange(0, CannonData.CannonBases.Count)];

        return MergeTowerParts(cannonBase, cannon);
    }

    public static Cannon GetRandomCannon()
    {
        return CannonData.Cannons[(int)GD.RandRange(0, CannonData.Cannons.Count)];
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
