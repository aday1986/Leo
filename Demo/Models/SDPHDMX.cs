using Leo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{

    /// <summary>
    /// SDPHDMX
    /// </summary>    
    [Table(TableName = "SDPHDMX", Description = "SDPHDMX")]
    public class SDPHDMX 
    {
        #region 自動生成
        /// <summary>
        /// DJBH(varchar NotNull)
        /// </summary>      
        [Column(ColumnName = "DJBH", Description = "", ColumnOrder = 1,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = false, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DJBH { get; set; }

        /// <summary>
        /// MIBH(int)
        /// </summary>      
        [Column(ColumnName = "MIBH", Description = "", ColumnOrder = 2,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? MIBH { get; set; }

        /// <summary>
        /// MXBH(int IsPrimaryKey NotNull)
        /// </summary>      
        [Column(ColumnName = "MXBH", Description = "", ColumnOrder = 3,
        TypeName = "int", IsPrimaryKey = true, Nullable = false, IsIdentity = true,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int MXBH { get; set; }

        /// <summary>
        /// SPDM(varchar)
        /// </summary>      
        [Column(ColumnName = "SPDM", Description = "", ColumnOrder = 4,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string SPDM { get; set; }

        /// <summary>
        /// GG1DM(varchar)
        /// </summary>      
        [Column(ColumnName = "GG1DM", Description = "", ColumnOrder = 5,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string GG1DM { get; set; }

        /// <summary>
        /// GG2DM(varchar)
        /// </summary>      
        [Column(ColumnName = "GG2DM", Description = "", ColumnOrder = 6,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string GG2DM { get; set; }

        /// <summary>
        /// SL(numeric)
        /// </summary>      
        [Column(ColumnName = "SL", Description = "", ColumnOrder = 7,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL { get; set; }

        /// <summary>
        /// SL_1(numeric)
        /// </summary>      
        [Column(ColumnName = "SL_1", Description = "", ColumnOrder = 8,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL_1 { get; set; }

        /// <summary>
        /// SL_2(numeric)
        /// </summary>      
        [Column(ColumnName = "SL_2", Description = "", ColumnOrder = 9,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL_2 { get; set; }

        /// <summary>
        /// SL_3(numeric)
        /// </summary>      
        [Column(ColumnName = "SL_3", Description = "", ColumnOrder = 10,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL_3 { get; set; }

        /// <summary>
        /// BZSL(numeric)
        /// </summary>      
        [Column(ColumnName = "BZSL", Description = "", ColumnOrder = 11,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? BZSL { get; set; }

        /// <summary>
        /// CKJ(numeric)
        /// </summary>      
        [Column(ColumnName = "CKJ", Description = "", ColumnOrder = 12,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? CKJ { get; set; }

        /// <summary>
        /// ZK(numeric)
        /// </summary>      
        [Column(ColumnName = "ZK", Description = "", ColumnOrder = 13,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? ZK { get; set; }

        /// <summary>
        /// DJ(numeric)
        /// </summary>      
        [Column(ColumnName = "DJ", Description = "", ColumnOrder = 14,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? DJ { get; set; }

        /// <summary>
        /// DJ_1(numeric)
        /// </summary>      
        [Column(ColumnName = "DJ_1", Description = "", ColumnOrder = 15,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? DJ_1 { get; set; }

        /// <summary>
        /// DJ_2(numeric)
        /// </summary>      
        [Column(ColumnName = "DJ_2", Description = "", ColumnOrder = 16,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? DJ_2 { get; set; }

        /// <summary>
        /// DJ_3(numeric)
        /// </summary>      
        [Column(ColumnName = "DJ_3", Description = "", ColumnOrder = 17,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? DJ_3 { get; set; }

        /// <summary>
        /// JE(numeric)
        /// </summary>      
        [Column(ColumnName = "JE", Description = "", ColumnOrder = 18,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE { get; set; }

        /// <summary>
        /// JE_1(numeric)
        /// </summary>      
        [Column(ColumnName = "JE_1", Description = "", ColumnOrder = 19,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE_1 { get; set; }

        /// <summary>
        /// JE_2(numeric)
        /// </summary>      
        [Column(ColumnName = "JE_2", Description = "", ColumnOrder = 20,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE_2 { get; set; }

        /// <summary>
        /// JE_3(numeric)
        /// </summary>      
        [Column(ColumnName = "JE_3", Description = "", ColumnOrder = 21,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE_3 { get; set; }

        /// <summary>
        /// BZJE(numeric)
        /// </summary>      
        [Column(ColumnName = "BZJE", Description = "", ColumnOrder = 22,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? BZJE { get; set; }

        /// <summary>
        /// BZS(numeric)
        /// </summary>      
        [Column(ColumnName = "BZS", Description = "", ColumnOrder = 23,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 0, DefaultVal = "")]
        public decimal? BZS { get; set; }

        /// <summary>
        /// HH(numeric)
        /// </summary>      
        [Column(ColumnName = "HH", Description = "", ColumnOrder = 24,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 0, DefaultVal = "")]
        public decimal? HH { get; set; }

        /// <summary>
        /// DJH(varchar)
        /// </summary>      
        [Column(ColumnName = "DJH", Description = "", ColumnOrder = 25,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DJH { get; set; }

        /// <summary>
        /// MIH(int)
        /// </summary>      
        [Column(ColumnName = "MIH", Description = "", ColumnOrder = 26,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? MIH { get; set; }

        /// <summary>
        /// MXH(int)
        /// </summary>      
        [Column(ColumnName = "MXH", Description = "", ColumnOrder = 27,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? MXH { get; set; }

        /// <summary>
        /// DJH_1(varchar)
        /// </summary>      
        [Column(ColumnName = "DJH_1", Description = "", ColumnOrder = 28,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DJH_1 { get; set; }

        /// <summary>
        /// MIH_1(int)
        /// </summary>      
        [Column(ColumnName = "MIH_1", Description = "", ColumnOrder = 29,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? MIH_1 { get; set; }

        /// <summary>
        /// MXH_1(int)
        /// </summary>      
        [Column(ColumnName = "MXH_1", Description = "", ColumnOrder = 30,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? MXH_1 { get; set; }

        /// <summary>
        /// BZ(varchar)
        /// </summary>      
        [Column(ColumnName = "BZ", Description = "", ColumnOrder = 31,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 150, Precision = 150, Scale = 0, DefaultVal = "")]
        public string BZ { get; set; }

        /// <summary>
        /// BYZD1(char)
        /// </summary>      
        [Column(ColumnName = "BYZD1", Description = "", ColumnOrder = 32,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD1 { get; set; }

        /// <summary>
        /// BYZD2(char)
        /// </summary>      
        [Column(ColumnName = "BYZD2", Description = "", ColumnOrder = 33,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD2 { get; set; }

        /// <summary>
        /// BYZD3(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD3", Description = "", ColumnOrder = 34,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD3 { get; set; }

        /// <summary>
        /// BYZD4(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD4", Description = "", ColumnOrder = 35,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD4 { get; set; }

        /// <summary>
        /// BYZD5(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD5", Description = "", ColumnOrder = 36,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD5 { get; set; }

        /// <summary>
        /// BYZD6(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD6", Description = "", ColumnOrder = 37,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1000, Precision = 1000, Scale = 0, DefaultVal = "")]
        public string BYZD6 { get; set; }

        /// <summary>
        /// BYZD7(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD7", Description = "", ColumnOrder = 38,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 100, Precision = 100, Scale = 0, DefaultVal = "")]
        public string BYZD7 { get; set; }

        /// <summary>
        /// BYZD8(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD8", Description = "", ColumnOrder = 39,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD8 { get; set; }

        /// <summary>
        /// BYZD9(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD9", Description = "", ColumnOrder = 40,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD9 { get; set; }

        /// <summary>
        /// BYZD10(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD10", Description = "", ColumnOrder = 41,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD10 { get; set; }

        /// <summary>
        /// BYZD11(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD11", Description = "", ColumnOrder = 42,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD11 { get; set; }

        /// <summary>
        /// BYZD12(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD12", Description = "", ColumnOrder = 43,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD12 { get; set; }

        /// <summary>
        /// BYZD13(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD13", Description = "", ColumnOrder = 44,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD13 { get; set; }

        /// <summary>
        /// BYZD14(datetime)
        /// </summary>      
        [Column(ColumnName = "BYZD14", Description = "", ColumnOrder = 45,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? BYZD14 { get; set; }

        /// <summary>
        /// BYZD15(datetime)
        /// </summary>      
        [Column(ColumnName = "BYZD15", Description = "", ColumnOrder = 46,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? BYZD15 { get; set; }

        /// <summary>
        /// ZDHHZQ(int)
        /// </summary>      
        [Column(ColumnName = "ZDHHZQ", Description = "", ColumnOrder = 47,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? ZDHHZQ { get; set; }

        /// <summary>
        /// WHHK(numeric)
        /// </summary>      
        [Column(ColumnName = "WHHK", Description = "", ColumnOrder = 48,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? WHHK { get; set; }

        /// <summary>
        /// ZDHHK(char)
        /// </summary>      
        [Column(ColumnName = "ZDHHK", Description = "", ColumnOrder = 49,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string ZDHHK { get; set; }

        /// <summary>
        /// CFWZ(varchar)
        /// </summary>      
        [Column(ColumnName = "CFWZ", Description = "", ColumnOrder = 50,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1000, Precision = 1000, Scale = 0, DefaultVal = "")]
        public string CFWZ { get; set; }

        #endregion

    }
}
