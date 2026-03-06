using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DownloadManager : MonoBehaviour
{
    [SerializeField] List<string> Labels = new List<string>();
    [SerializeField] DownloadUI downloadUI;
    [SerializeField] DownloadCheckUI checkUI;
    [SerializeField] GameObject startButton;

    long patchSize;
    List<object> labelKeys = new List<object>();
    private void Awake()
    {
        foreach (string key in Labels)
        {
            labelKeys.Add(key);
        }
    }
    async void Start()
    {
        startButton.SetActive(false);
        downloadUI.Close();
        checkUI.Close();
        await InitAddressable();
    }
    async UniTask InitAddressable()
    {
        DevelopUtility.Log("Init");
        await Addressables.InitializeAsync();
        await UpdateCatalogs();
        startButton.SetActive(true);
        DevelopUtility.Log("Init complete");
    }
    async UniTask UpdateCatalogs()
    {
        var catalogCheck = await Addressables.CheckForCatalogUpdates();
        if (catalogCheck.Count > 0)
        {
            DevelopUtility.Log($"카탈로그 업데이트");
            await Addressables.UpdateCatalogs(catalogCheck);
        }
    }
    public void GameStart()
    {
        CheckDownloadSize().Forget();
    }
    async UniTask CheckDownloadSize()
    {
        var handle = Addressables.GetDownloadSizeAsync(labelKeys);
        await handle;
        patchSize = handle.Result;
        Addressables.Release(handle);

        if (patchSize > decimal.Zero)
        {
            checkUI.Open();
            checkUI.SetSizeText(patchSize);
        }
        else
        {
            DevelopUtility.Log("게임 시작");
            CustomSceneManager.Instance.LoadManagerScene().Forget();
        }
    }
    public void DownloadStart()
    {
        checkUI.Close();
        downloadUI.Open();
        PatchFiles().Forget();
    }
    async UniTask PatchFiles()
    {
        downloadUI.SetPercentageText(0);
        downloadUI.SetTotalSize(patchSize);

        var handle = Addressables.DownloadDependenciesAsync(labelKeys, Addressables.MergeMode.Union);

        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();

            float percentage = status.TotalBytes > 0 ? (float)status.DownloadedBytes / status.TotalBytes : 0;

            downloadUI.SetSlider(percentage);
            downloadUI.SetPercentageText((int)(percentage * 100));
            downloadUI.SetSizeInfoText(status.DownloadedBytes);

            await UniTask.Delay(100);
        }

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            downloadUI.SetPercentageText(100);
            downloadUI.SetSlider(1);
            DevelopUtility.Log("패치완료");
            Addressables.Release(handle);
            CustomSceneManager.Instance.LoadManagerScene().Forget();
        }
        else
        {
            DevelopUtility.Log("다운로드실패");
            Addressables.Release(handle);
        }
    }
    public static string SetFileSizeText(long _size)
    {
        if (_size >= 1073741824.0)
        {
            return $"{_size / 1073741824.0:##.##} GB";
        }
        else if (_size >= 1048576.0)
        {
            return $"{_size / 1048576.0:##.##} MB";
        }
        else if (_size >= 1024.0)
        {
            return $"{_size / 1024.0:##.##} KB";
        }
        else
        {
            return $"{_size} Bytes";
        }
    }
    #region CustomEditor
    public void AddLabel(string _label)
    {
        Labels.Add(_label);
    }
    public void ClearLabel()
    {
        Labels.Clear();
    }
    #endregion
}
