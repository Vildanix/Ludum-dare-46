using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectUI : MonoBehaviour
{

    public GameObject targetToFollow;

    private RectTransform uiTransform;

    private void Awake()
    {
        uiTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetToFollow) {
            Vector3 position = Camera.main.WorldToScreenPoint(targetToFollow.transform.position);
            uiTransform.anchoredPosition = position;
        }
        
    }
}
