using System;

namespace EC226.Utility.JWT
{
    public class Payload<T> where T : class, new()
    {
        public Payload()
        {
            Data = new T();
        }

        public T Data = new T();

        public int ExpiredDate { get; set; }

        public bool IsExpired
        {
            get
            {
                return this.ExpiredDate < Convert.ToInt32(
                           (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));
            }
        }
    }
}
