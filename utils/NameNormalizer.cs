public static class NameNormalizer
{
    public static string Normalize(string name)
    {
        name = name.Trim();
        return char.ToUpper(name[0]) + name[1..].ToLower();
    }
}