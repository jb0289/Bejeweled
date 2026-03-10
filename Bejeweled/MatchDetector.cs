using System.Collections.Generic;
using System.Linq;

namespace Bejeweled;

public class MatchDetector
{
    public MatchDetector()
    {
    }

    public List<Position> FindMatches(Board board)
    {
        const int boardSize = 8;
        var matches = new HashSet<(int x, int y)>();

        for (var y = 0; y < boardSize; y++)
        {
            for (var x = 0; x < boardSize; x++)
            {
                var pos = new Position(x, y);
                var set = GetMatchingGems(pos, board);
                if (set.Count < 3)
                {
                    continue;
                }

                foreach (var gemPosition in set)
                {
                    matches.Add((gemPosition.GetX(), gemPosition.GetY()));
                }
            }
        }

        return matches.Select(m => new Position(m.x, m.y)).ToList();
    }

    public List<Position> GetMatchingGems(Position p, Board board)
    {
        if (!board.InBounds(p))
        {
            return new List<Position>();
        }

        var center = board.GetGem(p);
        if (center == null)
        {
            return new List<Position>();
        }

        var color = center.GetColor();

        // Scans in one direction and collect matching gems
        List<Position> ScanLine(int dx, int dy)
        {
            var line = new List<Position>();
            var xPos = p.GetX() + dx;
            var yPos = p.GetY() + dy;

            while (board.InBounds(new Position(xPos, yPos)))
            {
                var current = new Position(xPos, yPos);
                var gem = board.GetGem(current);
                if (gem == null || gem.GetColor() != color)
                    break;

                line.Add(current);
                xPos += dx;
                yPos += dy;
            }

            return line;
        }

        // This will check for matches in both horizontal directions
        var horizontal = new List<Position> { p };
        horizontal.AddRange(ScanLine(-1, 0)); // left
        horizontal.AddRange(ScanLine(1, 0));  // right

        // This will check for matches in both vertical directions
        var vertical = new List<Position> { p };
        vertical.AddRange(ScanLine(0, -1)); // up
        vertical.AddRange(ScanLine(0, 1));  // down

        // The results will only include positions with 3+ in a row
        var results = new HashSet<Position>();

        if (horizontal.Count >= 3)
        {
            foreach (var pos in horizontal)
                results.Add(pos);
        }

        if (vertical.Count >= 3)
        {
            foreach (var pos in vertical)
                results.Add(pos);
        }

        return results.ToList();
    }

    public bool IsValidMatch(Position p, Board board)
    {
        var matches = GetMatchingGems(p, board);
        return matches.Count >= 3;
    }
} 