using System.Text.Json;

const string DividerA = "[[2]]";
const string DividerB = "[[6]]";

PacketComparer comparer = new();

string[] lines = File.ReadAllLines("Input.txt");

int pairIndex = 1;
int inOrderSum = 0;
foreach (var pairs in lines.Chunk(3))
{
	if (comparer.Compare(pairs[0], pairs[1]) < 0)
	{
		inOrderSum += pairIndex;
	}
	pairIndex++;
}

Console.WriteLine(inOrderSum);

var ordered = lines.Append(DividerA).Append(DividerB)
	.Where(x => x.Length > 0).OrderBy(x => x, comparer).ToList();
Console.WriteLine((ordered.FindIndex(x => x == DividerA) + 1) * 
	(ordered.FindIndex(x => x == DividerB) + 1));

public class PacketComparer : IComparer<string>
{
	public int Compare(string left, string right) =>
		Compare(JsonDocument.Parse(left).RootElement, JsonDocument.Parse(right).RootElement);

	private int Compare(JsonElement left, JsonElement right) =>
		(left.ValueKind, right.ValueKind) switch
		{
			// Both numbers. Compare directly.
			(JsonValueKind.Number, JsonValueKind.Number) => left.GetInt32().CompareTo(right.GetInt32()),

			// Mixed. Convert lone number to list and re-compare.
			(JsonValueKind.Number, _) => Compare(JsonDocument.Parse($"[{left.GetInt32()}]").RootElement, right),
			(_, JsonValueKind.Number) => Compare(left, JsonDocument.Parse($"[{right.GetInt32()}]").RootElement),

			// Both lists. Compare each element and then check for a list running out of items.
			_ => CompareLists(left, right)
		};

	private int CompareLists(JsonElement left, JsonElement right)
	{
		foreach ((var l, var r) in left.EnumerateArray().Zip(right.EnumerateArray()))
		{
			int result = Compare(l, r);
			if (result != 0)
			{
				return result;
			}
		}

		// Return 0 if no remaining items in either list, -1 if left has no more
		// items (in order), or +1 if right has no more items (out of order).
		return left.GetArrayLength().CompareTo(right.GetArrayLength());
	}
}