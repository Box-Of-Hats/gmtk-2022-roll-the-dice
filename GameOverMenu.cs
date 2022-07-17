using Godot;
using System;

public class GameOverMenu : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Button>("Panel/Container/StartButton").Connect("pressed", this, nameof(Restart));

        SetScoreLabel(0);
    }

    public void SetScoreLabel(int waveNumber)
    {
        GetNode<Label>("Panel/ScoreLabel").Text = $"You made it to wave {waveNumber}";
    }

    public void Restart()
    {
        GetTree().Paused = false;
        var mapScene = GD.Load<PackedScene>("res://MainMenu.tscn");
        GetTree().ChangeSceneTo(mapScene);
    }
}
