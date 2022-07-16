using Godot;
using System;

public class Shop : Node2D
{
    public Node TowerList { get; set; }

    public PackedScene ButtonGenerator { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //TowerList = GetNode<Node>("TowerList");

        //ButtonGenerator = GD.Load<PackedScene>("res://TowerButton.tscn");

        //var newButton = ButtonGenerator.Instance<TowerButton>();
        //newButton.Turret = //???
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
