using QX_Frame.App.Base;
using QX_Frame.Helper_DG.Bantina;
namespace QX_Frame.Data.Entities
{

    [Table]
    public partial class TB_ClassName : Entity<DB_QX_Frame_Test, TB_ClassName>
    {

        [Key]
        public int ClassId { get; set; }

        [Column]
        public string ClassName { get; set; }

    }
}
