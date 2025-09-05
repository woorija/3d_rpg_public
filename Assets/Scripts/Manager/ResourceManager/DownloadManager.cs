using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class DownloadManager : MonoBehaviour
{
    [SerializeField] List<string> Labels = new List<string>();
    [SerializeField] DownloadUI downloadUI;
    [SerializeField] DownloadCheckUI checkUI;
    [SerializeField] GameObject startButton;

    long patchSize;
    Dictionary<string,long> patchDictionary = new Dictionary<string,long>();
    
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
        startButton.SetActive(true);
        DevelopUtility.Log("Init comp");
    }
    public void GameStart()
    {
        GetCheckDownloadSize();
    }
    async void GetCheckDownloadSize()
    {
        patchSize = 0;

        foreach(var label in Labels)
        {
            DevelopUtility.Log(label);
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle;
            patchSize += handle.Result;
            Addressables.Release(handle);
            DevelopUtility.Log(patchSize.ToString());
        }

        if(patchSize > decimal.Zero)
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
        PatchFiles();
    }
    async void PatchFiles()
    {
        foreach (var label in Labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle;

            if (handle.Result > 0)
            {
                DownLoadLabel(label);
            }
            Addressables.Release(handle);
        }
        CheckDownLoad();
    }
    async void DownLoadLabel(string _label)
    {
        patchDictionary.Add(_label, 0);

        var handle = Addressables.DownloadDependenciesAsync(_label);

        while(!handle.IsDone)
        {
            patchDictionary[_label] = handle.GetDownloadStatus().DownloadedBytes;
            await UniTask.Yield();
        }

        patchDictionary[_label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }
    async void CheckDownLoad()
    {
        downloadUI.SetPercentageText(0);
        downloadUI.SetTotalSize(patchSize);

        while (true)
        {
            float total = 0f;
            foreach (var value in patchDictionary.Values)
            {
                total += value;
            }

            float percentage = total / patchSize;
            downloadUI.SetSlider(percentage);
            downloadUI.SetPercentageText((int)(percentage * 100));
            downloadUI.SetSizeInfoText(total);

            if(total == patchSize)
            {
                break;
            }
            await UniTask.Delay(100);
        }
        downloadUI.SetPercentageText(100);
        downloadUI.SetSlider(1);
        //패치완료
        CustomSceneManager.Instance.LoadManagerScene().Forget();
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
