using UnityEngine;
using UnityEngine.UI;

public class CaucasusQuestManager : MonoBehaviour
{
    [Header("Ссылки")]
    public GameObject girl;              
    public GameObject elder;             
    public GameObject cup;                
    public GameObject crystal;            
    public Text questHintText;            

    [Header("Мост")]
    public Transform[] bridgePlanks;      
    public Transform respawnPoint;        
    public GameObject deathPlane;          

    
    public bool questStarted = false;
    public bool hasCup = false;
    public bool cupDelivered = false;
    public bool questCompleted = false;

    public Transform cupSpawnPoint;

    void Start()
    {
        if (crystal != null) crystal.SetActive(false);

        if (questHintText != null) questHintText.gameObject.SetActive(false);
        if (cup != null)
        {
            Renderer cupRenderer = cup.GetComponent<Renderer>();
            if (cupRenderer != null)
                cupRenderer.enabled = false;
        }
        
        if (bridgePlanks.Length > 0)
        {
            
            for (int i = 1; i < bridgePlanks.Length; i += 2)
            {
                Collider col = bridgePlanks[i].GetComponent<Collider>();
                if (col != null)
                    col.enabled = false;

                
                Renderer rend = bridgePlanks[i].GetComponent<Renderer>();
                if (rend != null)
                    rend.material.color = Color.gray;
            }
        }
    }

    
    public void StartQuest()
    {
        

        questStarted = true;
        hasCup = false;
        cupDelivered = false;

        
        if (cup != null)
        {
            if (cupSpawnPoint != null)
            {
                cup.transform.position = cupSpawnPoint.position;
                cup.transform.rotation = cupSpawnPoint.rotation;
            }

            Renderer cupRenderer = cup.GetComponent<Renderer>();
            if (cupRenderer != null)
            {
                cupRenderer.enabled = true;
                
            }

            Collider cupCollider = cup.GetComponent<Collider>();
            if (cupCollider != null)
                cupCollider.enabled = true;
        }

        if (questHintText != null)
        {
            questHintText.gameObject.SetActive(true);
            questHintText.text = "ШАГ 1: Возьми чашу у башни (F)";
        }
    }

    
    public void TakeCup()
    {

        hasCup = true;

        if (cup != null)
        {
            
            Collider cupCollider = cup.GetComponent<Collider>();
            if (cupCollider != null)
                cupCollider.enabled = false;

 
            cup.transform.SetParent(Camera.main.transform);
            cup.transform.localPosition = new Vector3(0.5f, -0.5f, 1f);
            cup.transform.localRotation = Quaternion.identity;


        }

        if (questHintText != null)
            questHintText.text = "ШАГ 2: Донеси чашу старейшине через мост!\nОсторожно, доски сломаны!";
    }


    public void PlayerFell()
    {
        
        if (cup != null)
        {
            
            cup.transform.SetParent(null);

            
            if (cupSpawnPoint != null)
            {
                cup.transform.position = cupSpawnPoint.position;
                cup.transform.rotation = cupSpawnPoint.rotation;

            }

            
            Renderer cupRenderer = cup.GetComponent<Renderer>();
            if (cupRenderer != null)
            {
                cupRenderer.enabled = true;
            }

            Collider cupCollider = cup.GetComponent<Collider>();
            if (cupCollider != null)
                cupCollider.enabled = true;
        }

        hasCup = false;

        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && respawnPoint != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;
            if (cc != null) cc.enabled = true;
            Debug.Log($"Игрок телепортирован к мосту: {respawnPoint.position}");
        }

        if (questHintText != null)
            questHintText.text = "Ты упал... Чаша вернулась к башне. Возьми её снова (F)";
    }

    
    public void DeliverCup()
    {
        if (!questStarted || questCompleted) return;
        if (!hasCup) return;

        cupDelivered = true;

        
        if (cup != null)
        {
            cup.SetActive(false);
            cup.transform.SetParent(null);
        }

        if (questHintText != null)
            questHintText.text = "ШАГ 3: Поговори со старейшиной";


    }

    
    public void CompleteQuest()
    {
        questCompleted = true;

        if (crystal != null)
            crystal.SetActive(true);

        if (questHintText != null)
            questHintText.text = "ЗАДАНИЕ ВЫПОЛНЕНО! Забери кристалл у старейшины";

        PlayerPrefs.SetInt("CaucasusQuestDone", 1);

        
        CaucasusGirlDialogue girlDialogue = FindObjectOfType<CaucasusGirlDialogue>();
        if (girlDialogue != null)
            girlDialogue.OnQuestCompleted();

    }

    public bool IsQuestCompleted()
    {
        return questCompleted;
    }

    public bool HasCup()
    {
        return hasCup;
    }
}