using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using Godot;
using System;

public class TurretStats : Control
{
    public VBoxContainer Container { get; set; }

    public ITurret Turret { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Container = GetNode<VBoxContainer>("VBoxContainer");

        if (Turret != null)
        {
            LoadTurret(Turret);
        }
    }

    public void LoadTurret(ITurret turret)
    {
        foreach (var child in Helpers.GetChildrenOfType<Control>(Container))
        {
            child.QueueFree();
        }

        Container.AddChild(MakeLabel($"$$$: {turret.Cost}", 1.2f));
        Container.AddChild(MakeLabel($"DMG: {turret.Damage}"));
        Container.AddChild(MakeLabel($"ROF: {turret.RateOfFire}"));
        Container.AddChild(MakeLabel($"PEN: {turret.MaxCollisions}"));
        Container.AddChild(MakeLabel($"RNG: {turret.Range}"));
    }

    public Label MakeLabel(string text, float size = 1f)
    {
        var label = new Label
        {
            Text = text,
            RectScale = new Vector2(size, size)
        };
        return label;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
