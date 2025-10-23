using DAL_QLKS;
using DTO_QLKS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient; // Required for SqlException

namespace BLL_QLKS
{
    public class BLLChiTietDichVu
    {
        private readonly DALChiTietDichVu dal = new DALChiTietDichVu();



        //public List<ChiTietDichVu> GetAllChiTietDichVu()
        //{
        //    DataTable dt = dal.GetAll();  // lấy dữ liệu từ DAL
        //    List<ChiTietDichVu> list = new List<ChiTietDichVu>();

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        list.Add(new ChiTietDichVu
        //        {
        //            ChiTietDichVuID = row["ChiTietDichVuID"].ToString(),
        //            HoaDonThueID = row["HoaDonThueID"].ToString(),
        //            LoaiDichVuID = row["LoaiDichVuID"].ToString(),
        //            SoLuong = Convert.ToInt32(row["SoLuong"]),
        //            //DonGia = Convert.ToDecimal(row["DonGia"]),
        //            GhiChu = row["GhiChu"].ToString()
        //        });
        //    }

        //    return list;
        //}

        //public List<ChiTietDichVu> GetAllChiTietDichVu()
        //{
        //    DataTable dt = dal.GetAll();
        //    List<ChiTietDichVu> list = new List<ChiTietDichVu>();

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        list.Add(new ChiTietDichVu
        //        {
        //            ChiTietDichVuID = row["ChiTietDichVuID"].ToString(),
        //            HoaDonThueID = row["HoaDonThueID"].ToString(),
        //            DichVuID = row["DichVuID"].ToString(),
        //            LoaiDichVuID = row["LoaiDichVuID"].ToString(),
        //            TenDichVu = row["TenLoaiDichVu"].ToString(),
        //            SoLuong = Convert.ToInt32(row["SoLuong"]),
        //            NgayBatDau = Convert.ToDateTime(row["NgayBatDau"]),
        //            NgayKetThuc = Convert.ToDateTime(row["NgayKetThuc"]),
        //            GhiChu = row["GhiChu"].ToString()
        //        });
        //    }

        //    return list;
        //}

        public DataTable GetAllChiTietDichVu()
        {
            return dal.GetAll(); // Lấy DataTable trực tiếp từ DAL
        }



        public DataTable GetAll()
        {
            string query = "SELECT * FROM ChiTietDichVu";
            return DBUtil.Query(query, new Dictionary<string, object>());
        }

        public DataTable GetByHoaDonThueID(string hoaDonThueID)
        {
            if (string.IsNullOrEmpty(hoaDonThueID))
                throw new ArgumentException("Mã hóa đơn thuê không được để trống.", nameof(hoaDonThueID));

            return dal.GetByHoaDonThueID(hoaDonThueID);
        }

        public string Insert(ChiTietDichVu ct)
        {
            try
            {
                if (ct == null)
                    return "Đối tượng Chi Tiết Dịch Vụ không được null.";
                if (string.IsNullOrEmpty(ct.HoaDonThueID))
                    return "Mã hóa đơn thuê không được để trống.";
                if (string.IsNullOrEmpty(ct.LoaiDichVuID))
                    return "Mã loại dịch vụ không được để trống.";
                if (ct.SoLuong <= 0)
                    return "Số lượng phải lớn hơn 0.";

                ct.ChiTietDichVuID = dal.GenerateNextID(); // ✅ dùng đúng tên hàm sinh mã CTDV

                dal.Insert(ct);
                return string.Empty;
            }
            catch (SqlException ex)
            {
                return $"Lỗi cơ sở dữ liệu khi thêm chi tiết dịch vụ: {ex.Message}";
            }
            catch (Exception ex)
            {
                return "Lỗi thêm chi tiết dịch vụ: " + ex.Message;
            }
        }


        public string Update(ChiTietDichVu ct)
        {
            try
            {
                if (ct == null)
                    return "Đối tượng Chi Tiết Dịch Vụ không được null.";
                if (string.IsNullOrEmpty(ct.ChiTietDichVuID))
                    return "Mã chi tiết dịch vụ không hợp lệ để cập nhật.";

                dal.Update(ct);
                return string.Empty;
            }
            catch (SqlException ex)
            {
                return $"Lỗi cơ sở dữ liệu khi cập nhật chi tiết dịch vụ: {ex.Message}";
            }
            catch (Exception ex)
            {
                return "Lỗi cập nhật chi tiết dịch vụ: " + ex.Message;
            }
        }

        public string Delete(string chiTietDichVuID)
        {
            try
            {
                if (string.IsNullOrEmpty(chiTietDichVuID))
                    return "Mã chi tiết dịch vụ không hợp lệ để xóa.";

                dal.Delete(chiTietDichVuID);
                return string.Empty;
            }
            catch (SqlException ex)
            {
                return $"Lỗi cơ sở dữ liệu khi xóa chi tiết dịch vụ: {ex.Message}";
            }
            catch (Exception ex)
            {
                return "Lỗi xóa chi tiết dịch vụ: " + ex.Message;
            }
        }






        public List<ChiTietDichVu> GetChiTietByHoaDonThueID(string hoaDonThueID)
        {
            DataTable dt = dal.GetByHoaDonThueID(hoaDonThueID);
            List<ChiTietDichVu> list = new List<ChiTietDichVu>();

            foreach (DataRow row in dt.Rows)
            {
                decimal donGia = row["DonGia"] == DBNull.Value ? 0 : Convert.ToDecimal(row["DonGia"]);
                int soLuong = Convert.ToInt32(row["SoLuong"]);

                list.Add(new ChiTietDichVu
                {
                    ChiTietDichVuID = row["ChiTietDichVuID"].ToString(),
                    HoaDonThueID = row["HoaDonThueID"].ToString(),
                    DichVuID = row["DichVuID"].ToString(),
                    LoaiDichVuID = row["LoaiDichVuID"].ToString(),
                    SoLuong = soLuong,
                    DonGia = donGia,
                    NgayBatDau = Convert.ToDateTime(row["NgayBatDau"]),
                    NgayKetThuc = Convert.ToDateTime(row["NgayKetThuc"]),
                    GhiChu = row["GhiChu"].ToString()
                });
            }

            return list;
        }




        public DataTable GetByID(string chiTietDichVuID)
        {
            if (string.IsNullOrEmpty(chiTietDichVuID))
                throw new ArgumentException("Mã chi tiết dịch vụ không được để trống.", nameof(chiTietDichVuID));

            return dal.GetByID(chiTietDichVuID);
        }


    }
}