/*********************************************************
 * CopyRight: QIXIAO CODE BUILDER. 
 * Version:4.2.0
 * Author:qixiao(∆‚–°)
 * Create:2017-08-24 10:38:28
 * E-mail: dong@qixiao.me | wd8622088@foxmail.com 
 * Personal WebSit: http://qixiao.me 
 * Technical WebSit: http://www.cnblogs.com/qixiaoyizhan/ 
 * Description:-.
 * Thx , Best Regards ~
 *********************************************************/

using QX_Frame.App.Base;
using QX_Frame.Bantina.Data;
using System;

namespace QX_Frame.Data.Entities
{
    /// <summary>
    /// public class TB_People
    /// </summary>
    [Serializable]
    [Table(TableName = "TB_People")]
    public class TB_People : Entity<DB_QX_Frame_Test, TB_People>
    {
        /// <summary>
        /// construction method
        /// </summary>
        public TB_People() { }

        // PK£®identity£©  
        [Key]
        public Guid Uid { get; set; }
        // 
        [Column]
        public String Name { get; set; }
        // 
        [Column]
        public Int32 Age { get; set; }
        // 
        [Column]
        [ForeignKey]
        public Int32 ClassId { get; set; }

        [ForeignTable]
        public TB_ClassName tb_ClassName { get; set; }
    }
}
