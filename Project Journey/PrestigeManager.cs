using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeManager : MonoBehaviour
{
    
    [SerializeField] private int prestigeRequirement = 1000000;
    [SerializeField] private TMP_Text prestigeRequirementText;
    [SerializeField] private Button prestigeButton;
    
    [SerializeField] private StructuralResourceTask structuralResourceTask;
    [SerializeField] private HarvestedResourceTask harvestedResourceTask;
    [SerializeField] private MinedResourceTask minedResourceTask;
    [SerializeField] private SpecialResourceTask specialResourceTask;
    
    // Start is called before the first frame update
    void Start()
    {
        prestigeRequirementText.text = "Prestige Requires: $" + prestigeRequirement.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrencyManager.Instance.factoryValue >= prestigeRequirement)
        {
            //---- Enable the button when the player can prestige
            prestigeButton.interactable = true;
            
        }
    }

    public void Prestige()
    {
        //---- Reset the values
        //---- Currency Manager
        CurrencyManager.Instance.ResetData();
        
        //---- Resource Storage Manager
        ResourceManager.Instance.ResetData();
        
        //---- Refinery Manager
        RefineryManager.Instance.ResetData();
        
        //---- Resource Gatherers
        structuralResourceTask.ResetData();
        harvestedResourceTask.ResetData();
        minedResourceTask.ResetData();
        specialResourceTask.ResetData();
        
        //---- Worker Manager
        WorkerManager.Instance.ResetData();
        
        //---- Export Facility Manager
        ExportFacilityManager.Instance.ResetData();

        //---- Save the game
        DataPersistenceManager.Instance.SaveGame();
        
        Debug.Log("Regular Data Reset for Prestige!");
        
        //---- Load the next scene
        SceneManager.Instance.LoadScene(SceneManager.Instance.nextScene);
    }

    //---- To test the prestige button by adding the prestige value to the factory value
    public void GivePrestigeValue() => CurrencyManager.Instance.AddFactoryValue(prestigeRequirement);

}
