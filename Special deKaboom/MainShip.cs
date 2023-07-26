using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainShip : MonoBehaviour
{
    
    [Header("Health")]
    [SerializeField] private Health health;
    
    [Header("Special Payload")]
    [SerializeField] private bool canLaunchPayload = false;
    [SerializeField] private GameObject specialPayloadPrefab;
    [SerializeField] private TMP_Text specialPayloadActiveText;
    [SerializeField] private TMP_Text specialPayloadNotActiveText;
    [Header("Special Payload Ready Light")]
    [SerializeField] private GameObject spReadyLight;
    [SerializeField] private Material spReadyLightOn;
    [SerializeField] private Material spReadyLightOff;
    
    [Header("Shield")]
    [SerializeField] private float shieldDuration = 10f;
    [SerializeField] private float shieldCooldown = 5f;
    [SerializeField] private bool shieldActive = false;
    [SerializeField] private bool shieldOnCooldown = false;
    [SerializeField] private GameObject shieldVisual;
    [Header("Shield Ready Light")]
    [SerializeField] private GameObject readyLight;
    [SerializeField] private Material readyLightOn;
    [SerializeField] private Material readyLightOff;
    
    
    
    private void Awake()
    {
        health = GetComponent<Health>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        health.noHealthEvent.AddListener(OnNoHealth);
        EnemyWeaponHandler.AllLaunchersDestroyedEvent.AddListener(OnAllLaunchersDestroyed);
        
        specialPayloadPrefab.SetActive(false);
        specialPayloadActiveText.gameObject.SetActive(false);
        specialPayloadNotActiveText.gameObject.SetActive(true);
        
        //---- Activate the shield at the start of the game
        shieldVisual.SetActive(false);
        ActivateShield();
        readyLight.GetComponent<MeshRenderer>().material = readyLightOn;
    }

    private void OnNoHealth()
    {
        print("Main ship destroyed!");
        
        //---- Go back to the main menu scene using the scene manager
        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }
    
    private void OnAllLaunchersDestroyed()
    {
        print("Special payload active!");
        
        specialPayloadActiveText.gameObject.SetActive(true);
        specialPayloadNotActiveText.gameObject.SetActive(false);
        spReadyLight.GetComponent<MeshRenderer>().material = spReadyLightOn;

        canLaunchPayload = true;
    }

    [ContextMenu("Launch Special Payload")]
    public void LaunchSpecialPayload()
    {
        if (canLaunchPayload)
        {
            specialPayloadPrefab.SetActive(true);
            print("Special payload launched!");
            specialPayloadActiveText.gameObject.SetActive(false);
            specialPayloadNotActiveText.gameObject.SetActive(true);
            
            spReadyLight.GetComponent<MeshRenderer>().material = spReadyLightOff;
            
            canLaunchPayload = false;
        }
        else
        {
            print("Special payload not active!");
        }
        
    }

    [ContextMenu("Activate Shield")]
    public void ActivateShield()
    {
        if (!shieldActive && !shieldOnCooldown)
        {
            shieldActive = true;
            health.godMode = true;
            shieldVisual.SetActive(true);
            StartCoroutine(DeactivateShieldAfterTime(shieldDuration));
            print("Shield activated!");
        }
        else if (shieldActive)
        {
            print("Shield already active!");
        }
        else if (shieldOnCooldown)
        {
            print("Shield on cooldown!");
        }
    }
    
    public void DeactivateShield()
    {
        if (shieldActive)
        {
            shieldActive = false;
            health.godMode = false;
            shieldVisual.SetActive(false);
            print("Shield deactivated!");
            
            //---- Begin Shield Cooldown
            StartCoroutine(ShieldCooldown());
        }
        else
        {
            print("Shield already inactive!");
        }
    }
    
    //---- Coroutine to deactivate the shield after a certain amount of time
    public IEnumerator DeactivateShieldAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        DeactivateShield();
    }
    
    //---- Coroutine to handle the shield cooldown
    public IEnumerator ShieldCooldown()
    {
        shieldOnCooldown = true;
        readyLight.GetComponent<MeshRenderer>().material = readyLightOff;
        yield return new WaitForSeconds(shieldCooldown);
        shieldOnCooldown = false;
        readyLight.GetComponent<MeshRenderer>().material = readyLightOn;
    }
}
