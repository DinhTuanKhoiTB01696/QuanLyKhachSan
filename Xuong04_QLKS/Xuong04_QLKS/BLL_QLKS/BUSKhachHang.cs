using DAL_QLKS;
using DTO_QLKS;
using System;
using System.Collections.Generic;

public class BUSKhachHang
{
    DALKhachHang dalKhachHang = new DALKhachHang();

    // ============================================================
    // LẤY DANH SÁCH KHÁCH HÀNG
    // ============================================================
    public List<KhachHang> GetAll()
    {
        return GetKhachHangList();
    }

    public List<KhachHang> GetKhachHangList()
    {
        return dalKhachHang.selectAll();
    }

    public KhachHang GetKhachHangById(string id)
    {
        return dalKhachHang.selectById(id);
    }

    // ============================================================
    // INSERT
    // ============================================================
    public string InsertKhachHang(KhachHang kH)
    {
        try
        {
            kH.KhachHangID = dalKhachHang.generateKhachHangID();
            if (string.IsNullOrEmpty(kH.KhachHangID))
            {
                return "Mã khách hàng không hợp lệ.";
            }

            dalKhachHang.insertKhachHang(kH);
            return string.Empty;
        }
        catch (Exception ex)
        {
            return "Lỗi: " + ex.Message;
        }
    }

    // ============================================================
    // UPDATE
    // ============================================================
    public string UpdateKhachHang(KhachHang kH)
    {
        try
        {
            if (string.IsNullOrEmpty(kH.KhachHangID))
            {
                return "Mã Khách Hàng không hợp lệ.";
            }

            dalKhachHang.updateKhachHang(kH);
            return string.Empty;
        }
        catch (Exception ex)
        {
            return "Lỗi: " + ex.Message;
        }
    }

    // ============================================================
    // DELETE
    // ============================================================
    public string DeleteKhachHang(string KhachHangId)
    {
        try
        {
            dalKhachHang.deleteKhachHang(KhachHangId);
            return string.Empty;
        }
        catch (Exception ex)
        {
            return "Lỗi: " + ex.Message;
        }
    }

    // ============================================================
    // MÃ KHÁCH HÀNG — GenerateKhachHangID (gốc)
    // ============================================================
    public string GenerateKhachHangID()
    {
        return dalKhachHang.generateKhachHangID();
    }

    // ============================================================
    // ALIAS CHO UNIT TEST — GenerateId() (bạn yêu cầu thêm)
    // ============================================================
    public string GenerateId()
    {
        return GenerateKhachHangID();
    }
}
