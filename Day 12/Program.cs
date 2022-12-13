(int dx, int dy)[] moves = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

string[] lines = File.ReadAllLines("Input.txt");

Coord? startPos = null;
Coord? endPos = null;
List<Coord> startPoints = new();

for (int row = 0; row < lines.Length; row++)
{
	for (int col = 0; col < lines[0].Length; col++)
	{
		if (lines[row][col] == 'S')
		{
			startPos = new Coord(col, row);
			lines[row] = lines[row].Replace('S', 'a');
		}
		if (lines[row][col] == 'E')
		{
			endPos = new Coord(col, row);
			lines[row] = lines[row].Replace('E', 'z');
		}
		if (lines[row][col] == 'a')
		{
			startPoints.Add(new Coord(col, row));
		}
	}
}

System.Diagnostics.Debug.Assert(startPos != null && endPos != null);

Console.WriteLine(GetMoveCount(startPos, endPos));
Console.WriteLine(startPoints.Min(x => GetMoveCount(x, endPos)));

int GetMoveCount(Coord start, Coord end)
{
	Queue<Coord> toCheck = new();
	toCheck.Enqueue(start);
	HashSet<Coord> visited = new() { start };
	Dictionary<Coord, Coord> fromTo = new()
	{
		{ start, null }
	};

	// Breadth-first search.
	while (toCheck.Count > 0)
	{
		var current = toCheck.Dequeue();
		foreach (var (dx, dy) in moves)
		{
			var newPos = new Coord(current.X + dx, current.Y + dy);
			if (IsValid(current, newPos) && !visited.Contains(newPos))
			{
				toCheck.Enqueue(newPos);
				fromTo.Add(newPos, current);
				visited.Add(newPos);
			}
		}
	}

	// Travel back from goal to get to start.
	var pos = end;
	int moveCount = 0;
	while (pos != start)
	{
		if (!fromTo.ContainsKey(pos))
		{
			return int.MaxValue;
		}
		pos = fromTo[pos];
		moveCount++;
	}

	return moveCount;
}

bool IsValid(Coord pos, Coord next)
{
	if (next.Y < 0 || next.Y == lines.Length || next.X < 0 || next.X == lines[0].Length)
	{
		return false;
	}
	if (lines[next.Y][next.X] - lines[pos.Y][pos.X] > 1)
	{
		return false;
	}
	return true;
}

record Coord(int X, int Y);