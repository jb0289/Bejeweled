namespace Bejeweled;

// Main program - handles user input and display
public class Program
{
    public static void Main()
    {
        var program = new Program();
        program.RunGame();
    }

    // Main game loop
    public void RunGame()
    {
        var board = new Board();
        var matchDetector = new MatchDetector();
        var resolver = new Resolver(matchDetector);

        // Clear any matches on the starting board
        resolver.ProcessCascades(board);

        while (true)
        {
            PrintBoard(board);
            Console.WriteLine("Pick two adjacent gems to swap (x1 y1 x2 y2) or 'q' to quit:");

            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input.Trim().ToLower() == "q") break;

            // Read and validate input
            if (!ReadSwapInput(input, out var p1, out var p2))
            {
                Console.WriteLine("Invalid input.USE Format:ex. 1 1 1 2");
                continue;
            }

            // Check bounds
            if (!board.InBounds(p1) || !board.InBounds(p2))
            {
                Console.WriteLine("Coordinates must be between 0 and 7.");
                continue;
            }

            // Check adjacency
            if (!board.IsAdjacent(p1, p2))
            {
                Console.WriteLine("Gems must be adjacent!");
                continue;
            }

            // Try the swap
            board.SwapGems(p1, p2);

            if (!matchDetector.IsValidMatch(p1, board) && !matchDetector.IsValidMatch(p2, board))
            {
                board.UndoSwap(p1, p2);
                Console.WriteLine("No match.");
                continue;
            }

            var removed = resolver.ProcessCascades(board);
            Console.WriteLine($"Great Move! Removed {removed} gem(s).");
        }

        Console.WriteLine("Thanks for playing!");
    }

    // Parses input string into two positions
    public bool ReadSwapInput(string input, out Position p1, out Position p2)
    {
        p1 = new Position(0, 0);
        p2 = new Position(0, 0);

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 4) return false;

        if (!int.TryParse(parts[0], out var x1) ||
            !int.TryParse(parts[1], out var y1) ||
            !int.TryParse(parts[2], out var x2) ||
            !int.TryParse(parts[3], out var y2))
            return false;

        p1 = new Position(x1, y1);
        p2 = new Position(x2, y2);
        return true;
    }

    // Displays the board with coordinates and legend
    public void PrintBoard(Board board)
    {
        const int size = 8;
        Console.WriteLine("  0 1 2 3 4 5 6 7");
        for (var y = 0; y < size; y++)
        {
            Console.Write($"{y} ");
            for (var x = 0; x < size; x++)
            {
                var gem = board.GetGem(new Position(x, y));
                var symbol = gem == null ? '.' : gem.GetColor().ToString()[0];
                Console.Write(symbol);
                Console.Write(' ');
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("B=Blue G=Green O=Orange P=Purple R=Red Y=Yellow W=White");
        Console.WriteLine();
    }
}