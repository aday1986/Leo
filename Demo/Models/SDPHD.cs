using Leo.Data;
using System;

namespace ConsoleApp1.Models
{
    /// <summary>
    /// SDPHD
    /// </summary>    
    [Table(TableName = "SDPHD", Description = "SDPHD")]
    public class SDPHD 
    {
        #region 自動生成
        /// <summary>
        /// DJBH(varchar IsPrimaryKey NotNull)
        /// </summary>      
        [Column(ColumnName = "DJBH", Description = "", ColumnOrder = 1,
        TypeName = "varchar", IsPrimaryKey = true, Nullable = false, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DJBH { get; set; }

        /// <summary>
        /// RQ(datetime)
        /// </summary>      
        [Column(ColumnName = "RQ", Description = "", ColumnOrder = 2,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? RQ { get; set; }

        /// <summary>
        /// YDJH(varchar)
        /// </summary>      
        [Column(ColumnName = "YDJH", Description = "", ColumnOrder = 3,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string YDJH { get; set; }

        /// <summary>
        /// DJXZ(char)
        /// </summary>      
        [Column(ColumnName = "DJXZ", Description = "", ColumnOrder = 4,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string DJXZ { get; set; }

        /// <summary>
        /// FPLX(char)
        /// </summary>      
        [Column(ColumnName = "FPLX", Description = "", ColumnOrder = 5,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string FPLX { get; set; }

        /// <summary>
        /// LXDJ(varchar)
        /// </summary>      
        [Column(ColumnName = "LXDJ", Description = "", ColumnOrder = 6,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string LXDJ { get; set; }

        /// <summary>
        /// DAYS(int)
        /// </summary>      
        [Column(ColumnName = "DAYS", Description = "", ColumnOrder = 7,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? DAYS { get; set; }

        /// <summary>
        /// DM1(varchar)
        /// </summary>      
        [Column(ColumnName = "DM1", Description = "", ColumnOrder = 8,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DM1 { get; set; }

        /// <summary>
        /// DM1_1(varchar)
        /// </summary>      
        [Column(ColumnName = "DM1_1", Description = "", ColumnOrder = 9,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string DM1_1 { get; set; }

        /// <summary>
        /// DM2(varchar)
        /// </summary>      
        [Column(ColumnName = "DM2", Description = "", ColumnOrder = 10,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DM2 { get; set; }

        /// <summary>
        /// DM2_1(varchar)
        /// </summary>      
        [Column(ColumnName = "DM2_1", Description = "", ColumnOrder = 11,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string DM2_1 { get; set; }

        /// <summary>
        /// DM3(varchar)
        /// </summary>      
        [Column(ColumnName = "DM3", Description = "", ColumnOrder = 12,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string DM3 { get; set; }

        /// <summary>
        /// DM3_1(varchar)
        /// </summary>      
        [Column(ColumnName = "DM3_1", Description = "", ColumnOrder = 13,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string DM3_1 { get; set; }

        /// <summary>
        /// DM4(varchar)
        /// </summary>      
        [Column(ColumnName = "DM4", Description = "", ColumnOrder = 14,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string DM4 { get; set; }

        /// <summary>
        /// DM4_1(varchar)
        /// </summary>      
        [Column(ColumnName = "DM4_1", Description = "", ColumnOrder = 15,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string DM4_1 { get; set; }

        /// <summary>
        /// QDDM(varchar)
        /// </summary>      
        [Column(ColumnName = "QDDM", Description = "", ColumnOrder = 16,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string QDDM { get; set; }

        /// <summary>
        /// QYDM(varchar)
        /// </summary>      
        [Column(ColumnName = "QYDM", Description = "", ColumnOrder = 17,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string QYDM { get; set; }

        /// <summary>
        /// YGDM(varchar)
        /// </summary>      
        [Column(ColumnName = "YGDM", Description = "", ColumnOrder = 18,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string YGDM { get; set; }

        /// <summary>
        /// SL(numeric)
        /// </summary>      
        [Column(ColumnName = "SL", Description = "", ColumnOrder = 19,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL { get; set; }

        /// <summary>
        /// SL_1(numeric)
        /// </summary>      
        [Column(ColumnName = "SL_1", Description = "", ColumnOrder = 20,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL_1 { get; set; }

        /// <summary>
        /// SL_2(numeric)
        /// </summary>      
        [Column(ColumnName = "SL_2", Description = "", ColumnOrder = 21,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL_2 { get; set; }

        /// <summary>
        /// SL_3(numeric)
        /// </summary>      
        [Column(ColumnName = "SL_3", Description = "", ColumnOrder = 22,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SL_3 { get; set; }

        /// <summary>
        /// BZSL(numeric)
        /// </summary>      
        [Column(ColumnName = "BZSL", Description = "", ColumnOrder = 23,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? BZSL { get; set; }

        /// <summary>
        /// JE(numeric)
        /// </summary>      
        [Column(ColumnName = "JE", Description = "", ColumnOrder = 24,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE { get; set; }

        /// <summary>
        /// JE_1(numeric)
        /// </summary>      
        [Column(ColumnName = "JE_1", Description = "", ColumnOrder = 25,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE_1 { get; set; }

        /// <summary>
        /// JE_2(numeric)
        /// </summary>      
        [Column(ColumnName = "JE_2", Description = "", ColumnOrder = 26,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE_2 { get; set; }

        /// <summary>
        /// JE_3(numeric)
        /// </summary>      
        [Column(ColumnName = "JE_3", Description = "", ColumnOrder = 27,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JE_3 { get; set; }

        /// <summary>
        /// BZJE(numeric)
        /// </summary>      
        [Column(ColumnName = "BZJE", Description = "", ColumnOrder = 28,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? BZJE { get; set; }

        /// <summary>
        /// CJ(numeric)
        /// </summary>      
        [Column(ColumnName = "CJ", Description = "", ColumnOrder = 29,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? CJ { get; set; }

        /// <summary>
        /// TJ(char)
        /// </summary>      
        [Column(ColumnName = "TJ", Description = "", ColumnOrder = 30,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string TJ { get; set; }

        /// <summary>
        /// TJRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "TJRQ", Description = "", ColumnOrder = 31,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? TJRQ { get; set; }

        /// <summary>
        /// XC(char)
        /// </summary>      
        [Column(ColumnName = "XC", Description = "", ColumnOrder = 32,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string XC { get; set; }

        /// <summary>
        /// XCRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "XCRQ", Description = "", ColumnOrder = 33,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? XCRQ { get; set; }

        /// <summary>
        /// YS(char)
        /// </summary>      
        [Column(ColumnName = "YS", Description = "", ColumnOrder = 34,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string YS { get; set; }

        /// <summary>
        /// YSRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "YSRQ", Description = "", ColumnOrder = 35,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? YSRQ { get; set; }

        /// <summary>
        /// JZ(char)
        /// </summary>      
        [Column(ColumnName = "JZ", Description = "", ColumnOrder = 36,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string JZ { get; set; }

        /// <summary>
        /// JZRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "JZRQ", Description = "", ColumnOrder = 37,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? JZRQ { get; set; }

        /// <summary>
        /// JS(char)
        /// </summary>      
        [Column(ColumnName = "JS", Description = "", ColumnOrder = 38,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string JS { get; set; }

        /// <summary>
        /// JSRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "JSRQ", Description = "", ColumnOrder = 39,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? JSRQ { get; set; }

        /// <summary>
        /// SH(char)
        /// </summary>      
        [Column(ColumnName = "SH", Description = "", ColumnOrder = 40,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string SH { get; set; }

        /// <summary>
        /// SHRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "SHRQ", Description = "", ColumnOrder = 41,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? SHRQ { get; set; }

        /// <summary>
        /// SP(char)
        /// </summary>      
        [Column(ColumnName = "SP", Description = "", ColumnOrder = 42,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string SP { get; set; }

        /// <summary>
        /// SPRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "SPRQ", Description = "", ColumnOrder = 43,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? SPRQ { get; set; }

        /// <summary>
        /// LL(char)
        /// </summary>      
        [Column(ColumnName = "LL", Description = "", ColumnOrder = 44,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string LL { get; set; }

        /// <summary>
        /// LLRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "LLRQ", Description = "", ColumnOrder = 45,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? LLRQ { get; set; }

        /// <summary>
        /// ZDR(varchar)
        /// </summary>      
        [Column(ColumnName = "ZDR", Description = "", ColumnOrder = 46,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string ZDR { get; set; }

        /// <summary>
        /// YSR(varchar)
        /// </summary>      
        [Column(ColumnName = "YSR", Description = "", ColumnOrder = 47,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string YSR { get; set; }

        /// <summary>
        /// JZR(varchar)
        /// </summary>      
        [Column(ColumnName = "JZR", Description = "", ColumnOrder = 48,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string JZR { get; set; }

        /// <summary>
        /// JSR(varchar)
        /// </summary>      
        [Column(ColumnName = "JSR", Description = "", ColumnOrder = 49,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string JSR { get; set; }

        /// <summary>
        /// SHR(varchar)
        /// </summary>      
        [Column(ColumnName = "SHR", Description = "", ColumnOrder = 50,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string SHR { get; set; }

        /// <summary>
        /// SPR(varchar)
        /// </summary>      
        [Column(ColumnName = "SPR", Description = "", ColumnOrder = 51,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string SPR { get; set; }

        /// <summary>
        /// LLR(varchar)
        /// </summary>      
        [Column(ColumnName = "LLR", Description = "", ColumnOrder = 52,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string LLR { get; set; }

        /// <summary>
        /// YXRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "YXRQ", Description = "", ColumnOrder = 53,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? YXRQ { get; set; }

        /// <summary>
        /// RQ_1(datetime)
        /// </summary>      
        [Column(ColumnName = "RQ_1", Description = "", ColumnOrder = 54,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? RQ_1 { get; set; }

        /// <summary>
        /// RQ_2(datetime)
        /// </summary>      
        [Column(ColumnName = "RQ_2", Description = "", ColumnOrder = 55,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? RQ_2 { get; set; }

        /// <summary>
        /// RQ_3(datetime)
        /// </summary>      
        [Column(ColumnName = "RQ_3", Description = "", ColumnOrder = 56,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? RQ_3 { get; set; }

        /// <summary>
        /// RQ_4(datetime)
        /// </summary>      
        [Column(ColumnName = "RQ_4", Description = "", ColumnOrder = 57,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? RQ_4 { get; set; }

        /// <summary>
        /// BZ(varchar)
        /// </summary>      
        [Column(ColumnName = "BZ", Description = "", ColumnOrder = 58,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 150, Precision = 150, Scale = 0, DefaultVal = "")]
        public string BZ { get; set; }

        /// <summary>
        /// BYZD1(char)
        /// </summary>      
        [Column(ColumnName = "BYZD1", Description = "", ColumnOrder = 59,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD1 { get; set; }

        /// <summary>
        /// BYZD2(char)
        /// </summary>      
        [Column(ColumnName = "BYZD2", Description = "", ColumnOrder = 60,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD2 { get; set; }

        /// <summary>
        /// BYZD3(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD3", Description = "", ColumnOrder = 61,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD3 { get; set; }

        /// <summary>
        /// BYZD4(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD4", Description = "", ColumnOrder = 62,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD4 { get; set; }

        /// <summary>
        /// BYZD5(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD5", Description = "", ColumnOrder = 63,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD5 { get; set; }

        /// <summary>
        /// BYZD6(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD6", Description = "", ColumnOrder = 64,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 100, Precision = 100, Scale = 0, DefaultVal = "")]
        public string BYZD6 { get; set; }

        /// <summary>
        /// BYZD7(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD7", Description = "", ColumnOrder = 65,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 100, Precision = 100, Scale = 0, DefaultVal = "")]
        public string BYZD7 { get; set; }

        /// <summary>
        /// BYZD8(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD8", Description = "", ColumnOrder = 66,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD8 { get; set; }

        /// <summary>
        /// BYZD9(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD9", Description = "", ColumnOrder = 67,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD9 { get; set; }

        /// <summary>
        /// BYZD10(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD10", Description = "", ColumnOrder = 68,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD10 { get; set; }

        /// <summary>
        /// BYZD11(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD11", Description = "", ColumnOrder = 69,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD11 { get; set; }

        /// <summary>
        /// BYZD12(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD12", Description = "", ColumnOrder = 70,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD12 { get; set; }

        /// <summary>
        /// BYZD13(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD13", Description = "", ColumnOrder = 71,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD13 { get; set; }

        /// <summary>
        /// BYZD14(datetime)
        /// </summary>      
        [Column(ColumnName = "BYZD14", Description = "", ColumnOrder = 72,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? BYZD14 { get; set; }

        /// <summary>
        /// BYZD15(datetime)
        /// </summary>      
        [Column(ColumnName = "BYZD15", Description = "", ColumnOrder = 73,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? BYZD15 { get; set; }

        /// <summary>
        /// ZS(varchar)
        /// </summary>      
        [Column(ColumnName = "ZS", Description = "", ColumnOrder = 74,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string ZS { get; set; }

        /// <summary>
        /// ZSR(varchar)
        /// </summary>      
        [Column(ColumnName = "ZSR", Description = "", ColumnOrder = 75,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string ZSR { get; set; }

        /// <summary>
        /// ZSRQ(datetime)
        /// </summary>      
        [Column(ColumnName = "ZSRQ", Description = "", ColumnOrder = 76,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? ZSRQ { get; set; }

        /// <summary>
        /// DM5(varchar)
        /// </summary>      
        [Column(ColumnName = "DM5", Description = "", ColumnOrder = 77,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DM5 { get; set; }

        /// <summary>
        /// DM5_1(varchar)
        /// </summary>      
        [Column(ColumnName = "DM5_1", Description = "", ColumnOrder = 78,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string DM5_1 { get; set; }

        /// <summary>
        /// DM6(varchar)
        /// </summary>      
        [Column(ColumnName = "DM6", Description = "", ColumnOrder = 79,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DM6 { get; set; }

        /// <summary>
        /// DM6_1(varchar)
        /// </summary>      
        [Column(ColumnName = "DM6_1", Description = "", ColumnOrder = 80,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DM6_1 { get; set; }

        /// <summary>
        /// BYZD16(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD16", Description = "", ColumnOrder = 81,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD16 { get; set; }

        /// <summary>
        /// BYZD17(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD17", Description = "", ColumnOrder = 82,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD17 { get; set; }

        /// <summary>
        /// BYZD18(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD18", Description = "", ColumnOrder = 83,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD18 { get; set; }

        /// <summary>
        /// BYZD19(int)
        /// </summary>      
        [Column(ColumnName = "BYZD19", Description = "", ColumnOrder = 84,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? BYZD19 { get; set; }

        /// <summary>
        /// BYZD20(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD20", Description = "", ColumnOrder = 85,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD20 { get; set; }

        /// <summary>
        /// HH(char)
        /// </summary>      
        [Column(ColumnName = "HH", Description = "", ColumnOrder = 86,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string HH { get; set; }

        /// <summary>
        /// ZDHHZQ(int)
        /// </summary>      
        [Column(ColumnName = "ZDHHZQ", Description = "", ColumnOrder = 87,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? ZDHHZQ { get; set; }

        /// <summary>
        /// WHHK(numeric)
        /// </summary>      
        [Column(ColumnName = "WHHK", Description = "", ColumnOrder = 88,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? WHHK { get; set; }

        /// <summary>
        /// ZDHHK(char)
        /// </summary>      
        [Column(ColumnName = "ZDHHK", Description = "", ColumnOrder = 89,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string ZDHHK { get; set; }

        /// <summary>
        /// JSHB(varchar)
        /// </summary>      
        [Column(ColumnName = "JSHB", Description = "", ColumnOrder = 90,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 10, Precision = 10, Scale = 0, DefaultVal = "")]
        public string JSHB { get; set; }

        /// <summary>
        /// HBHL(numeric)
        /// </summary>      
        [Column(ColumnName = "HBHL", Description = "", ColumnOrder = 91,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? HBHL { get; set; }

        /// <summary>
        /// FZXX(varchar)
        /// </summary>      
        [Column(ColumnName = "FZXX", Description = "", ColumnOrder = 92,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string FZXX { get; set; }

        /// <summary>
        /// FP(char)
        /// </summary>      
        [Column(ColumnName = "FP", Description = "", ColumnOrder = 93,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string FP { get; set; }

        /// <summary>
        /// FLOW_PATH(char)
        /// </summary>      
        [Column(ColumnName = "FLOW_PATH", Description = "", ColumnOrder = 94,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "'0'")]
        public string FLOW_PATH { get; set; }

        /// <summary>
        /// FLOW_STATUS(char)
        /// </summary>      
        [Column(ColumnName = "FLOW_STATUS", Description = "", ColumnOrder = 95,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "'0'")]
        public string FLOW_STATUS { get; set; }

        /// <summary>
        /// IsFocus(varchar)
        /// </summary>      
        [Column(ColumnName = "IsFocus", Description = "", ColumnOrder = 96,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string IsFocus { get; set; }

        /// <summary>
        /// XSFY(char)
        /// </summary>      
        [Column(ColumnName = "XSFY", Description = "", ColumnOrder = 97,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string XSFY { get; set; }

        /// <summary>
        /// ZRJE(numeric)
        /// </summary>      
        [Column(ColumnName = "ZRJE", Description = "", ColumnOrder = 98,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? ZRJE { get; set; }

        /// <summary>
        /// ZRHJE(numeric)
        /// </summary>      
        [Column(ColumnName = "ZRHJE", Description = "", ColumnOrder = 99,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? ZRHJE { get; set; }

        /// <summary>
        /// CY(char)
        /// </summary>      
        [Column(ColumnName = "CY", Description = "", ColumnOrder = 100,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string CY { get; set; }

        #endregion

    }
}
