using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Common
{
    public class XmlOperation
    {
        public static Message XmlToEntity(string sMsg)
        {
            try
            {
                XmlDocument rdoc = new XmlDocument();
                rdoc.LoadXml(sMsg);
                XmlNode rRoot = rdoc.FirstChild;
                Message model = new Message();
                model.ToUserName = rRoot["ToUserName"].InnerText;
                model.FromUserName = rRoot["FromUserName"].InnerText;
                model.MsgId = rRoot["MsgId"].InnerText;
                model.MsgType = rRoot["MsgType"].InnerText;
                model.Content = rRoot["Content"].InnerText;
                model.CreateTime = rRoot["CreateTime"].InnerText;
                model.Type = "send";
                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public static string GetMsgType(string sMsg)
        {
            try
            {
                string result = "";
                if (!string.IsNullOrEmpty(sMsg))
                {
                    XmlDocument rdoc = new XmlDocument();
                    rdoc.LoadXml(sMsg);
                    XmlNode rRoot = rdoc.FirstChild;

                    result = rRoot["MsgType"].InnerText;
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
