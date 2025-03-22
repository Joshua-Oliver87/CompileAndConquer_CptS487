using Microsoft.Xna.Framework;

namespace CompileAndConquer.Content.WaveManager
{
    public interface IWave
    {
        void Update(GameTime gameTime);
        bool IsComplete();
    }
}