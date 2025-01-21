using TMPro;
using UnityEngine;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] private GameObject usernamePanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text usernameText;

    public void FillRandomInput()
    {
        string randomString = GenerateRandomName();

        inputField.text = randomString;
    }

    public void SaveInputToPlayerPrefs()
    {
        string nameToSave = !string.IsNullOrEmpty(inputField.text) ? inputField.text : GenerateRandomName();

        PlayerPrefs.SetString(GameManager.UserNameKey, nameToSave);
        Debug.Log($"Player name saved: {nameToSave}");

        usernamePanel.SetActive(false);
        usernameText.text = nameToSave;
    }

    public void EditUsername()
    {
        string currentUsername = PlayerPrefs.GetString(GameManager.UserNameKey);
        usernamePanel.SetActive(true);
        inputField.text = currentUsername;

    }

    public void LoadInputFromPlayerPrefs()
    {
        string savedName = PlayerPrefs.GetString(GameManager.UserNameKey, string.Empty);
        inputField.text = savedName;
        Debug.Log($"Player name loaded: {savedName}");
    }

    // Function to generate a random name with a 6-digit number
    public string GenerateRandomName()
    {
        string randomUsername = usernames[Random.Range(0, usernames.Length)];
        string randomDigits = Random.Range(0, 10000).ToString("D4");

        return randomUsername + randomDigits;
    }

    // List of usernames for name generation
    private string[] usernames =
        {
            "James", "Emma", "Liam", "Olivia", "Noah", "Ava", "Sophia", "Jackson", "Isabella", "Lucas", "Mia",
            "Ethan", "Amelia", "Mason", "Harper", "Logan", "Evelyn", "Oliver", "Abigail", "Aiden", "Diego",
            "Alejandra", "Sofia", "Carlos", "Pablo", "Luis", "Carmen", "Juan", "Gabriel", "Rafael", "Fernanda",
            "Emily", "William", "Sophie", "Daniel", "Grace", "Lily", "Michael", "Ella", "Benjamin", "Chloe",
            "Thomas", "Victoria", "Julia", "Lucas", "Manuel", "Joaquín", "Renata", "Rodrigo", "Isabela"
        };
}
