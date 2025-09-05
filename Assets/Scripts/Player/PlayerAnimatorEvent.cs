using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour
{
    [SerializeField] PlayerEffectManager playerEffectList;
    [SerializeField] Player_Land stateLand;
    [SerializeField] PlayerController controller;
    [SerializeField] HitUtility hitUtility;

    void SetSkillPhysicalDamage(int _index)
    {
        hitUtility.SetSkillPhysicalMultiplier(_index);
    }
    void SetSkillMagicalDamage(int _index)
    {
        hitUtility.SetSkillMagicalMultiplier(_index);
    }
    void SetSkillMixDamage(int _index1, int _index2)
    {
        hitUtility.SetSkillMixMultiplier(_index1, _index2);
    }
    public void EffectPlay(int _index)
    {
        playerEffectList.PlayEffect(controller.currentPlaySkillId, _index);
    }
    public void IsNormalAttackHit()
    {
        SetSkillPhysicalDamage(0);
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.25f, 120f, 3f, 2, 10, 1f);
    }
    public void IsS100001Hit()
    {
        SetSkillPhysicalDamage(0);
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.75f, 120f, 3f, 2, 2, 1f);
    }
    public void IsS100002Hit()
    {
        SetSkillPhysicalDamage(0);
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.75f, 120f, 3f, 4, 1, 1f);
    }
    public void IsS210001Hit()
    {
        SetSkillPhysicalDamage(0);
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        centerPos += controller.transform.forward * 3;
        hitUtility.BoxHit(centerPos, new Vector3(0.8f, 1f, 1.5f), controller.transform.localRotation, 3, 1, 1f);
    }
    public void IsS210003_1Hit()
    {
        SetSkillPhysicalDamage(0);
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularHit(centerPos, 1.25f, 3f, 5, 2, 1f);
    }
    public void IsS210003_2Hit()
    {
        SetSkillPhysicalDamage(1);
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        hitUtility.CircularSectorHit(centerPos, transform.forward, 1.25f, 120f, 3f, 1, 1, 1f);
    }
    public void IsS210005Hit()
    {
        SetSkillPhysicalDamage(0);
        Vector3 centerPos = controller.transform.position;
        centerPos.y += 0.8f;
        centerPos += controller.transform.forward * 3;
        hitUtility.BoxHit(centerPos, new Vector3(0.8f, 1f, 2f), controller.transform.localRotation, 1, 5, 1f);
    }
    public void LandMotionSkip()
    {
        stateLand.OnMotionSkip();
    }
    public void AnimationEnd()
    {
        controller.AnimationEnd();
    }
}
