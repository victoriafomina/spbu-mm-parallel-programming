namespace FloydWarshallAlgorithm;

internal static class UserInteractions
{
    internal static List<int>[] ReadMatrixFromFile(string pathToFile)
    {
        if (!File.Exists(pathToFile))
        {
            throw new Exception($"File [{pathToFile}] does not exist");
        }

        return MatrixUtil.GetMatrixFromFile(pathToFile);
    }

    internal static void WriteMatrixToFile(List<int>[] matrix, string pathToFile)
    {
        int numberOfRows = matrix.GetLength(0);

        var isEnumerationFromOne = false;
        for (var i = 0; i < numberOfRows; i++)
        {
            int lastIndex = numberOfRows - 1;

            if (i != 0 && (matrix[0][i] != -1 || matrix[i][0] != -1))
            {
                break;
            }
            else if (i != lastIndex && (matrix[lastIndex][i] != -1 || matrix[i][lastIndex] != -1))
            {
                isEnumerationFromOne = true;
                break;
            }
        }

        var matrixInList = matrix.ToList();
        if (isEnumerationFromOne)
        {
            for (var i = 0; i < numberOfRows; ++i)
            {
                matrixInList[i].RemoveAt(0);
            }

            matrixInList.RemoveAt(0);
        }
        else
        {
            for (var i = 0; i < numberOfRows; ++i)
            {
                matrixInList[i].RemoveAt(numberOfRows - 1);
            }

            matrixInList.RemoveAt(numberOfRows - 1);
        }

        if (string.IsNullOrEmpty(pathToFile))
        {
            throw new Exception("Path to output file cannot be empty");
        }

        using StreamWriter file = new(pathToFile);

        foreach (var row in matrixInList)
        {
            file.WriteLine(string.Join(' ', row));
        }
    }
}
