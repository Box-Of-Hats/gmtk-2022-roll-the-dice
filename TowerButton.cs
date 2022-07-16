using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using Godot;
using System;

public class TowerButton : Button
{
    public ITurret Turret { get; set; }

    public Sprite TopSprite { get; set; }
    public Sprite BottomSprite { get; set; }
    public Label CostLabel { get; set; }
    public Label DamageLabel { get; set; }
    public TurretStats TurretStats { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TopSprite = GetNode<Sprite>("Icon/Top");
        BottomSprite = GetNode<Sprite>("Icon/Bottom");
        CostLabel = GetNode<Label>("CostLabel");
        DamageLabel = GetNode<Label>("DamageLabel");
        TurretStats = GetNode<TurretStats>("TurretStats");

        if (Turret is null)
        {
            GD.PrintErr("No turret set for TowerButton - ", this);
            return;
        }

        TurretStats.LoadTurret(Turret);


        // Set turret image to match the loaded turret
        // Top
        TopSprite.Texture = Helpers.TextureFromImagePath(Turret.TopSprite);

        // Bottom
        BottomSprite.Texture = Helpers.TextureFromImagePath(Turret.BottomSprite);

        CostLabel.Text = $"${Turret.Cost}";
        DamageLabel.Text = $"{Turret.Damage} dmg";


    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
