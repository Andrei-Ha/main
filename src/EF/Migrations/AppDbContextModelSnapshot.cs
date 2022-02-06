﻿// <auto-generated />
using System;
using Exadel.OfficeBooking.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Exadel.OfficeBooking.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Bookings.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RecuringBookingId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WorkplaceId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RecuringBookingId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkplaceId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Bookings.RecuringBooking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RecuringBooking");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Notifications.BookingNotification", b =>
                {
                    b.Property<string>("EmailAdress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailSubject")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("TEXT");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Map", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("FloorNumber")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsKitchenPresent")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMeetingRoomPresent")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("OfficeId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Office", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCityCenter")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsParkingAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.ParkingPlace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BookingId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsBooked")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("OfficeId")
                        .HasColumnType("TEXT");

                    b.Property<int>("PlaceNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("OfficeId");

                    b.ToTable("ParkingPlaces");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Workplace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasHeadset")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasKeyboard")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasMonitor")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasMouse")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasPC")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsBooked")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsNextToWindow")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("MapId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MapId");

                    b.ToTable("Workplaces");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Person.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EmploymentEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EmploymentStart")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PrefferedId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("TelegramId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PrefferedId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8c8807d4-134f-4c38-99a3-a28b31c032d8"),
                            Email = "iivanov@gmail.com",
                            EmploymentStart = new DateTime(2022, 2, 6, 10, 43, 57, 560, DateTimeKind.Local).AddTicks(5717),
                            FirstName = "Ivan",
                            LastName = "Ivanov",
                            Role = 0,
                            TelegramId = 123465
                        });
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Person.Vacation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("VacationEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("VacationStart")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Vacations");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Bookings.Booking", b =>
                {
                    b.HasOne("Exadel.OfficeBooking.Domain.Bookings.RecuringBooking", null)
                        .WithMany("Bookings")
                        .HasForeignKey("RecuringBookingId");

                    b.HasOne("Exadel.OfficeBooking.Domain.Person.User", "User")
                        .WithMany("BookingList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Exadel.OfficeBooking.Domain.OfficePlan.Workplace", "Workplace")
                        .WithMany("Bookings")
                        .HasForeignKey("WorkplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Workplace");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Bookings.RecuringBooking", b =>
                {
                    b.HasOne("Exadel.OfficeBooking.Domain.Person.User", null)
                        .WithMany("RecuringBookingList")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Map", b =>
                {
                    b.HasOne("Exadel.OfficeBooking.Domain.OfficePlan.Office", "Office")
                        .WithMany("Maps")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Office");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.ParkingPlace", b =>
                {
                    b.HasOne("Exadel.OfficeBooking.Domain.Bookings.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId");

                    b.HasOne("Exadel.OfficeBooking.Domain.OfficePlan.Office", "Office")
                        .WithMany("ParkingPlaces")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Office");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Workplace", b =>
                {
                    b.HasOne("Exadel.OfficeBooking.Domain.OfficePlan.Map", "Map")
                        .WithMany("Workspaces")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Map");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Person.User", b =>
                {
                    b.HasOne("Exadel.OfficeBooking.Domain.OfficePlan.Workplace", "Preffered")
                        .WithMany()
                        .HasForeignKey("PrefferedId");

                    b.Navigation("Preffered");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Person.Vacation", b =>
                {
                    b.HasOne("Exadel.OfficeBooking.Domain.Person.User", "User")
                        .WithMany("Vacations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Bookings.RecuringBooking", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Map", b =>
                {
                    b.Navigation("Workspaces");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Office", b =>
                {
                    b.Navigation("Maps");

                    b.Navigation("ParkingPlaces");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.OfficePlan.Workplace", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("Exadel.OfficeBooking.Domain.Person.User", b =>
                {
                    b.Navigation("BookingList");

                    b.Navigation("RecuringBookingList");

                    b.Navigation("Vacations");
                });
#pragma warning restore 612, 618
        }
    }
}
