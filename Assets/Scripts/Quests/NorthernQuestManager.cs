using UnityEngine;
using UnityEngine.UI;

public class NorthernQuestManager : MonoBehaviour
{
    public enum QuestState
    {
        NotStarted,
        TotemsQuest,
        BabyDeerFound,
        WarmthQuest,
        WarmthCompleted,
        Completed
    }

    [Header("Ññûëêè")]
    public GameObject elder;
    public GameObject[] totems;
    public GameObject babyDeer;
    public GameObject campfire;
    public GameObject crystal;
    public Text questHintText;

    [Header("Ìèíè-èãğà")]
    public WarmMinigame warmMinigame;

    public QuestState currentState = QuestState.NotStarted;
    private int currentTotemIndex = 0;

    void Start()
    {
        if (crystal != null) crystal.SetActive(false);
        if (babyDeer != null) babyDeer.SetActive(false);
        if (campfire != null) campfire.SetActive(false);
        if (questHintText != null) questHintText.gameObject.SetActive(false);

        for (int i = 0; i < totems.Length; i++)
        {
            if (totems[i] != null)
                totems[i].SetActive(false);
        }
    }

    public void StartQuest()
    {
        currentState = QuestState.TotemsQuest;
        currentTotemIndex = 0;

        if (totems.Length > 0 && totems[0] != null)
            totems[0].SetActive(true);

        if (questHintText != null)
        {
            questHintText.gameObject.SetActive(true);
            questHintText.text = $"Èäè ê òîòåìó 1/{totems.Length} è íàæìè F";
        }
    }

    public void TotemActivated(int totemIndex)
    {
        if (currentState != QuestState.TotemsQuest) return;
        if (totemIndex != currentTotemIndex) return;

        if (totems[totemIndex] != null)
            totems[totemIndex].SetActive(false);

        currentTotemIndex++;

        if (currentTotemIndex >= totems.Length)
        {
            currentState = QuestState.BabyDeerFound;

            if (babyDeer != null)
                babyDeer.SetActive(true);

            if (questHintText != null)
                questHintText.text = "ÎËÅÍ¨ÍÎÊ ÍÀÉÄÅÍ! Íàæìè F ÷òîáû çàáğàòü åãî";
        }
        else
        {
            if (totems[currentTotemIndex] != null)
            {
                totems[currentTotemIndex].SetActive(true);

                if (questHintText != null)
                    questHintText.text = $"Èäè ê òîòåìó {currentTotemIndex + 1}/{totems.Length} è íàæìè F";
            }
        }
    }

    public void PickupBabyDeer()
    {
        if (currentState != QuestState.BabyDeerFound) return;

        if (babyDeer != null)
            babyDeer.SetActive(false);

        currentState = QuestState.WarmthQuest;

        if (campfire != null)
            campfire.SetActive(true);

        if (questHintText != null)
            questHintText.text = "ÂÅĞÍÈÑÜ Ê ÎËÅÍÅÂÎÄÓ";
    }

    public void NeedToWarm()
    {
        if (currentState != QuestState.WarmthQuest) return;

        if (questHintText != null)
            questHintText.text = "ÈÄÈ Ê ÊÎÑÒĞÓ È ÑÎÃĞÅÉ ÎËÅÍ¨ÍÊÀ (ÏĞÎÁÅË)";
    }

    public void StartWarmMinigame()
    {
        if (currentState != QuestState.WarmthQuest) return;

        if (warmMinigame != null)
            warmMinigame.StartMinigame();
    }

    public void WarmthGameWon()
    {
        if (currentState != QuestState.WarmthQuest) return;

        currentState = QuestState.WarmthCompleted;

        if (campfire != null)
            campfire.SetActive(false);

        if (questHintText != null)
            questHintText.text = "ÎËÅÍ¨ÍÎÊ ÑÎÃĞÅÒ! Ïîãîâîğè ñ îëåíåâîäîì";
    }

    public void FinalDialogue()
    {
        if (currentState != QuestState.WarmthCompleted) return;

        if (crystal != null)
            crystal.SetActive(true);

        currentState = QuestState.Completed;

        if (questHintText != null)
            questHintText.text = "ÇÀÁÅĞÈ ÊĞÈÑÒÀËË Ó ÎËÅÍÅÂÎÄÀ (F)";

        PlayerPrefs.SetInt("NorthernQuestDone", 1);
    }

    public QuestState GetState()
    {
        return currentState;
    }

    public bool IsQuestCompleted()
    {
        return currentState == QuestState.Completed;
    }
}