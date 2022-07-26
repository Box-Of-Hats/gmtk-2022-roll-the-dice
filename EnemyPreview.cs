using gmtkjame2022rollthedice.Helpers;
using gmtkjame2022rollthedice.Models;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyPreview : Control
{
    private HBoxContainer _enemies { get; set; }


    private TextureRect _templateEnemy { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _enemies = GetNode<HBoxContainer>("Enemies");

        // Save the first sample enemy and use it as a template for future enemies
        var sampleEnemies = Helpers.GetChildrenOfType<TextureRect>(_enemies);
        _templateEnemy = sampleEnemies.First().Duplicate() as TextureRect;

        // Remove all sample enemies
        foreach (var enemy in sampleEnemies)
        {
            enemy.QueueFree();
        }
    }

    /// <summary>
    /// Load a list of <paramref name="enemies"/> into the enemy preview
    /// </summary>
    /// <param name="enemies">A list of enemies to preview, in order of appearance</param>
    public void LoadEnemyList(IEnumerable<EnemyModel> enemies)
    {
        // Clear any existing enemies
        var enemiesToClear = Helpers.GetChildrenOfType<TextureRect>(_enemies);
        foreach (var enemy in enemiesToClear)
        {
            enemy.QueueFree();
        }

        // Render new enemy list
        foreach (var enemy in enemies.Reverse())
        {
            var newEnemy = _templateEnemy.Duplicate() as TextureRect;
            newEnemy.Visible = true;
            newEnemy.Texture = enemy.SpriteTexture;
            _enemies.AddChild(newEnemy);
        }
    }

}
