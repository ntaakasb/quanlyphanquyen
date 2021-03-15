using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.DataModels
{
    public class OverViewModel
    {
        public long F_DHD_KEY { get; set; }
        //Mã đối tượng bệnh nhân
        public long F_DPS_KEY { get; set; }
        //ĐĂNG KÝ KHÁM
        public int COL1 { get; set; }
        public decimal COL1_PERCENT { get; set; }
        //ĐÃ KHÁM
        public int COL2 { get; set; }
        public decimal COL2_PERCENT { get; set; }
        //NỘI TRÚ
        public int COL3 { get; set; }
        public decimal COL3_PERCENT { get; set; }
        //KẾT THÚC ĐIỀU TRỊ
        public int COL4 { get; set; }
        public decimal COL4_PERCENT { get; set; }
        //PHẪU THUẬT
        public int COL5 { get; set; }
        public decimal COL5_PERCENT { get; set; }
    }
}
