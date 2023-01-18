using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFTItem : MonoBehaviour {
    public Text     units;
    new public Text name;
    int             iunits;
    string          uuid;
    // Start is called before the first frame update
    public void Init(string uuid, int units) {
        iunits = units;
        this.units.text = iunits.ToString();
        name.text = uuid;
        this.uuid = uuid;
        Elixir.ElixirController.Instance.assets.GetByID(this.uuid, (asset)=> {
            this.name.text = asset.information.name;
        });
    }

    public void DoGet() {
        Elixir.ElixirController.Instance.rewards.Reward(new string[] { uuid }, (assets)=> {
            iunits++;
            units.text = iunits.ToString();
        });
    }
}
