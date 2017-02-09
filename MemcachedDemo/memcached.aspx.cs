using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MemcachedDemo
{
    public partial class memcached : System.Web.UI.Page
    {
        public static MemcachedClient cache;
        protected void Page_Load(object sender, EventArgs e)
        {
          //存入key为a，value为123的一个缓存
           // AMemcached.cache.Add("a", "123");
            
            //读出key为a的缓存值
           // var s = AMemcached.cache.Get("a");
            var meCache = new AMemcached("me").cache;

            string key = "user_info";//key值
           
            object obj = new object();
            if (meCache.KeyExists(key))
            {
                obj = meCache.Get(key);
                Test t = (Test)obj;
                Response.Write("序号：" + t.Id + ",年龄：" + t.age + "");
            }
            else {
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(conStr);
                conn.Open();
                string sql = "Select * From Test";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                Test t = new Test();
                while (dr.Read()) {
                    t.Id = Convert.ToInt32( dr[0].ToString());
                    t.age = Convert.ToInt32(dr[1].ToString());
                }
                dr.Close();
                conn.Close();
                meCache.Set(key, t, System.DateTime.Now.AddMinutes(2));
              　Response.Write("序号：" + t.Id + ",年龄：" + t.age + "");

            }
           // Response.Write(s);
        }

        
    }
}