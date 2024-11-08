using UnityEngine;
using UnityEngine.UI;

public class PhoneInputMask : MonoBehaviour
{
    public InputField phoneInputField;

    void Start()
    {
        phoneInputField.onValueChanged.AddListener(FormatPhoneInput);
    }

    private void FormatPhoneInput(string input)
    {
        input = new string(System.Text.RegularExpressions.Regex.Replace(input, "[^0-9]", "").ToCharArray());

        if (input.Length > 11)
        {
            input = input.Substring(0, 11);
        }

        if (input.Length > 0)
        {
            input = "(" + input;
        }
        if (input.Length > 3)
        {
            input = input.Insert(3, ") ");
        }
        if (input.Length > 9)
        {
            input = input.Insert(10, "-");
        }

        phoneInputField.text = input;

        phoneInputField.caretPosition = input.Length;
    }

    void OnDestroy()
    {
        phoneInputField.onValueChanged.RemoveListener(FormatPhoneInput);
    }
}
