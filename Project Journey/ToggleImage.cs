using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleImage : MonoBehaviour
{
    public bool enableImage;
    [SerializeField] private Sprite enabledSprite, disabledSprite;

    private void Start() => GetComponent<UnityEngine.UI.Image>().sprite = enableImage ? enabledSprite : disabledSprite;

        public void Toggle()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = GetComponent<UnityEngine.UI.Image>().sprite == enabledSprite ? disabledSprite : enabledSprite;
        enableImage = !enableImage;
    }
}
