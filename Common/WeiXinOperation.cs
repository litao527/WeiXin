using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class WeiXinOperation
    {

        /// <summary>
        /// 验证url权限， 接入服务器
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool ValidUrl(string token, string echoStr, string signature, string timestamp, string nonce)
        {
            try
            {

                if (CheckSignature(token, signature, timestamp, nonce))
                {
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        //Utils.ResponseWrite(echoStr);
                        return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private static bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {

            try
            {
                string[] ArrTmp = { token, timestamp, nonce };
                Array.Sort(ArrTmp);     //字典排序
                string tmpStr = string.Join("", ArrTmp);
                tmpStr = HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
                tmpStr = tmpStr.ToLower();
                Common.Log.Error("tmpStr=" + tmpStr);
                if (tmpStr == signature)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 根据指定的密码和哈希算法生成一个适合于存储在配置文件中的哈希密码
        /// </summary>
        /// <param name="str">要进行哈希运算的密码</param>
        /// <param name="type"> 要使用的哈希算法</param>
        /// <returns>经过哈希运算的密码</returns>
        private static string HashPasswordForStoringInConfigFile(string str, string type)
        {
            return HashPasswordForStoringInConfigFile(str, type);
        }


        /// <summary>
        /// 获取关注回复内容
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetSubscribeContent(string openId, string sendId)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    string items = GetSubscribeItem();

                    StringBuilder sb = new StringBuilder();
                    string msgType = "news";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<ArticleCount>");
                    sb.Append("1");
                    sb.Append("</ArticleCount>");
                    sb.Append("<Articles>");
                    sb.Append(items);
                    sb.Append("</Articles>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }
        }

        private static string GetSubscribeItem()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<item>");
            sb.Append("<Title><![CDATA[");
            sb.Append("欢迎关注海霓科技，u-notebook改变你的生活");
            sb.Append("]]></Title>");
            sb.Append("<Description><![CDATA[");
            sb.Append("最省时间：操作简单 提高工作效率,掌握时间，才能掌握未来。信息时代，速度就是财富。 在数码本上随心书写的同时，也可保存成电子文档。整篇手写真迹一次识别成电子文档，免去重新打字输入的麻烦，为工作节省更多时间，提高工作效率");
            sb.Append("]]></Description>");
            sb.Append("<PicUrl><![CDATA[");
            sb.Append("http://mmbiz.qpic.cn/mmbiz/fBhianiadNmOG6Z27rpNtq27iaQXaUEmArd4UTicaxRAks3nYS1hmtcnqusBiaZvlXyibyQOPvsnMCQkgJlXzSBjWsmA/0");
            sb.Append("]]></PicUrl>");
            sb.Append("<Url><![CDATA[");
            sb.Append("http://www.u-notebook.com");
            sb.Append("]]></Url>");
            sb.Append("</item>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取默认回复内容
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetDefaultContent(string openId, string sendId)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    StringBuilder sb = new StringBuilder();
                    string content = "回复[1]查看文本消息\r\n回复[2]查看图文消息\r\n回复[3]听首歌吧\r\n回复[4]听听故事\r\n回复[5]看段激情小视频";
                    string msgType = "text";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<Content><![CDATA[");
                    sb.Append(content);
                    sb.Append("]]></Content>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// 获取文本信息
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetTextContent(string openId, string sendId)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    StringBuilder sb = new StringBuilder();
                    string content = "您好！我是马兄，现在给你返回的是文本信息";
                    string msgType = "text";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<Content><![CDATA[");
                    sb.Append(content);
                    sb.Append("]]></Content>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }
        }


        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetImageContent(string openId, string sendId)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    StringBuilder sb = new StringBuilder();
                    string picUrl = "http://mmbiz.qpic.cn/mmbiz/fBhianiadNmOG6Z27rpNtq27iaQXaUEmArdpWWdtnlrOnQic1DPNGUxLS2VVLVTUeFibqtAviclK7UduGBKmTQCfZ9Gw/0";
                    string mediaId = "Jj8xFxZUhi-JIrZ-0LhJSqK1ZIx8rUzVmqaVP_6GjsVV-3UB839dAE25OL3DAD9Q";
                    string msgType = "image";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<Image>");
                    sb.Append("<MediaId><![CDATA[");
                    sb.Append(mediaId);
                    sb.Append("]]></MediaId>");
                    sb.Append("</Image>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// 获取图文信息
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetNewsContent(string openId, string sendId)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    List<CnBlog> list = CnblogsOperation.GetCnblogInfo();
                    int count = 0;
                    string items = GetPicContent(list, out count);

                    StringBuilder sb = new StringBuilder();
                    string msgType = "news";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<ArticleCount>");
                    sb.Append(count);
                    sb.Append("</ArticleCount>");
                    sb.Append("<Articles>");
                    sb.Append(items);
                    sb.Append("</Articles>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }
        }


        /// <summary>
        /// 获取音乐信息
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetMusicContent(string openId, string sendId)
        {
            string result = "";
            List<CnBlog> listBlogs = null;
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    List<CnBlog> list = CnblogsOperation.GetCnblogInfo();
                    int count = 0;
                    string items = GetPicContent(list, out count);

                    StringBuilder sb = new StringBuilder();
                    string msgType = "music";
                    string musicTitle = "我的歌声里";
                    string description = "曲婉婷";
                    string musicUrl = "http://weixincourse-weixincourse.stor.sinaapp.com/mysongs.mp3";
                    string hqMusicUrl = "http://weixincourse-weixincourse.stor.sinaapp.com/mysongs.mp3";
                    string thumbMediaId = "Jj8xFxZUhi-JIrZ-0LhJSqK1ZIx8rUzVmqaVP_6GjsVV-3UB839dAE25OL3DAD9Q";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<Music>");
                    sb.Append("<Title><![CDATA[");
                    sb.Append(musicTitle);
                    sb.Append("]]></Title>");
                    sb.Append("<Description><![CDATA[");
                    sb.Append(description);
                    sb.Append("]]></Description>");
                    sb.Append("<MusicUrl><![CDATA[");
                    sb.Append(musicUrl);
                    sb.Append("]]></MusicUrl>");
                    sb.Append("<HQMusicUrl><![CDATA[");
                    sb.Append(hqMusicUrl);
                    sb.Append("]]></HQMusicUrl>");
                    sb.Append("<ThumbMediaId><![CDATA[");
                    sb.Append(thumbMediaId);
                    sb.Append("]]></ThumbMediaId>");
                    sb.Append("</Music>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                    listBlogs = list;
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }

        }



        /// <summary>
        /// 获取语音信息
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetVoiceContent(string openId, string sendId)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    List<CnBlog> list = CnblogsOperation.GetCnblogInfo();
                    int count = 0;
                    string items = GetPicContent(list, out count);

                    StringBuilder sb = new StringBuilder();
                    string msgType = "voice";
                    string media_id = "HmFJuR3GgK3Ip8G6_Yj-wDXx4k2OmApgYTDbiB6j6bL2DzKjWSX7c1wyWQ48v3Nj";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<Voice>");
                    sb.Append("<MediaId><![CDATA[");
                    sb.Append(media_id);
                    sb.Append("]]></MediaId>");
                    sb.Append("</Voice>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }
        }


        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <param name="openId">微信接收者</param>
        /// <param name="sendId">微信发送者</param>
        /// <returns></returns>
        public static string GetVideoContent(string openId, string sendId)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(sendId))
                {
                    List<CnBlog> list = CnblogsOperation.GetCnblogInfo();
                    int count = 0;
                    string items = GetPicContent(list, out count);

                    StringBuilder sb = new StringBuilder();
                    string msgType = "video";
                    string title = "张国荣《这些年来》";
                    string description = "经典！张国荣《这些年来》唱尽深情与眷恋!(他的离去，从未让他远去!)";
                    string media_id = "204128847";
                    sb.Append("<xml><ToUserName><![CDATA[");
                    sb.Append(openId);
                    sb.Append("]]></ToUserName><FromUserName><![CDATA[");
                    sb.Append(sendId);
                    sb.Append("]]></FromUserName><CreateTime>");
                    sb.Append(GetTimeStamp());
                    sb.Append("</CreateTime><MsgType><![CDATA[");
                    sb.Append(msgType);
                    sb.Append("]]></MsgType>");
                    sb.Append("<Video>");
                    sb.Append("<MediaId><![CDATA[");
                    sb.Append(media_id);
                    sb.Append("]]></MediaId>");
                    sb.Append("<Title><![CDATA[");
                    sb.Append(title);
                    sb.Append("]]></Title>");
                    sb.Append("<Description><![CDATA[");
                    sb.Append(description);
                    sb.Append("]]></Description>");
                    sb.Append("</Video>");
                    sb.Append("<MsgId></MsgId></xml>");

                    result = sb.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message);
                return result;
            }
        }
        private static string GetPicContent(List<CnBlog> list, out int count)
        {
            try
            {
                string[] picArray = new string[] { 
                    "http://mmbiz.qpic.cn/mmbiz/fBhianiadNmOG6Z27rpNtq27iaQXaUEmArd4UTicaxRAks3nYS1hmtcnqusBiaZvlXyibyQOPvsnMCQkgJlXzSBjWsmA/0", 
                    "http://mmbiz.qpic.cn/mmbiz/fBhianiadNmOG6Z27rpNtq27iaQXaUEmArdjChgRUTGaiaZQq38g2DCo3Saxy0HLI9wuIPx5GKFTgVcUyiaxQZfgfYw/0",
                    "http://mmbiz.qpic.cn/mmbiz/fBhianiadNmOG6Z27rpNtq27iaQXaUEmArdGea4gmzcN16xjTfq4ia36ibiag208muqwpw087Pl2ZOpxaQCVGhgcPxiaw/0",
                    "http://mmbiz.qpic.cn/mmbiz/fBhianiadNmOG6Z27rpNtq27iaQXaUEmArdnxTiapJfD9KD50GdbckzBjkOqxAraxgJrOjIGteJtBcvd0Au7WPWK7g/0",
                    "http://mmbiz.qpic.cn/mmbiz/fBhianiadNmOG6Z27rpNtq27iaQXaUEmArdBRRmlrvY6JiaS4leULRklptSd9vVtAe8ZhXCsuPu9DtFdOPy3dC8SHA/0"
                };
                StringBuilder sb = new StringBuilder();
                if (list != null && list.Count > 0)
                {
                    count = list.Count();
                    int i = 0;
                    foreach (var item in list)
                    {
                        sb.Append("<item>");
                        sb.Append("<Title><![CDATA[");
                        sb.Append(item.Title);
                        sb.Append("]]></Title>");
                        sb.Append("<Description><![CDATA[");
                        sb.Append(item.Summary);
                        sb.Append("]]></Description>");
                        sb.Append("<PicUrl><![CDATA[");
                        sb.Append(picArray[i]);
                        sb.Append("]]></PicUrl>");
                        sb.Append("<Url><![CDATA[");
                        sb.Append(item.StaticLink);
                        sb.Append("]]></Url>");
                        sb.Append("</item>");
                        i++;
                        if (i > 4)
                        {
                            i = 0;
                        }
                    }
                }
                else
                {
                    count = 0;
                }
                return sb.ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private static string GetTimeStamp()
        {
            try
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(ts.TotalSeconds).ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
