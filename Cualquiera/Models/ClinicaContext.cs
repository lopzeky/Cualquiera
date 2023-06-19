using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cualquiera.Models;

public partial class ClinicaContext : DbContext
{
    public ClinicaContext()
    {
    }

    public ClinicaContext(DbContextOptions<ClinicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Especialidade> Especialidades { get; set; }

    public virtual DbSet<HistorialMedico> HistorialMedicos { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Secretario> Secretarios { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("server = ZEKY\\SQLEXPRESS; database = Clinica; integrated security = true; Encrypt = false");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.ToTable("Administrador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Rut).HasMaxLength(10);
            entity.Property(e => e.Usuario).HasMaxLength(20);
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Hora).HasPrecision(0);
            entity.Property(e => e.IdMedico).HasColumnName("ID_Medico");
            entity.Property(e => e.IdPaciente).HasColumnName("ID_Paciente");
            entity.Property(e => e.IdSecretario).HasColumnName("ID_Secretario");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Pacientes");
        });

        modelBuilder.Entity<Especialidade>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Especialidad).HasMaxLength(20);
            entity.Property(e => e.EspecialidadOpc)
                .HasMaxLength(20)
                .HasColumnName("Especialidad_opc");
            entity.Property(e => e.EspecialidadOpc2)
                .HasMaxLength(20)
                .HasColumnName("Especialidad_opc_2");
            entity.Property(e => e.IdMedicos).HasColumnName("ID_medicos");
        });

        modelBuilder.Entity<HistorialMedico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Historial Medico");

            entity.ToTable("Historial_Medico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alergias).HasMaxLength(200);
            entity.Property(e => e.Diagnostico).HasMaxLength(500);
            entity.Property(e => e.IdCita).HasColumnName("ID_Cita");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.HistorialMedicos)
                .HasForeignKey(d => d.IdCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Medico_Citas");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.Nombres).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Rut).HasMaxLength(10);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.Nombres).HasMaxLength(50);
            entity.Property(e => e.Rut).HasMaxLength(10);
        });

        modelBuilder.Entity<Secretario>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.Nombres).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Rut).HasMaxLength(10);
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdMedicos).HasColumnName("ID_Medicos");
            entity.Property(e => e.NumeroTel).HasColumnName("numero_Tel");
            entity.Property(e => e.NumeroTelOp).HasColumnName("numero_Tel_OP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
