using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UVVConsultas.ViewModels;

public class ConsultaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A área da consulta é obrigatória.")]
    [StringLength(100, ErrorMessage = "A área deve ter no máximo 100 caracteres.")]
    [Display(Name = "Área da consulta")]
    public string Especialidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de atendimento é obrigatório.")]
    [StringLength(80, ErrorMessage = "O tipo deve ter no máximo 80 caracteres.")]
    [Display(Name = "Tipo de atendimento")]
    public string TipoAtendimento { get; set; } = string.Empty;

    [Required(ErrorMessage = "O médico é obrigatório.")]
    [StringLength(120, ErrorMessage = "O nome do médico deve ter no máximo 120 caracteres.")]
    [Display(Name = "Médico")]
    public string Medico { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data da consulta é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data")]
    public DateTime DataConsulta { get; set; } = DateTime.Today.AddDays(1);

    [Required(ErrorMessage = "A hora da consulta é obrigatória.")]
    [Display(Name = "Hora")]
    public string HoraConsulta { get; set; } = "09:00";

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    public IEnumerable<SelectListItem> AreasDisponiveis { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> TiposDisponiveis { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> MedicosDisponiveis { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> HorariosDisponiveis { get; set; } = new List<SelectListItem>();
}
