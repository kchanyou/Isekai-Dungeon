using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold;
    public int level;
    public float playTime;
    // ��Ÿ �����ϰ� ���� ���� �߰�
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
        Debug.Log("������ �����: " + filePath);
        // Steam Cloud�� ���ε��Ϸ��� Steamworks API�� ���� ���� ��� ���
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("������ �ҷ���");
        }
        else
        {
            Debug.Log("����� �����Ͱ� �����ϴ�.");
        }
    }
}
