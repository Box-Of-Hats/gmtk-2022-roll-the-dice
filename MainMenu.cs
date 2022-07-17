using Godot;
using System;

public class MainMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var startButton = GetNode<Button>("Panel/Container/StartButton");

        var twitterButton = GetNode<Button>("Panel/Container/TwitterButton");
        var githubButton = GetNode<Button>("Panel/Container/GitHubButton");

        twitterButton.Connect("pressed", this, nameof(OpenTwitter));
        githubButton.Connect("pressed", this, nameof(OpenGitHub));

        startButton.Connect("pressed", this, nameof(StartGame));
    }

    public void StartGame()
    {
        var mapScene = GD.Load<PackedScene>("res://Map.tscn");

        GetTree().ChangeSceneTo(mapScene);
    }

    public void OpenTwitter() => OS.ShellOpen("https://twitter.com/Box_Of_Hats");
    public void OpenGitHub() => OS.ShellOpen("https://github.com/Box-Of-Hats");

}
