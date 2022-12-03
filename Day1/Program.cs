Console.WriteLine(ElfCalories().Max());
Console.WriteLine(ElfCalories().OrderByDescending(x => x).Take(3).Sum());

static IEnumerable<int> ElfCalories()
{
	int calories = 0;
	foreach (string line in File.ReadAllLines("Input.txt"))
	{
		if (line.Length == 0)
		{
			yield return calories;
			calories = 0;
		}
		else
		{
			calories += int.Parse(line);
		}
	}
}