using Randomizer;

int TotalAmount = 0; // total amount of cases that will be randomized
bool IsStratified = false; // true if randomization is stratified
int NumberOfStratas = 1; // number of stratas if randomization is stratified
bool IsBlock = false; // true if randomization is block
int MinimalBlockSize = 0; // minimal size of block if randomization is block
int MaximalBlockSize = 0; // maximal size of block if randomization is block

List<Block> Blocks; // collection of non-randomized blocks with both 4 and 6 cases

// entering total amount of cases
bool Loop = true;
Console.WriteLine("Enter total number of cases that should be randomized (2 or more and even only), please");
while (Loop == true)
{
    string? TotalAmountString = Console.ReadLine();
    try
    {
        TotalAmount = Convert.ToInt32(TotalAmountString);
        if ((TotalAmount >= 2) && ((TotalAmount % 2) == 0))
            Loop = false;
        else
            Console.WriteLine("Sample size cannot be less than 2 and should be even, try again, please");
    }
    catch
    {
        Console.WriteLine("Wrong data. Enter total number of cases that should be randomized again, please");
    }
}

// entering whether randomization is stratified
Loop = true;
Console.WriteLine("Is randomization stratified? Enter 'y' or 'n'");
while (Loop == true)
{
    string? s = Console.ReadLine();
    if ((s == "y") || (s == "Y"))
    {
        IsStratified = true;
        Loop = false;
    }
    else if ((s == "n") || (s == "N"))
    {
        IsStratified = false;
        Loop = false;
    }
    else
        Console.WriteLine("Wrong data, try again. Is randomization stratified? Enter 'y' or 'n'");
}

// entering number of stratas if randomization is stratified
if (IsStratified == true)
{
    Loop = true;
    Console.WriteLine("Enter the number of stratas, please");
    while (Loop == true)
    {
        string? NumberOfStratasString = Console.ReadLine();
        try
        {
            NumberOfStratas = Convert.ToInt32(NumberOfStratasString);
            Loop = false;
        }
        catch
        {
            Console.WriteLine("Wrong data. Enter the number of stratas again, please");
        }
    }
}

// entering whether randomization is block
Loop = true;
Console.WriteLine("Is randomization block? Enter 'y' or 'n'");
while (Loop == true)
{
    string? s = Console.ReadLine();
    if ((s == "y") || (s == "Y"))
    {
        IsBlock = true;
        Loop = false;
    }
    else if ((s == "n") || (s == "N"))
    {
        IsBlock = false;
        Loop = false;
    }
    else
        Console.WriteLine("Wrong data, try again. Is randomization block? Enter 'y' or 'n'");
}

if (IsBlock == true)
{
    // entering minimal size of block
    bool Loop2 = true;
    while (Loop2 == true)
    {
        Loop = true;
        Console.WriteLine($"Enter minimal size of block (from 2 to {TotalAmount}) and even only, please");
        while (Loop == true)
        {
            string? s = Console.ReadLine();
            try
            {
                MinimalBlockSize = Convert.ToInt32(s);
                if ((MinimalBlockSize >= 2) && (MinimalBlockSize <= TotalAmount) && ((MinimalBlockSize % 2) == 0))
                    Loop = false;
                else
                    Console.WriteLine($"Minimal size of block should be in range (2 - {TotalAmount}) and even only. Try again, please");
            }
            catch
            {
                Console.WriteLine($"Wrong data. Enter minimal size of block (from 2 to {TotalAmount}) and even only again, please");
            }
        }

        // entering maximal size of block
        Loop = true;
        Console.WriteLine($"Enter maximal size of block (from {MinimalBlockSize} to {TotalAmount}) and even only, please");
        while (Loop == true)
        {
            string? s = Console.ReadLine();
            try
            {
                MaximalBlockSize = Convert.ToInt32(s);
                if ((MaximalBlockSize >= MinimalBlockSize) && (MaximalBlockSize <= TotalAmount) && ((MaximalBlockSize % 2) == 0))
                    Loop = false;
                else
                    Console.WriteLine($"Maximal size of block should be in range ({MinimalBlockSize} - {TotalAmount}) and even only. Try again, please");
            }
            catch
            {
                Console.WriteLine($"Wrong data. Enter maximal size of block ({MinimalBlockSize} - {TotalAmount}) and even only again, please");
            }
        }
        if (AreBlockSizesCorrect(TotalAmount, MinimalBlockSize, MaximalBlockSize) == true) // if such minimal and maximal block sizes can ever form necessary sample size
            Loop2 = false;
        else
            Console.WriteLine($"Minimal block size {MinimalBlockSize} and maximal block size {MaximalBlockSize} can never form necessary sample size. Try again, please");
    }
}

