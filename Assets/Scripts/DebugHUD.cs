using System.Reflection;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DebugHUD : MonoBehaviour
{
#if UNITY_EDITOR

    public TMP_Text debugText;
    public TMP_Text debugText2;
    public GameObject playerObject;
    public Transform player;
    private float deltaTime;
    Vector3 playerPOS;
    private bool showHUD = false;

    private float refreshTimer = 0f;
    private const float refreshRate = 0.5f;

    private Dictionary<GameObject, string> cache = new Dictionary<GameObject, string>();

    private void Start()
    {
        debugText.gameObject.SetActive(showHUD);
        debugText2.gameObject.SetActive(showHUD);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            showHUD = !showHUD;
            debugText.gameObject.SetActive(showHUD);
            debugText2.gameObject.SetActive(showHUD);
        }

        if (!showHUD)
            return;

        //Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1f / deltaTime;


        if (player != null)
            playerPOS = player.position;
        else
            playerPOS = Vector3.zero;

        refreshTimer -= Time.unscaledDeltaTime;
        if(refreshTimer <= 0f)
        {
            refreshTimer = refreshRate;
            cache.Clear();
        }
        debugText.text = GetVariablesText(playerObject) +
                         $"FPS: {Mathf.CeilToInt(fps)}\n" +
                         $"X/Y/Z: {playerPOS.x:F2}, {playerPOS.y:F2}, {playerPOS.z:F2}\n";

        debugText2.text = GetVariablesText(this.gameObject);
    }

    string GetVariablesText(GameObject obj)
    {
            if(obj == null)
        {
            return " ";
        }
            if(cache.TryGetValue(obj, out string cached))
        {
            return cached;
        }
            string result = $"--{obj.name}--\n";

            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

            foreach (var script in scripts)
            {
                if (script == this)
                    continue;

                result += $"[{script.GetType().Name}]\n";

                FieldInfo[] fields = script.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    object value = field.GetValue(script);
                    result += $"{field.Name}: {value}\n";
                }
            result += "\n";

                //debugText.text += "\n";
            }
        cache[obj] = result ;
        return result;
    }
#endif
}
