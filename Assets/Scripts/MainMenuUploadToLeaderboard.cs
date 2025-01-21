using Dan.Main;
using UnityEngine;

public class MainMenuUploadToLeaderboard : MonoBehaviour
{
    private readonly string publicLeaderboardKey = "4aee29268c5d1bc973f1c78f2421f4df9dc0d52cef8ab441303da1543f5133cc";

    public void SetLeaderboardEntry(string username, int score, string extra)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, extra);

        //LeaderboardCreator.ResetPlayer();
    }
}
