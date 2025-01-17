using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour {
    public static AchievementManager Instance;
    public List<Achievement> achievements = new List<Achievement>();
    private int achievementCount;

    //telefono
    public GameObject smartphoneCanvas;

    //Card contenitore REMIND del telefono
    public GameObject achievementCardPrefab;
    public Transform achievementCardParent;
    private int cardCount;

    //PopUp achievement raggiunto
    public GameObject PopUpCanvas;

    //Score update
    public ScoreManager scoreManager;

    private void Awake() {
        Instance = this;
        LoadAchievements();
        PopUpCanvas.SetActive(false);

        scoreManager = FindFirstObjectByType<ScoreManager>();
        cardCount = 0;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            ToggleSmartphone();
        }
    }

    void ToggleSmartphone() {
        bool isActive = smartphoneCanvas.gameObject.activeSelf;
        smartphoneCanvas.gameObject.SetActive(!isActive);
    }

    void LoadAchievements() {
        string path = Application.dataPath + "/Achievements.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            AchievementListWrapper wrapper = JsonUtility.FromJson<AchievementListWrapper>(json);
            achievements = new List<Achievement>(wrapper.achievements);
            achievementCount = achievements.Count;
        }
        else {
            Debug.LogError("File Achievements.json non trovato!");
        }
    }

    private class AchievementListWrapper {
        public Achievement[] achievements;
    }

    public void IncrementAchievement(string targetObject, int amount = 1) {
        Debug.Log(targetObject);
        Achievement achievement = achievements.Find(a => a.targetObject == targetObject);
        if (achievement != null) {
            achievement.IncrementProgress(amount);
            scoreManager.UpdateScore(achievement.taskScore);
            if (achievement.taskProgress == 1) {
                CreateAchievementCard(achievement);
                ShowAchievementPopup(achievement);
            }
            else if (achievement.taskProgress == achievement.taskGoal){
                CheckAchievementCount();
            }
        }
        else {
            Debug.LogWarning("Problema di lettura e/o achievement non inserito");
        }
    }

    void CreateAchievementCard(Achievement achievement) {
        if (achievement == null) {
            Debug.LogError("Achievement è null!");
            return;
        }
        GameObject card = Instantiate(achievementCardPrefab, achievementCardParent);

        TMP_Text nameText = card.transform.Find("NameTask").GetComponent<TMP_Text>();
        TMP_Text descriptionText = card.transform.Find("DescriptionTask").GetComponent<TMP_Text>();

        nameText.text = achievement.taskName;
        descriptionText.text = achievement.taskDescription;

        cardCount++;
    }

    void ShowAchievementPopup(Achievement achievement) {
        var titleText = PopUpCanvas.transform.Find("PanelPopUp/titleText").GetComponent<Text>();
        var descriptionText = PopUpCanvas.transform.Find("PanelPopUp/descriptionText").GetComponent<Text>();

        descriptionText.text = "Nuovo Achievement Sbloccato!";
        titleText.text = achievement.taskName;

        PopUpCanvas.SetActive(true);
        Invoke("DisableCanvas", 3f);
    }

    private void DisableCanvas() {
        PopUpCanvas.SetActive(false);
    }

    public void CheckAchievementCount(){
        if (achievementCount == cardCount){
            // TODO: Passa a schermata di vittoria
        }
    }
}