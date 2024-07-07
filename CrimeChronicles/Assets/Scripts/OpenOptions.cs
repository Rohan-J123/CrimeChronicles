using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOptions : MonoBehaviour
{
    [SerializeField] private GameObject option;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (spriteRenderer.bounds.Contains(mousePos))
        {
            OnMouseHover();
        }
        else
        {
            OnMouseExit();
        }
    }

    void OnMouseHover()
    {
        option.SetActive(true);
    }

    void OnMouseExit()
    {
        option.SetActive(false);
    }
}
