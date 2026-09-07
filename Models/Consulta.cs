using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UVVConsultas.Models;

public class Consulta
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A área da consulta é obrigatória.")]
    [StringLength(100, ErrorMessage = "A área deve ter no máximo 100 caracteres.")]
    public string Especialidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de atendimento é obrigatório.")]
    [StringLength(80, ErrorMessage = "O tipo deve ter no máximo 80 caracteres.")]
    public string TipoAtendimento { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome do médico é obrigatório.")]
    [StringLength(120, ErrorMessage = "O nome do médico deve ter no máximo 120 caracteres.")]
    public string Medico { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data e hora são obrigatórias.")]
    [DataType(DataType.DateTime)]
    public DateTime DataHora { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public int UsuarioId { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public Usuario? Usuario { get; set; }
}
