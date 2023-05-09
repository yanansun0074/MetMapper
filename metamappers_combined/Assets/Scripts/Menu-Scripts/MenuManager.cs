using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField]
    public GameObject menuspawn;

    [SerializeField]
    public GameObject selectables;

    [SerializeField]
    public GameObject selectable_button;

    public GameObject arrowSpawner;

    private int rendered = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        GameObject[] art = GameObject.FindGameObjectsWithTag("Art");
        GameObject[] portraits = GameObject.FindGameObjectsWithTag("Portrait");

        foreach (GameObject obj in art)
        {
            if (obj.layer == 8) {
                GameObject created_btn = (GameObject) Instantiate(selectable_button, selectables.transform);
                TextMeshProUGUI btn_txt = created_btn.GetComponentInChildren<TextMeshProUGUI>();

                Information info = obj.GetComponent<Information>();
                btn_txt.text = info.GetName();

                ObjButton addedInstance = created_btn.AddComponent<ObjButton>();
                addedInstance.artwork = obj;
            }
        }

        foreach (GameObject obj in portraits)
        {
            if (obj.layer == 8) {
                GameObject created_btn = (GameObject) Instantiate(selectable_button, selectables.transform);
                TextMeshProUGUI btn_txt = created_btn.GetComponentInChildren<TextMeshProUGUI>();
                
                Information info = obj.GetComponent<Information>();
                btn_txt.text = info.GetName();

                ObjButton addedInstance = created_btn.AddComponent<ObjButton>();
                addedInstance.artwork = obj;
            }
        }

        OpenDisplayMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (rendered == 0) {
            rendered = 1;
        } else if (rendered == 1) {
            CloseAll();

            rendered = 2;
        }

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
            GoBackToMain();
        }

        if (playerInput.actions["LoadGreek"].triggered) {
            SceneManager.LoadScene("Scene4-GreekSculptures");
        }

        if (playerInput.actions["LoadChinese"].triggered) {
            SceneManager.LoadScene("ChineseCeramics");
        }

        if (playerInput.actions["LoadArms"].triggered) {
            SceneManager.LoadScene("arms_armor");
        }

        if (playerInput.actions["LoadSamurai"].triggered) {
            SceneManager.LoadScene("SamuraiSplendor");
        }
    }

    void OpenMainMenu() {
        menus.SetActive(true);

        menus.transform.position = new Vector3(menuspawn.transform.position.x, menuspawn.transform.position.y + 0.2f, menuspawn.transform.position.z);
        menus.transform.rotation = menuspawn.transform.rotation;
        map.transform.position = new Vector3(menuspawn.transform.position.x, menuspawn.transform.position.y + 0.2f, menuspawn.transform.position.z);
        menus.transform.rotation = menuspawn.transform.rotation;

        main.SetActive(true);
        map.SetActive(false);
        ondisplay.SetActive(false);
        arrowSpawner.GetComponent<ArrowSpawner>().closed = false;
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

    void GoBackToMain() {
        menus.SetActive(true);
        main.SetActive(true);
        map.SetActive(false);
        ondisplay.SetActive(false);
    }

    void CloseAll() {
        menus.SetActive(false);
        main.SetActive(false);
        map.SetActive(false);
        ondisplay.SetActive(false);
        arrowSpawner.GetComponent<ArrowSpawner>().closed = true;
    }
}
