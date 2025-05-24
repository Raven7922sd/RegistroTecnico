using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Components.Models;

namespace RegistroTecnico.Components.DAL;
public class Contexto:DbContext
{
    public Contexto(DbContextOptions<Contexto> options):base (options){ }   

    public virtual DbSet<Tecnicos> Tecnicos {  get; set; }  
    public virtual DbSet<Clientes> Clientes { get; set; }
}
