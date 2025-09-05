using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Camera minimapCam;
    [SerializeField] GameObject player;
    private void Awake()
    {
        minimapCam = GetComponent<Camera>();
    }
    private void Update()
    {
        Vector3 pos = player.transform.position;
        pos.y += 300;
        minimapCam.transform.position = pos;
    }
}
