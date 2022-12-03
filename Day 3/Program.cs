string[] lines = File.ReadAllLines("Input.txt");
int total = 0;
foreach (string line in lines)
{
	var parts = line.Chunk(line.Length / 2).ToArray();
	total += Priority(parts[0].Distinct().First(ch => parts[1].Contains(ch)));
}
Console.WriteLine(total);

total = lines.Chunk(3).Sum(chunk =>
	Priority(chunk[0].Distinct().Single(ch => chunk[1].Contains(ch) && chunk[2].Contains(ch))));
Console.WriteLine(total);

static int Priority(char ch) => ch > 'Z' ? ch - 'a' + 1 : ch - 'A' + 27;