using UnityEngine;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private List<string> eventLog = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogEvent(string eventName)
    {
        string logEntry = System.DateTime.Now.ToString("HH:mm:ss") + " - " + eventName;
        eventLog.Add(logEntry);
        Debug.Log("Event Logged: " + logEntry);
    }

    // 이벤트 로그를 파일로 저장하거나, 서버로 전송하는 기능 추가 가능
    public void SaveEventLog()
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, "eventLog.txt");
        System.IO.File.WriteAllLines(filePath, eventLog);
        Debug.Log("이벤트 로그 저장됨: " + filePath);
    }
}
