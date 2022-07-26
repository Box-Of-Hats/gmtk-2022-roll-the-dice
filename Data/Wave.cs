using gmtkjame2022rollthedice.Models;
using System.Collections.Generic;
using System.Linq;

namespace gmtkjame2022rollthedice.Data
{
    public class Wave
    {
        /// <summary>
        /// An ordered list of enemy IDs to spawn
        /// </summary>
        private List<int> _enemies;

        private int _enemyIndex = -1;


        /// <summary>
        /// On completion, how much bonus money should the player recieve?
        /// </summary>
        public int CompletionBonus { get; set; } = 0;
        /// <summary>
        /// On completion, should this wave reward a new tower?
        /// </summary>
        public bool DiceReward { get; set; } = false;

        /// <summary>
        /// The number of seconds to wait before spawning each enemy
        /// </summary>
        public float SpawnDelay;

        public Wave(IEnumerable<int> enemies, float spawnDelay = 3f, bool diceReward = false)
        {
            _enemies = enemies.ToList();
            SpawnDelay = spawnDelay;
            DiceReward = diceReward;
        }

        public EnemyModel GetNextEnemy()
        {
            _enemyIndex += 1;

            if (_enemyIndex >= this._enemies.Count)
            {
                // No more enemies in the wave
                return null;
            }

            var enemyId = _enemies[_enemyIndex];

            return Data.Enemies.All[enemyId];
        }


        /// <summary>
        /// Get a list of enemies that will spawn in this wave
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EnemyModel> GetRemainingEnemies()
        {
            return this._enemies.Skip(_enemyIndex + 1)
                                .Select(enemyId => Data.Enemies.All[enemyId]);
        }
    }
}
