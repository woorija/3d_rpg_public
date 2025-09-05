using Cysharp.Threading.Tasks;
using UnityEngine;

public class DropItem : PoolBehaviour<DropItem>
{
    Rigidbody rigid;

    int id;
    int amount;
    float releaseTime;

    bool dropCompleted;
    bool isCollected;
    DropType dropType;
    
    PoolObject itemVfx;
    ItemTrail itemTrail;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!isCollected && Time.time >= releaseTime)
        {
            itemVfx.ReturnPool();
            Release(this);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isCollected && dropCompleted && other.CompareTag("Player"))
        {
            isCollected = true;
            itemVfx.ReturnPool();

            if (id == 0)
            {
                InventoryData.Instance.GetGold(amount);
            }
            else
            {
                InventoryData.Instance.GetItem(id, amount);
            }
            
            SetTrailVfx(other.transform);

            Release(this);
        }
    }
    public void Init(int _id, int _amount)
    {
        dropCompleted = false;
        isCollected = false;
        id = _id;
        amount = _amount;
        releaseTime = Time.time + 60f;
        dropType = SetDropType(id);

        SetItemVfx();

        rigid.AddForce(Random.Range(-3f, 3f), 4f, Random.Range(-3f, 3f), ForceMode.Impulse);
        Complete();
    }
    async void Complete()
    {
        await UniTask.Delay(2000);
        dropCompleted = true;
    }
    DropType SetDropType(int _id)
    {
        if(_id == 0)
        {
            return DropType.Gold;
        }
        else if (_id >= 300000000)
        {
            return DropType.Misc;
        }
        else if (_id >= 200000000)
        {
            return DropType.Useable;
        }
        else
        {
            return DropType.Equipment;
        }
    }

    void SetItemVfx()
    {
        switch (dropType)
        {
            case DropType.Gold:
                itemVfx = PoolManager.Instance.dropGoldPool.Get();
                break;
            case DropType.Equipment:
                itemVfx = PoolManager.Instance.dropEquipmentItemPool.Get();
                break;
            case DropType.Useable:
                itemVfx = PoolManager.Instance.dropUseableItemPool.Get();
                break;
            case DropType.Misc:
                itemVfx = PoolManager.Instance.dropMiscItemPool.Get();
                break;
        }
        itemVfx.transform.SetParent(transform);
        itemVfx.transform.position = transform.position;
    }
    void SetTrailVfx(Transform _transform)
    {
        switch (dropType)
        {
            case DropType.Gold:
                itemTrail = PoolManager.Instance.goldTrailPool.Get();
                break;
            case DropType.Equipment:
                itemTrail = PoolManager.Instance.equipmentItemTrailPool.Get();
                break;
            case DropType.Useable:
                itemTrail = PoolManager.Instance.useableItemTrailPool.Get();
                break;
            case DropType.Misc:
                itemTrail = PoolManager.Instance.miscItemTrailPool.Get();
                break;
        }
        itemTrail.Init(transform.position, _transform);
    }
}
