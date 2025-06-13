using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Components.DAL;
using RegistroTecnico.Components.Models;
using RegistroTecnico.Components.Models.Paginacion;

namespace RegistroTecnico.Components.Services;

public class SistemaService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guadar(Sistemas sistema)
    {
        if (!await ExisteSistema(sistema.SistemaId))
        {
            return await Insertar(sistema);
        }
        else { return await ModificarSistema(sistema); }
    }

    public async Task<bool> ExisteSistema(int SistemaId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Sistemas.AnyAsync(s => s.SistemaId == SistemaId);
    }

    public async Task<bool> Insertar(Sistemas sistema)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        context.Add(sistema);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ModificarSistema(Sistemas sistemas)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        context.Sistemas.Update(sistemas);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int SistemaId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Sistemas.AsNoTracking().Where(s => s.SistemaId == SistemaId).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Sistemas>> ListarSistemas(Expression<Func<Sistemas, bool>> criterio)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Sistemas.Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<Sistemas?> Buscar(int SistemaId)
    {
        await using var context=await DbFactory.CreateDbContextAsync();
        return await context.Sistemas.FirstOrDefaultAsync(s => s.SistemaId==SistemaId);
    }

    public async Task<PaginacionResultado<Sistemas>> BuscarSistemasAsync(
      string filtroCampo,
      string valorFiltro,
      DateTime? fechaDesde,
      DateTime? fechaHasta,
      int pagina,
      int tamanioPagina)
    {
        Expression<Func<Sistemas, bool>> filtro = s => true;

        if (filtroCampo == "SistemaId" && int.TryParse(valorFiltro, out var sistemaId))
            filtro = filtro.AndAlso(s => s.SistemaId == sistemaId);
        else if (filtroCampo == "Descripcion")
            filtro = filtro.AndAlso(s => s.Descripcion.ToLower().Contains(valorFiltro.ToLower()));
        else if (filtroCampo == "Complejidad" && int.TryParse(valorFiltro, out var complejidad))
            filtro = filtro.AndAlso(s => s.Complejidad == complejidad);

        if (fechaDesde.HasValue)
            filtro = filtro.AndAlso(s => s.Fecha >= DateOnly.FromDateTime(fechaDesde.Value));

        if (fechaHasta.HasValue)
            filtro = filtro.AndAlso(s => s.Fecha <= DateOnly.FromDateTime(fechaHasta.Value));

        await using var context = await DbFactory.CreateDbContextAsync();

        var totalRegistros = await context.Sistemas.CountAsync(filtro);
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanioPagina);

        var sistemas = await context.Sistemas
            .Where(filtro)
            .OrderBy(s => s.SistemaId)
            .Skip((pagina - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .AsNoTracking()
            .ToListAsync();

        return new PaginacionResultado<Sistemas>
        {
            Items = sistemas,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas
        };
    }
}
