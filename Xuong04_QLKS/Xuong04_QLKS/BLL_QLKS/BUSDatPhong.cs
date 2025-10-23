using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_QLKS;
using DTO_QLKS;


namespace BLL_QLKS
{
    public class BUSDatPhong
    {
        BUS_Phong busPhong = new BUS_Phong();
        DALDatPhong dalDatPhong = new DALDatPhong();

        public List<DatPhong> GetDatPhongList()
        {
            return dalDatPhong.selectAll();
        }

        public List<DatPhong> GetAllList()
        {
            return dalDatPhong.selectAll();
        }


        public DatPhong GetByID(string id)
        {
            return GetDatPhongById(id);
        }


        public DatPhongView GetDatPhongViewByID(string hoaDonThueID)
        {
            return dalDatPhong.GetThongTinDatPhongChiTiet(hoaDonThueID);
        }


        public DatPhong GetDatPhongById(string id)
        {
            return dalDatPhong.selectById(id);
        }

        public string InsertDatPhong(DatPhong datPhong)
        {
            try
            {
                // Tạo mã nếu chưa có
                if (string.IsNullOrEmpty(datPhong.HoaDonThueID))
                {
                    datPhong.HoaDonThueID = dalDatPhong.generateHoaDonThueID();
                }
                    
                if (string.IsNullOrEmpty(datPhong.PhongID) ||
                    string.IsNullOrEmpty(datPhong.KhachHangID))
                {
                    return null; // Thiếu dữ liệu bắt buộc
                }

                bool inserted = dalDatPhong.insertDatPhong(datPhong);
                if (!inserted)
                    return null;

                // Cập nhật trạng thái phòng

                return datPhong.HoaDonThueID; // Trả về mã hóa đơn khi thành công
            }
            catch
            {
                return null;
            }
        }







        public string UpdateDatPhong(DatPhong datPhong)
        {
            try
            {
                if (string.IsNullOrEmpty(datPhong.HoaDonThueID))
                {
                    return "Mã đặt phòng không hợp lệ.";
                }

                dalDatPhong.updateDatPhong(datPhong);

                // ✅ Nếu trạng thái cuối là Hủy thì cập nhật phòng thành trống (true)
                TrangThaiDatPhongBLL trangThaiBLL = new TrangThaiDatPhongBLL();
                string trangThaiCuoi = trangThaiBLL.GetTrangThaiCuoi(datPhong.HoaDonThueID);

                if (trangThaiCuoi == "TT003") // TT003 = Hủy
                {
                    busPhong.UpdateTinhTrangPhong(datPhong.PhongID, true); // true = Trống
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }


        public string DeleteDatPhong(string hoaDonThueID)
        {
            try
            {
                dalDatPhong.deleteDatPhong(hoaDonThueID);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }




        public bool KiemTraPhongDaDuocDat(string phongId, DateTime ngayDenMoi, DateTime ngayDiMoi)
        {
            return GetDatPhongConHieuLuc().Any(dp =>
                dp.PhongID == phongId &&
                !(dp.NgayDi <= ngayDenMoi || dp.NgayDen >= ngayDiMoi)
            );
        }






        public DatPhong GetDatPhongByID(string hoaDonThueID)
        {
            return dalDatPhong.selectById(hoaDonThueID); // đảm bảo DAL có hàm này
        }


        public List<DatPhong> GetDatPhongConHieuLuc()
        {
            var all = GetDatPhongList();
            var bllTrangThai = new TrangThaiDatPhongBLL();

            return all.Where(dp =>
            {
                string trangThaiCuoi = bllTrangThai.GetTrangThaiCuoi(dp.HoaDonThueID);
                return trangThaiCuoi != "TT003"; // TT003 là mã trạng thái Hủy
            }).ToList();
        }


        public bool KiemTraPhongDaDuocDat_1(string phongID, DateTime ngayDen, DateTime ngayDi)
        {
            return dalDatPhong.KiemTraPhongDaDuocDat_SP(phongID, ngayDen, ngayDi);
        }




        public List<DatPhongView> GetDanhSachDatPhongView()
        {
            var danhSachDatPhong = dalDatPhong.selectAll();
            var danhSachTrangThai = new LoaiTrangThaiDatPhongBLL().LayDanhSach();
            var danhSachPhong = new BUS_Phong().GetPhongList();

            var danhSachView = danhSachDatPhong.Select(dp =>
            {

                return new DatPhongView
                {
                    HoaDonThueID = dp.HoaDonThueID,
                    KhachHangID = dp.KhachHangID,
                    PhongID = dp.PhongID,
                    TenPhong = danhSachPhong.FirstOrDefault(p => p.PhongID == dp.PhongID)?.TenPhong ?? "",
                    NgayDen = dp.NgayDen,
                    NgayDi = dp.NgayDi,
                    MaNV = dp.MaNV,
                    GhiChu = dp.GhiChu,
                };
            }).ToList();

            return danhSachView;
        }

    }
}
