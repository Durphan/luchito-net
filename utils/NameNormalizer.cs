namespace luchito_net.Utils;

public static class NameNormalizer
{
    public static string Normalize(string name)
    {
        if (string.IsNullOrEmpty(name.Trim()))
        {
            throw new ArgumentException("Category name cannot be null or empty.");
        }
        name = name.Trim();
        return char.ToUpper(name[0]) + name[1..].ToLower();
    }
}