using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int gold = 100;

    [Header("��ġ ������ ������Ʈ�� ���")]
    public List<GameObject> objectPrefabs; // �ν����Ϳ��� ���
    public List<int> objectCosts;          // �� ������Ʈ�� �ش��ϴ� ���

    private Dictionary<string, int> costDict = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeCosts();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeCosts()
    {
        costDict.Clear();
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            string name = objectPrefabs[i].name;
            int cost = (i < objectCosts.Count) ? objectCosts[i] : 10;
            costDict[name] = cost;
        }
    }

    public int GetObjectCost(string objectName)
    {
        return costDict.ContainsKey(objectName) ? costDict[objectName] : -1;
    }

    public bool SpendGoldForObject(string objectName)
    {
        int cost = GetObjectCost(objectName);
        if (cost < 0)
        {
            Debug.LogError("������Ʈ ����� ã�� �� �����ϴ�: " + objectName);
            return false;
        }
        if (gold >= cost)
        {
            gold -= cost;
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}
