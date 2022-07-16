using gmtkjame2022rollthedice.Models;
using System.Collections.Generic;
using System.Linq;

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
                    MoveSpeed= 150
                }
            },
            {
                3, new EnemyModel()
                {
                    SpritePath = "res://sprites/enemy-003.png",
                    MoveSpeed = 100,
                }
            }
        };

    }

    public static class Waves
    {
        public static List<Wave> All = new List<Wave>()
        {
            new Wave(new []{ 1,1,1      }),
            new Wave(new []{ 1,1,2      }),
            new Wave(new []{ 2,2,2      }),
            new Wave(new []{ 2,2,2,2,2  }),
            new Wave(new []{ 3 }),

        };
    }

    public class Wave
    {
        /// <summary>
        /// An ordered list of enemy IDs to spawn
        /// </summary>
        public List<int> Enemies;

        /// <summary>
        /// The number of seconds to wait before spawning each enemy
        /// </summary>
        public float SpawnDelay;

        public Wave(IEnumerable<int> enemies, float spawnDelay = 3f)
        {
            Enemies = enemies.ToList();
            SpawnDelay = spawnDelay;
        }
    }
}
