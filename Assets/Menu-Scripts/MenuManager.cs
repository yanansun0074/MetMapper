using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    // player input
    PlayerInput playerInput;

    // all menus
    [SerializeField]
    public GameObject menus;

    [SerializeField]
    public GameObject main;

    [SerializeField]
    public GameObject map;

    [SerializeField]
    public GameObject ondisplay;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        CloseAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["ToggleMenu"].triggered) {
            if (!main.activeSelf && !menus.activeSelf) {
                OpenMainMenu();
            }
            else if (main.activeSelf || map.activeSelf || ondisplay.activeSelf) {
                CloseAll();
            }
        } 

        if (playerInput.actions["OnDisplay"].triggered) {
            OpenDisplayMenu();
        }

        if (playerInput.actions["Map"].triggered) {
            OpenMap();
        }

        if (playerInput.actions["Back"].triggered) {
            OpenMainMenu();
        }
    }

    void OpenMainMenu() {
        menus.SetActive(true);
        menus.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 0.5f);
        main.SetActive(true);
        map.SetActive(false);
        ondisplay.SetActive(false);
    }

    void OpenDisplayMenu() {
        menus.SetActive(true);
        main.SetActive(false);
        map.SetActive(false);
        ondisplay.SetActive(true);
    }

    void OpenMap() {
        menus.SetActive(true);
        main.SetActive(false);
        map.SetActive(true);
        ondisplay.SetActive(false);
    }

    void CloseAll() {
        menus.SetActive(false);
        main.SetActive(false);
        map.SetActive(false);
        ondisplay.SetActive(false);
    }
}
