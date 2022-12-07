DirDetails currentDir = new() { FullPath = "/" };
List<DirDetails> dirs = new() {	currentDir };

foreach (string line in File.ReadLines("Input.txt"))
{
	if (line.StartsWith("$ cd"))
	{
		string dirName = line.Substring(5);
		if (dirName == "..")
		{
			currentDir = currentDir.Parent;
		}
		else
		{
			var dir = dirs.FirstOrDefault(d => d.FullPath == dirName);
			if (dir == null)
			{
				dir = new DirDetails
				{
					FullPath = currentDir.FullPath + dirName + "/",
					Parent = currentDir
				};
				currentDir.Dirs.Add(dir);
			}
			currentDir = dir;
		}
	}
	else if (!line.StartsWith("$ ls") && !line.StartsWith("dir"))
	{
		AddSize(currentDir, int.Parse(line.Split(' ')[0]));
	}
}

List<long> sizes = new();
GetSize(dirs.First(), sizes);

Console.WriteLine(sizes.Where(x => x <= 100000).Sum());
long free = 70000000 - sizes.First();
long needed = 30000000 - free;
Console.WriteLine(sizes.OrderBy(x => x).First(x => x >= needed));

static void AddSize(DirDetails dir, int size)
{
	if (dir == null)
	{
		return;
	}
	dir.Size += size;
	AddSize(dir.Parent, size);
}

static void GetSize(DirDetails dir, List<long> sizes)
{
	sizes.Add(dir.Size);
	foreach (var d in dir.Dirs)
	{
		GetSize(d, sizes);
	}
}

class DirDetails
{
	public string FullPath;
	public List<DirDetails> Dirs = new();
	public DirDetails? Parent = null;
	public long Size = 0;
}
