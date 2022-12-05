using System.Text.RegularExpressions;

string[] _lines = File.ReadAllLines("Input.txt");

DoMoves();
DoMoves(true);

void DoMoves(bool isDayTwo = false)
{
	var stacks = new Stack<char>[9];
	foreach (string line in _lines.Take(8).Reverse())
	{
		int stackNum = 0;
		foreach (char c in line.Chunk(4).Select(ch => ch[1]))
		{
			if (stacks[stackNum] == null)
			{
				stacks[stackNum] = new Stack<char>();
			}
			if (c != ' ')
			{
				stacks[stackNum].Push(c);
			}
			stackNum++;
		}
	}

	foreach (string line in _lines.Skip(10))
	{
		var buffer = new Stack<char>();
		int[] parts = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToArray();

		while (parts[0]-- > 0)
		{
			char c = stacks[parts[1] - 1].Pop();
			if (isDayTwo)
			{
				buffer.Push(c);
			}
			else
			{
				stacks[parts[2] - 1].Push(c);
			}
		}

		if (isDayTwo)
		{
			foreach (char c in buffer)
			{
				stacks[parts[2] - 1].Push(c);
			}
		}
	}

	foreach (var stack in stacks)
	{
		Console.Write(stack.Pop());
	}
	Console.WriteLine();
}
