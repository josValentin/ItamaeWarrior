using UnityEngine;
using UnityEngine.UI;

public class SatoshiBalanceController : MonoBehaviour {
    public Text balanceText;
    // Start is called before the first frame update
    void Awake() {
        
    }
    private void OnEnable() {
        if (!balanceText) balanceText = GetComponent<Text>();
        Elixir.ElixirController.OnBalance += OnBalance;

        OnBalance(Elixir.ElixirController.balance);

    }
    private void OnDisable() {
        Elixir.ElixirController.OnBalance -= OnBalance;
    }
    void OnBalance(uint balance) {
        if (balanceText) balanceText.text = balance.ToString();

    }

}
