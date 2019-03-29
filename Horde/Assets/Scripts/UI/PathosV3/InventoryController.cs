using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    // SINGLETON
    public static InventoryController instance;

    [Header("UI OBJECTS")]
    [Space(5)]
    [Tooltip("The Boot UI Object")]
    public GameObject Boot;
    [Tooltip("The Inventory UI Object")]
    public GameObject Inventory;

    [Space(10)]

    [Header("UI INFO OBJECTS")]
    [Space(5)]
    [Tooltip("The text object where the title of the current item is displayed")]
    public Text TitleText;
    [Tooltip("The text object where the description of the current item is displayed")]
    public Text DescriptionText;
    [Tooltip("The text object where the lore of the current item is displayed")]
    public Text LoreText;

    private Button activeButton;
    private bool canView = true;
    
    // the Getter for canView
    public bool CanView { get { return canView; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Boot.active = false;
        Inventory.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Applies the MenuInfo to the UI Objects
    public void SetUpInformation(MenuInfo info, Button sender)
    {
        if (activeButton != sender)
        {
            activeButton = sender;
            TitleText.text = info.TitleText;
            DescriptionText.text = info.DescriptionText;
            LoreText.text = info.LoreText;
        }
    }

    public void ActivateInventory()
    {
        if(canView)
        {
            Boot.active = true;
            Inventory.active = true;
        }
    }
    public void DeactivateInventory()
    {
        Boot.active = false;
        Inventory.active = false;
    }
}
