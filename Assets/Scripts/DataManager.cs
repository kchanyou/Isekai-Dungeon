using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold;
    public int level;
    public float playTime;
    // 기타 저장하고 싶은 변수 추가
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerData playerData = new PlayerData();

    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("데이터 저장됨: " + filePath);
        // Steam Cloud에 업로드하려면 Steamworks API의 파일 저장 기능 사용
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("데이터 불러옴");
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
        }
    }
}
