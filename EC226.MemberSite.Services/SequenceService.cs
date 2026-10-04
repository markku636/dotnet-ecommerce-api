using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.Enums;
using EC226.Repository.UnitOfWork;

namespace EC226.MemberSite.Services
{
   public class SequenceService
   {
      private SequenceService() { }
      private static object thisLock = new object();
      public static SequenceService Current
      {
         get
         {
            return Inner.Current;
         }
      }

      //寫入Number
      public string GetNo(SequenceType sequenceType)
      {
         //解決多線程的問題，但無法解決多IIS主機的問題
         lock (thisLock)
         {
            int current = 0;
            var toDate = DateTime.UtcNow.Date.ToString("yyyyMMdd");

            var iUnitOfWork = new UnitOfWork();
            try
            {               
               iUnitOfWork.BeginTran();

               var thisSequence = iUnitOfWork.GetDbClient().Queryable<Sequences>().First(x => x.SequenceType == (int)sequenceType && x.SequenceDate == toDate);

               //如果沒有新增一筆
               if (thisSequence == null)
               {
                  current = current + 1;
                  Sequences sequence = new Sequences()
                  {

                     SequenceDate = toDate,
                     Number = current
                  };
                  iUnitOfWork.GetDbClient().Insertable<Sequences>(sequence).ExecuteCommand();
               }
               else
               {


                  thisSequence.Number = thisSequence.Number + 1;
                  current = thisSequence.Number;
               }


               iUnitOfWork.CommitTran();
            }
            catch (Exception ex)
            {
               iUnitOfWork.RollbackTran();

            }
       
            return toDate + string.Format("{0:0000}", current);
         }

      }

      class Inner
      {
         static Inner() { }
         internal static readonly SequenceService Current = new SequenceService();
      }
   }
}
