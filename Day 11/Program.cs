using System.Text.RegularExpressions;

Console.WriteLine(DoRounds());
Console.WriteLine(DoRounds(true));

long DoRounds(bool dayTwo = false)
{
	List<Monkey> monkeys = new();
	foreach (var lines in File.ReadLines("Input.txt").Chunk(7))
	{
		monkeys.Add(new Monkey(lines));
	}

	// Lowest common denominator for all divisors. We must perform a modulo by
	// this value when performing the item operations to keep them in range.
	int lcd = monkeys.Aggregate(1, (x, y) => x * y.DivBy);
		
	for (int rounds = 0; rounds < (dayTwo ? 10000 : 20); rounds++)
	{
		foreach (var monkey in monkeys)
		{
			for (int i = 0; i < monkey.Items.Count; i++)
			{
				monkey.NumInspections++;
				long val = monkey.Operand == int.MinValue ? monkey.Items[i] : monkey.Operand;
				switch (monkey.Operator)
				{
					case '*':
						monkey.Items[i] = ((monkey.Items[i] % lcd) * val) % lcd;
						break;

					case '+':
						monkey.Items[i] = ((monkey.Items[i] % lcd) + val) % lcd;
						break;
				}

				if (!dayTwo)
				{
					monkey.Items[i] /= 3;
				}

				if (monkey.Items[i] % monkey.DivBy == 0)
				{
					monkeys[monkey.ThrowIfTrue].Items.Add(monkey.Items[i]);
				}
				else
				{
					monkeys[monkey.ThrowIfFalse].Items.Add(monkey.Items[i]);
				}
			}
			monkey.Items.Clear();
		}
	}

	var top = monkeys.OrderByDescending(x => x.NumInspections).Take(2).ToArray();
	return (long)top[0].NumInspections * top[1].NumInspections;
}

class Monkey
{
	public int NumInspections { get; set; } = 0;
	public int RoundInspections { get; set; }
	public List<long> Items { get; set; } = new();
	public int DivBy { get; init; }
	public char Operator { get; init; }
	public int Operand { get; init; }
	public int ThrowIfTrue { get; init; }
	public int ThrowIfFalse { get; init; }

	public Monkey(string[] lines)
	{
		foreach (Match match in Regex.Matches(lines[1], @"(\d+)"))
		{
			Items.Add(int.Parse(match.Value));
		}
		Operator = lines[2][23];
		if (!int.TryParse(lines[2][25..], out int op))
		{
			// MinValue is marker for using the "old" value.
			op = int.MinValue;
		}
		Operand = op;
		DivBy = int.Parse(lines[3][21..]);
		ThrowIfTrue = int.Parse(lines[4][29..]);
		ThrowIfFalse = int.Parse(lines[5][30..]);
	}
}