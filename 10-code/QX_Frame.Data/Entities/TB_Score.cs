/*********************************************************
 * CopyRight: QIXIAO CODE BUILDER. 
 * Version:4.2.0
 * Author:qixiao(∆‚–°)
 * Create:2017-08-24 10:39:21
 * E-mail: dong@qixiao.me | wd8622088@foxmail.com 
 * Personal WebSit: http://qixiao.me 
 * Technical WebSit: http://www.cnblogs.com/qixiaoyizhan/ 
 * Description:-.
 * Thx , Best Regards ~
 *********************************************************/

using QX_Frame.App.Base;
using QX_Frame.Helper_DG.Bantina;
using System;

namespace QX_Frame.Data.Entities
{
    /// <summary>
    /// public class TB_Score
    /// </summary>
    [Serializable]
    [Table(TableName = "TB_Score")]
    public class TB_Score : Entity<DB_QX_Frame_Test, TB_Score>
    {
        /// <summary>
        /// construction method
        /// </summary>
        public TB_Score() { }

        // PK£®identity£©  
        [Key]
        public Guid Uid { get; set; }
        // 
        [Column]
        public Double Score1 { get; set; }
        // 
        [Column]
        public Double Score2 { get; set; }
        // 
        [Column]
        public Double Score3 { get; set; }
    }
}
