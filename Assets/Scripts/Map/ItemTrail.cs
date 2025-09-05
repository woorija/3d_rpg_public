using UnityEngine;

public class ItemTrail : PoolBehaviour<ItemTrail>
{
    Transform playerTransform;
    Vector3 startPos;

    float duration = 0.5f;
    float elapsed = 0f;
    float archeight = 1.2f;

    public void Init(Vector3 _pos, Transform _playerTransform)
    {
        startPos = _pos;
        playerTransform = _playerTransform;
        elapsed = 0f;

        transform.position = startPos;
    }
    private void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        Vector3 target = playerTransform.position + Vector3.up * 0.8f;

        Vector3 pos = Vector3.Lerp(startPos, target, t);
        pos.y += t * (1 - t) * 4 * archeight;

        transform.position = pos;

        if(t >= 1f)
        {
            Release(this);
        }
    }
}
