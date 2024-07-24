
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
            Console.Write($"{item}\t");
        }
        Console.WriteLine();
    }

    public static void DisplayMatrix<T>(T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]}\t");
            }
            Console.WriteLine();
        }
    }
}