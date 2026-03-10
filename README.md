# Bejeweled

## Repository
- Public GitHub link: `https://github.com/jb0289/Bejeweled.git`

## Build and Run
- Build: `dotnet build Bejeweled.sln`
- Run: `dotnet run --project Bejeweled/Bejeweled.csproj`

## What's Implemented
- `Board`, `Gem`, `GemColor`, `Position`, `GameState`, `MatchDetector`, `Resolver`, `Program`

## Not Implemented Yet
- `GameController`, `MoveValidator` (per assignment instructions)

## Algorithm Proposals
### Match Detection
My initial idea: I knew I needed to check each gem's neighbors in all directions to find 3 or more of the same color in a row. I wasn't sure how to implement the scanning part.

What I researched: I asked AI about scanning for matches and it showed me the `ScanLine` approach, a helper function that takes a direction (`dx`, `dy`) and walks in that direction collecting matching gems. After that, I googled match detection in C# and found `AddRange`, which let me combine the results from each direction into one list.

Final approach: For each gem on the board, I use `ScanLine` to check left, right, up, and down. I combine the results with `AddRange` into horizontal and vertical lists. If either list has 3 or more gems, it's a match. I collect all matches in a `HashSet` with `x,y` pairs to prevent duplicates.

### Gravity
My idea: I go through each column from bottom to top and collect all the gems that aren't null. Then I rewrite the column from the bottom up with those gems, and anything left at the top gets set to null. This is a simple column collapse, matching Bejeweled behavior.

### Cascade Loop
My idea: After a valid swap, I run a loop that keeps going until the board is stable. Each pass removes all matches, applies gravity to drop gems down, then fills empty spaces with new random gems. If a pass finds zero matches, the loop breaks. The method returns the total number of gems removed so score can be updated.

## Data Structure Proposals
### Board
I used a `Gem[,]` 2D array (8x8). The board is always 8x8, so a fixed array makes sense: fast coordinate access and less overhead than nested lists.

### Match Collection
I use a `HashSet` with `x,y` pairs during detection, then convert to a `List` for the public API. The `HashSet` prevents duplicates when horizontal and vertical matches overlap.

### Position
I use a `Position` class with `x` and `y` fields. This is cleaner than passing raw ints and keeps APIs consistent.

## GenAI Use
### What I Wrote First
- Designed the UML and class structure myself.
- Wrote initial implementations of all classes: `Board`, `Gem`, `GemColor`, `Position`, `GameState`, `MatchDetector`, `Resolver`, `Program`.
- Wrote gravity (column collapse) and cascade loop on my own.
- Had the general idea of checking directions for match detection, but wasn't sure how to implement it.

### What GenAI Suggested
- AI showed me how `ScanLine` works, a helper function that takes a direction and walks that way collecting matching gems.
- AI pointed out that `HashSet<Position>` won't deduplicate correctly unless `Position` has value equality (`Equals`/`GetHashCode`).
- AI suggested using a record, implementing `IEquatable`, or using a custom `IEqualityComparer`.
- AI explained that `ToList()` doesn't remove duplicates on its own.

### What I Adopted and Why
- Adopted the `ScanLine` approach after AI showed how it works; it was clear for direction-based checking.
- Found and used `AddRange` (from my own research) to combine directional scan results.
- Used a `HashSet` with `x,y` pairs instead of `HashSet<Position>`, which prevents duplicates without modifying the `Position` class.
- Converted to `List` at the end because that is what the UML methods return.

### What I Rejected and Why
- Rejected making `Position` a record because my UML defines it as a class and I wanted to keep that design.
- Rejected adding `IEquatable` to `Position`; unnecessary when `x,y` pairs already solve deduplication.
- Rejected using `Distinct().ToList()` because `HashSet` already handles duplicates.

## UML Design
![Bejeweled UML Diagram](Bejeweled/uml-diagram.png)

## Test Results
- Build: `dotnet build Bejeweled.sln` succeeded with 0 errors and 0 warnings.
- Demo run:
  - Board initializes with random gems and prints correctly.
  - Match detection finds horizontal and vertical matches of 3+.
  - Resolver removes matched gems and applies gravity.
  - Empty spaces fill with new random gems.
  - Cascade loop repeats until the board is stable.
  - User input accepts `x1 y1 x2 y2` format to enter grid coordinates.

## GenAI Chat History
- See [genai-chat-log.md](genai-chat-log.md) in this repo.
