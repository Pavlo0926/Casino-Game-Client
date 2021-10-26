using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringFormatUtils
{
    public static string CurrencyString(long payout) => payout.ToString("n0");

    public static string PayoutString(long payout)
    {
        if (payout < 0)
        {
            return $"<color=#FF0000>{payout.ToString("n0")}</color>";
        }
        else
        {
            return $"<color=#279F18>+{payout.ToString("n0")}</color>";
        }
    }

    public static string PercentagePayoutString(double payout)
    {
        if (payout < 0)
        {
            return $"<color=#FF0000>{(payout).ToString("0.#")}%</color>";
        }
        else
        {
            return $"<color=#279F18>+{(payout).ToString("0.#")}%</color>";
        }
    }
}
