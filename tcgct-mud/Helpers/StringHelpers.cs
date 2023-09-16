
public static class StringHelpers
{
    static int getEditDistance(string X, string Y)
    {
        int m = X.Length;
        int n = Y.Length;

        int[][] T = new int[m + 1][];
        for (int i = 0; i < m + 1; ++i)
        {
            T[i] = new int[n + 1];
        }

        for (int i = 1; i <= m; i++)
        {
            T[i][0] = i;
        }
        for (int j = 1; j <= n; j++)
        {
            T[0][j] = j;
        }

        int cost;
        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                cost = X[i - 1] == Y[j - 1] ? 0 : 1;
                T[i][j] = Math.Min(Math.Min(T[i - 1][j] + 1, T[i][j - 1] + 1),
                        T[i - 1][j - 1] + cost);
            }
        }

        return T[m][n];
    }

    public static double FindSimilarity(string x, string y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Strings must not be null");
        }

        double maxLength = Math.Max(x.Length, y.Length);
        if (maxLength > 0)
        {
            // optionally ignore case if needed
            return (maxLength - getEditDistance(x, y)) / maxLength;
        }
        return 1.0;
    }

}

