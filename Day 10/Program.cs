int regX = 1;
int cycle = 0;
int signalStrength = 0;
bool isAdd;

foreach (string line in File.ReadLines("Input.txt"))
{
	string[] parts = line.Split(' ');
	isAdd = parts.Length > 1;
	int nextEvent = cycle + (isAdd ? 2 : 1);

	while (cycle != nextEvent)
	{
		int pixelPos = cycle % 40;
		Console.Write((pixelPos >= regX - 1 && pixelPos <= regX + 1) ? "#" : ".");
		if (pixelPos == 0)
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