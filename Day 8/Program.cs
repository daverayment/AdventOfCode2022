string[] lines = File.ReadAllLines("Input.txt");
int rows = lines.Length;
int cols = lines[0].Length;
int numVisibleTrees = 0;
int maxScore = 0;

// X and Y movement for each direction.
var Moves = new (int x, int y)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

for (int row = 0; row < rows; row++)
{
	for (int col = 0; col < cols; col++)
	{
		(int score, bool visible) = TreeCheck(col, row);
		if (visible) { numVisibleTrees++; }
		maxScore = Math.Max(maxScore, score);
	}
}

Console.WriteLine(numVisibleTrees);
Console.WriteLine(maxScore);

(int score, bool visible) TreeCheck(int startX, int startY)
{
	int treeHeight = lines[startY][startX];
	int score = 0;
	bool visible = false;
	foreach (var move in Moves)
	{
		int directionScore = 0;
		int x = startX + move.x;
		int y = startY + move.y;
		while (IsInBounds(x, y))
		{
			directionScore++;
			if (lines[y][x] >= treeHeight)
			{
				break;
			}
			x += move.x;
			y += move.y;
		}
		if (!IsInBounds(x, y))
		{
			// Travelled past the edge without being blocked. Tree is visible.
			visible = true;
		}
		score = score == 0 ? directionScore : score * directionScore;
	}

	return (score, visible);
}

bool IsInBounds(int x, int y)
{
	return x >= 0 && x < lines[0].Length && y >= 0 && y < lines.Length;
}
