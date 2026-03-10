namespace Bejeweled;

public class Position
{
    private readonly int x;
    private readonly int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Position()
    {
        
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }
}
