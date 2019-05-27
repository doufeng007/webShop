using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Abp.File.Controllers
{
    public class FileHelper
    {
        public static string MakeThumbnail(string filePath, int intThumbWidth, int intThumbHeight)
        {
            var org_FileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            var org_Dir = System.IO.Path.GetDirectoryName(filePath);
            var suffix = System.IO.Path.GetExtension(filePath);
            var thumbPaht = System.IO.Path.Combine(org_Dir, $"{org_FileName}Thumb{suffix}");
            //缩略图保存的绝对路径
            string smallImagePath = thumbPaht;
            try
            {
                //原图加载
                using (System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(filePath))
                {
                    //原图宽度和高度
                    int width = sourceImage.Width;
                    int height = sourceImage.Height;
                    int smallWidth;
                    int smallHeight;
                    //获取第一张绘制图的大小,(比较 原图的宽/缩略图的宽  和 原图的高/缩略图的高)
                    if (((decimal)width) / height <= ((decimal)intThumbWidth) / intThumbHeight)
                    {
                        smallWidth = intThumbWidth;
                        smallHeight = intThumbWidth * height / width;
                    }
                    else
                    {
                        smallWidth = intThumbHeight * width / height;
                        smallHeight = intThumbHeight;
                    }


                    //判断缩略图在当前文件夹下是否同名称文件存在


                    //新建一个图板,以最小等比例压缩大小绘制原图
                    using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(smallWidth, smallHeight))
                    {
                        //绘制中间图
                        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                        {
                            //高清,平滑
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.Clear(Color.Black);
                            g.DrawImage(
                            sourceImage,
                            new System.Drawing.Rectangle(0, 0, smallWidth, smallHeight),
                            new System.Drawing.Rectangle(0, 0, width, height),
                            System.Drawing.GraphicsUnit.Pixel
                            );
                        }
                        //新建一个图板,以缩略图大小绘制中间图
                        using (System.Drawing.Image bitmap1 = new System.Drawing.Bitmap(intThumbWidth, intThumbHeight))
                        {
                            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap1))
                            {
                                //高清,平滑
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.Clear(Color.Black);
                                int lwidth = (smallWidth - intThumbWidth) / 2;
                                int bheight = (smallHeight - intThumbHeight) / 2;
                                g.DrawImage(bitmap, new Rectangle(0, 0, intThumbWidth, intThumbHeight), lwidth, bheight, intThumbWidth, intThumbHeight, GraphicsUnit.Pixel);
                                g.Dispose();
                                bitmap1.Save(smallImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }
                }

                return smallImagePath;
            }
            catch
            {
                try
                {
                    if (System.IO.File.Exists(smallImagePath))
                        System.IO.File.Delete(smallImagePath);
                    return "";
                }
                catch
                {
                    return "";
                }

            }

        }
    }
}
