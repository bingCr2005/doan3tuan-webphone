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

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<DienThoai> DienThoais { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<GioHangOld> GioHangOlds { get; set; }

    public virtual DbSet<HangDienThoai> HangDienThoais { get; set; }

    public virtual DbSet<HinhAnh> HinhAnhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<LienHe> LienHes { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TaiKhoanAdmin> TaiKhoanAdmins { get; set; }

    public virtual DbSet<TaiKhoanKhachHang> TaiKhoanKhachHangs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DBBanDienThoai;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BinhLuan__87CB66A0E43ED0CE");

            entity.ToTable("BinhLuan");

            entity.Property(e => e.MaBinhLuan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TrangThai).HasDefaultValue(0);

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaDienThoai)
                .HasConstraintName("FK__BinhLuan__MaDien__3D5E1FD2");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__BinhLuan__MaKhac__3E52440B");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.MaCtgh).HasName("PK__ChiTietG__1E4FAF542A8F50F5");

            entity.ToTable("ChiTietGioHang");

            entity.HasIndex(e => new { e.MaGioHang, e.MaDienThoai }, "UQ_GH_DT").IsUnique();

            entity.Property(e => e.MaCtgh).HasColumnName("MaCTGH");
            entity.Property(e => e.MaDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaGioHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SoLuong).HasDefaultValue(1);
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaDienThoai)
                .HasConstraintName("FK__ChiTietGi__MaDie__6477ECF3");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGioHang)
                .HasConstraintName("FK__ChiTietGi__MaGio__6383C8BA");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaDienThoai }).HasName("PK__ChiTietH__EF288DDDFB320A7F");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(50);
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaDienThoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaDie__5070F446");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaHoa__4F7CD00D");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__ChiTietHo__MaKhu__5165187F");
        });

        modelBuilder.Entity<DienThoai>(entity =>
        {
            entity.HasKey(e => e.MaDienThoai).HasName("PK__DienThoa__C765CE6750F1816D");

            entity.ToTable("DienThoai");

            entity.Property(e => e.MaDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ChatLieu).HasMaxLength(100);
            entity.Property(e => e.DoPhanGiaiManHinh).HasMaxLength(50);
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DungLuong).HasMaxLength(20);
            entity.Property(e => e.DungLuongPin).HasMaxLength(20);
            entity.Property(e => e.HangDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HeDieuHanh).HasMaxLength(50);
            entity.Property(e => e.KichThuocManHinh).HasMaxLength(20);
            entity.Property(e => e.MaNhaCungCap)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MauSac).HasMaxLength(50);
            entity.Property(e => e.Ram)
                .HasMaxLength(20)
                .HasColumnName("RAM");
            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
            entity.Property(e => e.TenDienThoai).HasMaxLength(200);
            entity.Property(e => e.Thumbnail).HasMaxLength(500);
            entity.Property(e => e.TrangThai).HasDefaultValue(1);

            entity.HasOne(d => d.HangDienThoaiNavigation).WithMany(p => p.DienThoais)
                .HasForeignKey(d => d.HangDienThoai)
                .HasConstraintName("FK__DienThoai__HangD__2C3393D0");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.DienThoais)
                .HasForeignKey(d => d.MaNhaCungCap)
                .HasConstraintName("FK__DienThoai__MaNha__2D27B809");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA30EC75755");

            entity.ToTable("GioHang");

            entity.HasIndex(e => e.MaKhachHang, "UQ__GioHang__88D2F0E4F452D8FD").IsUnique();

            entity.Property(e => e.MaGioHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.MaKhachHangNavigation).WithOne(p => p.GioHang)
                .HasForeignKey<GioHang>(d => d.MaKhachHang)
                .HasConstraintName("FK__GioHang__MaKhach__5EBF139D");
        });

        modelBuilder.Entity<GioHangOld>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA33DD90A66");

            entity.ToTable("GioHang_Old");

            entity.HasIndex(e => new { e.MaKhachHang, e.MaDienThoai }, "UQ_GioHang_KH_DT").IsUnique();

            entity.Property(e => e.MaGioHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SoLuongHang).HasDefaultValue(1);
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.GioHangOlds)
                .HasForeignKey(d => d.MaDienThoai)
                .HasConstraintName("FK__GioHang__MaDienT__4316F928");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.GioHangOlds)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__GioHang__MaKhach__4222D4EF");
        });

        modelBuilder.Entity<HangDienThoai>(entity =>
        {
            entity.HasKey(e => e.MaHangDienThoai).HasName("PK__HangDien__F037FDBE3447C88E");

            entity.ToTable("HangDienThoai");

            entity.Property(e => e.MaHangDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TenHangDienThoai).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(1);
        });

        modelBuilder.Entity<HinhAnh>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HinhAnh__A9C37A9BD85C6916");

            entity.ToTable("HinhAnh");

            entity.Property(e => e.MaHinhAnh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Url).HasMaxLength(500);

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.HinhAnhs)
                .HasForeignKey(d => d.MaDienThoai)
                .HasConstraintName("FK__HinhAnh__MaDienT__300424B4");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13BEA3336D5");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DiaChiGiaoHang).HasMaxLength(200);
            entity.Property(e => e.MaAdmin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TenNguoiNhan).HasMaxLength(50);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasDefaultValue(0);

            entity.HasOne(d => d.MaAdminNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaAdmin)
                .HasConstraintName("FK__HoaDon__MaAdmin__4BAC3F29");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__HoaDon__MaKhachH__4AB81AF0");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__HoaDon__MaKhuyen__4CA06362");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BD965BBEF6");

            entity.ToTable("KhuyenMai");

            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MucGiamGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SoLuongMaKhuyenMaiDaDung).HasDefaultValue(0);
            entity.Property(e => e.TenChuongTrinhKhuyenMai).HasMaxLength(200);
            entity.Property(e => e.TrangThai).HasDefaultValue(1);
        });

        modelBuilder.Entity<LienHe>(entity =>
        {
            entity.HasKey(e => e.MaLienHe).HasName("PK__LienHe__0E73388A21D38CFF");

            entity.ToTable("LienHe");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayGui)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TrangThai).HasDefaultValue(false);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCungCap).HasName("PK__NhaCungC__53DA92053A621B93");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNhaCungCap)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NguoiDaiDien).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TenNhaCungCap).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(1);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C65292CF2242F");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC0501CB74A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__TaiKhoan__A9D1053465A08C06").IsUnique();

            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoVaTen).HasMaxLength(100);
            entity.Property(e => e.LoaiTaiKhoan).HasMaxLength(20);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(20);
        });

        modelBuilder.Entity<TaiKhoanAdmin>(entity =>
        {
            entity.HasKey(e => e.MaAdmin).HasName("PK__TaiKhoan__49341E3825264529");

            entity.ToTable("TaiKhoanAdmin");

            entity.Property(e => e.MaAdmin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.MaAdminNavigation).WithOne(p => p.TaiKhoanAdmin)
                .HasForeignKey<TaiKhoanAdmin>(d => d.MaAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoanA__MaAdm__36B12243");
        });

        modelBuilder.Entity<TaiKhoanKhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__TaiKhoan__88D2F0E5AFA2493A");

            entity.ToTable("TaiKhoanKhachHang");

            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.MaKhachHangNavigation).WithOne(p => p.TaiKhoanKhachHang)
                .HasForeignKey<TaiKhoanKhachHang>(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoanK__MaKha__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
