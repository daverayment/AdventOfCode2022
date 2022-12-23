using System.Text.RegularExpressions;

var caves = File.ReadLines("Example.txt").Select(line => new Cave(line));

int time = 30;
int valvesLeft = caves.Count(x => x.FlowRate > 0);
var current = caves.Where(x => x.Name == "AA");


// Naive approach: multiply each linked cave's flowrate by the time remaining
// and travel there.
// Better: depth-first search of all linked cave's total rates (bearing in mind loopbacks)

Console.WriteLine();
public class Cave
{
	public string Name { get; init; }
	public int FlowRate { get; init; }
	public List<string> LinkedCaves { get; init; }
	public Cave(string line)
	{
		var matches = Regex.Matches(line, "([A-Z]{2})");
		this.Name = matches[0].Value;
		this.LinkedCaves = matches.Skip(1).Select(x => x.Value).ToList();
		this.FlowRate = int.Parse(Regex.Match(line, @"(\d+)").Value);
	}
}