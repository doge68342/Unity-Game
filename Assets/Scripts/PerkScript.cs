// using System.Numerics;
// using System.Runtime.CompilerServices;
// using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PerkScript : MonoBehaviour
{
    public Canvas canvas;
    public Image perkCard;
    public GameObject player;
    private PlayerScript playerScript;
    public WaveLogic waveLogic;
    public int perkFrequency;
    public int nextWaveTooOfferPerks;
    public int perksToOffer;
    private List<PerkData> perks = new List<PerkData>();
    public List<Image> perkCards = new List<Image>();
    private bool offerPerksDebounce;
    public TimeSlowScript timeSlowScript;

    public int NameToId(string name)
    {
        foreach (PerkData perk in perks)
        {
            if (perk.name == name)
            {
                return perk.id;
            }
        }
        return 0;
    }

    public class PerkData
    {
        public int id;
        public float increment;
        public string name;
        public string description;
        public float hue;

        public PerkData(int id, float increment, string name, string description, float hue)
        {
            this.id = id;
            this.increment = increment;
            this.name = name;
            this.description = description;
            this.hue = hue;
        }
    }

    public void newPerkCard(int type, float power, Vector2 position, Image perkCard)
    {
        Cursor.lockState = CursorLockMode.None;
        Image newPerkCard = Instantiate(perkCard);
        perkCards.Add(newPerkCard);
        newPerkCard.transform.position = position;
        newPerkCard.transform.SetParent(canvas.transform, false);
        newPerkCard.color = Color.HSVToRGB(perks[type].hue, 0.33f, 1f);

        TMP_Text[] textComponents = newPerkCard.GetComponentsInChildren<TMP_Text>();
        textComponents[0].text = perks[type].name;
        textComponents[1].text = perks[type].description;
        float saturation = 0.33f;
        textComponents[0].colorGradient = new VertexGradient(Color.white, Color.white, Color.HSVToRGB(perks[type].hue, saturation, 1f), Color.HSVToRGB(perks[type].hue, saturation, 1f));
        textComponents[1].colorGradient = new VertexGradient(Color.white, Color.white, Color.HSVToRGB(perks[type].hue, saturation, 1f), Color.HSVToRGB(perks[type].hue, saturation, 1f));
        textComponents[2].colorGradient = new VertexGradient(Color.white, Color.white, Color.HSVToRGB(perks[type].hue, saturation, 1f), Color.HSVToRGB(perks[type].hue, saturation, 1f));

        textComponents[2].GetComponent<Button>().onClick.AddListener(() =>
        {

            playerScript.changeStat(type, power * perks[type].increment);
            playerScript.heal();

            foreach(Image perkCard in perkCards)
            {
                Destroy(perkCard.gameObject);
            }
            perkCards.Clear();
            timeSlowScript.SlowTime(1);
            Cursor.lockState = CursorLockMode.Locked;
        });

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();

        nextWaveTooOfferPerks = perkFrequency;

        //-------- New Perks Here --------\\
        perks.Add(new PerkData(0, 0.25f, "damage", "Increases damage dealt per second", 0f / 360f));
        perks.Add(new PerkData(1, 5f,"dash power", "Increases the force of the dash", 175f / 360f));
        perks.Add(new PerkData(2, 25f,"health", "Increases maximum health", 305f / 360f));

    }

    // Update is called once per frame
    void Update()
    {
        if (waveLogic.wave == nextWaveTooOfferPerks && !offerPerksDebounce)
        {
            offerPerksDebounce = true;
            nextWaveTooOfferPerks += perkFrequency;
            timeSlowScript.SlowTime(0.2f);
            for (int i = 0; i < perksToOffer; i++)
            {
                newPerkCard(Mathf.FloorToInt(UnityEngine.Random.value * perks.Count), 1, new Vector2((i - 1) * 350, 0), perkCard);
            }
        }
        if (waveLogic.wave >= nextWaveTooOfferPerks)
        {
            offerPerksDebounce = false;
        }
    }

}
