using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using Godot;
using System;

public class Shop : Node2D
{
    [Signal]
    public delegate void TurretSelected(Turret turret);

    public Node TowerList { get; set; }
    public Label MoneyLabel { get; set; }

    public PackedScene ButtonGenerator { get; set; }

    private int _money;

    [Export]
    public int Money
    {
        get
        {
            return _money;
        }
        set
        {
            if (MoneyLabel != null)
            {
                MoneyLabel.Text = $"${value}";
            }
            _money = value;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        // Nodes
        MoneyLabel = GetNode<Label>("MoneyLabel");
        TowerList = GetNode<Node>("TowerList");

        // Spawners
        ButtonGenerator = GD.Load<PackedScene>("res://TowerButton.tscn");

        // Hack to set initial money label
        Money = Money;

        // Remove any existing buttons on load
        foreach (var button in Helpers.GetChildrenOfType<TowerButton>(TowerList))
        {
            button.QueueFree();
        }



        // Add buttons
        //AddButton(new TurretModel()
        //{
        //    RotateSpeed = 1,
        //    BulletSpeed = 400,
        //    RateOfFire = 0.3f,
        //    TopSprite = "res://sprites/top-2.png",
        //    BottomSprite = "res://sprites/base.png",
        //    Cost = 100,
        //    Damage = 1,
        //    Range = 100

        //});

        //AddButton(new TurretModel()
        //{
        //    RotateSpeed = 1,
        //    BulletSpeed = 400,
        //    RateOfFire = 0.6f,
        //    TopSprite = "res://sprites/top.png",
        //    BottomSprite = "res://sprites/base.png",
        //    Cost = 240,
        //    Damage = 3,
        //    Range = 400
        //});

        AddButton(TowerRandomiser.GetRandomTower());
        AddButton(TowerRandomiser.GetRandomTower());
        AddButton(TowerRandomiser.GetRandomTower());
    }

    public void Button_Pressed(TowerButton button)
    {
        GD.Print("Pressed button (shop): ", button.Turret.TopSprite, button.Turret.BottomSprite);
        EmitSignal(nameof(TurretSelected), button.Turret);

    }

    /// <summary>
    /// Add a new button to the tower list
    /// </summary>
    /// <param name="turret"></param>
    /// <returns></returns>
    public TowerButton AddButton(ITurret turret)
    {
        var newButton = ButtonGenerator.Instance<TowerButton>();
        newButton.Turret = turret;

        newButton.Connect("pressed", this, nameof(Button_Pressed), new Godot.Collections.Array(newButton));
        TowerList.AddChild(newButton);

        return newButton;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}

