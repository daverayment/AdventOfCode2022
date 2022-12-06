Console.WriteLine(MessageStart(4));
Console.WriteLine(MessageStart(14));

int MessageStart(int markerLen)
{
	string input = File.ReadLines("Input.txt").First();

	for (int i = 0; i < input.Length - markerLen; i++)
	{
		if (input.Substring(i, markerLen).Distinct().Count() == markerLen)
		{
			return i + markerLen;
		}
	}

	throw new ApplicationException("No marker found.");
}
