const int Day1Y = 2000000;

List<Pair> pairs = File.ReadLines("Input.txt").Select(line => new Pair(line)).ToList();

var ranges = GetRanges(Day1Y);
Console.WriteLine(GetYInfo(Day1Y).count);

for (int y = pairs.Min(p => p.Sensor.Y); y <= pairs.Max(p => p.Sensor.Y); y++)
{
	ranges = GetRanges(y);
	(_, int? gap) = GetYInfo(y, true);
	if (gap != null)
	{
		Console.WriteLine((long)gap * 4000000 + y);
		break;
	}
}

IEnumerable<(int XMin, int XMax)> GetRanges(int targetY)
{
	List<(int StartX, int EndX)> ranges = new();

	foreach (var p in pairs)
	{
		int sensorDistanceY = Math.Abs(p.Sensor.Y - targetY);
		if (sensorDistanceY <= p.Distance)
		{
			int xStart = p.Sensor.X - (p.Distance - sensorDistanceY);
			int xEnd = p.Sensor.X + (p.Distance - sensorDistanceY);
			ranges.Add(new(Math.Min(xStart, xEnd), Math.Max(xStart, xEnd)));
		}
	}

	return ranges.OrderBy(x => x.StartX);
}

(int count, int? gap) GetYInfo(int y, bool isDayTwo = false)
{
	(int SpanXMin, int SpanXMax) = ranges.First();
	foreach (var (XMin, XMax) in ranges.Skip(1))
	{
		if (XMin > SpanXMax + 1 && isDayTwo)
		{
			// Found a gap!
			return (-1, SpanXMax + 1);
		}
		SpanXMax = Math.Max(SpanXMax, XMax);
	}

	int width = SpanXMax - SpanXMin + 1;
	var beacons = pairs.Where(x => x.Beacon.Y == y).Select(x => x.Beacon).Distinct();
	return isDayTwo ? (width, null) : (width - beacons.Count(), null);
}

record Coord(int X, int Y);
struct Pair
{
	public Coord Sensor { get; set; }
	public Coord Beacon { get; set; }
	public int Distance { get; set; }
	public Pair(string line)
	{
		int[] coords = System.Text.RegularExpressions.Regex.Matches(
			line, @"(\d+)").Select(x => int.Parse(x.Value)).ToArray();
		Sensor = (new Coord(coords[0], coords[1]));
		Beacon = (new Coord(coords[2], coords[3]));
		Distance = Math.Abs(Sensor.X - Beacon.X) + Math.Abs(Sensor.Y - Beacon.Y);
	}
}

