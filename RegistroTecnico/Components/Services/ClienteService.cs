using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Components.DAL;
using RegistroTecnico.Components.Models;

namespace RegistroTecnicos.Services;
public class ClienteService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Clientes cliente)
    {
        if (!await Existe(cliente.ClienteId))
        {
            return await Insertar(cliente);
        }
        else
        {
            return await Modificar(cliente);
        }
    }

    public async Task<bool> Existe(int ClienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AnyAsync(c => c.ClienteId == ClienteId);
    }

    public async Task<bool> ExisteNombre(string ClienteNombre)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AnyAsync(c => c.ClienteNombre.ToLower() == ClienteNombre.ToLower());
    }

    public async Task<bool> ExisteRNC(string Rnc)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AnyAsync(c => c.Rnc.ToLower() == Rnc.ToLower());
    }

    private async Task<bool> Insertar(Clientes cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Clientes.Add(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Clientes cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Clientes?> Buscar(int ClienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.FirstOrDefaultAsync(c => c.ClienteId == ClienteId);
    }

    public async Task<bool> Eliminar(int ClienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AsNoTracking().Where(c => c.ClienteId == ClienteId).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.Where(criterio).Include(c => c.Tecnico).AsNoTracking().ToListAsync();
    }
}

