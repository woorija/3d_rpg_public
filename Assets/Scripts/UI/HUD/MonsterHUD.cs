using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHUD : PoolBehaviour<MonsterHUD>
{
    [SerializeField] Slider hpBar;
    [SerializeField] RectTransform hpBarRectTransform;

    Transform monsterTransform;
    public void SetSlider(int _hp, int _maxHp)
    {
        hpBar.maxValue = _maxHp;
        hpBar.value = _hp;
    }
    public void ChangeHp(int _hp) // 이 함수를 몬스터 스테이터스의 이벤트에 등록하기
    {
        hpBar.value = _hp;
    }
    public void SetTransform(Transform _monsterTransform)
    {
        monsterTransform = _monsterTransform;
    }
    void SetPos()
    {
        hpBarRectTransform.position = Camera.main.WorldToScreenPoint(monsterTransform.position);
    }
    private void Update()
    {
        SetPos();
    }
}
