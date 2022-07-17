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
                RotateSpeed = 0.4f,
                Cost = 50,
                Range = 300
            },
            new CannonBase()
            {
                SpritePath = "res://sprites/base-3.png",
                RotateSpeed = 0.4f,
                Cost = 50,
                Range = 500,
                Damage = 2
            }
        };


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
            Damage = 2,
            RateOfFire = 5f,

        },
        // Sniper
        new Cannon()
        {
            SpritePath = "res://sprites/top-3.png",
            Cost = 150,
            BulletSpeed = 3000,
            Range = 1000,
            Damage = 5,
            RateOfFire = 2f,
            BulletSize =  1.2f
        },
        // Slow pulse cannon
        new Cannon()
        {
            SpritePath = "res://sprites/top-4.png",
            Cost = 150,
            BulletSpeed = 200,
            Damage = 3,
            RateOfFire = 0.6f,
            BulletSize = 1.5f
        },
        // Railgun
        new Cannon()
        {
            SpritePath = "res://sprites/top-5.png",
            Cost = 150,
            BulletSpeed = 800,
            Damage = 1,
            RateOfFire = 0.6f,
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
            RateOfFire = 0.2f,
            BulletSize = 3f,
            MaxCollisions = 1,
            Range = 20
        },
    };


    }

}
