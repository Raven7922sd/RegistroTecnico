﻿using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Components.Models;

namespace RegistroTecnico.Components.DAL;
public class Contexto:DbContext
{
    public Contexto(DbContextOptions<Contexto> options):base (options){ }   

    public virtual DbSet<Tecnicos> Tecnicos { get; set; } 
    public virtual DbSet<Clientes> Clientes { get; set; }
    public virtual DbSet<Tickets> Tickets  { get; set; }
    public virtual DbSet<Sistemas>Sistemas { get; set; }
    public virtual DbSet<Ventas>Ventas { get; set; }
    public DbSet<VentasDetalles> VentasDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tickets>()
            .HasOne(t => t.Cliente)
            .WithMany()
            .HasForeignKey(t => t.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tickets>()
            .HasOne(t => t.Tecnico)
            .WithMany()
            .HasForeignKey(t => t.TecnicoId)
            .OnDelete(DeleteBehavior.Restrict);       
        
        modelBuilder.Entity<Ventas>()
            .HasOne(t => t.Cliente)
            .WithMany()
            .HasForeignKey(t => t.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VentasDetalles>()
            .HasOne(t => t.Sistema)
            .WithMany()
            .HasForeignKey(t => t.SistemaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}