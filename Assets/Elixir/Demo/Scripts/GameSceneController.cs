using UnityEngine;
using UnityEngine.UI;
public class GameSceneController : MonoBehaviour {
    public Text balance;
    public Text gameData;

    public NFTItem Item;
    void Start() {
        Item.gameObject.SetActive(false);
        balance.text = Elixir.ElixirController.balance.ToString();
        Elixir.ElixirController.OnBalance += OnBalance;
        gameData.text = JsonUtility.ToJson(Elixir.ElixirController.storage);
        // Enum NTFs

        foreach(Elixir.Rewards.NTF nft in Elixir.ElixirController.Instance.rewards.nfts) {
            NFTItem copy = Instantiate<GameObject>(Item.gameObject).GetComponent<NFTItem>();
            copy.Init(nft.assetId, nft.units);
            copy.transform.parent = Item.transform.parent;
            copy.gameObject.SetActive(true);
        }
        

    }
    public void OnBalance(uint balance) {
        this.balance.text = balance.ToString();
    }
    public void BalanceAdd() {
        Elixir.ElixirController.BalanceAdd(1);
    }
    public void BalanceSubtract() {
        Elixir.ElixirController.BalanceSubtract(1);
    }
    public void ChangeInt() {
        Elixir.ElixirController.storage.MyIntValue = Random.Range(-5000, 5000);
        gameData.text = JsonUtility.ToJson(Elixir.ElixirController.storage);
    }
    public void ChangeString() {
        Elixir.ElixirController.storage.MyStringValue = $"STRING_{Random.Range(0, 100)}";
        gameData.text = JsonUtility.ToJson(Elixir.ElixirController.storage);
    }
    public void ChangeFixed() {
        Elixir.ElixirController.storage.MyFloatValue = System.Math.Round(Random.Range(-1f, 1), 2);
        gameData.text = JsonUtility.ToJson(Elixir.ElixirController.storage);
    }
    public void Save() {
        Elixir.ElixirController.Save(false);
        gameData.text = JsonUtility.ToJson(Elixir.ElixirController.storage);
    }
}
