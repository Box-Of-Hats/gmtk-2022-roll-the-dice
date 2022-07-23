using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gmtkjame2022rollthedice.Helpers
{
  public static class Helpers
  {

    /// <summary>
    /// Get all children of a given type from a specified root node
    /// </summary>
    /// <typeparam name="ChildType"></typeparam>
    /// <param name="rootNode"></param>
    /// <returns></returns>
    public static IEnumerable<ChildType> GetChildrenOfType<ChildType>(Node rootNode)
    {
      if (rootNode == null)
      {
        throw new ArgumentNullException(nameof(rootNode));
      }

      foreach (ChildType child in rootNode.GetChildren())
      {
        if (child is ChildType)
        {
          yield return child;
        }

      }
    }

    /// <summary>
    /// Create a texture from a given image path
    /// </summary>
    /// <param name="imagePath"></param>
    /// <returns></returns>
    public static ImageTexture TextureFromImagePath(string imagePath)
    {
      var imageTexture = new ImageTexture();

      var image = GD.Load<Image>(imagePath);
      if (image is null)
      {
        GD.PrintErr("Could not find sprite to load at path: ", imagePath);
        return null;
      }

      imageTexture.CreateFromImage(image);

      return imageTexture;
    }

  }
}
