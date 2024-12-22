using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet Instance { get; private set; }

    public int money;
    public TMP_Text moneyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        
        moneyText.text = money.ToString();
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        
        if(money < 0)
        {
            money = 0;
        }
        
        moneyText.text = money.ToString();
    }
}
