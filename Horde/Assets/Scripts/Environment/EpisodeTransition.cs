using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            c.GetComponent<PlayerMovement>().lockMovementControls = true;
            c.GetComponent<PlayerMovement>().LookAt(transform.position);
            c.GetComponent<Animator>().SetTrigger("Hacking");
            StartCoroutine(TransitionWithDelay());
        }
    }

    private IEnumerator TransitionWithDelay()
    {
        yield return new WaitForSeconds(4.0f);

        GameManager.Instance.TransitionToNextEpisode();
    }
}
