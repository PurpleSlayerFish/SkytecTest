using UnityEngine;

namespace PurpleSlayerFish.Core.Services
{
    public class StringUtils
    {
        public string FromVector2(Vector2 vector, string separator, int roundTo) => string.Format("{0}{2}{1}", Mathf.Round(vector.x * roundTo) / roundTo, Mathf.Round(vector.y * roundTo) / roundTo, separator);
        public string FromFloat(float value, int roundTo) => $"{Mathf.Round(value * roundTo) / roundTo}";
        public string MinutesAndSeconds(float seconds) => string.Format("{0:00}:{1:00}", Mathf.FloorToInt(seconds / 60f), Mathf.FloorToInt(seconds % 60f));
    }
}