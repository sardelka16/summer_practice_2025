namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string str)
    {
        str = str.ToLower();
        if (String.IsNullOrEmpty(str))
        {
            return false;
        }
        string newStr = "";
        foreach (char c in str)
        {
            if (!Char.IsPunctuation(c) && !Char.IsWhiteSpace(c))
            {
                newStr += c;
            }
        }
        return newStr.SequenceEqual(newStr.Reverse());
    }
}
