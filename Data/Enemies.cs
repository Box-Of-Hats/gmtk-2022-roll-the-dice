using gmtkjame2022rollthedice.Models;
using System.Collections.Generic;

namespace gmtkjame2022rollthedice.Data
{
    public static class Enemies
    {
        public static Dictionary<int, EnemyModel> All = new Dictionary<int, EnemyModel>()
        {
            {
                1,  new EnemyModel()
               {
                    SpritePath = "res://sprites/enemy-001.png",
                    MoveSpeed = 100
                }
            },
            {
                2, new EnemyModel()
                {
                    SpritePath = "res://sprites/enemy-002.png",
                    MoveSpeed= 100,
                    HealthMultiplier = 1.5f
                }
            },
            {
                3, new EnemyModel()
                {
                    SpritePath = "res://sprites/enemy-003.png",
                    MoveSpeed = 400,
                    HealthMultiplier = 0.7f
                }
            },
            {
                4, new EnemyModel()
                {
                    SpritePath = "res://sprites/enemy-004.png",
                    MoveSpeed = 100,
                    HealthMultiplier = 5f
                }
            }
        };

    }
}
