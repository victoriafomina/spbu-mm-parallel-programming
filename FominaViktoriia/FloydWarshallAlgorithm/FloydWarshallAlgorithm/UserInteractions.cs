namespace FloydWarshallAlgorithm;

internal static class UserInteractions
{
    internal static List<int>[] ReadMatrixFromFile()
    {
        Console.WriteLine("""
            Распараллеленная реализация алгоритма Флойда-Уоршелла

            Введите путь к файлу с графом, в котором
            Число вершин V в графе не менее 5000, число рёбер – не менее 1000000 и не более
            V2/2. В файле с исходным графом данные представлены следующим образом:
            • 1 строка – число вершин в графе;
            • Последующие строки – описание рёбер в виде троек {индекс_вершины
            индекс_вершины вес_ребра}. Индекс первой вершины строго меньше индекса
            второй вершины.

            Путь к файлу:
            """);
        var pathToFile = Console.ReadLine();
        if (!File.Exists(pathToFile))
        {
            throw new Exception($"File [{pathToFile}] does not exist");
        }

        return MatrixUtil.GetMatrixFromFile(pathToFile);
    }

    internal static void WriteMatrixToFile(List<int>[] matrix)
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

        Console.WriteLine("""
            Введите путь к файлу,
            в который будет записан результат алгоритма Флойда-Уоршелла – 
            записанная в файл построчно с пробелом между элементами матрица путей, 
            где для каждого элемента индекс строки – начальная вершина пути, 
            индекс столбца – конечная вершина пути.

            Путь к файлу:
            """);

        var pathToFile = Console.ReadLine();
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
