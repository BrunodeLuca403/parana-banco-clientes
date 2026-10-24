using Clientes.Core.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clientes.Infrastructure.Persistence.Configuration
{
    public class ClienteEntityConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Cpf)
                .HasColumnType("char(11)")
                .IsRequired();

            builder.HasIndex(x => x.Cpf)
                .IsUnique();

            builder.Property(x => x.Email)
                .HasMaxLength(254)
                .IsRequired();

            builder.Property(x => x.DataNascimento)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.CreatAt)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
