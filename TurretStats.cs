using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using Godot;
using System;
using System.Linq;

public class TurretStats : Control
{
    public VBoxContainer Container { get; set; }

    public ITurret Turret { get; set; }

    private Label _templateLabel { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Container = GetNode<VBoxContainer>("VBoxContainer");

        if (Turret != null)
        {
            LoadTurret(Turret);
        }

        //Save the first label so we can reference the theme later
        _templateLabel = Container.GetChild<Label>(0);
        _templateLabel.Visible = false;

    }

    public void LoadTurret(ITurret turret)
    {
        foreach (var child in Helpers.GetChildrenOfType<Control>(Container).Skip(1))
        {
            child.QueueFree();
        }

        Container.AddChild(MakeLabel($"$$$: {turret.Cost}", 2f));
        Container.AddChild(MakeLabel($"DMG: {turret.Damage}"));
        Container.AddChild(MakeLabel($"ROF: {turret.RateOfFire}"));
        Container.AddChild(MakeLabel($"PEN: {turret.MaxCollisions}"));
        Container.AddChild(MakeLabel($"RNG: {turret.Range}"));
    }

    public Label MakeLabel(string text, float size = 1f)
    {
        var label = _templateLabel.Duplicate() as Label;
        label.Text = text;
        label.Visible = true;

        return label;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
