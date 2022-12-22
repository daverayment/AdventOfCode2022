using System.Text.RegularExpressions;

const int Day1Y = 2000000;

List<Pair> pairs = File.ReadLines("Input.txt").Select(line => new Pair(line)).ToList();
int xMin = pairs.Min(p => Math.Min(p.Beacon.X, p.Sensor.X));
int xMax = pairs.Max(p => Math.Max(p.Beacon.X, p.Sensor.X));
int yMin = pairs.Min(p => Math.Min(p.Beacon.Y, p.Sensor.Y));
int yMax = pairs.Max(p => Math.Max(p.Beacon.Y, p.Sensor.Y));

var ranges = GetRanges(Day1Y);

Console.WriteLine(GetYInfo(Day1Y).count);

for (int y = yMin; y <= yMax; y++)
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
	var rs = ranges.ToArray();
	if (isDayTwo)
	{
		// Detect gap at start or end of lines.
		int? g = rs[0].XMin > xMin ? xMin : (rs[0].XMax < xMax ? xMax : null);
		if (g != null &&
			g >= 0 && g <= 4000000 && y >= 0 && y <= 4000000)
		{
			return (-1, g);
		}
	}

	var (XMin, XMax) = rs[0];
	for (int i = 1; i < rs.Length; i++)
	{
		if (rs[i].XMin > XMax + 1 && isDayTwo)
		{
			// Found a gap!
			return (-1, XMax + 1);
		}
		XMax = Math.Max(XMax, rs[i].XMax);
	}

	int width = XMax - XMin + 1;
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
		int[] coords = Regex.Matches(line, @"(\d+)").Select(x => int.Parse(x.Value)).ToArray();
		Sensor = (new Coord(coords[0], coords[1]));
		Beacon = (new Coord(coords[2], coords[3]));
		Distance = Math.Abs(Sensor.X - Beacon.X) + Math.Abs(Sensor.Y - Beacon.Y);
	}
}

