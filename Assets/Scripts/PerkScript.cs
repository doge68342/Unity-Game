using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class PerkScript : MonoBehaviour
{
    public Image perkCard;
    private TMP_Text typeText;
    private TMP_Text descriptionText;
    private TMP_Text aquireText;
    public GameObject player;
    private PlayerScript playerScript;
    public WaveLogic waveLogic;
    public int perkFrequency;
    public int nextWaveTooOfferPerks;
    public void OfferPerks(int wave, int count)
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
        nextWaveTooOfferPerks += perkFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveLogic.wave == nextWaveTooOfferPerks)
        {
            OfferPerks(waveLogic.wave, 3);
            nextWaveTooOfferPerks += perkFrequency;
        }
    }
}
