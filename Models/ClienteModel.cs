using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace SinaisPeloMundo.Models
{
    public class ClienteModel
    {
        [Key]
        public int ClienteId { get; set; }

        public List<PedidoModel> Pedidos { get; set; } = new List<PedidoModel>();

        [Required]
        [MaxLength(60)]
        public string Nome { get; set; }

        [Required]
        public DateTime DtNascimento { get; set; }

        [Required]
        [MaxLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Telefone deve ter 11 dígitos.")]
        public string Telefone { get; set; }

        [Required]
        [MaxLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve ter 11 dígitos.")]
        public string Cpf { get; set; }

        [Required]
        [MaxLength(60)]
        public string Endereco { get; set; }

        [Required]
        [MaxLength(60)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Senha { get; set; }

        public bool Admin { get; set; } = false;

        public void SetSenhaHash()
        {
            var passwordHasher = new PasswordHasher<ClienteModel>();

            Senha = passwordHasher.HashPassword(this, Senha);
        }

        public bool SenhaValida(string senhaDigitada)
        {
            var passwordHasher = new PasswordHasher<ClienteModel>();

            var resultado = passwordHasher.VerifyHashedPassword(
                this,
                Senha,
                senhaDigitada
            );

            return resultado == PasswordVerificationResult.Success;
        }
    }
}