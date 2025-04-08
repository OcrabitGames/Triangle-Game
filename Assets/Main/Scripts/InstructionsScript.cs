using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsScript : MonoBehaviour
{
    public Button instructionButton;
    public TextMeshProUGUI instructionText;
    public GameObject instructionSprite;

    private string[] instructions = new string[]
    {
        "Instructions: \n\nThere is an invader in the crowd. \nWe need to capture them!",
        "Instructions: \n\nClick one of the foxes and click a spot on the map to place them.",
        "Instructions: \n\nClick space to make them follow the intruder!",
        "Instructions: \n\nForm a triangle around the intruder to capture them!",
    };

    private int currentInstructionIndex = 0;

    void Start()
    {
        if (instructionButton != null)
        {
            instructionButton.onClick.AddListener(NextInstruction);
        }

        if (instructionText != null)
        {
            instructionText.SetText(instructions[currentInstructionIndex]);
        }

        if (instructionSprite != null)
        {
            instructionSprite.SetActive(false);
        }
    }

    void NextInstruction()
    {
        if (instructions.Length == 0 || instructionText == null)
            return;

        currentInstructionIndex++;

        if (currentInstructionIndex >= instructions.Length)
        {
            gameObject.SetActive(false);
            return;
        }

        instructionText.SetText(instructions[currentInstructionIndex]);

        if (instructionSprite != null && currentInstructionIndex == 1)
        {
            instructionSprite.SetActive(true);
        } else if (instructionSprite != null){
            instructionSprite.SetActive(false);
        }
    }
}
