using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public struct ItemSlotData
{
    public int equipmentSlotId;
    public int useableSlotId;
    public int useableSlotAmount;
    public int miscSlotId;
    public int miscSlotAmount;
    public ItemSlotData(int _equipmentSlotId, int _useableSlotId, int _useableSlotAmount, int _miscSlotId, int _miscSlotAmount)
    {
        equipmentSlotId = _equipmentSlotId;
        useableSlotId = _useableSlotId;
        useableSlotAmount = _useableSlotAmount;
        miscSlotId = _miscSlotId;
        miscSlotAmount = _miscSlotAmount;
    }
}
public class SaveDataWorld : BaseSaveData
{
    PlayerStatus status;
    public SaveDataWorld(string _path, int _bufferSize, PlayerStatus _status) : base(_path, _bufferSize)
    {
        status = _status;
        dataVersion = 1;
    }
    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        BinaryUtility.SaveData(writer, CustomSceneManager.Instance.currentMapName);
        BinaryUtility.SaveData(writer, status.transform.position);
        Padding();

        base.SaveData();
    }
    public override void LoadData()
    {
        base.LoadData();
        int loadeddataVersion = BinaryUtility.LoadIntData(reader);
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            LoadScene();
        }
    }
    async void LoadScene()
    {
        await CustomSceneManager.Instance.LoadScene(BinaryUtility.LoadStringData(reader), BinaryUtility.LoadVector3Data(reader));
    }
}
public class SaveDataInventory : BaseSaveData
{
    InventoryData inventoryData;
    public SaveDataInventory(string _path, int _bufferSize, InventoryData _inventoryData) : base(_path, _bufferSize)
    {
        inventoryData = _inventoryData;
        dataVersion = 1;
    }
    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        for (int i = 0; i < inventoryData.maxcount; i++)
        {
            BinaryUtility.SaveData(writer, inventoryData.SaveEquipmentSlotId(i));
            BinaryUtility.SaveData(writer, inventoryData.SaveUseableSlotId(i));
            BinaryUtility.SaveData(writer, inventoryData.SaveUseableSlotAmount(i));
            BinaryUtility.SaveData(writer, inventoryData.SaveMiscSlotId(i));
            BinaryUtility.SaveData(writer, inventoryData.SaveMiscSlotAmount(i));
        }
        BinaryUtility.SaveData(writer, inventoryData.gold);
        Padding();

        base.SaveData();
    }
    public override void LoadData()
    {
        base.LoadData();
        int loadeddataVersion = BinaryUtility.LoadIntData(reader);
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            for (int i = 0; i < inventoryData.maxcount; i++)
            {
                int equipId = BinaryUtility.LoadIntData(reader);
                int useableId = BinaryUtility.LoadIntData(reader);
                int useableAmount = BinaryUtility.LoadIntData(reader);
                int miscId = BinaryUtility.LoadIntData(reader);
                int miscAmount = BinaryUtility.LoadIntData(reader);
                ItemSlotData slotData = new ItemSlotData(equipId, useableId, useableAmount, miscId, miscAmount);
                inventoryData.LoadSlotData(i, slotData);
            }
            inventoryData.LoadGold(BinaryUtility.LoadLongData(reader));
            inventoryData.LoadUI();
        }
    }
}
public class SaveDataEquipment : BaseSaveData
{
    EquipmentData equipmentData;
    public SaveDataEquipment(string _path, int _bufferSize, EquipmentData _equipmentData) : base(_path, _bufferSize)
    {
        equipmentData = _equipmentData;
        dataVersion = 1;
    }
    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        BinaryUtility.SaveData(writer, equipmentData.SaveEquipmentItems());
        Padding();

