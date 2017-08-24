namespace QX_Frame.Data.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using QX_Frame.Helper_DG.Bantina;

    public partial class DB_QX_Frame_Test : Bantina
    {
        public DB_QX_Frame_Test()
            : base(Configs.QX_Frame_Data_Config.ConnectionString_DB_QX_Frame_Test)
        {
        }
    }
}
