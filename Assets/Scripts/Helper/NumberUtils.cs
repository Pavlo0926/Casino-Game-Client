using System.Globalization;

static class NumberUtils
{
    public static string ToFriendlyQuantityString(uint num)
    {
        if (num > 999999999)
        {
            return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999999)
        {
            return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999)
        {
            return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }

    public static string ToFriendlyQuantityString(int num)
    {
        if (num > 999999999)
        {
            return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999999)
        {
            return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999)
        {
            return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }
}