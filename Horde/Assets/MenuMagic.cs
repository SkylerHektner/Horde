using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMagic : MonoBehaviour
{
    public GameObject PlayGameCanvas;
    public GameObject ProgressCanvas;


    // Start is called before the first frame update
    void Start()
    {
        PlayGameCanvas.SetActive(true);
        ProgressCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HidePlayGameCanvas()
    {
        PlayGameCanvas.SetActive(false);
        ProgressCanvas.SetActive(true);
    }

    public void BeginNewGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Level2");
    }
}
