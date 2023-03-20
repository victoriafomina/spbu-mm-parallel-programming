namespace FloydWarshallAlgorithm;

internal static class MatrixUtil
{
    internal static List<int>[] GetSubmatrix(List<int>[] matrix, int firstRow, int lastRow)
    {
        int newRowsNumber = lastRow - firstRow + 1;
        var submatrix = new List<int>[newRowsNumber];

        for (var i = 0; i <= lastRow - firstRow; i++)
        {
            submatrix[i] = matrix[i + firstRow];
        }

        return submatrix;
    }

    internal static List<int>[] GetMatrixFromFile(string pathToFile)
    {
        if (!File.Exists(pathToFile))
        {
            throw new Exception($"Файл с матрицей смежности по пути [{pathToFile}] не был найден");
        }

        string preMatrix;
        try
        {
            preMatrix = File.ReadAllText(pathToFile);
        }
        catch (Exception)
        {
            throw;
        }

        return GetMatrixFromString(preMatrix);
    }

    private static List<int>[] GetMatrixFromString(string str)
    {        
        var preMatrix = str.Split("\n");

        bool isIntFlag;

        isIntFlag = int.TryParse(preMatrix[0], out int numberOfVertices);
        if (!isIntFlag || numberOfVertices < 0)
        {
            if (!isIntFlag)
            {
                throw new Exception($"Количество вершин в графе [{numberOfVertices} нецелое число");
            }
            else
            {
                throw new Exception($"Количество вершин в графе [{numberOfVertices}] < 1");
            }
        }

        var matrix = new List<int>[numberOfVertices + 1];
        for (var i = 0; i < numberOfVertices + 1; i++)
        {
            matrix[i] = new List<int>();

            for (var j = 0; j < numberOfVertices + 1; ++j)
            {
                if (i == j)
                {
                    matrix[i].Add(0);
                }
                else
                {
                    matrix[i].Add(-1);
                }               
            }
        }

        for (int i = 1; i < preMatrix.Length; i++)
        {
            // Проверяем тройки на корректность

            var triple = preMatrix[i].Split();

            isIntFlag = int.TryParse(triple[0], out int fromVertex);
            if (!isIntFlag)
            {
                throw new Exception($"Вершина [{fromVertex} должна быть целым числом]");
            }

            isIntFlag = int.TryParse(triple[1], out int toVertex);
            if (!isIntFlag)
            {
                throw new Exception($"Вершина [{toVertex} должна быть целым числом]");
            }

            isIntFlag = int.TryParse(triple[2], out int edgeWeight);
            if (!isIntFlag)
            {
                throw new Exception($"Ребро [{toVertex} должно быть целым числом]");
            }
            
            matrix[fromVertex][toVertex] = edgeWeight;
            matrix[toVertex][fromVertex] = edgeWeight;
        }

        return matrix;
    }
}
