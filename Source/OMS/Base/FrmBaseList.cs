using System;
using OMS.DAC;
using OMS.Models;

namespace OMS
{
    public class FrmBaseList : FrmBase
    {
        #region Property

        public int CurrentPageDefault { get; set; }

        public int NumRowOnPageDefault { get; set; }       

        #endregion

        #region override Method

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.CurrentPageDefault = 1;
            this.NumRowOnPageDefault = GetDefaultDllPaging();
        }

        #endregion

        #region Method

        protected int GetDefaultDllPaging()
        {
            using (DB db = new DB())
            {
                Config_HService service = new Config_HService(db);
                string ret = service.GetDefaultValueDrop(M_Config_H.CONFIG_CD_PAGING);
                return string.IsNullOrEmpty(ret) ? 0 :  int.Parse(ret);
            }
        }

        #endregion

    }
}