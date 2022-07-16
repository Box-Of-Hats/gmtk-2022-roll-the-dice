using gmtkjame2022rollthedice;
using gmtkjame2022rollthedice.Helpers;
using gmtkjame2022rollthedice.Models;
using Godot;
using System;
using System.Collections.Generic;

public class TowerRandomiser : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public static List<Cannon> Cannons = new List<Cannon>()
    {
        // Basic
        new Cannon()
        {
            SpritePath = "res://sprites/top.png",
            Cost = 100,
            Damage = 1,
            RateOfFire = 1f

        },
        // Rapid fire
        new Cannon()
        {
            SpritePath = "res://sprites/top-2.png",
            Cost = 150,
            Damage = 5,
            RateOfFire = 5f
        },
        // Sniper
        new Cannon()
        {
            SpritePath = "res://sprites/top-3.png",
            Cost = 150,
            BulletSpeed = 2000,
            Damage = 5,
            RateOfFire = 2f
        },
        // Slow pulse cannon
        new Cannon()
        {
            SpritePath = "res://sprites/top-4.png",
            Cost = 150,
            BulletSpeed = 250,
            Damage = 3,
            RateOfFire = 1f
        },
    };

    public static List<CannonBase> CannonBases = new List<CannonBase>()
    {
        new CannonBase()
        {
            SpritePath = "res://sprites/base.png",
            RotateSpeed = 0.1f,
            Cost = 20
        }
    };


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public static ITurret GetRandomTower()
    {
        GD.Randomize();

        var cannon = Cannons[(int)GD.RandRange(0, Cannons.Count)];

        var cannonBase = CannonBases[(int)GD.RandRange(0, CannonBases.Count - 1)];

        var turret = new Turret()
        {
            // Cannon
            BulletSpeed = cannon.BulletSpeed,
            Damage = cannon.Damage,
            RateOfFire = cannon.RateOfFire,
            TopSprite = cannon.SpritePath,
            Cost = cannon.Cost + cannonBase.Cost,

            // Base
            BottomSprite = cannonBase.SpritePath,
        };

        return turret;
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
