using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{
    [SerializeField] private string inputField;

    // List of usernames for name generation
    private string[] usernames = 
        { 
            "James", "Emma", "Liam", "Olivia", "Noah", "Ava", "Sophia", "Jackson", "Isabella", "Lucas", "Mia",
            "Ethan", "Amelia", "Mason", "Harper", "Logan", "Evelyn", "Oliver", "Abigail", "Aiden", "Diego", 
            "Alejandra", "Sofia", "Carlos", "Pablo", "Luis", "Carmen", "Juan", "Gabriel", "Rafael", "Fernanda", 
            "Emily", "William", "Sophie", "Daniel", "Grace", "Lily", "Michael", "Ella", "Benjamin", "Chloe", 
            "Thomas", "Victoria", "Julia", "Lucas", "Manuel", "Joaquín", "Renata", "Rodrigo", "Isabela"
        };

    // Function to generate a random name with a 6-digit number
    public string GenerateRandomName()
    {
        string randomUsername = usernames[Random.Range(0, usernames.Length)];
        string randomDigits = Random.Range(0, 1000000).ToString("D6");

        return randomUsername + randomDigits;
    }
}
