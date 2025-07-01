using System.Drawing;

namespace AssetManagement.Client.Helpers
{
    public class ColorHelper
    {
        public static string[] GetBackgroundColors(string color, int arraySize)
        {
            var colors = new List<string>();
            for (var i = 0; i < arraySize; i++)
                colors.Add(color.ToLower());
            return colors.ToArray();
        }
    }
}
