using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

public partial class DBBanDienThoaiContext : DbContext
{
    public DBBanDienThoaiContext()
    {
    }

    public DBBanDienThoaiContext(DbContextOptions<DBBanDienThoaiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<DienThoai> DienThoais { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HangDienThoai> HangDienThoais { get; set; }

    public virtual DbSet<HinhAnh> HinhAnhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TaiKhoanAdmin> TaiKhoanAdmins { get; set; }

    public virtual DbSet<TaiKhoanKhachHang> TaiKhoanKhachHangs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBBanDienThoai;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BinhLuan__87CB66A0660ACD23");

            entity.Property(e => e.MaBinhLuan).IsFixedLength();
            entity.Property(e => e.MaDienThoai).IsFixedLength();
            entity.Property(e => e.MaKhachHang).IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(0);

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.BinhLuans).HasConstraintName("FK__BinhLuan__MaDien__3D5E1FD2");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.BinhLuans).HasConstraintName("FK__BinhLuan__MaKhac__3E52440B");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaDienThoai }).HasName("PK__ChiTietH__EF288DDD5802E29D");

            entity.Property(e => e.MaHoaDon).IsFixedLength();
            entity.Property(e => e.MaDienThoai).IsFixedLength();
            entity.Property(e => e.MaKhuyenMai).IsFixedLength();

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.ChiTietHoaDons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaDie__5070F446");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaHoa__4F7CD00D");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.ChiTietHoaDons).HasConstraintName("FK__ChiTietHo__MaKhu__5165187F");
        });

        modelBuilder.Entity<DienThoai>(entity =>
        {
            entity.HasKey(e => e.MaDienThoai).HasName("PK__DienThoa__C765CE67B4DC5F0E");

            entity.Property(e => e.MaDienThoai).IsFixedLength();
            entity.Property(e => e.HangDienThoai).IsFixedLength();
            entity.Property(e => e.MaNhaCungCap).IsFixedLength();
            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
            entity.Property(e => e.TrangThai).HasDefaultValue(1);

            entity.HasOne(d => d.HangDienThoaiNavigation).WithMany(p => p.DienThoais).HasConstraintName("FK__DienThoai__HangD__2C3393D0");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.DienThoais).HasConstraintName("FK__DienThoai__MaNha__2D27B809");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA3D082824A");

            entity.Property(e => e.MaGioHang).IsFixedLength();
            entity.Property(e => e.MaDienThoai).IsFixedLength();
            entity.Property(e => e.MaKhachHang).IsFixedLength();
            entity.Property(e => e.SoLuongHang).HasDefaultValue(1);

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.GioHangs).HasConstraintName("FK__GioHang__MaDienT__4316F928");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.GioHangs).HasConstraintName("FK__GioHang__MaKhach__4222D4EF");
        });

        modelBuilder.Entity<HangDienThoai>(entity =>
        {
            entity.HasKey(e => e.MaHangDienThoai).HasName("PK__HangDien__F037FDBEAFC8A007");

            entity.Property(e => e.MaHangDienThoai).IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(1);
        });

        modelBuilder.Entity<HinhAnh>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HinhAnh__A9C37A9B551962CB");

            entity.Property(e => e.MaHinhAnh).IsFixedLength();
            entity.Property(e => e.MaDienThoai).IsFixedLength();

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.HinhAnhs).HasConstraintName("FK__HinhAnh__MaDienT__300424B4");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13B67CFC66C");

            entity.Property(e => e.MaHoaDon).IsFixedLength();
            entity.Property(e => e.MaAdmin).IsFixedLength();
            entity.Property(e => e.MaKhachHang).IsFixedLength();
            entity.Property(e => e.MaKhuyenMai).IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(0);

            entity.HasOne(d => d.MaAdminNavigation).WithMany(p => p.HoaDons).HasConstraintName("FK__HoaDon__MaAdmin__4BAC3F29");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDons).HasConstraintName("FK__HoaDon__MaKhachH__4AB81AF0");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.HoaDons).HasConstraintName("FK__HoaDon__MaKhuyen__4CA06362");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BD467ED5C3");

            entity.Property(e => e.MaKhuyenMai).IsFixedLength();
            entity.Property(e => e.SoLuongMaKhuyenMaiDaDung).HasDefaultValue(0);
            entity.Property(e => e.TrangThai).HasDefaultValue(1);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCungCap).HasName("PK__NhaCungC__53DA9205836F9C0A");

            entity.Property(e => e.MaNhaCungCap).IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(1);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C65292369444F");

            entity.Property(e => e.MaTaiKhoan).IsFixedLength();
        });

        modelBuilder.Entity<TaiKhoanAdmin>(entity =>
        {
            entity.HasKey(e => e.MaAdmin).HasName("PK__TaiKhoan__49341E3873C3CB87");

            entity.Property(e => e.MaAdmin).IsFixedLength();

            entity.HasOne(d => d.MaAdminNavigation).WithOne(p => p.TaiKhoanAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoanA__MaAdm__36B12243");
        });

        modelBuilder.Entity<TaiKhoanKhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__TaiKhoan__88D2F0E5CAF5528B");

            entity.Property(e => e.MaKhachHang).IsFixedLength();

            entity.HasOne(d => d.MaKhachHangNavigation).WithOne(p => p.TaiKhoanKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoanK__MaKha__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
