using CustomInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ConsoleWidget
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform scrollRectContent;
    [SerializeField] private GameObject baseLog;

    [SerializeField, ReadOnly] private int logsCount = 0;
    [SerializeField] private Color firstColor;
    [SerializeField] private Color secondColor;


    public void AddLog(string log)
    {
        TextMeshProUGUI text = baseLog.GetComponentInChildren<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError("ConsoleWidget не содержит текстового объекта");
            return;
        }

        logsCount++;


        GameObject go = MonoBehaviour.Instantiate(baseLog, parent: scrollRectContent);
        go.SetActive(true);

        text = go.GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"{logsCount}. {log}";
        
        go.GetComponentInChildren<Image>().color = logsCount % 2 == 0 ? firstColor : secondColor;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
    }
}
