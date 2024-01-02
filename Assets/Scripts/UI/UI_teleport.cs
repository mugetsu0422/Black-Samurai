using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class UI_teleport : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject map_mask;
    public GameObject location_mask;
    public Navigator navigator;
    public int pad;
    private int selected_scene;
    private int selected_location;

    private List<String> scenes;
    private Dictionary<String, int> number_of_location;
    public GameObject option;
    public GameObject location;
    private int state;

    private Vector3 option_pos;
    private Vector3 locate_pos;

    void Awake()
    {
        option_pos = map_mask.GetComponent<RectTransform>().position;
        locate_pos = location_mask.GetComponent<RectTransform>().position;
    }
    void OnEnable()
    {
        state = 0;
        selected_scene = 0;
        selected_location = 0;

        location_mask.GetComponent<RectTransform>().position = locate_pos;

        map_mask.GetComponent<RectTransform>().position = option_pos;

        location_mask.SetActive(false);
        map_mask.SetActive(true);
        scenes = Save_Point.savePointData.getScenes();

        number_of_location = new Dictionary<string, int>();
        foreach (string i in scenes)
        {
            number_of_location.Add(i, Save_Point.savePointData.SavePoint(i, true).Count);
        }

        LoadScene_text();
        LoadLocation_text(0, scenes[selected_scene]);
    }
    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && selected_scene < scenes.Count - 1)
            {
                Vector3 position = map_mask.GetComponent<RectTransform>().position;
                position.y -= pad;
                map_mask.GetComponent<RectTransform>().position = position;
                selected_scene += 1;
                LoadLocation_text(0, scenes[selected_scene], load_mode.init);
            }


            if (Input.GetKeyDown(KeyCode.UpArrow) && selected_scene > 0)
            {
                Vector3 position = map_mask.GetComponent<RectTransform>().position;
                position.y += pad;
                map_mask.GetComponent<RectTransform>().position = position;
                selected_scene -= 1;
                LoadLocation_text(0, scenes[selected_scene], load_mode.init);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                state = 1;
                map_mask.SetActive(false);
                location_mask.SetActive(true);

                Vector3 position = location_mask.GetComponent<RectTransform>().position;
                position.y += pad * selected_location;
                location_mask.GetComponent<RectTransform>().position = position;
                selected_location = 0;
                LoadLocation_text(0, scenes[selected_scene], load_mode.init);
            }
        }

        else if (state == 1)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && selected_location < number_of_location[scenes[selected_scene]] - 1)
            {
                Vector3 position = location_mask.GetComponent<RectTransform>().position;
                position.y -= pad;
                location_mask.GetComponent<RectTransform>().position = position;
                selected_location += 1;
                LoadLocation_text(selected_location, scenes[selected_scene], load_mode.down);
            }


            if (Input.GetKeyDown(KeyCode.UpArrow) && selected_location > 0)
            {
                Vector3 position = location_mask.GetComponent<RectTransform>().position;
                position.y += pad;
                location_mask.GetComponent<RectTransform>().position = position;
                selected_location -= 1;
                LoadLocation_text(selected_location, scenes[selected_scene], load_mode.up);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = 0;
                map_mask.SetActive(true);
                location_mask.SetActive(false);
            }


            if (Input.GetKeyDown(KeyCode.Return))
            {
                var temp = GameObject.Find("InGameUI_group");
                if (temp)
                {
                    var navigator2 = temp.transform.Find("navigator").GetComponent<Navigator>();
                    List<Save_Point.SavePoint> saves = Save_Point.savePointData.SavePoint(scenes[selected_scene], true);
                    StartCoroutine(navigator2.Teleport(scenes[selected_scene], saves[selected_location].location));
                    gameObject.SetActive(false);
                }
            }
        }
    }


    void LoadScene_text()
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            Transform child = option.transform.GetChild(i);
            child.gameObject.SetActive(true);
            child.GetComponentInChildren<Text>().text = scenes[i];
        }
    }

    enum load_mode
    {
        init,
        up,
        down,
    }
    void LoadLocation_text(int index, string scene, load_mode load_Mode = load_mode.init)
    {


        if (load_Mode == load_mode.init)
        {
            for (int i = 0; i < 8; i++)
            {
                location.transform.GetChild(i).gameObject.SetActive(false);
            }
            List<Save_Point.SavePoint> saves = Save_Point.savePointData.SavePoint(scene, true);
            int min = Math.Min(8, saves.Count);
            for (int i = 0; i < min; i++)
            {
                Transform child = location.transform.GetChild(i);
                child.gameObject.SetActive(true);
                child.GetComponentInChildren<Text>().text = saves[i].name;
            }
        }
        else if (load_Mode == load_mode.up)
        {
            if (number_of_location[scene] < 8 || index > number_of_location[scene] - 8) return;
            List<Save_Point.SavePoint> saves = Save_Point.savePointData.SavePoint(scene, true);
            for (int i = index; i < index + 8; i++)
            {
                Transform child = location.transform.GetChild(i - index);
                child.GetComponentInChildren<Text>().text = saves[i - index].name;
            }

        }
        else if (load_Mode == load_mode.down)
        {
            if (number_of_location[scene] < 8 || index < 8) return;
            List<Save_Point.SavePoint> saves = Save_Point.savePointData.SavePoint(scene, true);
            for (int i = index - 8; i < index; i++)
            {
                Transform child = location.transform.GetChild(i - (index - 8));
                child.GetComponentInChildren<Text>().text = saves[index - 8].name;
            }
        }

    }
}
