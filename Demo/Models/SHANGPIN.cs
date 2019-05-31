using Leo.Data;
using System;

namespace ConsoleApp1.Models
{
    /// SHANGPIN
    /// </summary>    
    [Table(TableName = "SHANGPIN", Description = "SHANGPIN")]
    public class SHANGPIN 
    {
        #region 自動生成
        /// <summary>
        /// SPDM(varchar IsPrimaryKey NotNull)
        /// </summary>      
        [Column(ColumnName = "SPDM", Description = "", ColumnOrder = 1,
        TypeName = "varchar", IsPrimaryKey = true, Nullable = false, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string SPDM { get; set; }

        /// <summary>
        /// SPMC(varchar)
        /// </summary>      
        [Column(ColumnName = "SPMC", Description = "", ColumnOrder = 2,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string SPMC { get; set; }

        /// <summary>
        /// ZJF(varchar)
        /// </summary>      
        [Column(ColumnName = "ZJF", Description = "", ColumnOrder = 3,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 80, Precision = 80, Scale = 0, DefaultVal = "")]
        public string ZJF { get; set; }

        /// <summary>
        /// DWMC(varchar)
        /// </summary>      
        [Column(ColumnName = "DWMC", Description = "", ColumnOrder = 4,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string DWMC { get; set; }

        /// <summary>
        /// FJSX1(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX1", Description = "", ColumnOrder = 5,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string FJSX1 { get; set; }

        /// <summary>
        /// FJSX2(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX2", Description = "", ColumnOrder = 6,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string FJSX2 { get; set; }

        /// <summary>
        /// FJSX3(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX3", Description = "", ColumnOrder = 7,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string FJSX3 { get; set; }

        /// <summary>
        /// FJSX4(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX4", Description = "", ColumnOrder = 8,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string FJSX4 { get; set; }

        /// <summary>
        /// FJSX5(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX5", Description = "", ColumnOrder = 9,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string FJSX5 { get; set; }

        /// <summary>
        /// FJSX6(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX6", Description = "", ColumnOrder = 10,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 6, Precision = 6, Scale = 0, DefaultVal = "")]
        public string FJSX6 { get; set; }

        /// <summary>
        /// BZJJ(numeric)
        /// </summary>      
        [Column(ColumnName = "BZJJ", Description = "", ColumnOrder = 11,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? BZJJ { get; set; }

        /// <summary>
        /// JJ1(numeric)
        /// </summary>      
        [Column(ColumnName = "JJ1", Description = "", ColumnOrder = 12,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JJ1 { get; set; }

        /// <summary>
        /// JJ2(numeric)
        /// </summary>      
        [Column(ColumnName = "JJ2", Description = "", ColumnOrder = 13,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? JJ2 { get; set; }

        /// <summary>
        /// BZSJ(numeric)
        /// </summary>      
        [Column(ColumnName = "BZSJ", Description = "", ColumnOrder = 14,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? BZSJ { get; set; }

        /// <summary>
        /// SJ1(numeric)
        /// </summary>      
        [Column(ColumnName = "SJ1", Description = "", ColumnOrder = 15,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SJ1 { get; set; }

        /// <summary>
        /// SJ2(numeric)
        /// </summary>      
        [Column(ColumnName = "SJ2", Description = "", ColumnOrder = 16,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SJ2 { get; set; }

        /// <summary>
        /// SJ3(numeric)
        /// </summary>      
        [Column(ColumnName = "SJ3", Description = "", ColumnOrder = 17,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SJ3 { get; set; }

        /// <summary>
        /// SJ4(numeric)
        /// </summary>      
        [Column(ColumnName = "SJ4", Description = "", ColumnOrder = 18,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? SJ4 { get; set; }

        /// <summary>
        /// PIC(varchar)
        /// </summary>      
        [Column(ColumnName = "PIC", Description = "", ColumnOrder = 19,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 50, Precision = 50, Scale = 0, DefaultVal = "")]
        public string PIC { get; set; }

        /// <summary>
        /// BZDW(varchar)
        /// </summary>      
        [Column(ColumnName = "BZDW", Description = "", ColumnOrder = 20,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BZDW { get; set; }

        /// <summary>
        /// BZSL(int)
        /// </summary>      
        [Column(ColumnName = "BZSL", Description = "", ColumnOrder = 21,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "0")]
        public int? BZSL { get; set; }

        /// <summary>
        /// SPTM(varchar)
        /// </summary>      
        [Column(ColumnName = "SPTM", Description = "", ColumnOrder = 22,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string SPTM { get; set; }

        /// <summary>
        /// BZHU(varchar)
        /// </summary>      
        [Column(ColumnName = "BZHU", Description = "", ColumnOrder = 23,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 450, Precision = 450, Scale = 0, DefaultVal = "")]
        public string BZHU { get; set; }

        /// <summary>
        /// TZSY(char)
        /// </summary>      
        [Column(ColumnName = "TZSY", Description = "", ColumnOrder = 24,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string TZSY { get; set; }

        /// <summary>
        /// KCSL(numeric)
        /// </summary>      
        [Column(ColumnName = "KCSL", Description = "", ColumnOrder = 25,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? KCSL { get; set; }

        /// <summary>
        /// CBJE(numeric)
        /// </summary>      
        [Column(ColumnName = "CBJE", Description = "", ColumnOrder = 26,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "0")]
        public decimal? CBJE { get; set; }

        /// <summary>
        /// BYZD1(char)
        /// </summary>      
        [Column(ColumnName = "BYZD1", Description = "", ColumnOrder = 27,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD1 { get; set; }

        /// <summary>
        /// BYZD2(char)
        /// </summary>      
        [Column(ColumnName = "BYZD2", Description = "", ColumnOrder = 28,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD2 { get; set; }

        /// <summary>
        /// BYZD3(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD3", Description = "", ColumnOrder = 29,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD3 { get; set; }

        /// <summary>
        /// BYZD4(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD4", Description = "", ColumnOrder = 30,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD4 { get; set; }

        /// <summary>
        /// BYZD5(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD5", Description = "", ColumnOrder = 31,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD5 { get; set; }

        /// <summary>
        /// BYZD6(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD6", Description = "", ColumnOrder = 32,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 150, Precision = 150, Scale = 0, DefaultVal = "")]
        public string BYZD6 { get; set; }

        /// <summary>
        /// BYZD7(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD7", Description = "", ColumnOrder = 33,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 150, Precision = 150, Scale = 0, DefaultVal = "")]
        public string BYZD7 { get; set; }

        /// <summary>
        /// BYZD8(int)
        /// </summary>      
        [Column(ColumnName = "BYZD8", Description = "", ColumnOrder = 34,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? BYZD8 { get; set; }

        /// <summary>
        /// BYZD9(int)
        /// </summary>      
        [Column(ColumnName = "BYZD9", Description = "", ColumnOrder = 35,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? BYZD9 { get; set; }

        /// <summary>
        /// BYZD10(int)
        /// </summary>      
        [Column(ColumnName = "BYZD10", Description = "", ColumnOrder = 36,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? BYZD10 { get; set; }

        /// <summary>
        /// BYZD11(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD11", Description = "", ColumnOrder = 37,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD11 { get; set; }

        /// <summary>
        /// BYZD12(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD12", Description = "", ColumnOrder = 38,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD12 { get; set; }

        /// <summary>
        /// BYZD13(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD13", Description = "", ColumnOrder = 39,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD13 { get; set; }

        /// <summary>
        /// BYZD14(datetime)
        /// </summary>      
        [Column(ColumnName = "BYZD14", Description = "", ColumnOrder = 40,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? BYZD14 { get; set; }

        /// <summary>
        /// BYZD15(datetime)
        /// </summary>      
        [Column(ColumnName = "BYZD15", Description = "", ColumnOrder = 41,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? BYZD15 { get; set; }

        /// <summary>
        /// BYZD16(char)
        /// </summary>      
        [Column(ColumnName = "BYZD16", Description = "", ColumnOrder = 42,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD16 { get; set; }

        /// <summary>
        /// BYZD17(char)
        /// </summary>      
        [Column(ColumnName = "BYZD17", Description = "", ColumnOrder = 43,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "")]
        public string BYZD17 { get; set; }

        /// <summary>
        /// BYZD18(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD18", Description = "", ColumnOrder = 44,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD18 { get; set; }

        /// <summary>
        /// BYZD19(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD19", Description = "", ColumnOrder = 45,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD19 { get; set; }

        /// <summary>
        /// BYZD20(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD20", Description = "", ColumnOrder = 46,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string BYZD20 { get; set; }

        /// <summary>
        /// BYZD21(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD21", Description = "", ColumnOrder = 47,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 150, Precision = 150, Scale = 0, DefaultVal = "")]
        public string BYZD21 { get; set; }

        /// <summary>
        /// BYZD22(varchar)
        /// </summary>      
        [Column(ColumnName = "BYZD22", Description = "", ColumnOrder = 48,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 150, Precision = 150, Scale = 0, DefaultVal = "")]
        public string BYZD22 { get; set; }

        /// <summary>
        /// BYZD23(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD23", Description = "", ColumnOrder = 49,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD23 { get; set; }

        /// <summary>
        /// BYZD24(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD24", Description = "", ColumnOrder = 50,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD24 { get; set; }

        /// <summary>
        /// BYZD25(numeric)
        /// </summary>      
        [Column(ColumnName = "BYZD25", Description = "", ColumnOrder = 51,
        TypeName = "numeric", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 9, Precision = 18, Scale = 4, DefaultVal = "")]
        public decimal? BYZD25 { get; set; }

        /// <summary>
        /// FJSX7(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX7", Description = "", ColumnOrder = 52,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX7 { get; set; }

        /// <summary>
        /// FJSX8(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX8", Description = "", ColumnOrder = 53,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX8 { get; set; }

        /// <summary>
        /// FJSX9(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX9", Description = "", ColumnOrder = 54,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX9 { get; set; }

        /// <summary>
        /// FJSX10(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX10", Description = "", ColumnOrder = 55,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX10 { get; set; }

        /// <summary>
        /// FJSX11(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX11", Description = "", ColumnOrder = 56,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX11 { get; set; }

        /// <summary>
        /// FJSX12(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX12", Description = "", ColumnOrder = 57,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX12 { get; set; }

        /// <summary>
        /// FJSX13(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX13", Description = "", ColumnOrder = 58,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX13 { get; set; }

        /// <summary>
        /// FJSX14(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX14", Description = "", ColumnOrder = 59,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX14 { get; set; }

        /// <summary>
        /// FJSX15(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX15", Description = "", ColumnOrder = 60,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX15 { get; set; }

        /// <summary>
        /// FJSX16(varchar)
        /// </summary>      
        [Column(ColumnName = "FJSX16", Description = "", ColumnOrder = 61,
        TypeName = "varchar", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 20, Precision = 20, Scale = 0, DefaultVal = "")]
        public string FJSX16 { get; set; }

        /// <summary>
        /// PRINTCOUNT(int)
        /// </summary>      
        [Column(ColumnName = "PRINTCOUNT", Description = "", ColumnOrder = 62,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? PRINTCOUNT { get; set; }

        /// <summary>
        /// PRINTTIME(datetime)
        /// </summary>      
        [Column(ColumnName = "PRINTTIME", Description = "", ColumnOrder = 63,
        TypeName = "datetime", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 8, Precision = 23, Scale = 3, DefaultVal = "")]
        public DateTime? PRINTTIME { get; set; }

        /// <summary>
        /// LastChanged(timestamp NotNull)
        /// </summary>      
        [Column(ColumnName = "LastChanged", Description = "", ColumnOrder = 64,
        TypeName = "timestamp", IsPrimaryKey = false, Nullable = false, IsIdentity = false,
        Length = 8, Precision = 8, Scale = 0, DefaultVal = "")]
        public DateTime LastChanged { get; set; }

        /// <summary>
        /// State(int)
        /// </summary>      
        [Column(ColumnName = "State", Description = "", ColumnOrder = 65,
        TypeName = "int", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 4, Precision = 10, Scale = 0, DefaultVal = "")]
        public int? State { get; set; }

        /// <summary>
        /// ONLINE(char)
        /// </summary>      
        [Column(ColumnName = "ONLINE", Description = "", ColumnOrder = 66,
        TypeName = "char", IsPrimaryKey = false, Nullable = true, IsIdentity = false,
        Length = 1, Precision = 1, Scale = 0, DefaultVal = "'0'")]
        public string ONLINE { get; set; }

        #endregion

    }
}
