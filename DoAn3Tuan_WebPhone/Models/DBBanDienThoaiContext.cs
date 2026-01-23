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

    public virtual DbSet<BaiViet> BaiViets { get; set; }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<DanhMucBaiViet> DanhMucBaiViets { get; set; }

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
    public virtual DbSet<TinNhanChat> TinNhanChats { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

       // => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DBBanDienThoai;Trusted_Connection=True;TrustServerCertificate=True;");
    //=> optionsBuilder.UseSqlServer("Server=BINGCR2005;Database=DBBanDienThoai;Trusted_Connection=True;TrustServerCertificate=True");
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DBBanDienThoai;Trusted_Connection=True;TrustServerCertificate=True;");
   // => optionsBuilder.UseSqlServer("Server=BINGCR2005;Database=DBBanDienThoai;Trusted_Connection=True;TrustServerCertificate=True");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.MaBaiViet).HasName("PK__BaiViet__AEDD5647060D3A55");

            entity.ToTable("BaiViet");

            entity.Property(e => e.MaBaiViet)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HinhAnh).HasMaxLength(500);
            entity.Property(e => e.LuotXem).HasDefaultValue(0);
            entity.Property(e => e.MaDanhMucBv).HasColumnName("MaDanhMucBV");
            entity.Property(e => e.MaDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaNguoiViet)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NgayDang).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TieuDe).HasMaxLength(255);
            entity.Property(e => e.TomTat).HasMaxLength(500);
            entity.Property(e => e.TrangThai).HasDefaultValue(1);

            entity.HasOne(d => d.MaDanhMucBvNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaDanhMucBv)
                .HasConstraintName("FK__BaiViet__MaDanhM__0B91BA14");

            entity.HasOne(d => d.MaDienThoaiNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaDienThoai)
                .HasConstraintName("FK__BaiViet__MaDienT__0D7A0286");

            entity.HasOne(d => d.MaNguoiVietNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaNguoiViet)
                .HasConstraintName("FK__BaiViet__MaNguoi__0C85DE4D");
        });

        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BinhLuan__87CB66A0C070ED4F");

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
                .HasConstraintName("FK__BinhLuan__MaDien__5070F446");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__BinhLuan__MaKhac__5165187F");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.MaCtgh).HasName("PK__ChiTietG__1E4FAF54188AE968");

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
                .HasConstraintName("FK__ChiTietGi__MaDie__797309D9");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGioHang)
                .HasConstraintName("FK__ChiTietGi__MaGio__787EE5A0");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaDienThoai }).HasName("PK__ChiTietH__EF288DDD6DBC3EB7");

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
                .HasConstraintName("FK__ChiTietHo__MaDie__6383C8BA");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaHoa__628FA481");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__ChiTietHo__MaKhu__6477ECF3");
        });

        modelBuilder.Entity<DanhMucBaiViet>(entity =>
        {
            entity.HasKey(e => e.MaDanhMucBv).HasName("PK__DanhMucB__FFCBF47B4B1B7ECA");

            entity.ToTable("DanhMucBaiViet");

            entity.Property(e => e.MaDanhMucBv).HasColumnName("MaDanhMucBV");
            entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
        });

        modelBuilder.Entity<DienThoai>(entity =>
        {
            entity.HasKey(e => e.MaDienThoai).HasName("PK__DienThoa__C765CE6793AF66BD");

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
                .HasConstraintName("FK__DienThoai__HangD__3F466844");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.DienThoais)
                .HasForeignKey(d => d.MaNhaCungCap)
                .HasConstraintName("FK__DienThoai__MaNha__403A8C7D");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA38114F853");

            entity.ToTable("GioHang");

            entity.HasIndex(e => e.MaKhachHang, "UQ__GioHang__88D2F0E4BDBF0F29").IsUnique();

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
                .HasConstraintName("FK__GioHang__MaKhach__73BA3083");
        });

        modelBuilder.Entity<GioHangOld>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA3EF4DEA2B");

            entity.ToTable("GioHang_Old");

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
                .HasConstraintName("FK__GioHang__MaDienT__5629CD9C");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.GioHangOlds)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__GioHang__MaKhach__5535A963");
        });

        modelBuilder.Entity<HangDienThoai>(entity =>
        {
            entity.HasKey(e => e.MaHangDienThoai).HasName("PK__HangDien__F037FDBEFC8BCEFD");

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
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HinhAnh__A9C37A9BAA47B20C");

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
                .HasConstraintName("FK__HinhAnh__MaDienT__4316F928");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13B520A8CE5");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DiaChiGiaoHang).HasMaxLength(255);
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
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenNguoiNhan).HasMaxLength(100);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasDefaultValue(0);

            entity.HasOne(d => d.MaAdminNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaAdmin)
                .HasConstraintName("FK__HoaDon__MaAdmin__5EBF139D");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__HoaDon__MaKhachH__5DCAEF64");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__HoaDon__MaKhuyen__5FB337D6");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BD0D0DB63C");

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
            entity.HasKey(e => e.MaLienHe).HasName("PK__LienHe__0E73388AE4D893D5");

            entity.ToTable("LienHe");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayGui)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TrangThai)
                .HasColumnType("int") 
                .HasDefaultValue(0);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCungCap).HasName("PK__NhaCungC__53DA92058C00DCF9");

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
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C652909E99429");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC063F26417").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__TaiKhoan__A9D105340FBB8E6C").IsUnique();

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
            entity.HasKey(e => e.MaAdmin).HasName("PK__TaiKhoan__49341E387C129649");

            entity.ToTable("TaiKhoanAdmin");

            entity.Property(e => e.MaAdmin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.MaAdminNavigation).WithOne(p => p.TaiKhoanAdmin)
                .HasForeignKey<TaiKhoanAdmin>(d => d.MaAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoanA__MaAdm__49C3F6B7");
        });

        modelBuilder.Entity<TaiKhoanKhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__TaiKhoan__88D2F0E50C0FB5BE");

            entity.ToTable("TaiKhoanKhachHang");

            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.MaKhachHangNavigation).WithOne(p => p.TaiKhoanKhachHang)
                .HasForeignKey<TaiKhoanKhachHang>(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoanK__MaKha__4CA06362");
        });
        modelBuilder.Entity<TinNhanChat>(entity =>
        {
            entity.HasKey(e => e.MaTinNhan);

            entity.ToTable("TinNhanChat");

            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength(); // Quan trọng vì là CHAR(10) [cite: 1, 8]

            entity.Property(e => e.Email).HasMaxLength(100);

            entity.Property(e => e.NgayGui)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

        // Cấu hình mối quan hệ với bảng TaiKhoanKhachHang [cite: 8, 12]
             entity.HasOne(d => d.MaKhachHangNavigation)
            .WithMany() // Nếu mày không thêm ICollection bên TaiKhoanKhachHang thì để trống
            .HasForeignKey(d => d.MaKhachHang)
            .HasConstraintName("FK__TinNhanCh__MaKha__...");
    });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
