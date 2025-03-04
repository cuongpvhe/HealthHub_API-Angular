using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HealthHub_API.Models
{
    public partial class WebAppYteContext : DbContext
    {
        public WebAppYteContext()
        {
        }

        public WebAppYteContext(DbContextOptions<WebAppYteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DanhGiaChatLuong> DanhGiaChatLuongs { get; set; } = null!;
        public virtual DbSet<DanhGium> DanhGia { get; set; } = null!;
        public virtual DbSet<GioiTinh> GioiTinhs { get; set; } = null!;
        public virtual DbSet<HoiDap> HoiDaps { get; set; } = null!;
        public virtual DbSet<Khoa> Khoas { get; set; } = null!;
        public virtual DbSet<LichKham> LichKhams { get; set; } = null!;
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public virtual DbSet<QuanTri> QuanTris { get; set; } = null!;
        public virtual DbSet<Solieudichbenh> Solieudichbenhs { get; set; } = null!;
        public virtual DbSet<TinhThanh> TinhThanhs { get; set; } = null!;
        public virtual DbSet<Tintuc> Tintucs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanhGiaChatLuong>(entity =>
            {
                entity.HasKey(e => e.IddanhGiaChatLuong)
                    .HasName("PK_KetQuaDanhGia");

                entity.ToTable("DanhGiaChatLuong");

                entity.Property(e => e.IddanhGiaChatLuong).HasColumnName("IDDanhGiaChatLuong");

                entity.Property(e => e.DanhGiaChatLuong1)
                    .HasMaxLength(10)
                    .HasColumnName("DanhGiaChatLuong");
            });

            modelBuilder.Entity<DanhGium>(entity =>
            {
                entity.HasKey(e => e.IddanhGia);

                entity.Property(e => e.IddanhGia).HasColumnName("IDDanhGia");

                entity.Property(e => e.IddanhGiaChatLuong).HasColumnName("IDDanhGiaChatLuong");

                entity.Property(e => e.IdlichKham).HasColumnName("IDLichKham");

                entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");

                entity.Property(e => e.IdquanTri).HasColumnName("IDQuanTri");

                entity.HasOne(d => d.IddanhGiaChatLuongNavigation)
                    .WithMany(p => p.DanhGia)
                    .HasForeignKey(d => d.IddanhGiaChatLuong)
                    .HasConstraintName("FK_DanhGia_DanhGiaChatLuong");

                entity.HasOne(d => d.IdlichKhamNavigation)
                    .WithMany(p => p.DanhGia)
                    .HasForeignKey(d => d.IdlichKham)
                    .HasConstraintName("FK_DanhGia_LichKham");

                entity.HasOne(d => d.IdnguoiDungNavigation)
                    .WithMany(p => p.DanhGia)
                    .HasForeignKey(d => d.IdnguoiDung)
                    .HasConstraintName("FK_DanhGia_NguoiDung");

                entity.HasOne(d => d.IdquanTriNavigation)
                    .WithMany(p => p.DanhGia)
                    .HasForeignKey(d => d.IdquanTri)
                    .HasConstraintName("FK_DanhGia_QuanTri");
            });

            modelBuilder.Entity<GioiTinh>(entity =>
            {
                entity.HasKey(e => e.IdgioiTinh);

                entity.ToTable("GioiTinh");

                entity.Property(e => e.IdgioiTinh).HasColumnName("IDGioiTinh");

                entity.Property(e => e.GioiTinh1)
                    .HasMaxLength(6)
                    .HasColumnName("GioiTinh");
            });

            modelBuilder.Entity<HoiDap>(entity =>
            {
                entity.HasKey(e => e.Idhoidap);

                entity.ToTable("HoiDap");

                entity.Property(e => e.Idhoidap).HasColumnName("IDHoidap");

                entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");

                entity.Property(e => e.IdquanTri).HasColumnName("IDQuanTri");

                entity.Property(e => e.NgayGui)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdnguoiDungNavigation)
                    .WithMany(p => p.HoiDaps)
                    .HasForeignKey(d => d.IdnguoiDung)
                    .HasConstraintName("FK_HoiDap_NguoiDung");

                entity.HasOne(d => d.IdquanTriNavigation)
                    .WithMany(p => p.HoiDaps)
                    .HasForeignKey(d => d.IdquanTri)
                    .HasConstraintName("FK_HoiDap_QuanTri");
            });

            modelBuilder.Entity<Khoa>(entity =>
            {
                entity.HasKey(e => e.Idkhoa);

                entity.ToTable("Khoa");

                entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");

                entity.Property(e => e.TenKhoa).HasMaxLength(50);
            });

            modelBuilder.Entity<LichKham>(entity =>
            {
                entity.HasKey(e => e.IdlichKham);

                entity.ToTable("LichKham");

                entity.Property(e => e.IdlichKham).HasColumnName("IDLichKham");

                entity.Property(e => e.BatDau).HasColumnType("smalldatetime");

                entity.Property(e => e.ChuDe).HasMaxLength(100);

                entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");

                entity.Property(e => e.IdquanTri).HasColumnName("IDQuanTri");

                entity.Property(e => e.KetQuaKham).HasMaxLength(200);

                entity.Property(e => e.KetThuc).HasColumnType("smalldatetime");

                entity.Property(e => e.MoTa).HasMaxLength(300);

                entity.Property(e => e.ZoomInfo).HasMaxLength(100);

                entity.HasOne(d => d.IdnguoiDungNavigation)
                    .WithMany(p => p.LichKhams)
                    .HasForeignKey(d => d.IdnguoiDung)
                    .HasConstraintName("FK_LichKham_NguoiDung");

                entity.HasOne(d => d.IdquanTriNavigation)
                    .WithMany(p => p.LichKhams)
                    .HasForeignKey(d => d.IdquanTri)
                    .HasConstraintName("FK_LichKham_QuanTri");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.IdnguoiDung);

                entity.ToTable("NguoiDung");

                entity.Property(e => e.IdnguoiDung).HasColumnName("IDNguoiDung");

                entity.Property(e => e.IdgioiTinh).HasColumnName("IDGioiTinh");

                entity.Property(e => e.Idtinh).HasColumnName("IDTinh");

                entity.Property(e => e.NgayCapNhat).HasColumnType("smalldatetime");

                entity.Property(e => e.NgayTao).HasColumnType("smalldatetime");

                entity.Property(e => e.RefreshTokenExpiry).HasColumnType("smalldatetime");

                entity.Property(e => e.SoCmnd).HasColumnName("SoCMND");

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdgioiTinhNavigation)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.IdgioiTinh)
                    .HasConstraintName("FK_NguoiDung_GioiTinh");

                entity.HasOne(d => d.IdtinhNavigation)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.Idtinh)
                    .HasConstraintName("FK_NguoiDung_TinhThanh");
            });

            modelBuilder.Entity<QuanTri>(entity =>
            {
                entity.HasKey(e => e.IdquanTri);

                entity.ToTable("QuanTri");

                entity.Property(e => e.IdquanTri).HasColumnName("IDQuanTri");

                entity.Property(e => e.AnhBia)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");

                entity.Property(e => e.NgayCapNhat).HasColumnType("smalldatetime");

                entity.Property(e => e.NgayTao).HasColumnType("smalldatetime");

                entity.Property(e => e.RefreshTokenExpiry).HasColumnType("smalldatetime");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.ThongtinZoom).HasMaxLength(100);

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TrinhDo).HasMaxLength(50);

                entity.Property(e => e.VaiTro)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('Doctor')");

                entity.HasOne(d => d.IdkhoaNavigation)
                    .WithMany(p => p.QuanTris)
                    .HasForeignKey(d => d.Idkhoa)
                    .HasConstraintName("FK_QuanTri_Khoa");
            });

            modelBuilder.Entity<Solieudichbenh>(entity =>
            {
                entity.HasKey(e => e.Idthongke);

                entity.ToTable("Solieudichbenh");

                entity.Property(e => e.Idthongke).HasColumnName("IDThongke");

                entity.Property(e => e.Dichbenh)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Quocgia).HasMaxLength(50);
            });

            modelBuilder.Entity<TinhThanh>(entity =>
            {
                entity.HasKey(e => e.Idtinh);

                entity.ToTable("TinhThanh");

                entity.Property(e => e.Idtinh).HasColumnName("IDTinh");

                entity.Property(e => e.TenTinh).HasMaxLength(30);
            });

            modelBuilder.Entity<Tintuc>(entity =>
            {
                entity.HasKey(e => e.Idtintuc);

                entity.ToTable("Tintuc");

                entity.Property(e => e.Idtintuc).HasColumnName("IDTintuc");

                entity.Property(e => e.Hinhanh).HasMaxLength(100);

                entity.Property(e => e.Ngaydang)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TheLoai)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
