using gmtkjame2022rollthedice.Models;
using System.Collections.Generic;

namespace gmtkjame2022rollthedice.Data
{
  public static class CannonData
  {
    public static List<CannonBase> CannonBases = new List<CannonBase>()
        {
            new CannonBase()
            {
                SpritePath = "res://sprites/base.png",
                RotateSpeed = 0.05f,
                Cost = 20,
                Range = 100
            },
            new CannonBase()
            {
                SpritePath = "res://sprites/base-2.png",
                RotateSpeed = 0.05f,
                Cost = 50,
                Range = 250
            },
            new CannonBase()
            {
                SpritePath = "res://sprites/base-3.png",
                RotateSpeed = 0.2f,
                Cost = 60,
                Range = 50,
                Damage = 2
            },

            new CannonBase()
            {
                SpritePath = "res://sprites/base-4.png",
                RotateSpeed = 0.1f,
                Cost = 45,
                Range = 200
            },
            new CannonBase()
            {
                SpritePath = "res://sprites/base-5.png",
                RotateSpeed = 0.01f,
                Cost = 50,
                Range = 100,
                Damage = 1
            },
            new CannonBase()
            {
                SpritePath = "res://sprites/base-6.png",
                RotateSpeed = 0.15f,
                Cost = 55,
                Range = 270
            }
        };


    public static List<Cannon> Cannons = new List<Cannon>()
    {
        // Basic
        new Cannon()
        {
            SpritePath = "res://sprites/top.png",
            Cost = 110,
            Damage = 1,
            RateOfFire = 1f,
            Range = 400
        },
        // Rapid fire
        new Cannon()
        {
            SpritePath = "res://sprites/top-2.png",
            Cost = 150,
            Damage = 1,
            RateOfFire = 0.5f,
            Range = 360
        },
        // Sniper
        new Cannon()
        {
            SpritePath = "res://sprites/top-3.png",
            Cost = 160,
            BulletSpeed = 3000,
            Range = 800,
            Damage = 5,
            RateOfFire = 2.2f,
            BulletSize =  1.2f
        },
        // Slow pulse cannon
        new Cannon()
        {
            SpritePath = "res://sprites/top-4.png",
            Cost = 140,
            BulletSpeed = 160,
            Damage = 3,
            RateOfFire = 0.6f,
            BulletSize = 1.5f
        },
        // Railgun
        new Cannon()
        {
            SpritePath = "res://sprites/top-5.png",
            Cost = 155,
            BulletSpeed = 800,
            Damage = 2,
            RateOfFire = 1.2f,
            BulletSize = 3f,
            MaxCollisions = 3
        },
        // Sprayer
        new Cannon()
        {
            SpritePath = "res://sprites/top-6.png",
            Cost = 200,
            BulletSpeed = 500,
            Damage = 1,
            RateOfFire = 0.33f,
            BulletSize = 3f,
            MaxCollisions = 1,
            Range = 20
        },
    };


  }

}
