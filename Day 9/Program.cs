string[] lines = File.ReadAllLines("Input.txt");

Dictionary<char, (int dx, int dy)> Moves = new()
{
	{ 'U', (0, -1) }, { 'D', (0, 1) }, { 'L', (-1, 0) }, { 'R', (1, 0) }
};

DoMoves(2);
DoMoves(10);

void DoMoves(int numKnots)
{
	List<Coord> visited = new();
	var knots = new Coord[numKnots];
	for (int i = 0; i < numKnots; i++)
	{
		knots[i] = new Coord { X = 0, Y = 0 };
	}

	foreach (string line in lines)
	{
		var move = Moves[line[0]];
		for (int i = 0; i < int.Parse(line[2..]); i++)
		{
			// Move head.
			knots[0].X += move.dx;
			knots[0].Y += move.dy;

			// Move following knots.
			for (int k = 1; k < numKnots; k++)
			{
				int distX = knots[k - 1].X - knots[k].X;
				int distY = knots[k - 1].Y - knots[k].Y;
				if (Math.Abs(distX) > 1 || Math.Abs(distY) > 1)
				{
					knots[k].X += Math.Clamp(distX, -1, 1);
					knots[k].Y += Math.Clamp(distY, -1, 1);
				}
			}

			if (!visited.Contains(knots[numKnots-1]))
			{
				visited.Add(knots[numKnots-1]);
			}
		}
	}

	Console.WriteLine(visited.Count());
}

public struct Coord { public int X; public int Y; }
