// SpawnPatterns/LineSpawnPattern.cs
using Microsoft.Xna.Framework;

namespace CompileAndConquer.SpawnPatterns
{
    public class LineSpawnPattern : ISpawnPattern
    {
      private readonly float _y;
      public LineSpawnPattern(float y) => _y = y;
    
      public Vector2 GetPosition(int i,int total,Vector2 screen)
      {
        float gap = screen.X/(total+1);
        float x   = gap*(i+1);
        return new Vector2(x, _y);
      }
    }
}