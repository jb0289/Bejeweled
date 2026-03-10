namespace Bejeweled;


public class Gem
{
    // The color of a gem
    private readonly GemColor color;

    
    private static readonly Random random = new();

    // Constructor - creates a gem with the given color
    public Gem(GemColor color)
    {
        this.color = color;
    }

    // Returns the color of a gem
    public GemColor GetColor()
    {
        return color;
    }

    // Creates a gem with a random color
    public static Gem CreateRandom()
    {
        var colors = Enum.GetValues<GemColor>();
        return new Gem(colors[random.Next(colors.Length)]);
    }
}