﻿// <auto-generated />
using System;
using MemeEconomy.Insights.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MemeEconomy.Insights.Migrations
{
    [DbContext(typeof(MemeEconomyContext))]
    partial class MemeEconomyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MemeEconomy.Insights.Models.Investment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Amount");

                    b.Property<Guid>("OpportunityId");

                    b.Property<DateTime>("Timestamp");

                    b.Property<int>("Upvotes");

                    b.HasKey("Id");

                    b.HasIndex("OpportunityId");

                    b.ToTable("Investments");
                });

            modelBuilder.Entity("MemeEconomy.Insights.Models.Opportunity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MemeUri");

                    b.Property<string>("PostId")
                        .IsRequired();

                    b.Property<DateTime>("Timestamp");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasAlternateKey("PostId");

                    b.ToTable("Opportunities");
                });

            modelBuilder.Entity("MemeEconomy.Insights.Models.Investment", b =>
                {
                    b.HasOne("MemeEconomy.Insights.Models.Opportunity", "Opportunity")
                        .WithMany("Investments")
                        .HasForeignKey("OpportunityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}