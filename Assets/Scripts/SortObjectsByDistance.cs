using UnityEngine;

public static class SortObjectsByDistance
{

    /// <summary>
    /// Sort <see cref="GameObject"/> by distance using Bubble sort.
    /// </summary>
    /// <param name="gameObjectsToSort"></param> objects to sort.
    /// <param name="position"></param> position to mesure to.
    /// <returns></returns>
    public static GameObject[] SortByDistance(GameObject[] gameObjectsToSort, Vector3 position)
    {
        float[] distances = new float[gameObjectsToSort.Length];
        for (int i = 0; i < gameObjectsToSort.Length; i++)
        {
            distances[i] = Vector3.Distance(gameObjectsToSort[i].transform.position, position);
        }

        for (int i = 0; i < distances.Length; i++)
        {
            for (int j = 0; j < distances.Length - 1; j++)
            {
                if (!(distances[j] > distances[j + 1])) continue;
                
                GameObject temp = gameObjectsToSort[j];
                gameObjectsToSort[j] = gameObjectsToSort[j + 1];
                gameObjectsToSort[j + 1] = temp;
                
                distances[j] += distances[j + 1];
                distances[j + 1] = distances[j] - distances[j + 1];
                distances[j] -= distances[j + 1];
            }   
        }

        return gameObjectsToSort;
    }
}
