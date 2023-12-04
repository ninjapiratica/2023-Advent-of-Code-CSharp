﻿//await Day1_Part1("../../../Day1.txt");
//await Day1_Part2("../../../Day1.txt");
//await Day2_Part1("../../../Day2.txt");
//await Day2_Part2("../../../Day2.txt");
await Day3_Part1("../../../Day3.txt");
await Day3_Part2("../../../Day3.txt");


async Task Day1_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    var total = lines.Sum(x =>
    {
        var firstIndex = x.IndexOfAny(numbers);
        var lastIndex = x.LastIndexOfAny(numbers);

        var innerTotal = ((int)x[firstIndex] - 48) * 10 + (int)x[lastIndex] - 48;

        return innerTotal;
    });

    Console.WriteLine($"Day 1 Part 1 Result: {total}");
}

async Task Day1_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numbers = new Dictionary<string, int> {
        { "0",     0 },
        { "1",     1 },
        { "2",     2 },
        { "3",     3 },
        { "4",     4 },
        { "5",     5 },
        { "6",     6 },
        { "7",     7 },
        { "8",     8 },
        { "9",     9 },
        { "zero",  0 },
        { "one",   1 },
        { "two",   2 },
        { "three", 3 },
        { "four",  4 },
        { "five",  5 },
        { "six",   6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine" , 9 },
    };

    int FindFirstNumber(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            foreach (var number in numbers.Where(n => n.Key.Length <= line.Length - i))
            {
                for (int j = 0; j < number.Key.Length; j++)
                {
                    if (number.Key[j] != line[i + j])
                    {
                        goto nextNumber;
                    }
                }

                return number.Value;

            nextNumber:;
            }
        }
        return 0;
    }

    int FindLastNumber(string line)
    {
        for (int i = line.Length - 1; i >= 0; i--)
        {
            foreach (var number in numbers.Where(n => n.Key.Length <= i + 1))
            {
                for (int j = 0; j < number.Key.Length; j++)
                {
                    if (number.Key[number.Key.Length - 1 - j] != line[i - j])
                    {
                        goto nextNumber;
                    }
                }

                return number.Value;

            nextNumber:;
            }
        }

        return 0;
    }

    var total = lines.Sum(x =>
    {
        var innerTotal = FindFirstNumber(x) * 10 + FindLastNumber(x);

        return innerTotal;
    });

    Console.WriteLine($"Day 1 Part 2 Result: {total}");
}

async Task Day2_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var total = 0;
    foreach (var line in lines)
    {
        var parts = line.Split(':');

        var gameNumber = int.Parse(parts[0].Split(' ')[1]);

        var sets = parts[1].Split(';');

        foreach (var set in sets)
        {
            var pulls = set.Split(',');

            foreach (var pull in pulls)
            {
                var colorParts = pull.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var count = int.Parse(colorParts[0]);

                var possible = colorParts[1] switch
                {
                    "blue" => count <= 14,
                    "red" => count <= 12,
                    "green" => count <= 13,
                    _ => false
                };

                if (!possible)
                {
                    goto done;
                }
            }
        }

        total += gameNumber;

    done:;

    }

    Console.WriteLine($"Day 2 Part 1 Result: {total}");
}

async Task Day2_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var total = 0;
    foreach (var line in lines)
    {
        var parts = line.Split(':');

        var gameNumber = int.Parse(parts[0].Split(' ')[1]);

        var sets = parts[1].Split(';');

        var red = 0;
        var green = 0;
        var blue = 0;

        foreach (var set in sets)
        {
            var pulls = set.Split(',');

            foreach (var pull in pulls)
            {
                var colorParts = pull.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var count = int.Parse(colorParts[0]);

                switch (colorParts[1])
                {
                    case "blue":
                        blue = Math.Max(blue, count);
                        break;
                    case "red":
                        red = Math.Max(red, count);
                        break;
                    case "green":
                        green = Math.Max(green, count);
                        break;
                };
            }
        }

        total += (red * green * blue);
    }

    Console.WriteLine($"Day 2 Part 2 Result: {total}");
}

async Task Day3_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numberPositions = new Dictionary<(int X, int Y), (List<(int X, int Y)> Positions, int Number)>();
    var parts = new HashSet<(int X, int Y)>();

    for (int x = 0; x < lines.Length; x++)
    {
        for (int y = 0; y < lines[x].Length; y++)
        {
            var val = lines[x][y];
            if (char.IsNumber(val))
            {
                numberPositions.TryGetValue((x, y - 1), out var previousNumberPosition);
                var positions = previousNumberPosition.Positions ?? [];
                positions.Add((x, y));

                var number = previousNumberPosition.Number;
                number = number * 10 + Math.Max((int)char.GetNumericValue(val), 0);

                numberPositions.Add((x, y), (positions, number));
            }
            else if (val != '.')
            {
                parts.Add((x, y));
            }
        }
    }

    var total = 0;

    foreach (var part in parts)
    {
        for (int i = part.X - 1; i < part.X + 2; i++)
        {
            for (int j = part.Y - 1; j < part.Y + 2; j++)
            {
                if (numberPositions.TryGetValue((i, j), out var numberPosition))
                {
                    total += numberPositions[numberPosition.Positions.Last()].Number;
                    foreach (var item in numberPosition.Positions)
                    {
                        numberPositions.Remove(item);
                    }
                }
            }
        }
    }

    Console.WriteLine($"Day 3 Part 1 Result: {total}");
}

async Task Day3_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numberPositions = new Dictionary<(int X, int Y), (List<(int X, int Y)> Positions, int Number)>();
    var parts = new HashSet<(int X, int Y)>();

    for (int x = 0; x < lines.Length; x++)
    {
        for (int y = 0; y < lines[x].Length; y++)
        {
            var val = lines[x][y];
            if (char.IsNumber(val))
            {
                numberPositions.TryGetValue((x, y - 1), out var previousNumberPosition);
                var positions = previousNumberPosition.Positions ?? [];
                positions.Add((x, y));

                var number = previousNumberPosition.Number;
                number = number * 10 + Math.Max((int)char.GetNumericValue(val), 0);

                numberPositions.Add((x, y), (positions, number));
            }
            else if (val == '*')
            {
                parts.Add((x, y));
            }
        }
    }

    var total = 0;

    foreach (var part in parts)
    {
        var adjacentNumbers = new Dictionary<List<(int X, int Y)>, int>();

        for (int i = part.X - 1; i < part.X + 2; i++)
        {
            for (int j = part.Y - 1; j < part.Y + 2; j++)
            {
                if (numberPositions.TryGetValue((i, j), out var numberPosition))
                {
                    adjacentNumbers.TryAdd(numberPosition.Positions, numberPositions[numberPosition.Positions.Last()].Number);
                }
            }
        }

        if(adjacentNumbers.Count == 2)
        {
            total += adjacentNumbers.First().Value * adjacentNumbers.Last().Value;
        }
    }

    Console.WriteLine($"Day 3 Part 2 Result: {total}");
}