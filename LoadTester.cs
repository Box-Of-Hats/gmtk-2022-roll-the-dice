using Godot;
using System;
using gmtkjame2022rollthedice.Data;
using gmtkjame2022rollthedice.Helpers;
using gmtkjame2022rollthedice.Interfaces;

/// <summary>
/// Node used to perform any resource loading tests on initial load.
/// This helps prevent runtime errors later on in cases where an import would fail
/// </summary>
public class LoadTester : Node
{
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    ensureEnemySpritesLoad();
    ensureCannonSpritesLoad();

    // Remove this node from the tree after checks are complete
    QueueFree();
  }

  private void ensureEnemySpritesLoad()
  {
    foreach (var enemyId in Enemies.All.Keys)
    {
      var enemy = Enemies.All[enemyId];
      ensureSpriteIsLoadable(enemy);
    }
  }

  private void ensureCannonSpritesLoad()
  {
    // Cannon bases
    foreach (var cannonBase in CannonData.CannonBases)
    {
      ensureSpriteIsLoadable(cannonBase);
    }

    // Cannons
    foreach (var cannon in CannonData.Cannons)
    {
      ensureSpriteIsLoadable(cannon);
    }
  }


  /// <summary>
  /// Ensure the sprite of a given object is loadable as a texture
  /// </summary>
  /// <param name="spriteObj"></param>
  private void ensureSpriteIsLoadable(IHasSpritePath spriteObj)
  {
    GD.Print($"Testing load of {spriteObj.GetType()} with sprite {spriteObj.SpritePath}");
    var loadedSprite = Helpers.TextureFromImagePath(spriteObj.SpritePath);
    if (loadedSprite is null)
    {
      throw new Exception($"Failed while asserting sprite load for sprite: {spriteObj.SpritePath}. Check that a file exists at that location and that the import type is set to 'Image'");
    }
  }

}
