
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.MemberSite.Model.Const;
using Microsoft.Extensions.Hosting;


namespace EC226.Utility.Helper
{
   public class MailHelper
   {
      public static string PrepareMailTemplate(IHostEnvironment _env, string langFolderName, string templateName)
      {
         if(string.IsNullOrWhiteSpace(langFolderName))
         {
            langFolderName = HardCodeKey.EmailTemplateFolder.DE;
         }

         var path = Path.Combine(_env.ContentRootPath, "Templates", langFolderName, $"{templateName}.html");
         var templateContent = File.ReadAllText(path);

         return templateContent;
      }

      public static string ReplaceTemplate(string template, Dictionary<string, string> paramDic)
      {
         foreach (var param in paramDic)
         {
            template = template.Replace("{~" + param.Key + "~}", param.Value);
         }

         return template;
      }
   }
}
