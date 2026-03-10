namespace Bejeweled;

// Represents the 8x8 game board that holds all gems
public class Board
{
    // 8x8 grid of gems
    private readonly Gem[,] grid = new Gem[8, 8];

    // Constructor: initializes the board with random gems
    public Board()
    {
        InitializeBoard();
    }

    // Fills the board with randomly generated gems
    public void InitializeBoard()
    {
        const int boardSize = 8;
        for (var y = 0; y < boardSize; y++)
        {
            for (var x = 0; x < boardSize; x++)
            {
                grid[y, x] = Gem.CreateRandom();
            }
        }
    }

    // Returns the gem at the given position
    public Gem GetGem(Position p)
    {
        return grid[p.GetY(), p.GetX()];
    }

    // Places a gem at the given position
    public void SetGem(Position p, Gem gem)
    {
        grid[p.GetY(), p.GetX()] = gem;
    }

    // Swaps the gems at two positions
    public void SwapGems(Position p1, Position p2)
    {
        var temp = GetGem(p1);
        SetGem(p1, GetGem(p2));
        SetGem(p2, temp);
    }

    // Reverses a swap by swapping the two positions back
    public void UndoSwap(Position p1, Position p2)
    {
        SwapGems(p1, p2);
    }

    // Removes a gem from the board by setting the position to null
    public void RemoveGem(Position p)
    {
        SetGem(p, null!);
    }

    // Checks if two positions are directly next to each other (no diagonals)
    public bool IsAdjacent(Position p1, Position p2)
    {
        var x = Math.Abs(p1.GetX() - p2.GetX());
        var y = Math.Abs(p1.GetY() - p2.GetY());
        return x + y == 1;
    }

    // Checks if a position is within the 8x8 board boundaries
    public bool InBounds(Position p)
    {
        const int boardSize = 8;
        return p.GetX() >= 0 && p.GetX() < boardSize &&
               p.GetY() >= 0 && p.GetY() < boardSize;
    }
}

