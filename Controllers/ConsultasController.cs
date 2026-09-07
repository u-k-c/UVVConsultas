using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UVVConsultas.Data;
using UVVConsultas.Models;
using UVVConsultas.ViewModels;

namespace UVVConsultas.Controllers;

[Authorize]
public class ConsultasController : Controller
{
    private readonly AppDbContext _context;

    private static readonly string[] AreasDisponiveis =
    {
        "Cardiologia",
        "Dermatologia",
        "Endocrinologia",
        "Ginecologia",
        "Neurologia",
        "Ortopedia",
        "Pediatria",
        "Psiquiatria",
        "Clínica Geral"
    };

    private static readonly string[] TiposDisponiveis =
    {
        "Consulta inicial",
        "Consulta de retorno",
        "Avaliação inicial",
        "Acompanhamento",
        "Consulta de urgência",
        "Atendimento virtual"
    };

    private static readonly string[] MedicosDisponiveis =
    {
        "Dr. Marcos Silva",
        "Dra. Camila Ribeiro",
        "Dr. Rafael Costa",
        "Dra. Ana Souza",
        "Dr. Eduardo Martins",
        "Dra. Livia Nunes"
    };

    public ConsultasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var usuarioId = ObterUsuarioId();

        var consultas = await _context.Consultas
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.DataHora)
            .AsNoTracking()
            .ToListAsync();

        return View(consultas);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(CarregarModelo(new ConsultaViewModel()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ConsultaViewModel model)
    {
        model = CarregarModelo(model);

        if (!ModelState.IsValid)
            return View(model);

        if (!ValoresSelecionadosSaoValidos(model))
        {
            ModelState.AddModelError(string.Empty, "Selecione uma especialidade, tipo de atendimento e médico válidos.");
            return View(model);
        }

        var dataHora = ValidarEObterDataHora(model);
        if (dataHora is null)
        {
            ModelState.AddModelError(string.Empty, "A consulta deve estar no futuro e usar um horário entre 07:00 e 19:00, em intervalos de 30 minutos.");
            return View(model);
        }

        var usuarioId = ObterUsuarioId();
        var jaExiste = await _context.Consultas
            .AnyAsync(c => c.UsuarioId == usuarioId && c.DataHora == dataHora && c.Medico == model.Medico);

        if (jaExiste)
        {
            ModelState.AddModelError(string.Empty, "Já existe uma consulta agendada para esse médico e horário.");
            return View(model);
        }

        var consulta = new Consulta
        {
            Especialidade = model.Especialidade,
            TipoAtendimento = model.TipoAtendimento,
            Medico = model.Medico,
            DataHora = dataHora.Value,
            Descricao = model.Descricao,
            UsuarioId = usuarioId
        };

        _context.Consultas.Add(consulta);
        await _context.SaveChangesAsync();

        TempData["MensagemSucesso"] = "Consulta cadastrada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var usuarioId = ObterUsuarioId();
        var consulta = await _context.Consultas
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        if (consulta is null)
            return NotFound();

        var model = new ConsultaViewModel
        {
            Id = consulta.Id,
            Especialidade = consulta.Especialidade,
            TipoAtendimento = consulta.TipoAtendimento,
            Medico = consulta.Medico,
            DataConsulta = consulta.DataHora.Date,
            HoraConsulta = consulta.DataHora.ToString("HH:mm"),
            Descricao = consulta.Descricao
        };

        return View(CarregarModelo(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ConsultaViewModel model)
    {
        model = CarregarModelo(model);

        if (!ModelState.IsValid)
            return View(model);

        if (!ValoresSelecionadosSaoValidos(model))
        {
            ModelState.AddModelError(string.Empty, "Selecione uma especialidade, tipo de atendimento e médico válidos.");
            return View(model);
        }

        var dataHora = ValidarEObterDataHora(model);
        if (dataHora is null)
        {
            ModelState.AddModelError(string.Empty, "A consulta deve estar no futuro e usar um horário entre 07:00 e 19:00, em intervalos de 30 minutos.");
            return View(model);
        }

        var usuarioId = ObterUsuarioId();
        var consulta = await _context.Consultas
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        if (consulta is null)
            return NotFound();

        var jaExiste = await _context.Consultas
            .AnyAsync(c => c.UsuarioId == usuarioId && c.Id != id && c.DataHora == dataHora && c.Medico == model.Medico);

        if (jaExiste)
        {
            ModelState.AddModelError(string.Empty, "Já existe uma consulta agendada para esse médico e horário.");
            return View(model);
        }

        consulta.Especialidade = model.Especialidade;
        consulta.TipoAtendimento = model.TipoAtendimento;
        consulta.Medico = model.Medico;
        consulta.DataHora = dataHora.Value;
        consulta.Descricao = model.Descricao;

        _context.Update(consulta);
        await _context.SaveChangesAsync();

        TempData["MensagemSucesso"] = "Consulta atualizada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var usuarioId = ObterUsuarioId();
        var consulta = await _context.Consultas
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        if (consulta is null)
            return NotFound();

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();

        TempData["MensagemSucesso"] = "Consulta removida com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    private ConsultaViewModel CarregarModelo(ConsultaViewModel model)
    {
        model.AreasDisponiveis = AreasDisponiveis.Select(a => new SelectListItem(a, a));
        model.TiposDisponiveis = TiposDisponiveis.Select(t => new SelectListItem(t, t));
        model.MedicosDisponiveis = MedicosDisponiveis.Select(m => new SelectListItem(m, m));
        model.HorariosDisponiveis = GerarHorarios().Select(h => new SelectListItem(h, h));
        return model;
    }

    private static List<string> GerarHorarios()
    {
        var horarios = new List<string>();
        var inicio = TimeSpan.FromHours(7);
        var fim = TimeSpan.FromHours(19);

        for (var time = inicio; time < fim; time = time.Add(TimeSpan.FromMinutes(30)))
        {
            horarios.Add(time.ToString("hh\\:mm"));
        }

        return horarios;
    }

    private static DateTime? ValidarEObterDataHora(ConsultaViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.HoraConsulta))
            return null;

        if (!TimeSpan.TryParse(model.HoraConsulta, out var hora))
            return null;

        var inicio = TimeSpan.FromHours(7);
        var fim = TimeSpan.FromHours(19);

        if (hora < inicio || hora >= fim)
            return null;

        if (hora.Minutes % 30 != 0)
            return null;

        var data = model.DataConsulta.Date.Add(hora);
        return data > DateTime.Now ? data : null;
    }

    private static bool ValoresSelecionadosSaoValidos(ConsultaViewModel model)
    {
        return AreasDisponiveis.Contains(model.Especialidade)
            && TiposDisponiveis.Contains(model.TipoAtendimento)
            && MedicosDisponiveis.Contains(model.Medico);
    }

    private int ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim is not null && int.TryParse(claim.Value, out var usuarioId) ? usuarioId : 0;
    }
}
