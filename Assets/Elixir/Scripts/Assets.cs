namespace Elixir {
    public class Assets : BaseWS {
        [System.Serializable]
        public class Information {
            public string[] category;
            public string description;
            public string gameId;
            public string name;
            public int numVotes;
            public int price;
            public int soldUnits;
            public int supply;
            public string tag;
            public bool instock;
            public string type;
        }
        [System.Serializable]
        public class Media {
            public string images;
            public string glb;
        }

        [System.Serializable]
        public class Asset {
            public string _id;
            public string status;
            public Information information;
            public Media media;
        }

        public Asset[] assets;
        public Asset asset;

        public void Get(callback OnOk = null, callback OnError = null) {
            ElixirController.Instance.StartCoroutine(base.Get($"/assets/{GameID}/assets", () => {
                OnOk?.Invoke();
            }, OnError, true, true));
        }

        // TODO: In Game NFT market
        public delegate void callbackGetAsset(Asset asset);
        public void GetByID(string _id, callbackGetAsset OnOk = null, callback OnError = null) {
            ElixirController.Instance.StartCoroutine(base.Get($"/assets/{GameID}/asset/{_id}", () => {
                OnOk?.Invoke(asset);
            }, OnError, true, true));
        }
    }
}