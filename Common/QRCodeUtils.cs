using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace Common
{
    public class QRCodeUtils
    {
        #region 通过类容获取二维码图片
        /// <summary>
        /// 输入普通文本
        /// </summary>
        /// <param name="content"></param>
        /// <returns>图片地址</returns>
        public static string GetText(string content)
        {
            return GetQRCode(content, null, null);
        }

        /// <summary>
        /// 输入Url：  www.baidu.com  或者 Http://www.baidu.com
        /// </summary>
        /// <param name="content"></param>
        /// <returns>图片地址</returns>
        public static string GetHttpUrl(string content)
        {
            if (content.ToUpper().IndexOf("HTTP://") >= 0 || content.ToUpper().IndexOf("HTTPS://") >= 0)
                return GetQRCode(content, null, null);
            else
                return GetQRCode("http://" + content, null, null);
        }

        /// <summary>
        /// 生成扫描后可登录wifi的二维码连接
        /// </summary>
        /// <param name="no">账号</param>
        /// <param name="pwd">密码</param>
        /// <param name="type">
        /// 加密类型:
        ///  1:   WPA               (WPA/WPA2)
        ///  2:   WEP               (WEP)
        ///  3:   nopass            (无加密)
        /// </param>
        /// <returns></returns>
        public static string GetWifi(string no, string pwd, int type)
        {
            string tStr = "WPA";//默认第一种
            if (type == 1) tStr = "WPA";
            else if (type == 1) tStr = "WEP";
            else if (type == 1) tStr = "nopass";
            StringBuilder str = new StringBuilder();
            str.AppendFormat("WIFI:T:{0};S:{1};P:{2};;", no, pwd, tStr);
            return GetQRCode(str.ToString(), null, null);
        }
        #endregion


        #region  生成二维码
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="imgName"></param>
        /// <param name="saveUrl">格式（\\文件名\\子文件名\\...）</param>
        /// <returns></returns>
        public static string GetQRCode(string content, string imgName, string saveUrl)
        {
            saveUrl = saveUrl == null ? "\\QRCode\\" : saveUrl;
            imgName = imgName == null ? DateTime.Now.ToString("yyyyMMddhhmmssffff") : imgName;
            string path = "";
            try
            {
                var imgBack = GetQRCodeImg(content);
                string filename = imgName;//string.Format(DateTime.Now.ToString(), "yyyymmddhhmmss");
                filename = filename.Replace(" ", "");
                filename = filename.Replace("/", "");
                filename = filename.Replace(":", "");
                filename = filename.Replace("-", "");
                //filename = filename.Replace(".", "");
                filename += ".jpg";
                path = AppDomain.CurrentDomain.BaseDirectory + saveUrl;
                if (!Directory.Exists(path))
                {//目录不存在，就创建
                    Directory.CreateDirectory(path);
                }
                path += filename;
                if (imgBack != null)
                {
                    imgBack.Save(path);
                    return saveUrl + filename;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region 获取生成的二维码
        /// <summary>
        /// 获取生成的二维码 
        /// 输入内容即可:Image 对象
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Image GetQRCodeImg(string content)
        {
            Bitmap bt;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //二维码生成规则参数
            //二维码背景颜色
            //qrCodeEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
            ////二维码编码方式
            //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            ////每个小方格的宽度
            //qrCodeEncoder.QRCodeScale = 10;
            ////二维码版本号
            //qrCodeEncoder.QRCodeVersion = 5;
            ///纠错等级
            //qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            try
            {

                bt = qrCodeEncoder.Encode(content, Encoding.Default);
                Image imgBack = new Bitmap(bt);
                Graphics g = Graphics.FromImage(imgBack);
                g.DrawImage(imgBack, 10, 10, imgBack.Width - 20, imgBack.Height - 20);
                g.FillRectangle(System.Drawing.Brushes.White, 0, 0, imgBack.Width, 10);//相片四周刷一层白色边框
                g.FillRectangle(System.Drawing.Brushes.White, 0, 0, 10, imgBack.Height);//相片四周刷一层白色边框
                g.FillRectangle(System.Drawing.Brushes.White, 0, imgBack.Height - 10, imgBack.Width, 10);//相片四周刷一层白色边框
                g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width - 10, 0, 10, imgBack.Height);//相片四周刷一层白色边框
                GC.Collect();
                return imgBack;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取生成的二维码 
        /// 输入内容即可:
        /// 输出字节流，作为图片使用（不保存到本地/服务器）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] ResponseQRCode(string text)
        {
            var img = GetQRCodeImg(text);
            MemoryStream stream = new MemoryStream();
            img.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }
        #endregion

        /// <summary>
        /// 图片类容,放到图片中间的头像路径
        /// </summary>
        /// <param name="content"></param>
        /// <param name="imgUrl">头像的图片路径</param>
        /// <param name="uno">唯一标识编号（也是文件名）</param>
        /// <returns></returns>
        public static string GetCodeMore(string content, string imgUrl, string uno)
        {
            string path = "";
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            System.Drawing.Image image = qrCodeEncoder.Encode(content, Encoding.UTF8);
            MemoryStream MStream = new MemoryStream();
            image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);
            MemoryStream MStream1 = new MemoryStream();
            CombinImage(image, HttpContext.Current.Server.MapPath(imgUrl)).Save(MStream1, System.Drawing.Imaging.ImageFormat.Jpeg);
            MemoryStream MStream2 = new MemoryStream();
            Image i = Image.FromStream(MStream1);
            string saveUrl = "\\QRCode\\HeadImg\\";
            string filename = uno;//string.Format(DateTime.Now.ToString(), "yyyymmddhhmmss");
            filename = filename.Replace(" ", "");
            filename = filename.Replace("/", "");
            filename = filename.Replace(":", "");
            filename = filename.Replace("-", "");
            //filename = filename.Replace(".", "");
            filename += ".jpg";
            path = AppDomain.CurrentDomain.BaseDirectory + saveUrl;
            if (!Directory.Exists(path))
            {//目录不存在，就创建
                Directory.CreateDirectory(path);
            }
            path += filename;
            i.Save(path);
            return saveUrl + filename;
        }
        ///
        /// 调用此函数后使此两种图片合并，类似相册，有个
        /// 背景图，中间贴自己的目标图片
        /// 粘贴的源图片
        /// 粘贴的目标图片
        protected static Image CombinImage(Image imgBack, string destImg)
        {

            Image img = Image.FromFile(destImg); //照片图片
            if (img.Height != 65 || img.Width != 65)
            {
                img = KiResizeImage(img, 45, 45, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);
            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
            g.DrawImage(imgBack, 10, 10, imgBack.Width - 20, imgBack.Height - 20);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, imgBack.Width, 10);//相片四周刷一层白色边框
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, 10, imgBack.Height);//相片四周刷一层白色边框
            g.FillRectangle(System.Drawing.Brushes.White, 0, imgBack.Height - 10, imgBack.Width, 10);//相片四周刷一层白色边框
            g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width - 10, 0, 10, imgBack.Height);//相片四周刷一层白色边框
            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
            g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 5, imgBack.Height / 2 - img.Height / 2 - 5, 55, 55);//相片四周刷一层黑色边框
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }
        ///
        /// Resize图片
        ///
        /// 原始Bitmap
        /// 新的宽度
        /// 新的高度
        /// 保留着，暂时未用
        /// 处理以后的图片
        protected static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }


        #region 获取二维码的内容
        /// <summary>
        /// 图片在服务器路径
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public static string Decoder(string imgUrl)
        {
            string result = "";
            string SaveImgUrl = HttpContext.Current.Server.MapPath(imgUrl);
            if (System.IO.File.Exists(SaveImgUrl))
            {
                QRCodeDecoder decoder = new QRCodeDecoder();
                result = decoder.decode(new ThoughtWorks.QRCode.Codec.Data.QRCodeBitmapImage(new Bitmap(Image.FromFile(SaveImgUrl))));
            }

            return result;
        }
        #endregion
    }
}