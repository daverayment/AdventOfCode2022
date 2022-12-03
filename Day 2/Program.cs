var lines = File.ReadAllLines("Input.txt");
Console.WriteLine(lines.Sum(line => Score(line, false)));
Console.WriteLine(lines.Sum(line => Score(line, true)));

static int Score(string line, bool isDayTwo)
{
	// Loss/Draw/Win responses for an opponent playing Rock, Paper, Scissors.
	string[] allPlays = { "ZXY", "XYZ", "YZX" };

	// Loss/Draw/Win responses to the current opponent.
	string replies = allPlays[line[0] - 'A'];

	if (isDayTwo)
	{
		// Rewrite line to be a win, loss or draw.
		line = line[0] + " " + replies[line[2] - 'X'];
	}

	// Multiply the index of the match by 3 to get the score.
	int score = replies.IndexOf(line[2]) * 3;

	// Add the value of Rock, Paper or Scissors and return.
	return 1 + (line[2] - 'X') + score;
}