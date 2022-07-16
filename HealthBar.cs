using Godot;
using System;

public class HealthBar : Node2D
{
    [Export(PropertyHint.Range, "0, 100")]
    public int CurrentHealth { get; set; } = 100;

    [Export(PropertyHint.Range, "0, 100")]
    public int MaxHealth { get; set; } = 100;

    [Export(PropertyHint.Range, "0, 100")]
    public int MinHealth { get; set; } = 0;

    public Label Label { get; set; }

    public ColorRect Green { get; set; }

    private float MaxRectangleLength { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Validity check
        if (MinHealth >= MaxHealth)
        {
            throw new Exception($"Min health ({MinHealth}) must be less than max health ({MaxHealth})");
        }

        // Nodes
        Label = GetNode<Label>("Label");
        Green = GetNode<ColorRect>("Green");

        MaxRectangleLength = Green.RectSize.x;

        UpdateHealth(CurrentHealth);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GlobalRotation = 0;
    }

    public void UpdateHealth(int newHealth)
    {
        // Keep health in allowed range
        newHealth = Math.Min(MaxHealth, Math.Max(0, newHealth));

        CurrentHealth = newHealth;

        // Update display

        Label.Text = $"{CurrentHealth}/{MaxHealth}";
        var percentageHealth = (float)CurrentHealth / (float)MaxHealth;
        Green.RectSize = new Vector2(percentageHealth * MaxRectangleLength, Green.RectSize.y);

    }


}
