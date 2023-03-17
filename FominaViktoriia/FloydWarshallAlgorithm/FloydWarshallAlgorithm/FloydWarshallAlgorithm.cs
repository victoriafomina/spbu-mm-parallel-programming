using MPI;

namespace FloydWarshallAlgorithm;

internal static class FloydWarshallAlgorithm
{
    internal static void RunOnNode(List<int>[] matrix, Communicator comm)
    {
        // Количество столбцов в матрице
        int columnsNumber = matrix[0].Count;

        // Сохраняем граничные индексы строк для каждой ноды
        var linesBorders = new (int, int)[comm.Size];
        for (int i = 0; i < linesBorders.Length; i++)
        {
            linesBorders[i].Item1 = columnsNumber / comm.Size * i;
            linesBorders[i].Item2 = i != comm.Size - 1
                ? linesBorders[i].Item1 + columnsNumber / comm.Size - 1
                : columnsNumber - 1;
        }

        for (var k = 0; k < columnsNumber; k++)
        {
            List<int> line;
            var nodeLinesBorders = linesBorders[comm.Rank];

            if (k < nodeLinesBorders.Item1 || k > nodeLinesBorders.Item2)
            {
                int receiveNodeIndex = 0;
                for (var i = 0; i < linesBorders.Length; i++)
                {
                    if (i != comm.Rank && k >= linesBorders[i].Item1 && k <= linesBorders[i].Item2)
                    {
                        receiveNodeIndex = i;
                    }
                }

                // Получаем k строку, которой нет в подматрице ноды
                line = comm.Receive<List<int>>(receiveNodeIndex, k);
            }
            else
            {
                line = matrix[k - nodeLinesBorders.Item1];

                for (var i = 0; i < comm.Size; i++)
                {
                    if (i != comm.Rank)
                    {
                        // Отправляем k строку,которая есть в подматрице ноды
                        comm.Send(matrix[k - nodeLinesBorders.Item1], i, k);
                    }
                }
            }
            
            int numberOfRows = nodeLinesBorders.Item2 - nodeLinesBorders.Item1 + 1;
            for (var i = 0; i < numberOfRows; i++)
            {
                for (var j = 0; j < columnsNumber; j++)
                {
                    if (matrix[i][k] == -1 || line[j] == -1)
                    {
                        continue;
                    }
                    else if (matrix[i][j] == -1)
                    {
                        matrix[i][j] = matrix[i][k] + line[j];
                    }
                    else
                    {
                        matrix[i][j] = Math.Min(matrix[i][j], matrix[i][k] + line[j]);
                    }                    
                }
            }
        }
    }

    internal static void Run(List<int>[] matrix)
    {
        int rowsNumber = matrix.Length;
        for (var k = 0; k < rowsNumber; k++)
        {
            for (var i = 0; i < rowsNumber; i++)
            {
                for (var j = 0; j < rowsNumber; j++)
                {
                    if (matrix[i][k] == -1 || matrix[k][j] == -1)
                    {
                        continue;
                    }
                    else if (matrix[i][j] == -1)
                    {
                        matrix[i][j] = matrix[i][k] + matrix[k][j];
                    }
                    else
                    {
                        matrix[i][j] = Math.Min(matrix[i][j], matrix[i][k] + matrix[k][j]);
                    }
                }
            }

        }
    }
}