        base.SaveData();
    }
    public override void LoadData()
    {
        base.LoadData();

        int loadeddataVersion = BinaryUtility.LoadIntData(reader);
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            equipmentData.LoadEquipmentItems(BinaryUtility.LoadIntListData(reader));
        }
    }
}
public class SaveDataQuickSlot : BaseSaveData
{
    QuickSlotData quickSlotData;
    public SaveDataQuickSlot(string _path, int _bufferSize, QuickSlotData _quickSlotData) : base(_path, _bufferSize)
    {
        quickSlotData = _quickSlotData;
        dataVersion = 1;
    }
    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        BinaryUtility.SaveData(writer, quickSlotData.SaveQuickSlots());
        Padding();

        base.SaveData();
    }
    public override void LoadData()
    {
        base.LoadData();
        int loadeddataVersion = BinaryUtility.LoadIntData(reader);
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            quickSlotData.LoadQuickSlots(BinaryUtility.LoadIntArrayData(reader));
        }
    }
}
public class SaveDataPlayer : BaseSaveData
{
    PlayerStatus status;
    public SaveDataPlayer(string _path, int _bufferSize, PlayerStatus _status) : base(_path, _bufferSize)
    {
        status = _status;
        dataVersion = 1;
    }
    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        BinaryUtility.SaveData(writer, status.gender);
        BinaryUtility.SaveData(writer, status.playerClass);
        BinaryUtility.SaveData(writer, status.classRank);
        BinaryUtility.SaveData(writer, status.Level);
        BinaryUtility.SaveData(writer, status.allocatedAbilityPoints);
        BinaryUtility.SaveData(writer, status.RemainingAbilityPoint);
        BinaryUtility.SaveData(writer, status.Hp);
        BinaryUtility.SaveData(writer, status.Mp);
        BinaryUtility.SaveData(writer, status.Exp);
        BinaryUtility.SaveData(writer, status.Stamina);
        Padding();

        base.SaveData();
    }
    public async override void LoadData()
    {
        base.LoadData();
        int loadeddataVersion = BinaryUtility.LoadIntData(reader);
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            await status.LoadPlayerData(BinaryUtility.LoadIntData(reader), BinaryUtility.LoadIntData(reader), BinaryUtility.LoadIntData(reader), BinaryUtility.LoadIntData(reader));
            status.LoadBaseStatusData(BinaryUtility.LoadIntListData(reader), BinaryUtility.LoadIntData(reader));
            status.LoadCurrentStatusData(BinaryUtility.LoadIntData(reader), BinaryUtility.LoadIntData(reader), BinaryUtility.LoadIntData(reader), BinaryUtility.LoadFloatData(reader));
        }
    }
}
public class SaveDataQuest : BaseSaveData
{
    QuestManager questManager;
    public SaveDataQuest(string _path, int _bufferSize,  QuestManager _questManager) : base(_path, _bufferSize)
    {
        questManager = _questManager;
        dataVersion = 1;
    }
    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        
        int completeQuestsCount = questManager.completeQuests.Count;
        BinaryUtility.SaveData(writer, completeQuestsCount);
        for(int i = 0; i < completeQuestsCount; i++)
        {
            BinaryUtility.SaveData(writer, questManager.completeQuests[i].questId);
        }

        int completableQuestsCount = questManager.completableQuests.Count;
        BinaryUtility.SaveData(writer, completableQuestsCount);
        for (int i = 0; i < completableQuestsCount; i++)
        {
            BinaryUtility.SaveData(writer, questManager.completableQuests[i].questId);
        }

        int inProgressQuestsCount = questManager.inProgressQuests.Count;
        BinaryUtility.SaveData(writer, inProgressQuestsCount);
        for (int i = 0; i < inProgressQuestsCount; i++)
        {
            BinaryUtility.SaveData(writer, questManager.inProgressQuests[i].questId);
        }
        BinaryUtility.SaveData(writer, questManager.blockedQuestIds);
        BinaryUtility.SaveData(writer, questManager.talkDatas);
        BinaryUtility.SaveData(writer, questManager.huntDatas);
        Padding();

