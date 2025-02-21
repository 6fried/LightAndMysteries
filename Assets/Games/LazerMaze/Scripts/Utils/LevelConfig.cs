using UnityEngine;
using System.Collections.Generic;

public class LevelConfig
{
    public List<Vector3> sourcePositions = new List<Vector3>();
    public List<Vector3> sourceRotations = new List<Vector3>();
    public List<Vector3> targetPositions = new List<Vector3>();
    public string[,] grid;
    public string levelDescription;

    public LevelConfig(List<Vector3> srcPositions, List<Vector3> srcRotations, List<Vector3> tgtPositions, string[,] lvlGrid, string lvlDescription)
    {
        sourcePositions = srcPositions;
        sourceRotations = srcRotations;
        targetPositions = tgtPositions;
        grid = lvlGrid;
        levelDescription = lvlDescription;
    }

    public static LevelConfig FromLevelConfigData(LevelConfigData data)
    {
        List<Vector3> srcPositions = new List<Vector3>();
        List<Vector3> srcRotations = new List<Vector3>();
        List<Vector3> tgtPositions = new List<Vector3>();


        Debug.Log(data.targets[0].position[0]);

        foreach (var source in data.sources)
        {
            double[] pos = source.position;
            double[] rot = source.rotation;
            srcPositions.Add(new Vector3((float)pos[0], (float)pos[1], (float)pos[2]));
            srcRotations.Add(new Vector3((float)rot[0], (float)rot[1], (float)rot[2]));
        }

        foreach (var target in data.targets)
        {
            Vector3 targetPos = new Vector3((float)target.position[0], (float)target.position[1], (float)target.position[2]);
            tgtPositions.Add(targetPos);
        }

        string[,] grid = ParseGrid(data.grid);

        return new LevelConfig(srcPositions, srcRotations, tgtPositions, grid, data.description);
    }

    private static string[,] ParseGrid(string[] gridLines)
    {
        int rows = gridLines.Length;
        int cols = gridLines[0].Length;
        string[,] grid = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = $"{gridLines[i][j]}";
            }
        }
        return grid;
    }

    public void PrintLevelInfo()
    {
        Debug.Log($"Nombre de sources: {sourcePositions.Count}");
        for (int i = 0; i < sourcePositions.Count; i++)
        {
            Debug.Log($"Source {i + 1}: Position {sourcePositions[i]}, Rotation {sourceRotations[i]}");
        }

        Debug.Log($"Nombre de cibles: {targetPositions.Count}");
        foreach (var pos in targetPositions)
        {
            Debug.Log($"Cible: Position {pos}");
        }

        if (grid != null)
        {
            string output = "Matrice stringgée :\n";
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    output += grid[i, j] + " ";
                }
                output += "\n";
            }
            Debug.Log(output);
        }

        if (!string.IsNullOrEmpty(levelDescription))
        {
            Debug.Log($"Description du niveau:\n{levelDescription}");
        }
    }
}

[System.Serializable]
public class LevelConfigData
{
    public Source[] sources;
    public Target[] targets;
    public string[] grid;
    public string description;

    [System.Serializable]
    public class Source
    {
        public double[] position;
        public double[] rotation;
    }
    [System.Serializable]
    public class Target
    {
        public double[] position;
    }
}
