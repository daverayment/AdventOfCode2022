using System.Diagnostics;
using System.Text.RegularExpressions;

var caves = File.ReadLines("Input.txt")
	.Select(line => Regex.Matches(line, @"(\d+|[A-Z]{2})"))
	.Select(matches => (
		Name: matches[0].Value,
		Flow: int.Parse(matches[1].Value),
		Links: matches.Skip(2).Select(x => x.Value).ToArray()))
	.ToDictionary(cave => cave.Name, cave => cave);

// Distances between each pair of caves in which we're interested (either start
// cave AA or caves with non-zero flow rates).
var distances =
	(from start in caves.Values
	 from end in caves.Values
	 where (start.Name == "AA" || (start.Flow > 0 && end.Flow > 0)) &&
		start.Name != end.Name
	 select new { Caves = (start.Name, end.Name), Distance = CalcDistance(start.Name, end.Name) }
	).ToDictionary(x => x.Caves, x => x.Distance);

var watch = Stopwatch.StartNew();
Console.WriteLine(Search());
Console.WriteLine(watch.ElapsedMilliseconds);

int Search()
{
	Stack<State> states = new();
	states.Push(new State
	{
		CurrentCave = "AA",
		CavesLeft = caves.Where(x => x.Value.Flow > 0).OrderByDescending(x => x.Value.Flow).Select(x => x.Key).ToArray(),
		FlowRate = 0,
		Pressure = 0,
		TimeLeft = 30
	});

	int maxPressure = 0;
	int numStates = 0;

	do
	{
		var current = states.Pop();
		numStates++;

		if (current.TimeLeft == 0 || !current.CavesLeft.Any())
		{
			maxPressure = Math.Max(maxPressure,
				current.Pressure + current.TimeLeft * current.FlowRate);
		}
		else
		{
			// Add states to move to each cave in turn and open the valve there.
			foreach (var cave in current.CavesLeft)
			{
				int moveTime = distances[(current.CurrentCave, cave)] + 1;
				moveTime = Math.Min(moveTime, current.TimeLeft);

				int newPressure = current.Pressure + current.FlowRate * moveTime;

				states.Push(new State
				{
					CurrentCave = cave,
					CavesLeft = current.CavesLeft.Where(x => x != cave).ToArray(),
					FlowRate = current.FlowRate + caves[cave].Flow,
					Pressure = newPressure,
					TimeLeft = current.TimeLeft - moveTime
				});
			}
		}
	} while (states.Count > 0);

	Console.WriteLine(numStates);
	return maxPressure;
}

// Breadth-first search to calculate distance between 2 caves.
int CalcDistance(string start, string end)
{
	var toVisit = new Queue<(string Name, int Distance)>();
	toVisit.Enqueue((start, 0));
	var visited = new HashSet<string> { start };
	do
	{	
		var (currentName, currentDistance) = toVisit.Dequeue();
		if (currentName == end)
		{
			return currentDistance;
		}
		foreach (string name in caves[currentName].Links)
		{
			if (!visited.Contains(name))
			{
				toVisit.Enqueue((name, currentDistance + 1));
				visited.Add(currentName);
			}
		}
	} while (toVisit.Any());

	return int.MaxValue;
}

struct State
{
	public string CurrentCave;
	public int TimeLeft;
	public string[] CavesLeft;
	public int Pressure;
	public int FlowRate;
}