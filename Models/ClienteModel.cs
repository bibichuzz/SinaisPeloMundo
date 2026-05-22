using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SinaisPeloMundo.Models
{
    public class ClienteModel
    {
        [Key]
        public int ClienteId { get; set; }

        public List<PedidoModel> Pedidos { get; set; } = new List<PedidoModel>();

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(60, ErrorMessage = "Nome deve ter no máximo 60 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
        public DateTime DtNascimento { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [MaxLength(11, ErrorMessage = "Telefone deve ter 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Telefone deve conter apenas números.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório.")]
        [MaxLength(11, ErrorMessage = "CPF deve ter 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        [MaxLength(60, ErrorMessage = "Endereço deve ter no máximo 60 caracteres.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [MaxLength(60, ErrorMessage = "E-mail deve ter no máximo 60 caracteres.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [MaxLength(255, ErrorMessage = "Senha muito grande.")]
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
