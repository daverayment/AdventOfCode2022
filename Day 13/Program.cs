using System.Text.Json;

int pairIndex = 1;
int inOrderSum = 0;

foreach (var pairs in File.ReadLines("Input.txt").Chunk(3))
{
	Console.WriteLine($"\nComparing {pairs[0]} and {pairs[1]}.");
	bool inOrder = Compare(JsonDocument.Parse(pairs[0]).RootElement, JsonDocument.Parse(pairs[1]).RootElement) < 0;
	Console.WriteLine(inOrder ? "In order." : "Out of order.");
	if (inOrder) { inOrderSum += pairIndex; }
	pairIndex++;
}

Console.WriteLine(inOrderSum);

static int Compare(JsonElement left, JsonElement right) =>
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

static int CompareLists(JsonElement left, JsonElement right)
{
	foreach ((var l, var r) in left.EnumerateArray().Zip(right.EnumerateArray()))
	{
		int result = Compare(l, r);
		if (result == 0)
		{
			// Equal. Keep iterating.
			continue;
		}
		return result;
	}

	// Return 0 if no remaining items in either list, -1 if left has no more
	// items (in order), or +1 if right has no more items (out of order).
	return left.GetArrayLength().CompareTo(right.GetArrayLength());
}
