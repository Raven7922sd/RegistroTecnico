using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegistroTecnico.Components.DAL;
using RegistroTecnico.Components.Models;

namespace RegistroTecnico.Components.Services;
public class ClienteService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Clientes clientes)
    {
        if (!await ExisteClienteId(clientes.ClienteId))
        {
            return (await InsertarCliente(clientes));
        }
        else
        {
            return (await ModificarCliente(clientes));
        }
    }

    public async Task<bool> ExisteClienteId(int ClienteId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Clientes.AnyAsync(c => c.ClienteId == ClienteId);
    }

    public async Task<bool> InsertarCliente(Clientes cliente)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        context.Add(cliente);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ModificarCliente(Clientes cliente)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        context.Update(cliente);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExisteNombre(string ClienteNombre)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Clientes.AnyAsync(n => n.ClienteNombre == ClienteNombre);
    }

    public async Task<bool> ExisteRnc(string Rnc)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Clientes.AnyAsync(r => r.Rnc == Rnc);
    }

    public async Task<bool> EliminarCliente(int ClienteId)
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        return await context.Clientes.AsNoTracking().Where(c => c.ClienteId == ClienteId).ExecuteDeleteAsync() > 0;
    }

    public async Task<Clientes?>BuscarCliente(int ClienteId)
    {
        await using var context= await DbFactory.CreateDbContextAsync();
        return await context.Clientes.FirstOrDefaultAsync(b => b.ClienteId == ClienteId);
    }

    public async Task<List<Clientes>>ListarClientes(Expression<Func<Clientes, bool>> criterio)
    {
        using var context=await DbFactory.CreateDbContextAsync();
        return await context.Clientes.Where(criterio).AsNoTracking().ToListAsync();
    }
}
