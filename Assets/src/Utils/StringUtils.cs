
public static class StringUtils
{
    public static string CapitalizeFirstLetter(this string _line)
    {
        if (_line[0] >= 'a' && _line[0] <= 'z')
        {
            var chars = _line.ToCharArray();
            chars[0] -= (char)('a' - 'A');
            return new string(chars);
        }
        return _line;
    }
}
