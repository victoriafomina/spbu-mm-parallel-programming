using System;

namespace FloydWarshallAlgorithm;

public static class UserInteractions
{
    public static List<int>[] ReadMatrixFromFile()
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

    public static void WriteMatrixToFile(List<int>[] matrix)
    {
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

        foreach (var row in matrix)
        {
            file.WriteLine(string.Join(' ', row));
        }
    }
}
