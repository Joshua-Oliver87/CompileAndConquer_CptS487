using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CompileAndConquer.Content.WaveManager
{
    public interface IWave
    {
        void Update(GameTime gameTime);
        bool IsComplete();
    }
}