        base.SaveData();
    }
    public override void LoadData()
    {
        base.LoadData();
        int loadeddataVersion = BinaryUtility.LoadIntData(reader);
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            List<int> tempList;
            tempList = BinaryUtility.LoadIntListData(reader);
            questManager.LoadCompleteQuests(tempList);
            tempList = BinaryUtility.LoadIntListData(reader);
            questManager.LoadCompletableQuests(tempList);
            tempList = BinaryUtility.LoadIntListData(reader);
            questManager.LoadInProgressQuests(tempList);
            tempList = BinaryUtility.LoadIntListData(reader);
            questManager.LoadBlockedQuestIds(tempList);
            questManager.LoadTalkDatas(BinaryUtility.LoadNestedDictBoolData(reader));
            questManager.LoadHuntDatas(BinaryUtility.LoadNestedDictIntData(reader));
            questManager.SetQuestData();
        }
    }
}
public class SaveDataSkill : BaseSaveData
{
    SkillData skillData;
    public SaveDataSkill(string _path, int _bufferSize, SkillData _skillData) : base(_path, _bufferSize)
    {
        skillData = _skillData;
        dataVersion = 1;
    }
    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        BinaryUtility.SaveData(writer, skillData.SkillPoint);
        skillData.SaveSkillData();
        BinaryUtility.SaveData(writer, skillData.saveSkillIdList);
        BinaryUtility.SaveData(writer, skillData.saveSkillLevelList);
        Padding();

        base.SaveData();
    }
    public override void LoadData()
    {
        base.LoadData();
        int loadeddataVersion = BinaryUtility.LoadIntData(reader);
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            skillData.LoadData(BinaryUtility.LoadIntData(reader), BinaryUtility.LoadIntListData(reader), BinaryUtility.LoadIntListData(reader));
        }
    }
}
public class SaveDataSetting : BaseSaveData
{
    SettingDataSO settingData;
    public SaveDataSetting(string _path, int _bufferSize, SettingDataSO _settingData) : base(_path, _bufferSize)
    {
        settingData = _settingData;
        dataVersion = 2;
    }

    public override void SaveData()
    {
        ResetStream();

        BinaryUtility.SaveData(writer, dataVersion);
        BinaryUtility.SaveData(writer, settingData.resolution);
        BinaryUtility.SaveData(writer, settingData.screenMode);
        BinaryUtility.SaveData(writer, settingData.vsync);
        BinaryUtility.SaveData(writer, settingData.frameRate);
        BinaryUtility.SaveData(writer, settingData.shadowType);
        BinaryUtility.SaveData(writer, settingData.shadowResolution);
        BinaryUtility.SaveData(writer, settingData.textureResolution);
        BinaryUtility.SaveData(writer, settingData.antiAliasing);
        BinaryUtility.SaveData(writer, settingData.bgmVolume);
        BinaryUtility.SaveData(writer, settingData.sfxVolume);
        Padding();

        base.SaveData();
    }
    public override void LoadData()
    {
        base.LoadData();
        int loadeddataVersion = reader.ReadInt32();
        Debug.Log(loadeddataVersion);
        if (loadeddataVersion == 1)
        {
            settingData.resolution = reader.ReadInt32();
            settingData.screenMode = reader.ReadBoolean();
            settingData.vsync = reader.ReadBoolean();
            settingData.frameRate = reader.ReadInt32();
            settingData.shadowType = reader.ReadInt32();
            settingData.shadowResolution = reader.ReadInt32();
            settingData.textureResolution = reader.ReadInt32();
            settingData.antiAliasing = reader.ReadInt32();
        }
        else if (loadeddataVersion == 2)
        {
            settingData.resolution = reader.ReadInt32();
            settingData.screenMode = reader.ReadBoolean();
            settingData.vsync = reader.ReadBoolean();
            settingData.frameRate = reader.ReadInt32();
            settingData.shadowType = reader.ReadInt32();
            settingData.shadowResolution = reader.ReadInt32();
            settingData.textureResolution = reader.ReadInt32();
            settingData.antiAliasing = reader.ReadInt32();
            settingData.bgmVolume = reader.ReadSingle();
            settingData.sfxVolume = reader.ReadSingle();
        }
    }
}
public class DataManager : SingletonBehaviour<DataManager>
{
    string inventoryPath;
    string equipmentPath;
    string quickSlotPath;
    string playerPath;
    string worldPath;
    string questPath;
    string skillPath;
    string settingPath;
    string keyPath;
    public SaveDataInventory inventorySaveData { get; private set; }
    public SaveDataEquipment equipmentSaveData { get; private set; }
    public SaveDataQuickSlot quickSlotSaveData { get; private set; }
    public SaveDataPlayer playerSaveData { get; private set; }
    public SaveDataWorld worldSaveData { get; private set; }
    public SaveDataQuest questSaveData { get; private set; }
    public SaveDataSkill skillSaveData { get; private set; }
    public SaveDataSetting settingSaveData { get; private set; }
    [SerializeField] SettingDataSO settingDataSO;

