using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

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
    [Tooltip("The Image pain that the image will appear on")]
    public RawImage VisualCanvas;
    [Tooltip("The Streamer that will play the video")]
    public VideoStreamer streamer;

    [Space(10)]

    [Header("START UP")]
    [Tooltip("The Button whose information will be displayed by default")]
    public InventoryButton StartUpButton;

    public UnityEvent InventoryChangedEvent;

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
        Boot.SetActive(false);
        Inventory.SetActive(false);
        if(InventoryChangedEvent == null)
        {
            InventoryChangedEvent = new UnityEvent();
        }
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
            Debug.Log("TYPE IS: " + info.Visual.GetType().ToString());
            if(info.Visual is VideoClip)
            {
                Debug.Log("VIDEO");
                if(streamer == null)
                {
                    Debug.Log("NO STREAMER");
                }
                streamer.endPlaying();
                streamer.SetVideo(info.Visual as VideoClip);
                streamer.beginPlaying();
            }
            else if(info.Visual is Sprite)
            {
                Debug.Log("Image");
                Sprite spr = info.Visual as Sprite;
                VisualCanvas.texture = (info.Visual as Sprite).texture;
            }
            else
            {
                Debug.Log("Visual is of type: ");
                Debug.Log(info.Visual.GetType().ToString());
            }
        }
    }

    public void ActivateInventory()
    {
        if(canView)
        {
            Boot.SetActive(true);
            Inventory.SetActive(true);
            SetUpInformation(StartUpButton.Information, StartUpButton.GetComponent<Button>());
        }
    }
    public void DeactivateInventory()
    {
        Boot.SetActive(false);
        Inventory.SetActive(false);
    }
}
