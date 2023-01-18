using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private TextMeshProUGUI txtSatoshis;
    [SerializeField] private TextMeshProUGUI txtHighScore;
    [SerializeField] private Transform lifesParent;
    [SerializeField] private Sprite lifeLostSprite;
    private Sprite lifeActiveSprite;

    [SerializeField] private Image imgFilledSpecialAttack;
    [SerializeField] private Button btnSpecialAttack;
    bool accumulateForSpecial = true;

    [SerializeField] private Image imgFilledFastAttack;

    private List<Image> lifeImagesList = new List<Image>();

    [HideInInspector]
    public List<Enemy> sushiTempList = new List<Enemy>();


    int score = 0;

    [SerializeField]
    private int _Lifes = 4;

    private int _maxLifes;

    private const int _TargetCountMultiplied = 25000;
    private int _multiplier = 1;

    private static int _Satoshis = 0;

    GameData data;

    bool showRevivePopup = true;

    [SerializeField] private RevivePopup revivePopup;

    [SerializeField] private SceneTransitor transitor;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        data = SaveSystem.LoadData();

        if (data == null)
            data = new GameData();


        txtHighScore.text = "High Score: " + data.highScore.ToString();

        txtScore.text = score.ToString();

        if (ElixirGameController._EnableElixir) {
            _Satoshis = (int)ElixirGameController.GetBalance();
            Debug.Log("Satoshis obtained from Elixir");
        }
        else
        {
            _Satoshis = data.satoshis;
            Debug.Log("Satoshis obtained from the Game Data");
        }

        txtSatoshis.text = _Satoshis.ToString();

        InitLifes();

        //
        imgFilledSpecialAttack.fillAmount = 0;
        btnSpecialAttack.onClick.AddListener(OnSpecialAttackButtonClick);
        btnSpecialAttack.interactable = false;
    }

    public void SetFastAttackFilledValue(float value)
    {
        imgFilledFastAttack.fillAmount = value;
    }

    void InitLifes()
    {
        if (_Lifes <= 0)
            _Lifes = 1;

        _maxLifes = _Lifes;

        Image img = lifesParent.GetChild(0).GetComponent<Image>();

        lifeActiveSprite = img.sprite;

        lifeImagesList.Add(img);

        float deltaX = img.rectTransform.sizeDelta.x;
        float padding = 5f;
        for (int i = 1; i < _Lifes; i++)
        {
            Image imgs = Instantiate(img, lifesParent);

            Vector2 anchorPos = lifeImagesList[lifeImagesList.Count - 1].rectTransform.anchoredPosition; // Get the last anchored position on the list

            anchorPos.x += deltaX + padding;

            imgs.rectTransform.anchoredPosition = anchorPos;

            lifeImagesList.Add(imgs);
        }
    }

    // Update is called once per frame
    public void AddPoint()
    {
        score += 1000;
        txtScore.text = score.ToString();

        if (score >= _TargetCountMultiplied * _multiplier)
        {
            _multiplier++;

            _Satoshis++;
            txtSatoshis.text = _Satoshis.ToString();
            Debug.Log($"¡Has ganado 1 Satoshi! Tienes: {_Satoshis}");

            AddLife();


            ElixirGameController.AddBalance(1);
        }

        if (score > data.highScore)
            txtHighScore.text = "High Score: " + score.ToString();

        if (accumulateForSpecial)
        {
            imgFilledSpecialAttack.fillAmount += 0.035f;
            if (imgFilledSpecialAttack.fillAmount >= 1)
            {
                btnSpecialAttack.interactable = true;
            }
        }

    }

    bool lifeDamaged = false;
    public void RemoveLife()
    {
        if (!lifeDamaged)
        {
            lifeImagesList[_Lifes - 1].sprite = lifeLostSprite;

            lifeDamaged = true;
            return;
        }

        _Lifes--;

        lifeImagesList[_Lifes].enabled = false;

        lifeDamaged = false;

        if (_Lifes <= 0)
        {
            _Lifes = 0;
            Debug.Log("Has perdido...");
            Time.timeScale = 0;

#if UNITY_STANDALONE

            //SaveDataAndReturnToMenu();
            ReturnMenuCoverTransitor();


#else
            if (showRevivePopup)
            {
                revivePopup.Show();
                showRevivePopup = false;
            }
            else
            {
                ReturnMenuCoverTransitor();
            }
#endif



            return;
        }

    }

    public void ReturnMenuCoverTransitor()
    {
        transitor.CoverTransition();

    }

    public void SaveDataAndReturnToMenu()
    {
        data.satoshis = _Satoshis;

        CloudOnce.Leaderboards.HighScore.SubmitScore(score);
        if (data.highScore < score)
            data.highScore = score;

        SaveSystem.SaveData(data);

        Time.timeScale = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void AddLife()
    {
        if (_Lifes == _maxLifes)
            return;

        _Lifes++;
        if (lifeDamaged)
        {
            lifeImagesList[_Lifes - 2].sprite = lifeActiveSprite;
            lifeDamaged = false;
        }
        lifeImagesList[_Lifes - 1].sprite = lifeActiveSprite;
        lifeImagesList[_Lifes - 1].enabled = true;
    }

    public void RestoreLifes()
    {
        _Lifes = 4;
    }

    public void OnSpecialAttackButtonClick()
    {
        SetScaleTimeZero();
        imgFilledSpecialAttack.fillAmount = 0;
        btnSpecialAttack.interactable = false;
        accumulateForSpecial = false;
        SoundEffectsManager.Play_Chidori();
    }

    public void SetScaleTimeZero()
    {
        Time.timeScale = 0;
    }

    public void ReactiveSpecialAttackBehaviour()
    {
        accumulateForSpecial = true;

    }
}
