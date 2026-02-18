using UnityEngine;

public class RESETSALLL : MonoBehaviour
{
    void Start()
    {
        // Сбрасываем всё нахуй
        PlayerPrefs.DeleteAll();
        Debug.Log("?? ВЕСЬ ПРОГРЕСС СБРОШЕН!");

        // Удаляем этот объект после сброса
        Destroy(gameObject);
    }
}