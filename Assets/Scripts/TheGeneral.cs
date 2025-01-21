using TMPro;
using UnityEngine;

public class TheGeneral : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalPanelGreetingsText;
    [SerializeField] private TextMeshProUGUI finalPanelFeedbackText;

    public void MessageFromTheGeneral()
    {
        SetGreetings();
        SetGeneralFeedback();
    }

    private void SetGreetings()
    {
        int playerRank = GameManager.playerRank;

        switch (playerRank)
        {
            case 1:
                finalPanelGreetingsText.text = "Lieutenant Rodgers,";
                break;

            case 2:
                finalPanelGreetingsText.text = "Lieutenant Rodgers,";
                break;

            case 3:
                finalPanelGreetingsText.text = "Captain Rodgers,";
                break;

            case 4:
                finalPanelGreetingsText.text = "Major Rodgers,";
                break;

            case 5:
                finalPanelGreetingsText.text = "Lieutenant Colonel Rodgers,";
                break;

            case 6:
                finalPanelGreetingsText.text = "Colonel Rodgers,";
                break;
        }
    }

    private void SetGeneralFeedback()
    {
        int randomInt = Random.Range(1, 6);

        if (GameManager.totalEnemiesDown < 40)
        {
            switch (randomInt)
            {
                case 1:
                    finalPanelFeedbackText.text = "In the heat of battle, setbacks are a part of the journey, " +
                        "and even the best pilots face challenges. Regroup, strategize, and emerge stronger.";
                    break;

                case 2:
                    finalPanelFeedbackText.text = "Facing impossible odds is a pilot's reality, but you have to remember " +
                        "that courage is forged in adversity. Keep pushing forward with resilience and determination.";
                    break;

                case 3:
                    finalPanelFeedbackText.text = "In moments of difficulty, perseverance becomes your greatest ally. Stay resolute, " +
                        "navigate through challenges, and victory will be within the reach of your wings.";
                    break;

                case 4:
                    finalPanelFeedbackText.text = "Even in the face of adversity, dare to be bold and courageous. " +
                        "Embrace your inner eagle, for it is the emblem of true resilience in the face of aerial challenges.";
                    break;

                case 5:
                    finalPanelFeedbackText.text = "Failures are the seeds of future triumphs. Rise from the ashes, " +
                        "sharpen your wings, and soar again.";
                    break;
            }
        }
        else if (GameManager.totalEnemiesDown >= 40 && GameManager.totalEnemiesDown < 130)
        {
            switch (randomInt)
            {
                case 1:
                    finalPanelFeedbackText.text = "Your efforts have been commendable, but remember, triumph is born from meticulous " +
                        "preparation. Hone your skills, plan your tactics, and strive for greatness.";
                    break;

                case 2:
                    finalPanelFeedbackText.text = "While progress may seem steady, remember that victory often lies in strategy and " +
                        "finesse. Explore alternative approaches, and you may find a higher success.";
                    break;

                case 3:
                    finalPanelFeedbackText.text = "If you believe you achieved only a modest progress, remember that success is a " +
                        "journey, not a destination. Stay resilient, learn from setbacks, and continue your pursuit of greatness.";
                    break;

                case 4:
                    finalPanelFeedbackText.text = "Your dedication shines, but like a soaring jet, success demands calculated navigation. " +
                        "Fine-tune your flight plan, and you will elevate your performance to new heights.";
                    break;

                case 5:
                    finalPanelFeedbackText.text = "Every flight step is a stride toward supremacy. Embrace the rhythm of the air, " +
                        "assimilate your experiences, and persist in your ascent to greatness.";
                    break;
            }
        }
        else
        {
            switch (randomInt)
            {
                case 1:
                    finalPanelFeedbackText.text = "You have demonstrated exceptional skill and bravery in the air. Your leadership shines " +
                        "bright, inspiring those around you. Continue to lead with courage and conviction.";
                    break;

                case 2:
                    finalPanelFeedbackText.text = "Your exceptional performance reflects a deep dedication and passion for excellence. " +
                        "Continue inspiring us, propelled by your loyalty to our mission and comrades in the skies.";
                    break;

                case 3:
                    finalPanelFeedbackText.text = "Your outstanding achievements amidst adversity demonstrate a keen ability to seize " +
                        "opportunities where others see chaos. Your success transcends through the air.";
                    break;

                case 4:
                    finalPanelFeedbackText.text = "Your remarkable success is a testament to your unwavering commitment to excellence. " +
                        "Keep striving for greatness with intention, effort, and intelligent strategy.";
                    break;

                case 5:
                    finalPanelFeedbackText.text = "Your stellar accomplishments in the skies speak volumes about your dedication and " +
                        "expertise. Continue to soar, inspiring others with your remarkable skill and valor.";
                    break;
            }
        }
    }
}
