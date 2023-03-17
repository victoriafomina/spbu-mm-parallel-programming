using FloydWarshallAlgorithm;
using MPI;

using (var env = new MPI.Environment(ref args))
{
    var comm = Communicator.world;

    if (comm.Size == 1)
    {
        var matrix = UserInteractions.ReadMatrixFromFile();
        FloydWarshallAlgorithm.FloydWarshallAlgorithm.Run(matrix);
        UserInteractions.WriteMatrixToFile(matrix);
        return;
    }

    if (comm.Rank == 0)
    {
        var matrix = UserInteractions.ReadMatrixFromFile();

        if (matrix.Length < comm.Size)
        {
            FloydWarshallAlgorithm.FloydWarshallAlgorithm.Run(matrix);
            UserInteractions.WriteMatrixToFile(matrix);
            return;
        }

        var rowsNumber = matrix.GetLength(0);

        for (var i = 1; i < comm.Size; i++)
        {
            // Получаем подматрицы для запуска алгоритма на нодах
            int firstRow = rowsNumber / comm.Size * i;
            int lastRow = i != comm.Size - 1
                ? firstRow + rowsNumber / comm.Size - 1
                : rowsNumber - 1;

            var submatrix = MatrixUtil.GetSubmatrix(matrix, firstRow, lastRow);
            comm.Send(submatrix, i, 0);
        }

        int submatrixLastRow = rowsNumber / comm.Size - 1;
        var nodeSubmatrix = MatrixUtil.GetSubmatrix(matrix, 0, submatrixLastRow);


        FloydWarshallAlgorithm.FloydWarshallAlgorithm.RunOnNode(nodeSubmatrix, comm);

        var matrixOfShortestWays = new List<int>[matrix.Length];
        for (int i = 0; i <= submatrixLastRow; ++i)
        {
            matrixOfShortestWays[i] = nodeSubmatrix[i];
        }

        var currentMatrixRow = submatrixLastRow + 1;
        for (var i = 1; i < comm.Size; i++)
        {
            var receivedMatrix = comm.Receive<List<int>[]>(i, 0);

            for (var j = 0; j < receivedMatrix.Length; ++j)
            {
                matrixOfShortestWays[currentMatrixRow] = receivedMatrix[j];
                ++currentMatrixRow;
            }

        }

        UserInteractions.WriteMatrixToFile(matrixOfShortestWays);
    }
    else
    {
        var receivedMatrix = comm.Receive<List<int>[]>(0, 0);
        
        FloydWarshallAlgorithm.FloydWarshallAlgorithm.RunOnNode(receivedMatrix, comm);

        comm.Send(receivedMatrix, 0, 0);
    }
}