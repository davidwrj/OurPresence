using System.Text;

namespace OurPresence.Modeller.Domain.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void TrimEnd(this StringBuilder stringBuilder, string value)
        {
            if (value == null)
                return;

            if (stringBuilder.ToString().EndsWith(value))
            {
                var x = stringBuilder.ToString().Length;
                var y = value.Length;
                stringBuilder.Remove(x - y, y);

                TrimEnd(stringBuilder, value);
            }
        }
    }
}

