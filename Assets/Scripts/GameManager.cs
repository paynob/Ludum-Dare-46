using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canary canary;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    int targetDiamonds = 10;

    [SerializeField]
    float secondsToGetDiamonds = 60f;

    [SerializeField]
    Slider timeSlider, playerOxigen, canaryOxigen;

    [SerializeField]
    TextMeshProUGUI timeText, diamondsText;

    public GameObject gameOverPanel;

    private float remainingTime;
    private int currentDiamonds;
    private Image timeSliderFillArea;


    private void Start()
    {
        remainingTime = secondsToGetDiamonds;
        timeSlider.maxValue = secondsToGetDiamonds;
        timeSliderFillArea = timeSlider.fillRect.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        if( remainingTime < 0 )
            remainingTime = 0;

        timeSlider.value = remainingTime;
        float h, s, v;
        Color.RGBToHSV( timeSliderFillArea.color, out h, out s, out v );
        h = Mathf.Lerp( 0, 1 / 3f, remainingTime / secondsToGetDiamonds );
        timeSliderFillArea.color = Color.HSVToRGB( h, s, v );
        timeText.text = $"{Mathf.RoundToInt( remainingTime ) / 60}:{Mathf.RoundToInt(remainingTime)%60}";

        playerOxigen.value = player.oxigen;
        canaryOxigen.value = canary.oxigen;

    }

    public void CollectDiamond()
    {
        currentDiamonds++;
        diamondsText.text = currentDiamonds.ToString();
    }

    public void Die() {
        gameOverPanel.GetComponent<Animator>().Play("Base Layer.GameOver");
    }
}

