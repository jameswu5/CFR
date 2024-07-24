
namespace CFR;

public static class Utility
{
    public static int Choose(int n, int k)
    {
        if (k < 0 || k > n)   return 0;
        if (k == 0 || k == n) return 1;

        k = Math.Min(k, n - k);
        int result = 1;

        for (int i = 1; i <= k; i++)
        {
            result *= n - (k - i);
            result /= i;
        }

        return result;
    }

    public static void DisplayArray<T>(T[] array)
    {
        foreach (T item in array)
        {
            string s = typeof(T) == typeof(double) ? $"{item:F3}" : item.ToString();
            Console.Write($"{s}\t");
        }
        Console.WriteLine();
    }

    public static void DisplayMatrix<T>(T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                string s = typeof(T) == typeof(double) ? $"{matrix[i, j]:F3}" : matrix[i, j].ToString();
                Console.Write($"{s}\t");
            }
            Console.WriteLine();
        }
    }

    // Fisher-Yates Shuffle
    public static void Shuffle<T>(T[] array)
    {
        Random random = new();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (array[j], array[i]) = (array[i], array[j]);
        }
    }
}