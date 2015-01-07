using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace weixin
{
    /// <summary>
    /// weixin 的摘要说明
    /// </summary>
    public class weixin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string sToken = "litao527";
            string sAppID = "wx414ca95a24d4d83e";
            string appSecret = "536b956413c5aad204565265e36bdd9d";
            string sEncodingAESKey = "Kgrml7sKAhJFVZRhrxeEDxAeJ3KCipd7hDon0shE8Sp";

            if (context.Request.HttpMethod != "POST")
            {
                //验证微信开发者接入
                try
                {

                    string token = "litao527";
                    string echoStr = context.Request.QueryString["echoStr"];
                    string signature = context.Request.QueryString["signature"];
                    string timestamp = context.Request.QueryString["timestamp"];
                    string nonce = context.Request.QueryString["nonce"];

                    bool result = Common.WeiXinOperation.ValidUrl(token, echoStr, signature, timestamp, nonce);
                    if (result)
                    {
                        if (!string.IsNullOrEmpty(echoStr))
                        {
                            context.Response.Write(echoStr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.Log.Error(ex.Message);
                    throw;
                }
            }
            else
            {
                Common.wxApi.WXBizMsgCrypt wxcpt = new Common.wxApi.WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

                //接收消息
                string sReqData = "";
                Stream s = context.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                sReqData = Encoding.UTF8.GetString(b);

                string encrypt_type = context.Request.QueryString["encrypt_type"];
                string sReqMsgSig = context.Request.QueryString["msg_signature"];
                string sReqTimeStamp = context.Request.QueryString["timestamp"];
                string sReqNonce = context.Request.QueryString["nonce"];


                string sMsg = "";  //解析之后的明文
                int ret = 0;
                ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
                if (ret != 0)
                {
                    Common.Log.Error("ERR: Decrypt fail, ret: " + ret);
                    return;
                }
                Model.Message blogModel = Common.XmlOperation.XmlToEntity(sMsg);
                //记录到数据库
                DAL.DataAcess.Add(blogModel);
                //Common.Log.Error("接收到的微信消息：" + sMsg);

                //回复消息
                XmlDocument rdoc = new XmlDocument();
                rdoc.LoadXml(sMsg);
                XmlNode rRoot = rdoc.FirstChild;
                string openId = rRoot["FromUserName"].InnerText;
                string sendId = rRoot["ToUserName"].InnerText;
                string msgType = rRoot["MsgType"].InnerText;
                if (msgType == "text")
                {
                    string content = rRoot["Content"].InnerText;
                    switch (content)
                    {
                        case "1":
                            string textContent = Common.WeiXinOperation.GetTextContent(openId, sendId);
                            string textEncryptMsg = ""; //xml格式的密文
                            ret = wxcpt.EncryptMsg(textContent, sReqTimeStamp, sReqNonce, ref textEncryptMsg);
                            context.Response.Write(textEncryptMsg);
                            context.Response.End();
                            break;
                        case "2":
                            string newsContent = Common.WeiXinOperation.GetNewsContent(openId, sendId);
                            string newsEncryptMsg = ""; //xml格式的密文
                            ret = wxcpt.EncryptMsg(newsContent, sReqTimeStamp, sReqNonce, ref newsEncryptMsg);
                            context.Response.Write(newsEncryptMsg);
                            context.Response.End();
                            break;
                        case "3":
                            string musicContent = Common.WeiXinOperation.GetMusicContent(openId, sendId);
                            string musicEncryptMsg = ""; //xml格式的密文
                            ret = wxcpt.EncryptMsg(musicContent, sReqTimeStamp, sReqNonce, ref musicEncryptMsg);
                            context.Response.Write(musicEncryptMsg);
                            context.Response.End();
                            break;
                        case "4":
                            string voiceContent = Common.WeiXinOperation.GetVoiceContent(openId, sendId);
                            string voiceEncryptMsg = ""; //xml格式的密文
                            ret = wxcpt.EncryptMsg(voiceContent, sReqTimeStamp, sReqNonce, ref voiceEncryptMsg);
                            context.Response.Write(voiceEncryptMsg);
                            context.Response.End();
                            break;
                        case "5":
                            string videoContent = Common.WeiXinOperation.GetVideoContent(openId, sendId);
                            string videoEncryptMsg = ""; //xml格式的密文
                            ret = wxcpt.EncryptMsg(videoContent, sReqTimeStamp, sReqNonce, ref videoEncryptMsg);
                            context.Response.Write(videoEncryptMsg);
                            context.Response.End();
                            break;
                        default:
                            string defaultContent = Common.WeiXinOperation.GetDefaultContent(openId, sendId);
                            string defaultEncryptMsg = ""; //xml格式的密文
                            ret = wxcpt.EncryptMsg(defaultContent, sReqTimeStamp, sReqNonce, ref defaultEncryptMsg);
                            context.Response.Write(defaultEncryptMsg);
                            context.Response.End();
                            break;
                    }
                }
                else if (msgType == "image")
                {
                    string imageContent = Common.WeiXinOperation.GetImageContent(openId, sendId);
                    string imageEncryptMsg = ""; //xml格式的密文
                    ret = wxcpt.EncryptMsg(imageContent, sReqTimeStamp, sReqNonce, ref imageEncryptMsg);
                    context.Response.Write(imageEncryptMsg);
                    context.Response.End();
                }
                else if (msgType == "voice")
                {
                    string voiceContent = Common.WeiXinOperation.GetVoiceContent(openId, sendId);
                    string voiceEncryptMsg = ""; //xml格式的密文
                    ret = wxcpt.EncryptMsg(voiceContent, sReqTimeStamp, sReqNonce, ref voiceEncryptMsg);
                    context.Response.Write(voiceEncryptMsg);
                    context.Response.End();
                }
                else
                {
                    string defaultContent = Common.WeiXinOperation.GetDefaultContent(openId, sendId);
                    context.Response.Write(defaultContent);
                    context.Response.End();
                }

            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}