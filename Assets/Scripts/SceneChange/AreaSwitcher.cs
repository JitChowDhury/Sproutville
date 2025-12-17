using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AreaSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject doorSprite;
    private bool playerInside;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (doorSprite != null)
        {
            doorSprite.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInside && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(ChangeScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    IEnumerator ChangeScene()
    {
        if (doorSprite != null)
        {
            doorSprite.GetComponent<SpriteRenderer>().enabled = true;
        }
        yield return new WaitForSeconds(1);


        SceneManager.LoadScene(sceneToLoad);
    }
}
