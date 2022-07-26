using gmtkjame2022rollthedice.Helpers;
using Godot;
using System;
using System.Collections.Generic;

public class Dice : RigidBody2D
{
    [Signal]
    public delegate void RollFinished(Texture spritePath);

    public AnimationPlayer AnimationPlayer { get; set; }


    public List<Texture> Sprites = new List<Texture>();

    public Sprite Icon { get; set; }

    private int SpriteIndex { get; set; } = 0;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Icon = GetNode<Sprite>("Icon");
    }

    public void EndRoll()
    {
        EmitSignal(nameof(RollFinished), CurrentSprite());
    }

    public void Roll()
    {
        AnimationPlayer.Play("RollCannons");
    }

    public void NextSprite()
    {
        SpriteIndex = (SpriteIndex + 1) % Sprites.Count;
        Icon.Texture = Sprites[SpriteIndex];
    }

    /// <summary>
    /// Get the currently rolled sprite
    /// </summary>
    /// <returns></returns>
    public Texture CurrentSprite()
    {
        return Sprites[SpriteIndex];
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
