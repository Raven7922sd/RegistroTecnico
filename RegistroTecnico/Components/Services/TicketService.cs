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

    public async Task<bool>ModificarTickets(Tickets ticket)
    {
        using var context = await DbFactory.CreateDbContextAsync();
        context.Tickets.Update(ticket);
        return await context.SaveChangesAsync()>0;
    }
    
    public async Task<Tickets?> BuscarTicket(int TickeId)
    {
        await using var context= await DbFactory.CreateDbContextAsync();
        return await context.Tickets.FirstOrDefaultAsync(n=>n.TicketId==TickeId);
    } 

    public async Task<bool>EliminarTicket(int TicketId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Tickets.AsNoTracking().Where(l =>l.TicketId==TicketId).ExecuteDeleteAsync()>0;
    }

    public async Task<List<Tickets>> Listar(Expression<Func<Tickets, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Tickets.Where(criterio).Include(c => c.Cliente).Include(t => t.Tecnico).AsNoTracking().ToListAsync();
    }
}
