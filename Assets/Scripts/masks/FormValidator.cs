using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class FormValidator : MonoBehaviour
{
    public InputField nomeInput;
    public InputField emailInput;
    public InputField cpfInput;
    public InputField telefoneInput;

    public Text nomeErrorText;
    public Text emailErrorText;
    public Text cpfErrorText;
    public Text telefoneErrorText;

    public Button confirmar;

    private void Start()
    {
        confirmar.onClick.AddListener(() => ValidateForm());

        nomeInput.onValueChanged.AddListener(delegate { ClearErrorText(nomeErrorText); });
        emailInput.onValueChanged.AddListener(delegate { ClearErrorText(emailErrorText); });
        cpfInput.onValueChanged.AddListener(delegate { ClearErrorText(cpfErrorText); });
        telefoneInput.onValueChanged.AddListener(delegate { ClearErrorText(telefoneErrorText); });
    }

    public void ValidateForm()
    {
        // Validação do Nome
        if (string.IsNullOrEmpty(nomeInput.text) || nomeInput.text.Length < 3)
        {
            nomeErrorText.text = "Nome inválido.";
        }
        else
        {
            nomeErrorText.text = "";
        }

        // Validação do Email
        if (string.IsNullOrEmpty(emailInput.text) || !IsValidEmail(emailInput.text))
        {
            emailErrorText.text = "Email inválido.";
        }
        else
        {
            emailErrorText.text = "";
        }

        // Validação do CPF (com máscara)
        if (string.IsNullOrEmpty(cpfInput.text) || !IsValidCPF(cpfInput.text))
        {
            cpfErrorText.text = "CPF inválido.";
        }
        else
        {
            cpfErrorText.text = "";
        }

        // Validação do Telefone (com máscara)
        if (string.IsNullOrEmpty(telefoneInput.text) || !IsValidTelefone(telefoneInput.text))
        {
            telefoneErrorText.text = "Telefone inválido.";
        }
        else
        {
            telefoneErrorText.text = "";
        }
    }

    private bool IsValidEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    private bool IsValidCPF(string cpf)
    {
        string cpfPattern = @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$";
        return Regex.IsMatch(cpf, cpfPattern) && IsCpfValidLogic(cpf);
    }

    private bool IsCpfValidLogic(string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11 || new string(cpf[0], 11) == cpf)
            return false;

        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(tempCpf[i].ToString()) * mult1[i];
        }

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        if (remainder != int.Parse(cpf[9].ToString()))
            return false;

        tempCpf += remainder;
        sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(tempCpf[i].ToString()) * mult2[i];
        }

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        return remainder == int.Parse(cpf[10].ToString());
    }

    private bool IsValidTelefone(string telefone)
    {
        string telefonePattern = @"^\(\d{2}\) \d{5}-\d{4}$";
        return Regex.IsMatch(telefone, telefonePattern);
    }

    private void ClearErrorText(Text errorText)
    {
        errorText.text = "";
    }
}
