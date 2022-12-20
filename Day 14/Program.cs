var allCoords = File.ReadLines("Example.txt")
	.Select(line => line.Split(" -> ")
		.Select(x => x.Split(',')).ToArray()
		.Select(coord => new Coord(int.Parse(coord[0]), int.Parse(coord[1]))).ToList());

var coordEnum = allCoords.SelectMany(list => list.Select(coord => coord));
Coord min = new(coordEnum.Min(xy => xy.X), 0);
Coord max = new(coordEnum.Max(xy => xy.X), coordEnum.Max(xy => xy.Y));
var grid = new char[max.X - min.X + 1, max.Y + 1];

foreach (var list in allCoords)
{
	Coord last = list[0];
	for (int i = 1; i < list.Count; i++)
	{
		DrawLine(last, list[i]);
		last = list[i];
	}
}

DrawGrid();
Console.WriteLine("");

void DrawLine(Coord last, Coord next)
{
	int dx = next.X - last.X;
	int dy = next.Y - last.Y;
	int numMoves = Math.Abs(dx) + Math.Abs(dy);
	(int currentX, int currentY) = (last.X - min.X, last.Y);
	for (int i = 0; i < numMoves; i++)
	{
		grid[currentX, currentY] = '#';
		currentX += dx == 0 ? 0 : (dx < 0 ? -1 : 1);
		currentY += dy == 0 ? 0 : (dy < 0 ? -1 : 1);
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
				_ => '#'
			});
		}
		Console.WriteLine("");
	}
}

record Coord(int X, int Y);