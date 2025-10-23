using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_QLKS;
using DTO_QLKS;

namespace BLL_QLKS
{
    public class BUSQLDichVu
    {
        DALQLDichVu dalQLDichVu = new DALQLDichVu();



        public List<DichVu> GetDichVuList()
        {
            return dalQLDichVu.selectAll();
        }

        public DichVu GetDichVuById(string id)
        {
            return dalQLDichVu.selectById(id);
        }
        public string InsertDichVu(DichVu dv)
        {
            try
            {
                dv.DichVuID = dalQLDichVu.GenerateNextMaDichVuID();
                if (string.IsNullOrEmpty(dv.DichVuID))
                {
                    return "Mã phòng không hợp lệ.";
                }
                dalQLDichVu.insertDichVu(dv);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }
        public string UpdateDichVu(DichVu dv)
        {
            try
            {
                if (string.IsNullOrEmpty(dv.DichVuID))
                {
                    return "Mã Khách Hàng không hợp lệ.";
                }
                dalQLDichVu.updateDichVu(dv);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }

        public string DeleteDichVu(string DichVuId)
        {
            try
            {
                dalQLDichVu.deleteDichVu(DichVuId);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }
        public string GenerateNextMaDichVu()
        {
            return dalQLDichVu.GenerateNextMaDichVuID();
        }


    }
}
