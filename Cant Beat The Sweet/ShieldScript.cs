using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    //private float shieldTimer = 5f;

    [Header("GameObjects")]
    [SerializeField] GameObject _player;

    [Header("Values")]
    public bool isInvincible = false;

    //[SerializeField] float FlashingTime = 3f;
    [SerializeField] float TimeInterval = 0.1f;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    private void Awake()
    {
        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;
    }

    public IEnumerator Invincibility(float shieldTimer)
    {
        Log("Invincible_Start");
        Log("InvincibleLength: " + shieldTimer);

        isInvincible = true;
        //_player.GetComponent<Collider>().enabled = false;

        StartCoroutine(Flash(shieldTimer, TimeInterval));

        yield return new WaitForSeconds(shieldTimer);

        isInvincible = false;
        //_player.GetComponent<Collider>().enabled = true;

        Log("Invincible_End");
    }

    private IEnumerator Flash(float flashingTime, float intervalTime)
    {
        //----------- this counts up time until the float set in FlashingTime
        float elapsedTime = 0f;
        //This repeats our coroutine until the FlashingTime is elapsed
        while (elapsedTime < flashingTime)
        {
            //----------- This gets an array with all the renderers in our gameobject's children
            Renderer[] RendererArray = GetComponents<Renderer>();
            //this turns off all the Renderers
            foreach (Renderer r in RendererArray)
                r.material.SetColor(Color1, Color.red);
                //r.enabled = false;
            //----------- then add time to elapsedtime
            elapsedTime += Time.deltaTime;
            //----------- then wait for the Timeinterval set
            yield return new WaitForSeconds(intervalTime);
            //----------- then turn them all back on
            foreach (Renderer r in RendererArray)
                r.material.SetColor(Color1, Color.white);
                //r.enabled = true;
            elapsedTime += Time.deltaTime;
            //----------- then wait for another interval of time
            yield return new WaitForSeconds(intervalTime);

            if (!isInvincible)
            {
                yield break;
            }
        }

    }

    //-------- Logging Control Method
    private void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }

}