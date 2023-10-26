using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Totalcare.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Subscriptiontype> Subscriptiontypes { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=orcl)));User Id=totalcare;Password=12345;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("TOTALCARE")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C007597");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Companyname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COMPANYNAME");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.History)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("HISTORY");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Bankid).HasName("SYS_C007599");

            entity.ToTable("BANK");

            entity.Property(e => e.Bankid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("BANKID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardnum)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CARDNUM");
            entity.Property(e => e.Cvv)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CVV");
            entity.Property(e => e.Expirydate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("EXPIRYDATE");
        });

        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.HasKey(e => e.Beneficiaryid).HasName("SYS_C007585");

            entity.ToTable("BENEFICIARY");

            entity.Property(e => e.Beneficiaryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("BENEFICIARYID");
            entity.Property(e => e.Proofdocument)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PROOFDOCUMENT");
            entity.Property(e => e.Relationship)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("RELATIONSHIP");
            entity.Property(e => e.Requeststatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("REQUESTSTATUS");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Beneficiaries)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_BENEFICIARYSUBID");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C007595");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<Home>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C007593");

            entity.ToTable("HOME");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Bodytxt1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("BODYTXT1");
            entity.Property(e => e.Bodytxt2)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("BODYTXT2");
            entity.Property(e => e.Facebooklink)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("FACEBOOKLINK");
            entity.Property(e => e.Headtxt1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("HEADTXT1");
            entity.Property(e => e.Headtxt2)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("HEADTXT2");
            entity.Property(e => e.Imagepath1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH1");
            entity.Property(e => e.Imagepath2)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH2");
            entity.Property(e => e.Instagramlink)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("INSTAGRAMLINK");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C007606");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Subscriptionid).HasName("SYS_C007601");

            entity.ToTable("SUBSCRIPTION");

            entity.Property(e => e.Subscriptionid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SUBSCRIPTIONID");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Subscriptiondate)
                .HasColumnType("DATE")
                .HasColumnName("SUBSCRIPTIONDATE");
            entity.Property(e => e.Typeid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TYPEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Type).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.Typeid)
                .HasConstraintName("FK_TYPEID");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SUBSCRIPTION");
        });

        modelBuilder.Entity<Subscriptiontype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SUBID");

            entity.ToTable("SUBSCRIPTIONTYPE");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"TOTALCARE\".\"ISEQ$$_73970\".nextval ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Details)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DETAILS");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C007588");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Requeststatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("REQUESTSTATUS");
            entity.Property(e => e.Testimonialdate)
                .HasColumnType("DATE")
                .HasColumnName("TESTIMONIALDATE");
            entity.Property(e => e.Testimonialsubject)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALSUBJECT");
            entity.Property(e => e.Testimonialtext)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALTEXT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TESTIMONIALUSERID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C007581");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Email, "SYS_C007582").IsUnique();

            entity.HasIndex(e => e.Phonenumber, "SYS_C007583").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FULLNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Profilepicture)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PROFILEPICTURE");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("FK_USERSROLEID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
