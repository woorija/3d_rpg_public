using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour
{
    [SerializeField]
    [Interfaces(typeof(ICSVRead))]
    private List<MonoBehaviour> databaseScripts;

    private List<ICSVRead> databases;
    private void Awake()
    {
        databases = new List<ICSVRead>();
    }
    private async void Start()
    {
        foreach (var script in databaseScripts)
        {
            if(script is ICSVRead csvRead)
            {
                databases.Add(csvRead);
            }
        }
        DevelopUtility.Log("CSV load Start");
        List<UniTask> tasks = new List<UniTask>();
        foreach (var database in databases)
        {
            tasks.Add(database.ReadCSV());
        }

        // UniTask.WhenAll로 병렬 실행
        await UniTask.WhenAll(tasks);
        DevelopUtility.Log("CSV load comp");

        DataManager.Instance.LoadGame();
    }
}
