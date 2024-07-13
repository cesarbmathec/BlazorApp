using System.ComponentModel.DataAnnotations;

public class Paciente
{
    public int id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
    public DateOnly fecha_nacimiento { get; set; }

    [Required(ErrorMessage = "El género es obligatorio.")]
    public string genero { get; set; } = string.Empty;

    public string direccion { get; set; } = string.Empty;

    public string telefono { get; set; } = string.Empty;

    public string cedula_identidad { get; set; } = string.Empty;

    [CustomValidation(typeof(Paciente), nameof(ValidateEmail))]
    public string correo_electronico { get; set; } = string.Empty;
    public static ValidationResult? ValidateEmail(string correo_electronico, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(correo_electronico) || new EmailAddressAttribute().IsValid(correo_electronico))
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Formato de correo electrónico inválido.");
        }
    }
}