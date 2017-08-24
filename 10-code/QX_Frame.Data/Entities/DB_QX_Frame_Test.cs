namespace QX_Frame.Data.Entities
{
    using QX_Frame.Bantina.Data;

    public partial class DB_QX_Frame_Test : Bankinate
    {
        public DB_QX_Frame_Test()
            : base(Configs.QX_Frame_Data_Config.ConnectionString_DB_QX_Frame_Test)
        {
        }
    }
}
