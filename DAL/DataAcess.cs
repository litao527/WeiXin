using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataAcess
    {
        static WeiXinEntities db = new WeiXinEntities();
        public static int Add(Message model)
        {
            db.Message.Add(model);
            return db.SaveChanges();
        }

        public static IEnumerable<Message> GetAllEntitys()
        {
            return db.Message.Where(c => c.Id != 0);
        }

        public static int AddBlogs(List<CnBlog> blogs)
        {
            try
            {
                if (blogs != null && blogs.Count > 0)
                {
                    foreach (var item in blogs)
                    {
                        if (db.CnBlog.Where(c => c.ArticleId == item.ArticleId).Count() > 0)
                        {
                            continue;
                        }
                        else
                        {
                            db.CnBlog.Add(item);
                        }
                    }
                    return db.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