// randomization
for (int Index = 0; Index < NumberOfStratas; Index++)
{
    Console.WriteLine();
    Console.WriteLine($"Strata {Index}:");
    Blocks = new List<Block>();

    if (IsBlock == true) // if randomization is block
    {
        int counter = 1;
        int ValuesInBlocks = 0;
        int value = 0;
        ValuesInBlocks = 0;
        foreach (Block block in Blocks)
        {
            ValuesInBlocks += block.Size;
        }
        do
        {
            value = GetEvenRandomValue(MinimalBlockSize, MaximalBlockSize);
            if (ValuesInBlocks + value == TotalAmount)
            {
                Blocks.Add(new Block { Id = counter, Size = value, Data = Shuffle(FillArray(value)) });
                ValuesInBlocks += value;
                counter++;
            }
            else if (((ValuesInBlocks + value) < TotalAmount) && ((ValuesInBlocks + value + MinimalBlockSize) <= TotalAmount))
            {
                Blocks.Add(new Block { Id = counter, Size = value, Data = Shuffle(FillArray(value)) });
                ValuesInBlocks += value;
                counter++;
            }
        }
        while (ValuesInBlocks < TotalAmount);

        // showing and writing data to file
        List<string> Data = new List<string>();
        foreach (Block block in Blocks)
        {
            string s = String.Join("", block.Data.ToArray());
            Data.Add(s);
            Console.WriteLine(s);
        }
        File.WriteAllLines($"strata{Index}.txt", Data);
    }

    else // if randomization is not block
    {
        List<int> Values = new List<int>();
        for (int index = 0; index < TotalAmount / 2; index++)
            Values.Add(1);  // 1 - group 1
        for (int index = 0; index < TotalAmount / 2; index++)
            Values.Add(2);  // 2 - group 2
        Values = Shuffle(Values);

        string Data = String.Empty;
        foreach (int x in Values)
            Data += x.ToString();
        Console.WriteLine(Data); // showing data
        File.WriteAllText($"strata{Index}.txt", Data);
    }
}

Console.WriteLine();
Console.WriteLine("Successfully completed! Press any key to exit");
Console.ReadLine();

public static partial class Program
{
    public static List<int> Shuffle(List<int> list)
    {
        string s = list.Count.ToString();
        int LengthOrder = s.Length;
        int Order = 1;
        for (int index = 1; index <= LengthOrder; index++)
        {
            Order *= 10;
        }

        var random = System.Security.Cryptography.RandomNumberGenerator.Create();
        for (int index = 0; index < list.Count; index++) // changing all items of array
        {
            byte[] bytes = new byte[sizeof(int)]; // size = 4
            int rand;
            do
            {
                random.GetNonZeroBytes(bytes); // shuffling array of bytes
                rand = BitConverter.ToInt32(bytes); // getting random integer from -2,147,483,648 to 2,147,483,647
                rand = Math.Abs(rand % Order); // getting module of last digit in integer
            }
            while (rand >= list.Count); // looping while last digit exeeds array size
            int value = list[rand]; // changing array's elements
            list[rand] = list[index];
            list[index] = value;
        }
        return list;
    }

    public static int GetEvenRandomValue(int MinValue, int MaxValue)
    {
        string s = MaxValue.ToString();
        int LengthOrder = s.Length;
        int Order = 1;
        for (int index = 1; index <= LengthOrder; index++)
        {
            Order *= 10;
        }

        var random = System.Security.Cryptography.RandomNumberGenerator.Create();
        byte[] bytes = new byte[sizeof(int)]; // size = 4
        int rand;
        do
        {
            random.GetNonZeroBytes(bytes); // shuffling array of bytes
            rand = BitConverter.ToInt32(bytes); // getting random integer from -2,147,483,648 to 2,147,483,647
            rand = Math.Abs(rand % Order); // getting module of last digit in integer
        }
        while ((rand < MinValue) || (rand > MaxValue) || ((rand % 2) == 1)); // looping while last digit is not in acceptable range and is not even
        return rand;
    }

    public static List<int> FillArray(int ArraySize)
    {
        List<int> Data = new List<int>();
        for (int index = 0; index < ArraySize / 2; index++)
        {
            Data.Add(1);
        }
        for (int index = 0; index < ArraySize / 2; index++)
        {
            Data.Add(2);
        }
        return Data;
    }

    public static bool AreBlockSizesCorrect(int TotalAmount, int MinimalBlockSize, int MaximalBlockSize)
    {
        // creating array of possible block sizes
        List<int> Digits = new List<int>();
        for (int index = MinimalBlockSize; index <= MaximalBlockSize; index += 2)
            Digits.Add(index);

        int[] digits = Digits.ToArray();
        int[] NumberOfDigits = new int[digits.Length]; // the number of digits in current order
        NumberOfDigits[0] = 1;
        int Order = 0;
        bool Loop = true;
        bool Result = false; // return value
        do
        {
            int sum = 0;
            int index = 0;
            foreach (int digit in digits)
            {
                sum += digit * NumberOfDigits[index];
                index++;
            }
            if (sum == TotalAmount) // success
            {
                Result = true;
                Loop = false;
            }
            else if (sum < TotalAmount)
            {
                NumberOfDigits[Order]++; // increasing the number of current digits in array
            }
            else // go to next digit in array
            {
                while ((Order < NumberOfDigits.Length) && ((digits[Order] * NumberOfDigits[Order]) > TotalAmount))
                {
                    Order++;
                }
                if (Order < NumberOfDigits.Length)
                {
                    NumberOfDigits[Order]++;
                    for (index = 0; index < Order; index++)
                    {
                        NumberOfDigits[index] = 0;
                        Order = 0;
                    }
                }
                else
                {
                    Loop = false;
                }
            }
        }
        while ((Order < NumberOfDigits.Length) && (Loop == true));
        return Result;
    }
}