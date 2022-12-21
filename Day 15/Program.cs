using System.Text.RegularExpressions;

List<Pair> pairs = File.ReadLines("Input.txt").Select(line => new Pair(line)).ToList();

int y = 2000000;

List<(int StartX, int EndX)> ranges = new();

foreach (var p in pairs)
{
	int sensorDistanceY = Math.Abs(p.Sensor.Y - y);
	if (sensorDistanceY <= p.Distance)
	{
		int xStart = p.Sensor.X - (p.Distance - sensorDistanceY);
		int xEnd = p.Sensor.X + (p.Distance - sensorDistanceY);
		ranges.Add(new (Math.Min(xStart, xEnd), Math.Max(xStart, xEnd)));
	}
}

var total = 0;
var ordered = ranges.OrderBy(x => x.StartX).ToArray();

for (int i = ordered.First().StartX; i <= ordered.Last().EndX; i++)
{
	foreach (var range in ranges)
	{
		if (i >= range.StartX && i <= range.EndX)
		{
			total++;
			break;
		}
	}
}

int beaconCount = pairs.Where(x => x.Beacon.Y == y).Select(x => x.Beacon).Distinct().Count();

Console.WriteLine(total - beaconCount);

record Coord(int X, int Y);
struct Pair
{
	public Coord Sensor { get; set; }
	public Coord Beacon { get; set; }
	public int Distance { get; set; }
	public Pair(string line)
	{
		int[] coords = Regex.Matches(line, @"(\d+)").Select(x => int.Parse(x.Value)).ToArray();
		Sensor = (new Coord(coords[0], coords[1]));
		Beacon = (new Coord(coords[2], coords[3]));
		Distance = Math.Abs(Sensor.X - Beacon.X) + Math.Abs(Sensor.Y - Beacon.Y);
	}
}
