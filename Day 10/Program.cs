int regX = 1;
int cycle = 0;
int signalStrength = 0;

foreach (string line in File.ReadLines("Input.txt"))
{
	string[] parts = line.Split(' ');
	bool isAdd = parts.Length > 1;
	int nextEvent = cycle + (isAdd ? 2 : 1);

	while (cycle != nextEvent)
	{
		int pixelPos = cycle % 40;
		Console.Write((pixelPos >= regX - 1 && pixelPos <= regX + 1) ? "#" : ".");
		if (pixelPos == 39)
		{
			Console.WriteLine("");
		}

		cycle++;
		if ((cycle + 20) % 40 == 0)
		{
			signalStrength += cycle * regX;
		}
	}

	if (isAdd)
	{
		regX += int.Parse(parts[1]);
	}
}

Console.WriteLine(signalStrength);