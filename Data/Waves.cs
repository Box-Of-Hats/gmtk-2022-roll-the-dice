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
            new Wave(new []{ 1,1,2      }),
            new Wave(new []{ 2,2,2      }),
            new Wave(new []{ 2,2,1,1,2,2,2 }, 2),
            new Wave(new []{ 2,2,2,2,2,2,2,}, 2),
            new Wave(new []{ 3,3,3,3,3 }),
            new Wave(new []{ 3,3,3,3,3 }, 2),
            new Wave(new []{ 3,3,3,3,3,3,3,3,2,2 }, 1),
        };

        public Wave GetNextWave()
        {
            _waveNo += 1;
            return _all[_waveNo];
        }

        public Wave GetCurrentWave()
        {
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
