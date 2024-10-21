using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneActivator : MonoBehaviour
{
    public bool canInteract = false;
    [SerializeField] private GameObject tombstoneCollider;

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            tombstoneCollider.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }
}
