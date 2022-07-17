using System.Collections.Generic;

namespace gmtkjame2022rollthedice.Data
{
    public class Waves
    {
        private int _waveNo = 0;

        public int WaveDisplayNumber => _waveNo + 1;

        private readonly List<Wave> _all = new List<Wave>()
        {
            new Wave(new []{ 1,1,1      }, diceReward: true),
            new Wave(new []{ 1,1,1,1,1  }),
            new Wave(new []{ 1,1,1,2,2  },2),
            new Wave(new []{ 2,2,2,2    },2),
            new Wave(new []{ 2,2,1,1,2,2,2 }, 2),
            new Wave(new []{ 1,1,1,2,2,2,3,}, 2, diceReward: true),
            new Wave(new []{ 2,3,2,3,2,3 },2),
            new Wave(new []{ 3,3,3,3,3 }, 2),
            new Wave(new []{ 2,2,2,3,3,3,3,3,3,3 }, 1, diceReward: true),
            new Wave(new []{ 1,2,3,4,   }, 2),
            new Wave(new []{ 3,3,3,3,4,4    }, 2),
            new Wave(new []{ 4,4,4,4    }, 2),
            new Wave(new []{ 4,4,3,3,2,2,2,2,}, 2),
            new Wave(new []{ 3,3,3,3,3,3,3,4,4,4,4,1,1,1 }, 1, diceReward: true),
            new Wave(new []{ 4,4,4,4,4,4,3,3,3,3,3 }, 1),
            new Wave(new []{ 1,2,3,4,5 }, 2),
            new Wave(new []{ 3,3,3,3,5,5 }, 2),
            new Wave(new []{ 3,3,3,3,3,3,3,3,5,5,5,4,4 }, 2, diceReward: true),
            new Wave(new []{ 3,3,3,3,3,3,3,3,5,5,5,4,4 }, 1),
            new Wave(new []{ 1,1,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5 }, 2.5f),
            new Wave(new []{ 1,1,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5 }, 2),
            new Wave(new []{ 1,1,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5 }, 1),
            new Wave(new []{ 1,1,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5 }, 0.5f),

           };

        public Wave GetNextWave()
        {
            _waveNo += 1;
            return _all[_waveNo];
        }

        public Wave GetCurrentWave()
        {
            if (_waveNo >= _all.Count)
            {
                // All waves completed!
                return null;
            }

            return _all[_waveNo];
        }

        public void AdvanceWave()
        {
            _waveNo += 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="waveNo">0-indexed wave number</param>
        /// <returns></returns>
        private Wave GetWave(int waveNo)
        {
            return _all[waveNo];
        }

    }

}
