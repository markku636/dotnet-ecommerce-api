using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// {{ModelDescription}}
    /// </summary>
    [SugarTable("{{RawTableName}}")]
    public partial class {{ModelClassName}}
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public {{ModelClassName}}()
        {
        }
		{% for field in ModelFields -%}
        /// <summary>
        /// {{field.ColumnDescription}}
        /// </summary>
        [Display(Name = "{{field.ColumnDescription}}")]
        {% if field.IsIdentity == true and field.IsPrimarykey == true -%}
[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        {% elsif field.IsIdentity == true and field.IsPrimarykey == false -%}
[SugarColumn(IsIdentity = true)]
        {% elsif field.IsIdentity == false and field.IsPrimarykey == true -%}
[SugarColumn(IsPrimaryKey = true)]
{% endif -%}
{% if field.IsNullable == false -%}[Required(ErrorMessage = "請輸入{0}")]{% endif -%}
{% if field.DataType == 'nvarchar' and field.Length > 0 -%}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超過{1}字")]{% endif -%}
{% if field.DataType == 'varchar' and field.Length > 0 -%}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超過{1}字")]{% endif -%}
{% if field.DataType == 'nchar' and field.Length > 0 -%}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超過{1}字")]{% endif -%}
{% if field.DataType == 'nchar' or field.DataType == 'nvarchar' or field.DataType == 'varchar'  or field.DataType == 'text' or field.DataType == 'binary' or field.DataType == 'char' or field.DataType == 'xml' %}
		  public System.String {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'int' and field.IsNullable == false %}
        public System.Int32 {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'int' and field.IsNullable == true %}
        public System.Int32? {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'smallint' and field.IsNullable == false %}
        public System.Int32 {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'smallint' and field.IsNullable == true %}
        public System.Int32? {{field.DbColumnName}}  { get; set; }        
        {% elsif field.DataType == 'tinyint' and field.IsNullable == false and field.Length == 1 %}
        public System.Boolean {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'tinyint' and field.IsNullable == true and field.Length == 1 %}
        public System.Boolean? {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'tinyint' and field.IsNullable == false and field.Length != 1 %}
        public System.Int32 {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'tinyint' and field.IsNullable == true and field.Length != 1 %}
        public System.Int32? {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'bigint' and field.IsNullable == false %}
        public System.Int64 {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'bigint' and field.IsNullable == true %}
        public System.Int64? {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'float' and field.IsNullable == false %}
        public float {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'float' and field.IsNullable == true %}
        public float? {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'bit' and field.IsNullable == false %}
        public System.Boolean {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'bit' and field.IsNullable == true %}
        public System.Boolean? {{field.DbColumnName}}  { get; set; }        
        {% elsif field.DataType == 'datetime' and field.IsNullable == false %}
        public System.DateTime {{field.DbColumnName}} { get; set; }
        {% elsif field.DataType == 'datetime' and field.IsNullable == true %}
        public System.DateTime? {{field.DbColumnName}} { get; set; }
        {% elsif field.DataType == 'smalldatetime' and field.IsNullable == false %}
        public System.DateTime {{field.DbColumnName}} { get; set; }
        {% elsif field.DataType == 'smalldatetime' and field.IsNullable == true %}
        public System.DateTime? {{field.DbColumnName}} { get; set; }
        {% elsif field.DataType == 'date' and field.IsNullable == false %}
        public System.DateTime {{field.DbColumnName}} { get; set; }
        {% elsif field.DataType == 'date' and field.IsNullable == true %}
        public System.DateTime? {{field.DbColumnName}} { get; set; }
        {% elsif field.DataType == 'uniqueidentifier' and field.IsNullable == false %}
        public System.Guid {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'uniqueidentifier' and field.IsNullable == true %}
        public System.Guid? {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'decimal' and field.IsNullable == false %}
        public System.Decimal {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'decimal' and field.IsNullable == true %}
        public System.Decimal? {{field.DbColumnName}}  { get; set; }
        {% elsif (field.DataType == 'money' or field.DataType == 'smallmoney') and field.IsNullable == false %}
        public System.Decimal {{field.DbColumnName}}  { get; set; }
        {% elsif (field.DataType == 'money' or field.DataType == 'smallmoney') and field.IsNullable == true %}
        public System.Decimal? {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'numeric' and field.IsNullable == false %}
        public System.Decimal {{field.DbColumnName}}  { get; set; }
        {% elsif field.DataType == 'numeric' and field.IsNullable == true %}
        public System.Decimal? {{field.DbColumnName}}  { get; set; }
        {% endif %}
		{% endfor %}
    }
}
