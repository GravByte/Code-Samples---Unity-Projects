using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
	// Adjustable fade speed can be changed from where it is called
    public float fadeSpeed = 1.5f;
	
	// The fade canvas is just a black image on a canvas that is overlayed with interactability disabled
    public void FadeCanvas()
    {
        Debug.Log("Start Fade");
        StartCoroutine(DoFade());
    }
    
    IEnumerator DoFade()
    {
		// Finds the canvas group component on  the fade canvas
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
		
		// Continually increases the alpha value on the canvas until the image alpha is solid.
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += (Time.deltaTime / 2) * fadeSpeed;
            yield return null;
        }

        canvasGroup.interactable = false;
        yield return null;
    }
}
