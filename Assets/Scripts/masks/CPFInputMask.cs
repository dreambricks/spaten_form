using UnityEngine;
using UnityEngine.UI;

public class CPFInputMask : MonoBehaviour
{
    public InputField cpfInputField;

    void Start()
    {
        cpfInputField.onValueChanged.AddListener(FormatCPFInput);
    }

    private void FormatCPFInput(string input)
    {
        input = new string(System.Text.RegularExpressions.Regex.Replace(input, "[^0-9]", "").ToCharArray());

        if (input.Length > 11)
        {
            input = input.Substring(0, 11);
        }

        if (input.Length > 3)
        {
            input = input.Insert(3, ".");
        }
        if (input.Length > 7)
        {
            input = input.Insert(7, ".");
        }
        if (input.Length > 11)
        {
            input = input.Insert(11, "-");
        }

        cpfInputField.text = input;

        cpfInputField.caretPosition = input.Length;
    }

    void OnDestroy()
    {
        cpfInputField.onValueChanged.RemoveListener(FormatCPFInput);
    }
}
