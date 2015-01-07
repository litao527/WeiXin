using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public class CnblogsOperation
    {
        public static List<CnBlog> GetCnblogInfo()
        {
            try
            {
                /*
                新闻目录
                热门新闻:http://wcf.open.cnblogs.com/news/hot/10
                数字表示当前页要加载的新闻数目
                最近新闻:http://wcf.open.cnblogs.com/news/recent/10
                数字表示当前页要加载的新闻数目
                推荐新闻: http://wcf.open.cnblogs.com/news/recommend/paged/1/5
                数字表示当前页要加载的新闻数目，可同过改变数字加载更多。
                新闻内容
                http://wcf.open.cnblogs.com/news/item/199054
                数字表示要获取的当前新闻的id。
                新闻评论
                http://wcf.open.cnblogs.com/news/item/199054/comments/1/5
                199054表示新闻id(新闻的id可以通过新闻目录接口获取的数据中获取)，1表示评论的页数，5表示每页要加载的评论数。
                博客目录
                所有博客：1.http://wcf.open.cnblogs.com/blog/sitehome/recent/5。
                2.http://wcf.open.cnblogs.com/blog/sitehome/paged/1/10
                48小时阅读排行：http://wcf.open.cnblogs.com/blog/48HoursTopViewPosts/5
                十天推荐排行: http://wcf.open.cnblogs.com/blog/TenDaysTopDiggPosts/5
                5代表当前页要加载的数目，当前只能加一页，可以累加数字加载更多。
                博客内容：http://wcf.open.cnblogs.com/blog/post/body/3535626
                335626代表的博客的的id，博客id可以通过获取的博客目录中获得。
                博客评论：http://wcf.open.cnblogs.com/blog/post/3535784/comments/1/5
                335626代表的博客的的id，1代表页数，5代表当前页加载的评论数，可以他能够过改变1或者5来加载更多。
                5．搜索博主
                http://wcf.open.cnblogs.com/blog/bloggers/search?t=721881283
                t后面跟的是博客园的帐号
                */

                string blogUrl = "http://wcf.open.cnblogs.com/blog/sitehome/recent/5";
                HttpWebRequest webRequest = WebRequest.Create(blogUrl) as HttpWebRequest;

                StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string data = responseReader.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                XmlNode root = doc.DocumentElement;
                XmlNodeList listNodes = root.ChildNodes;
                List<CnBlog> cnblogs = new List<CnBlog>();

                foreach (var item in listNodes)
                {
                    XmlElement xe = item as XmlElement;
                    if (xe.Name == "entry")
                    {
                        CnBlog entry = new CnBlog();
                        foreach (XmlNode xmlNode in xe.ChildNodes)
                        {
                            if (xmlNode.Name == "id")
                            {
                                entry.ArticleId = xmlNode.InnerText;
                            }
                            if (xmlNode.Name == "title")
                            {
                                entry.Title = xmlNode.InnerText;
                            }
                            if (xmlNode.Name == "summary")
                            {
                                entry.Summary = xmlNode.InnerText;
                            }
                            if (xmlNode.Name == "link")
                            {
                                entry.Link = xmlNode.Attributes.Item(1).Value;
                            }
                            if (xmlNode.Name == "published")
                            {
                                entry.Published = xmlNode.InnerText;
                            }
                            if (xmlNode.Name == "updated")
                            {
                                entry.Updated = xmlNode.InnerText;
                            }
                        }
                        cnblogs.Add(entry);
                    }
                }
                //生成静态页
                List<CnBlog> newListBlogs = new List<CnBlog>();
                if (cnblogs != null && cnblogs.Count > 0)
                {
                    newListBlogs = CnblogsOperation.GenerationBlogToHtml(cnblogs);
                }

                return newListBlogs;
                //return cnblogs;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                throw;
            }
        }

        public static List<CnBlog> GenerationBlogToHtml(List<CnBlog> listBlogs)
        {
            try
            {
                List<CnBlog> newListBlogs = new List<CnBlog>();
                if (listBlogs != null && listBlogs.Count > 0)
                {
                    foreach (var item in listBlogs)
                    {
                        string blogUrl = "http://wcf.open.cnblogs.com/blog/post/body/";
                        //HttpWebRequest webRequest = null;

                        //StreamReader responseReader = null;
                        //blogUrl += item.ArticleId;
                        //webRequest = WebRequest.Create(blogUrl) as HttpWebRequest;
                        //responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                        //data = responseReader.ReadToEnd();
                        //XmlDocument doc = new XmlDocument();
                        //doc.LoadXml(data);
                        //XmlNode node = doc.SelectSingleNode("/string");
                        //string content = node.InnerText;

                        blogUrl += item.ArticleId;
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(blogUrl);
                        HttpWebResponse res;
                        Encoding strEncode = Encoding.GetEncoding("UTF-8");
                        try
                        {
                            res = (HttpWebResponse)req.GetResponse();
                        }
                        catch (WebException ex)
                        {
                            res = (HttpWebResponse)ex.Response;
                        }
                        StreamReader sr1 = new StreamReader(res.GetResponseStream(), strEncode);
                        string strHtml = sr1.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(strHtml);
                        XmlNode node = doc.SelectSingleNode("/string");
                        string content = node.InnerText;
                        //生成静态页
                        Encoding encode = Encoding.GetEncoding("UTF-8");
                        string htmlDir = AppDomain.CurrentDomain.BaseDirectory + "blogs\\";
                        if (!Directory.Exists(htmlDir))
                        {
                            Directory.CreateDirectory(htmlDir);
                        }
                        string templatePath = htmlDir + "template.html";
                        StreamReader sr = new StreamReader(templatePath, encode);
                        string templateText = sr.ReadToEnd();
                        string htmlText = templateText.Replace("@title@", item.Title);
                        htmlText = htmlText.Replace("@blogsUrl@", item.Link);
                        htmlText = htmlText.Replace("@content@", content);
                        string htmlfilename = item.ArticleId + ".html";
                        string htmlPath = htmlDir + htmlfilename;

                        StreamWriter sw = new StreamWriter(htmlPath, false, encode);
                        sw.Write(htmlText);
                        sw.Flush();
                        sr.Close();
                        sw.Close();
                        item.StaticLink = "http://www.litao.u-notebook.com/blogs/" + item.ArticleId + ".html";
                        newListBlogs.Add(item);

                    }
                    DAL.DataAcess.AddBlogs(newListBlogs);
                    //return newListBlogs;
                }

                return newListBlogs;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                throw;
            }

        }
    }
}
