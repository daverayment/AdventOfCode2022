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
	 select new { Caves = (Start: start.Name, End: end.Name), Distance = CalcDistance(start.Name, end.Name) }
	).ToDictionary(x => x.Caves, x => x.Distance);

// Create a unique index for each of the caves with flows > 0. Uses a single bit
// for each valve so we can do calculations on the bits in Day 2.
Dictionary<string, int> valveIndexes = caves.Where(x => x.Value.Flow > 0)
	.Select((x, index) => new { x.Value.Name, index })
	.ToDictionary(x => x.Name, x => 1 << x.index);
// The maximum pressure released for each combination of valve visits.
Dictionary<int, int> valveStates = new();

// Day 1.
Console.WriteLine(Search());
// Day 2.
Search(true);
Console.WriteLine((from a in valveStates
				   from b in valveStates
				   where (a.Key & b.Key) == 0
				   select a.Value + b.Value).Max());

int Search(bool isDayTwo = false)
{
	valveStates.Clear();
	int maxPressure = 0;
	Stack<State> states = new();
	states.Push(new State(
		CurrentCave: "AA",
		TimeLeft:    isDayTwo ? 26 : 30,
		OpenValves:  0,
		CavesLeft:   caves.Where(x => x.Value.Flow > 0).OrderByDescending(x => x.Value.Flow).Select(x => x.Key).ToArray(),
		Pressure:    0,
		FlowRate:    0));

	do
	{
		var current = states.Pop();

		// The pressure released if we stayed here until time runs out.
		int stopScore = current.Pressure + current.TimeLeft * current.FlowRate;

		// Update the score for this combination of valves if the route is better.
		if (!valveStates.ContainsKey(current.OpenValves) ||
			stopScore > valveStates[current.OpenValves])
		{
			valveStates[current.OpenValves] = stopScore;
		}

		if (current.TimeLeft == 0 || !current.CavesLeft.Any())
		{
			maxPressure = Math.Max(maxPressure, stopScore);
		}
		else
		{
			// Add states to move to each cave in turn and open the valve there.
			foreach (var cave in current.CavesLeft)
			{
				int moveTime = Math.Min(current.TimeLeft,
					distances[(current.CurrentCave, cave)] + 1);

				states.Push(new State(
					CurrentCave: cave,
					TimeLeft:	 current.TimeLeft - moveTime,
					OpenValves:	 current.OpenValves | valveIndexes[cave],
					CavesLeft:	 current.CavesLeft.Where(x => x != cave).ToArray(),
					Pressure:	 current.Pressure + current.FlowRate * moveTime,
					FlowRate:	 current.FlowRate + caves[cave].Flow));
			}
		}
	} while (states.Count > 0);

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

record State(string CurrentCave, int TimeLeft, int OpenValves,
	string[] CavesLeft, int Pressure, int FlowRate);
