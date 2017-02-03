using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PresentationWPF.BaoCao.DungChung;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using System.Data;

namespace PresentationWPF.BaoCao._BTV.QLTS
{
    /// <summary>
    /// Interaction logic for QLTS_BIEN_BAN_BAN_GIAO_TSCD.xaml
    /// </summary>
    public partial class QLTS_BIEN_BAN_BAN_GIAO_TSCD : UserControl
    {
        public QLTS_BIEN_BAN_BAN_GIAO_TSCD()
        {
            InitializeComponent();
        }

        private bool Validation()
        {
            if (txtSoBienBan.Text.IsNullOrEmptyOrSpace())
                return false;
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                LMessage.ShowMessage("Chưa nhập số biên bản bàn giao.", LMessage.MessageBoxType.Information);
                return null;
            }


            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            
            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoBienBan", txtSoBienBan.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
    }
}
