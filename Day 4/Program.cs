Console.WriteLine(LineValues().Count(v => (v[0] >= v[2] && v[1] <= v[3]) || (v[2] >= v[0] && v[3] <= v[1])));
Console.WriteLine(LineValues().Count(v => (v[0] >= v[2] && v[0] <= v[3]) || (v[0] <= v[2] && v[2] <= v[1])));

static IEnumerable<int[]> LineValues() => File.ReadLines("Input.txt")
	.SelectMany(line => line.Split(',')
	.SelectMany(half => half.Split('-'))
	.Select(x => int.Parse(x))
	.Chunk(4));