    [SerializeField] InventoryData inventoryData;
    [SerializeField] EquipmentData equipmentData;
    [SerializeField] QuickSlotData quickSlotData;
    [SerializeField] SkillData skillData;
    [SerializeField] QuestManager questManager;
    [SerializeField] SoundManager soundManager;

    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] ResolutionManager resolutionManager;
    [SerializeField] RebindingManager rebindingManager;
    protected override void Awake()
    {
        inventoryPath = $"{Application.persistentDataPath}/gamedata1.dat";
        equipmentPath = $"{Application.persistentDataPath}/gamedata2.dat";
        quickSlotPath = $"{Application.persistentDataPath}/gamedata3.dat";
        playerPath = $"{Application.persistentDataPath}/gamedata4.dat";
        worldPath = $"{Application.persistentDataPath}/gamedata5.dat";
        questPath = $"{Application.persistentDataPath}/gamedata6.dat";
        skillPath = $"{Application.persistentDataPath}/gamedata7.dat";
        settingPath = $"{Application.persistentDataPath}/gamedata8.dat";
        keyPath = $"{Application.persistentDataPath}/gamedata9.dat";

        inventorySaveData = new SaveDataInventory(inventoryPath, 2048, inventoryData);
        equipmentSaveData = new SaveDataEquipment(equipmentPath, 128, equipmentData);
        quickSlotSaveData = new SaveDataQuickSlot(quickSlotPath, 128, quickSlotData);
        playerSaveData = new SaveDataPlayer(playerPath, 512, playerStatus);
        worldSaveData = new SaveDataWorld(worldPath, 64, playerStatus);
        questSaveData = new SaveDataQuest(questPath, 4096, questManager);
        skillSaveData = new SaveDataSkill(skillPath, 2048, skillData);
        settingSaveData = new SaveDataSetting(settingPath, 64, settingDataSO);
    }
    private void OnDestroy()
    {
        inventorySaveData.Dispose();
        equipmentSaveData.Dispose();
        quickSlotSaveData.Dispose();
        playerSaveData.Dispose();
        worldSaveData.Dispose();
        questSaveData.Dispose();
        skillSaveData.Dispose();
        settingSaveData.Dispose();
    }
    /*
    private void SaveData<T>(T _data, string _path)
    {
        string json = JsonUtility.ToJson(_data);
        //string encrypt = EncryptionUtility.Encrypt(json);

        using (StreamWriter writer = new StreamWriter(_path, false, Encoding.UTF8))
        {
            writer.Write(json);
        }
        /*
        using (StreamWriter writer = new StreamWriter($"{_path}.dat", false, Encoding.UTF8))
        {
            writer.Write(json);
        }
        
    }*/
    public void SaveGame()
    {
        SaveInventory();
        SavePlayer();
        SaveQuest();
        SaveSkill();
        SaveWorld();
        SaveSetting();
        SaveKey();
    }
    public void SaveInventory()
    {
        DevelopUtility.Log("인벤토리세이브");
        inventorySaveData.SaveData();
    }
    public void SavePlayer()
    {
        DevelopUtility.Log("플레이어세이브");
        playerSaveData.SaveData();
    }
    public void SaveQuickSlot()
    {
        DevelopUtility.Log("퀵슬롯세이브");
        quickSlotSaveData.SaveData();
    }
    public void SaveEquipment()
    {
        DevelopUtility.Log("장비세이브");
        equipmentSaveData.SaveData();
    }
    public void SaveQuest()
    {
        DevelopUtility.Log("퀘스트세이브");
        questSaveData.SaveData();
    }
    public void SaveWorld()
    {
        DevelopUtility.Log("좌표세이브");
        worldSaveData.SaveData();
    }
    public void SaveSkill()
    {
        DevelopUtility.Log("스킬세이브");
        skillSaveData.SaveData();
    }
    public void SaveSetting()
    {
        DevelopUtility.Log("세팅세이브");
        settingSaveData.SaveData();
    }
    public void SaveKey()
    {
        DevelopUtility.Log("키세이브");
        using (StreamWriter writer = new StreamWriter(keyPath, false, Encoding.UTF8))
        {
            writer.Write(rebindingManager.AssetToJson());
        }
    }
    public void LoadGame()
    {
        LoadInventory();
        LoadSkill();
        LoadEquipment();
        LoadQuickSlot();
        LoadPlayer();
        LoadQuest();
        LoadWorld();
        LoadSetting();
        LoadKey();
    }
    public void LoadInventory()
    {
        if (File.Exists(inventoryPath))
        {
            inventorySaveData.LoadData();
        }
        else
        {
            InventoryData.Instance.Init();
        }
    }
    public void LoadPlayer()
    {
        if (File.Exists(playerPath))
        {
            playerSaveData.LoadData();
        }
        else
        {
            playerStatus.Init();
        }
    }
    public void LoadEquipment()
    {
        if (File.Exists(equipmentPath))
        {
            equipmentSaveData.LoadData();
        }
        else
        {
            EquipmentData.Instance.Init();
        }
    }
    public void LoadQuickSlot()
    {
        if (File.Exists(quickSlotPath))
        {
            quickSlotSaveData.LoadData();
        }
        else
        {
            quickSlotData.Init();
        }
    }
    public async void LoadWorld()
    {
        if (File.Exists(worldPath))
        {
            worldSaveData.LoadData();
        }
        else
        {
            await CustomSceneManager.Instance.LoadScene("Map1Scene", Vector3.zero);
        }
    }
    public void LoadQuest()
    {
        if (File.Exists(questPath))
        {
            questSaveData.LoadData();
        }
        else
        {
            QuestManager.Instance.SetQuestData();
        }
    }
    public void LoadSkill()
    {
        if (File.Exists(skillPath))
        {
            skillSaveData.LoadData();
        }
        else
        {
            SkillData.Instance.Init();
        }
    }
    public void LoadSetting()
    {
        if (File.Exists(settingPath))
        {
            settingSaveData.LoadData();
        }
        else
        {
            settingDataSO.Init();
        }
        resolutionManager.LoadSetting(settingDataSO);
        soundManager.LoadSetting(settingDataSO);
        GraphicsManager.Instance.LoadSetting(settingDataSO);
        UIManager.Instance.SettingUIClose();
    }
    public void LoadKey()
    {
        if (File.Exists(keyPath))
        {
            string json;
            using (StreamReader reader = new StreamReader(keyPath, Encoding.UTF8))
            {
                json = reader.ReadToEnd();
            }
            rebindingManager.AssetLoadFromJson(json);
        }
        else
        {
            rebindingManager.Init();
        }
    }
}
