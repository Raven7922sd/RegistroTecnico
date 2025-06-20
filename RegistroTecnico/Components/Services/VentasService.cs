using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Components.DAL;
using RegistroTecnico.Components.Models.Paginacion;
using RegistroTecnico.Components.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace RegistroTecnico.Components.Services;

public class VentasService(IDbContextFactory<Contexto>DbFactory)
{

    public async Task<bool> Guardar(Ventas ventas)
    {
        if (!await ExisteId(ventas.VentaId))
        { return (await Insertar(ventas)); }

        else { return await Modificar(ventas); }
    }

    public async Task<bool> ExisteId(int VentaId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Ventas.AnyAsync(v=>v.VentaId == VentaId);
    }

    public async Task<bool> Insertar(Ventas ventas)
    {
        using var context = await DbFactory.CreateDbContextAsync();
        context.Ventas.Add(ventas);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Ventas ventas)
    {
        using var context = await DbFactory.CreateDbContextAsync();
        ventas.Cliente = null;
        ventas.VentasDetalles.FirstOrDefault().Sistema = null;

        context.Ventas.Update(ventas);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Ventas?> BuscarVentas(int Ventaid)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Ventas
            .Include(c => c.Cliente)
            .Include(c => c.VentasDetalles).FirstOrDefaultAsync(n => n.VentaId == Ventaid);
    }

    public async Task<bool> EliminarVentas(int VentaId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Ventas.AsNoTracking().Where(l => l.VentaId == VentaId).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Ventas>> ListarVentas(Expression<Func<Ventas, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ventas.Where(criterio)
            .Include(c => c.Cliente)
            .Include(s => s.VentasDetalles).ThenInclude(d=>d.Sistema)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PaginacionResultado<Ventas>> BuscarVentasAsync(
    string filtroCampo,
    string valorFiltro,
    DateTime? fechaDesde,
    DateTime? fechaHasta,
    int pagina,
    int tamanioPagina)
    {
        Expression<Func<Ventas, bool>> filtro = t => true;

        if (filtroCampo == "VentaId" && int.TryParse(valorFiltro, out var ventaid))
            filtro = filtro.AndAlso(t => t.VentaId == ventaid);
        else if (filtroCampo == "ClienteNombre")
            filtro = filtro.AndAlso(t => t.Cliente != null && t.Cliente.ClienteNombre.ToLower().Contains(valorFiltro.ToLower()));
        else if (filtroCampo == "Monto" && double.TryParse(valorFiltro, out var monto))
            filtro = filtro.AndAlso(m => m.VentasDetalles.FirstOrDefault().Monto == monto);
        else if (filtroCampo == "Descripcion")
            filtro = filtro.AndAlso(t => t.VentasDetalles.FirstOrDefault().Sistema.Descripcion.ToLower().Contains(valorFiltro.ToLower()));

        if (fechaDesde.HasValue)
            filtro = filtro.AndAlso(t => t.Fecha >= DateOnly.FromDateTime(fechaDesde.Value));

        if (fechaHasta.HasValue)
            filtro = filtro.AndAlso(t => t.Fecha <= DateOnly.FromDateTime(fechaHasta.Value));

        await using var context = await DbFactory.CreateDbContextAsync();

        var totalRegistros = await context.Ventas.CountAsync(filtro);
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanioPagina);

        var ventas = await context.Ventas
            .Include(t => t.Cliente)
            .Include(s => s.VentasDetalles).ThenInclude(d=>d.Sistema)
            .Where(filtro)
            .OrderBy(t => t.VentaId)
            .Skip((pagina - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .AsNoTracking()
            .ToListAsync();

        return new PaginacionResultado<Ventas>
        {
            Items = ventas,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas
        };
    }
}