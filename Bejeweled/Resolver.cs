namespace Bejeweled;

// Handles removing matches, applying the gravity, filling empty spaces, and cascading
public class Resolver
{
    // Reference to the match detector
    private readonly MatchDetector matchDetector;

    // Constructor: takes a MatchDetector instance
    public Resolver(MatchDetector matchDetector)
    {
        this.matchDetector = matchDetector;
    }

    // Finds and removes all matching gems from the board, returns the count removed
    public int RemoveMatches(Board board)
    {
        var matches = matchDetector.FindMatches(board);
        foreach (var p in matches)
        {
            board.RemoveGem(p);
        }

        return matches.Count;
    }

    // Drops gems down to fill gaps left by removed gems
    public void ApplyGravity(Board board)
    {
        const int boardSize = 8;
        for (var x = 0; x < boardSize; x++)
        {
            // Collect all non-null gems from bottom to top
            var gems = new List<Gem>();
            for (var y = boardSize - 1; y >= 0; y--)
            {
                var current = board.GetGem(new Position(x, y));
                if (current != null)
                {
                    gems.Add(current);
                }
            }

            // Place gems back, starting from the bottom
            var writeY = boardSize - 1;
            foreach (var gem in gems)
            {
                board.SetGem(new Position(x, writeY), gem);
                writeY--;
            }

            // Clears remaining spaces at the top
            while (writeY >= 0)
            {
                board.RemoveGem(new Position(x, writeY));
                writeY--;
            }
        }
    }

    // Fills any empty spaces on the board with new random gems
    public void FillEmptySpaces(Board board)
    {
        const int boardSize = 8;
        for (var y = 0; y < boardSize; y++)
        {
            for (var x = 0; x < boardSize; x++)
            {
                var p = new Position(x, y);
                if (board.GetGem(p) == null)
                {
                    board.SetGem(p, Gem.CreateRandom());
                }
            }
        }
    }

    // Loops removing matches, applying gravity, and filling until no matches remain
    public int ProcessCascades(Board board)
    {
        var removed = 0;
        while (true)
        {
            var matches = RemoveMatches(board);
            if (matches == 0)
            {
                break;
            }

            removed += matches;
            ApplyGravity(board);
            FillEmptySpaces(board);
        }

        return removed;
    }
}