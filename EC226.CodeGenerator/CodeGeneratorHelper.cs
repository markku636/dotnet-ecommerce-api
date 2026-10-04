using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using EC226.MemberSite.Model.ViewModels.Basics;
using DotLiquid;
using SqlSugar;
using EC226.MemberSite.Model.FromBody;
using System.Globalization;
using System.Text.RegularExpressions;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace EC226.CodeGenerator
{
   /// <summary>
   /// 自動產生程式碼
   /// </summary>
   public static class CodeGeneratorHelper
   {

      private static string RemoveSpecWord(string str)
      {
         str = str.Replace("$", "");
         str = str.Replace("_", "");

         return str;
      }

      /// <summary>
      /// 單表產生對應數據
      /// </summary>
      /// <param name="tableName">表名稱</param>
      /// <param name="tableDescription">表說明</param>
      /// <param name="columns">表字段</param>
      public static byte[] CodeGenerator(string tableName, string tableDescription, List<DbColumnInfo> columns, string fileType)
      {
         //ModelClassName
         //ModelName
         //ModelFields  Name Comment
         var dt = DateTime.Now;
         byte[] data;
         var obj = new
         {
            ModelCreateTime = dt,
            ModelName = RemoveSpecWord(tableName),
            ModelDescription = !string.IsNullOrWhiteSpace(tableDescription) ? RemoveSpecWord(tableDescription).Replace("\r\n", string.Empty) : RemoveSpecWord(tableName),
            ModelClassName = RemoveSpecWord(tableName),
            RawTableName = tableName,
            ModelFields = columns.Select(r => new
            {
               r.DbColumnName,
               ColumnDescription = !string.IsNullOrEmpty(r.ColumnDescription) ? r.ColumnDescription.Replace("\r\n", string.Empty) : r.DbColumnName.Replace("\r\n", string.Empty),
               r.DataType,
               r.DecimalDigits,
               r.DefaultValue,
               r.IsIdentity,
               r.IsNullable,
               r.IsPrimarykey,
               //Length = (r.DataType == "nvarchar" && r.Length > 0) ? r.Length / 2 : r.Length,
               r.Length,
               r.PropertyName,
               r.PropertyType,
               r.Scale,
               r.TableId,
               r.TableName,
               r.Value
            }).ToArray(),
            PrimaryKey = columns.FirstOrDefault(x => x.IsPrimarykey)?.DbColumnName
         };
         var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CodeGeneratorHelper)).Assembly;
         using (MemoryStream ms = new MemoryStream())
         {
            using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, false))
            {
               string file;
               string result;
               Template template;

               switch (fileType)
               {
                  case "AllFiles":
                     //Controller
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Controllers.Controller.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/Api.Startup/Controllers/" + RemoveSpecWord(tableName) + "Controller.cs", result);

                        ZipArchiveEntry entry4 = zip.CreateEntry("Controllers/" + RemoveSpecWord(tableName) + "Controller.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry4.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     //IRespository
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.IRepository/I" + RemoveSpecWord(tableName) + "Repository.cs", result);

                        ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + RemoveSpecWord(tableName) + "Repository.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     //Respository
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.Repository/" + RemoveSpecWord(tableName) + "Repository.cs", result);

                        ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + RemoveSpecWord(tableName) + "Repository.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }

                     //IServices
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.IServices/I" + RemoveSpecWord(tableName) + "Services.cs", result);

                        ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + RemoveSpecWord(tableName) + "Services.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     //Services
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.Services/" + RemoveSpecWord(tableName) + "Services.cs", result);

                        ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + RemoveSpecWord(tableName) + "Services.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }

                     //Model
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.Model/Entities/" + RemoveSpecWord(tableName) + ".cs", result);

                        ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + RemoveSpecWord(tableName) + ".cs");
                        using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }

                     break;
                  case "EntityFiles":
                     //Model
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.Model/Entities/" + RemoveSpecWord(tableName) + ".cs", result);

                        ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + RemoveSpecWord(tableName) + ".cs");
                        using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     break;
                  case "ServicesFiles":
                     //IServices
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.IServices/I" + RemoveSpecWord(tableName) + "Services.cs", result);

                        ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + RemoveSpecWord(tableName) + "Services.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     //Services
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.Services/" + RemoveSpecWord(tableName) + "Services.cs", result);

                        ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + RemoveSpecWord(tableName) + "Services.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     break;
                  case "InterFaceFiles":
                     //IRespository
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.IRepository/I" + RemoveSpecWord(tableName) + "Repository.cs", result);

                        ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + RemoveSpecWord(tableName) + "Repository.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     //Respository
                     using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                     {
                        file = reader.ReadToEnd();
                        template = Template.Parse(file);
                        result = template.Render(Hash.FromAnonymousObject(obj));
                        WriteStringToFile("/EC226.MemberSite.Repository/" + RemoveSpecWord(tableName) + "Repository.cs", result);

                        ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + RemoveSpecWord(tableName) + "Repository.cs");
                        using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                        {
                           entryStream.Write(result);
                        }
                     }
                     break;
               }

            }
            data = ms.ToArray();
         }
         return data;
      }



      /// <summary>
      /// 產生整個資料庫檔案
      /// </summary>
      /// <param name="tableName">表名稱</param>
      /// <param name="tableDescription">表說明</param>
      /// <param name="columns">表字段</param>
      public static byte[] CodeGeneratorAll(List<DbTableInfoAndColumns> dbModels, string fileType)
      {
         //ModelClassName
         //ModelName
         //ModelFields  Name Comment

         byte[] data;

         var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CodeGeneratorHelper)).Assembly;
         using (MemoryStream ms = new MemoryStream())
         {
            using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, false))
            {
               string file;
               string result;
               Template template;

               foreach (var item in dbModels)
               {


                  var obj = new
                  {
                     ModelName = RemoveSpecWord(item.Name),

                     ModelDescription = !string.IsNullOrWhiteSpace(item.Description) ? RemoveSpecWord(item.Description).Replace("\r\n", string.Empty) : RemoveSpecWord(item.Name),
                     RawTableName = item.Name,
                     ModelClassName = RemoveSpecWord(item.Name),
                     ModelFields = item.columns.Select(r => new
                     {
                        r.DbColumnName,
                        ColumnDescription = !string.IsNullOrEmpty(r.ColumnDescription) ? r.ColumnDescription.Replace("\r\n", string.Empty) : r.DbColumnName.Replace("\r\n", string.Empty),
                        r.DataType,
                        r.DecimalDigits,
                        r.DefaultValue,
                        r.IsIdentity,
                        r.IsNullable,
                        r.IsPrimarykey,
                        Length = (r.DataType == "nvarchar" && r.Length > 0) ? r.Length / 2 : r.Length,
                        r.PropertyName,
                        r.PropertyType,
                        r.Scale,
                        r.TableId,
                        r.TableName,
                        r.Value
                     }).ToArray(),
                     PrimaryKey = item.columns.FirstOrDefault(x => x.IsPrimarykey)?.DbColumnName
                  };

                  switch (fileType)
                  {
                     case "AllFiles":
                        //Controller
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Controllers.Controller.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));


                           WriteStringToFile("/EC226.MemberSite.Api/Controllers/" + RemoveSpecWord(item.Name) + "Controller.cs", result);

                           ZipArchiveEntry entry4 = zip.CreateEntry("Controllers/" + RemoveSpecWord(item.Name) + "Controller.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry4.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        //IRespository
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));
                           WriteStringToFile("/EC226.MemberSite.IRepository/I" + item.Name + "Repository.cs", result);

                           ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + item.Name + "Repository.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        //Respository
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));

                           WriteStringToFile("/EC226.MemberSite.Repository/" + RemoveSpecWord(item.Name) + "Repository.cs", result);

                           ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + RemoveSpecWord(item.Name) + "Repository.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }

                        //IServices
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));
                           WriteStringToFile("/EC226.MemberSite.IServices/I" + item.Name + "Services.cs", result);

                           ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + item.Name + "Services.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        //Services
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));
                           WriteStringToFile("/EC226.MemberSite.Services/" + RemoveSpecWord(item.Name) + "Services.cs", result);

                           ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + RemoveSpecWord(item.Name) + "Services.cs");


                           using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }

                        //Model
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));

                           WriteStringToFile("/EC226.MemberSite.Model/Entities/" + RemoveSpecWord(item.Name) + ".cs", result);
                           ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + RemoveSpecWord(item.Name) + ".cs");
                           using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }


                        break;
                     case "EntityFiles":
                        //Model
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.DbModel.Model.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));
                           WriteStringToFile("/EC226.MemberSite.Model/Entities/" + RemoveSpecWord(item.Name) + ".cs", result);

                           ZipArchiveEntry entry1 = zip.CreateEntry("Entity/" + RemoveSpecWord(item.Name) + ".cs");
                           using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        break;
                     case "ServicesFiles":
                        //IServices
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.IServices.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));
                           WriteStringToFile("/EC226.MemberSite.IServices/I" + item.Name + "Services.cs", result);

                           ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + item.Name + "Services.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        //Services
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Services.Services.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));

                           WriteStringToFile("/EC226.MemberSite.Services/" + RemoveSpecWord(item.Name) + "Services.cs", result);
                           ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + RemoveSpecWord(item.Name) + "Services.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        break;
                     case "InterFaceFiles":
                        //IRespository
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.IRepository.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));

                           WriteStringToFile("/EC226.MemberSite.IRepository/I" + item.Name + "Repository.cs", result);
                           ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + item.Name + "Repository.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        //Respository
                        using (var reader = new StreamReader(assembly.GetManifestResourceStream("EC226.CodeGenerator.CrudTemplete.Repositories.Repository.tpl"), Encoding.UTF8))
                        {
                           file = reader.ReadToEnd();
                           template = Template.Parse(file);
                           result = template.Render(Hash.FromAnonymousObject(obj));
                           WriteStringToFile("/EC226.MemberSite.Repository/" + RemoveSpecWord(item.Name) + "Repository.cs", result);

                           ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + RemoveSpecWord(item.Name) + "Repository.cs");
                           using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                           {
                              entryStream.Write(result);
                           }
                        }
                        break;
                  }
               }


            }
            data = ms.ToArray();
         }
         return data;
      }
      static void WriteStringToFile(string path, string content)
      {

         string currentDirectory = Directory.GetCurrentDirectory();
         DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);
         DirectoryInfo parentDirectory = directoryInfo.Parent;

         string fullPath = parentDirectory.FullName + path;

         // 檢查資料夾是否存在，如果不存在，則創建
         string directory = Path.GetDirectoryName(fullPath);
         if (!Directory.Exists(directory))
         {
            Directory.CreateDirectory(directory);
         }

         // 將字串寫入檔案，如果檔案已存在，這將覆蓋它
         File.WriteAllText(fullPath, content);
      }

   }
}
