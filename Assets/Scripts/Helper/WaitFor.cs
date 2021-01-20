using System.Collections;

namespace Assets.Scripts.Helper
{
    public static class WaitFor {
        public static IEnumerator Frames(int frameCount)
        {
            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }
    }
}