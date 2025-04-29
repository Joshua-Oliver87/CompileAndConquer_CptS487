using Microsoft.Xna.Framework;

namespace CompileAndConquer.SpawnPatterns
{
    public class PyramidSpawnPattern : ISpawnPattern
    {
        private readonly int rows;
        private readonly float spacing;
        private readonly float y;

        /// <param name="rows">Number of rows in the pyramid (total spawns = rows*(rows+1)/2).</param>
        /// <param name="spacing">Horizontal & vertical spacing between enemies.</param>
        /// <param name="yOffset">Vertical offset from the top of the screen.</param>
        public PyramidSpawnPattern(int rows, float spacing, float y = 0f)
        {
            this.rows = rows;
            this.spacing = spacing;
            this.y = y;
        }

        public Vector2 GetPosition(int index, int total, Vector2 screenSize)
        {
            // Find which row this index sits in
            int count = 0;
            for (int r = 0; r < rows; r++)
            {
                int rowCount = r + 1;
                if (index < count + rowCount)
                {
                    int idxInRow = index - count;
                    float rowWidth = rowCount * spacing;
                    float startX = (screenSize.X - rowWidth) / 2f;
                    float x = startX + idxInRow * spacing + spacing / 2f;
                    float y_new = y + r * spacing;
                    return new Vector2(x, y_new);
                }
                count += rowCount;
            }

            // fallback
            return Vector2.Zero;
        }
    }
}