Console.WriteLine(NextChunk().Count(c => (c[0] >= c[2] && c[1] <= c[3]) || (c[2] >= c[0] && c[3] <= c[1])));
Console.WriteLine(NextChunk().Count(c => (c[0] >= c[2] && c[0] <= c[3]) || (c[0] <= c[2] && c[2] <= c[1])));

static IEnumerable<int[]> NextChunk() => File.ReadLines("Input.txt")
	.SelectMany(line => line.Split(',')
	.SelectMany(half => half.Split('-'))
	.Select(x => int.Parse(x))
	.Chunk(4));