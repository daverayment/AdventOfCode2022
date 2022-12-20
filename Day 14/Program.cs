(int Dx, int Dy)[] validMoves = { (0, 1), (-1, 1), (1, 1), (0, 0) };

var allCoords = File.ReadLines("Input.txt")
	.Select(line => line.Split(" -> ")
		.Select(x => x.Split(',')).ToArray()
		.Select(coord => new Coord(int.Parse(coord[0]), int.Parse(coord[1]))).ToList());

var coordEnum = allCoords.SelectMany(list => list.Select(coord => coord));
Coord min = new(coordEnum.Min(xy => xy.X), 0);
Coord max = new(coordEnum.Max(xy => xy.X), coordEnum.Max(xy => xy.Y + 2));

// Width needs to accommodate a 'full' cavern for Day 2.
int width = max.Y * 2 + 1;
char[,] grid;

Console.WriteLine(Simulate());
Console.WriteLine(Simulate(true) + 1);
Console.WriteLine("");

int Simulate(bool isDayTwo = false)
{
	InitGrid();
	int sandCount = 0;
	while (IsSandInBounds(isDayTwo))
	{
		sandCount++;
	}
	DrawGrid();
	return sandCount;
}

void InitGrid()
{
	grid = new char[width, max.Y];
	foreach (var list in allCoords)
	{
		Coord last = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			DrawLine(last, list[i]);
			last = list[i];
		}
	}
}

bool IsSandInBounds(bool isDayTwo)
{
	(int X, int Y) sand = (width / 2, 0);
	var start = sand;
	(int X, int Y) move;
	do
	{
		move = GetMove(sand, isDayTwo);
		sand.X += move.X;
		sand.Y += move.Y;
	} while (move != (0, 0) && move != (-1, -1));

	if (move == (-1, -1))
	{
		return false;
	}
	else
	{
		grid[sand.X, sand.Y] = 'o';
		return !(sand == start);
	}
}

bool InBounds((int x, int y) xy, bool isDayTwo)
{
	return isDayTwo ? true : xy.y < grid.GetLength(1);
	//return xy.x >= 0 && xy.x < grid.GetLength(0) && xy.y < grid.GetLength(1);
}

(int X, int Y) GetMove((int X, int Y) sand, bool isDayTwo)
{
	foreach ((int dx, int dy) move in validMoves)
	{
		int newX = sand.X + move.dx;
		int newY = sand.Y + move.dy;
		if (!InBounds((newX, newY), isDayTwo))
		{
			return (-1, -1);
		}
		// Take into account the infinite floor for day 2.
		char gridChar = (isDayTwo && newY == max.Y) ? '#' : grid[newX, newY];
		if (gridChar == (char)0)
		{
			return move;
		}
	}

	return (0, 0);
}

void DrawLine(Coord last, Coord next)
{
	grid[width / 2 + last.X - 500, last.Y] = '#';
	while (last != next)
	{
		var coord = new Coord(last.X + (next.X == last.X ? 0 : (next.X > last.X ? 1 : -1)),
			last.Y + (next.Y == last.Y ? 0 : (next.Y > last.Y ? 1 : -1)));
		grid[width / 2 + coord.X - 500, coord.Y] = '#';
		last = coord;
	}
}

void DrawGrid()
{
	for (int y = 0; y < grid.GetLength(1); y++)
	{
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			Console.Write(grid[x, y] switch
			{
				(char)0 => '.',
				_ => grid[x, y]
			});
		}
		Console.WriteLine("");
	}
	Console.WriteLine("");
}

record Coord(int X, int Y);