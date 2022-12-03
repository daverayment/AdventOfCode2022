string[] lines = File.ReadAllLines("Input.txt");

Console.WriteLine(lines.Sum(line =>
{
	var halves = line.Chunk(line.Length / 2).ToArray();
	return Priority(halves[0].Distinct().First(ch => halves[1].Contains(ch)));
}));

Console.WriteLine(lines.Chunk(3).Sum(chunk =>
	Priority(chunk[0].Distinct().Single(ch => 
		chunk[1].Contains(ch) && chunk[2].Contains(ch)))));

static int Priority(char ch) => ch > 'Z' ? ch - 'a' + 1 : ch - 'A' + 27;