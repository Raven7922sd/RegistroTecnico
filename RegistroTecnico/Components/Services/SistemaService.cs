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

    public async Task<(bool Exito, string? Mensaje)> Eliminar(int sistemaId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();

        bool estaEnUso = await context.VentasDetalles.AnyAsync(d => d.SistemaId == sistemaId);
        if (estaEnUso)
            return (false, "No se puede eliminar el sistema porque está en uso en una venta.");

        var sistema = await context.Sistemas.FindAsync(sistemaId);
        if (sistema == null)
            return (false, "Sistema no encontrado.");

        context.Sistemas.Remove(sistema);
        await context.SaveChangesAsync();

        return (true, null);
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

        if (filtroCampo == "SistemaId" && !string.IsNullOrWhiteSpace(valorFiltro) && int.TryParse(valorFiltro, out var sistemaId))
        {
            filtro = filtro.AndAlso(s => s.SistemaId == sistemaId);
        }
        else if (filtroCampo == "Descripcion" && !string.IsNullOrWhiteSpace(valorFiltro))
        {
            filtro = filtro.AndAlso(s => s.Descripcion.ToLower().Contains(valorFiltro.ToLower()));
        }
        else if (filtroCampo is "Baja" or "Media" or "Alta")
        {
            filtro = filtro.AndAlso(t => t.Complejidad.ToLower() == filtroCampo.ToLower());
        }
        else if (filtroCampo == "Coste" && double.TryParse(valorFiltro, out var monto))
            filtro = filtro.AndAlso(m => m.Coste == monto);

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

    public async Task<bool> RestarExistenciaAsync(int sistemaId, int cantidadVendida)
    {
        await using var context = await DbFactory.CreateDbContextAsync();

        var sistema = await context.Sistemas.FirstOrDefaultAsync(s => s.SistemaId == sistemaId);

        if (sistema == null)
            return false;

        if (sistema.Existencia < cantidadVendida)
            return false;

        sistema.Existencia -= cantidadVendida;

        context.Sistemas.Update(sistema);
        return await context.SaveChangesAsync() > 0;
    }
    public async Task<bool> RestaurarExistenciaAsync(int sistemaId, int cantidad)
    {
        await using var ctx = await DbFactory.CreateDbContextAsync();
        var sistema = await ctx.Sistemas.FindAsync(sistemaId);
        if (sistema == null) return false;
        sistema.Existencia += cantidad;
        await ctx.SaveChangesAsync();
        return true;
    }
}