using UnityEngine;

public class BossHUDTrigger : MonoBehaviour
{
    [SerializeField] BaseBlackBoard blackBoard;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(CustomUtility.CheckHeightDifference(other.gameObject.transform.position.y, transform.position.y, 10f))
            {
                blackBoard.GetHUD();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blackBoard.ReleaseHUD();
        }
    }
}
