using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour {

    [SerializeField] private ItemData acceptedItem;

    private void Start() {
        // TODO: reference to the parent
    }
    public bool AcceptsItem(Item item) {
        return item.data == acceptedItem;
    }

    public void OnDrop() {
        GetComponent<Renderer>().material.color = Color.green;
        // TODO: animazione di successo
        
        GetComponentInParent<Inspectable>().Resolve();
    }
}