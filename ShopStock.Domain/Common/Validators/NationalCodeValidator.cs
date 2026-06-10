using System.Text.RegularExpressions;

namespace ShopStock.Domain.Common.Validators;

public static class NationalCodeValidator
{
    public static bool IsValid(string nationalCode)
    {
        if (string.IsNullOrWhiteSpace(nationalCode))
            return false;

        if (!Regex.IsMatch(nationalCode, @"^\d{10}$"))
            return false;

        if (new string(nationalCode[0], 10) == nationalCode)
            return false;

        int check = int.Parse(nationalCode[9].ToString());

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(nationalCode[i].ToString()) * (10 - i);
        }

        int remainder = sum % 11;

        return (remainder < 2 && check == remainder) ||
               (remainder >= 2 && check == 11 - remainder);
    }
}