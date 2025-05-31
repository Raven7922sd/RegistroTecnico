using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RegistroTecnico.Components.DAL;
using RegistroTecnico.Components.Models;

namespace RegistroTecnico.Components.Services;

public class TicketService(IDbContextFactory<Contexto>DbFactory)
{
    public async Task<bool> Guardar(Tickets tickets)
    {
        if (!await ExisteTicketId(tickets.TicketId))
        { return (await InsertarTicket(tickets)); }

        else { return await ModificarTickets(tickets); }
    }
    
    public async Task<bool> ExisteTicketId(int TicketId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Tickets.AnyAsync(t => t.TicketId == TicketId);
    }

    public async Task<bool>InsertarTicket(Tickets ticket)
    {
        using var context=await DbFactory.CreateDbContextAsync();
        context.Tickets.Add(ticket);
        return await context.SaveChangesAsync()>0;
    }

    public async Task<bool> ModificarTickets(Tickets ticket)
    {
        using var context = await DbFactory.CreateDbContextAsync();
        ticket.Cliente = null;
        ticket.Tecnico = null;

        context.Tickets.Update(ticket);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<Tickets?> BuscarTicket(int TickeId)
    {
        await using var context= await DbFactory.CreateDbContextAsync();
        return await context.Tickets.Include(c=>c.Cliente).Include(c=>c.Tecnico).FirstOrDefaultAsync(n=>n.TicketId==TickeId);
    } 

    public async Task<bool>EliminarTicket(int TicketId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Tickets.AsNoTracking().Where(l =>l.TicketId==TicketId).ExecuteDeleteAsync()>0;
    }

    public async Task<List<Tickets>> ListarTickets(Expression<Func<Tickets, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Tickets.Where(criterio)
            .Include(c=>c.Cliente)
            .Include(t=>t.Tecnico)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Tickets>> BuscarTicketsAsync(
    string filtroCampo,
    string valorFiltro,
    DateTime? fechaDesde,
    DateTime? fechaHasta)
    {
        Expression<Func<Tickets, bool>> filtro = t => true;

        if (filtroCampo == "TicketId" && int.TryParse(valorFiltro, out var ticketId))
        {
            filtro = filtro.AndAlso(t => t.TicketId == ticketId);
        }
        else if (filtroCampo == "ClienteNombre")
        {
            filtro = filtro.AndAlso(t =>
                t.Cliente != null && t.Cliente.ClienteNombre.ToLower().Contains(valorFiltro.ToLower()));
        }
        else if (filtroCampo == "TecnicoNombre")
        {
            filtro = filtro.AndAlso(t =>
                t.Tecnico != null && t.Tecnico.TecnicoNombre.ToLower().Contains(valorFiltro.ToLower()));
        }
        else if (filtroCampo == "Asunto")
        {
            filtro = filtro.AndAlso(t =>
                t.Asunto.ToLower().Contains(valorFiltro.ToLower()));
        }
        else if (filtroCampo == "Descripcion")
        {
            filtro = filtro.AndAlso(t => 
            t.Descripcion.ToLower().Contains(valorFiltro.ToLower()));
        }
        else if (!string.IsNullOrEmpty(filtroCampo))
        {
            filtro = filtro.AndAlso(t => t.Prioridad.ToLower() == filtroCampo.ToLower());
        }
        if (fechaDesde.HasValue)
        {
            var desde = DateOnly.FromDateTime(fechaDesde.Value);
            filtro = filtro.AndAlso(t => t.Fecha >= desde);
        }
        if (fechaHasta.HasValue)
        {
            var hasta = DateOnly.FromDateTime(fechaHasta.Value);
            filtro = filtro.AndAlso(t => t.Fecha <= hasta);
        }

        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Tickets
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Where(filtro)
            .AsNoTracking()
            .ToListAsync();
    }
}
