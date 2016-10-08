using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //---------------------------------------------------------------------------------------------
        //组件设置a.MD5File为2，3时 的实例代码

        if (Request.QueryString["access2008_cmd"]!=null && Request.QueryString["access2008_cmd"] == "2")//服务器提交MD5验证后的文件信息进行验证
        {
          //  Request.QueryString["access2008_File_name"];    //文件名
          //  Request.QueryString["access2008_File_size"];    //文件大小，单位字节
          //  Request.QueryString["access2008_File_type"];    //文件类型 例如.gif .png
          //  Request.QueryString["access2008_File_md5"];     //文件的MD5签名

            Response.Write("0");//返回命令  0 = 开始上传文件， 2 = 不上传文件，前台直接显示上传完成
            Response.End();
        }
        else if (Request.QueryString["access2008_cmd"] != null && Request.QueryString["access2008_cmd"] == "3") //服务器提交文件信息进行验证
        {
            //  Request.QueryString["access2008_File_name"];    //文件名
            //  Request.QueryString["access2008_File_size"];    //文件大小，单位字节
            //  Request.QueryString["access2008_File_type"];    //文件类型 例如.gif .png

            Response.Write("1");//返回命令 0 = 开始上传文件,1 = 提交MD5验证后的文件信息进行验证, 2 = 不上传文件，前台直接显示上传完成
            Response.End();
        }
        //---------------------------------------------------------------------------------------------

        if (Request.Files["Filedata"] != null)//判断是否有文件上传上来
        {
            SaveImages("File/");
            //其他表单数据接收

            if (Request.QueryString["access2008_File_md5"] != null)
            {
                Response.Write("<br/>");
                Response.Write("MD5效验" + Request.QueryString["access2008_File_md5"]);
            }
            Response.Write("<br/>");
            Response.Write("你选择的是<font color='#ff0000'>" + Request.Form["select"] + "</font>--<font color='#0000ff'>" + Request.Form["select2"] + "</font>");
            if (Request.Form["access2008_box_info_max"] != null)
            {
                Response.Write("<br />组件文件列表统计,总数量：" + Request.Form["access2008_box_info_max"] + ",剩余：" + (Convert.ToInt32(Request.Form["access2008_box_info_upload"]) - 1) + ",完成：" + (Request.Form["access2008_box_info_over"] + 1));
            }
            Response.Write(" <br />旋转角度：" + Request.Form["access2008_image_rotation"]);

            //MP3信息提取
            outMP3Info();

            Response.End();
        }


    }
    
    /// <summary>
    /// 获取cookie
    /// </summary>
    /// <param name="value">cookie名称</param>
    /// <returns></returns>
    private string getCookie(string value)
    {
        return Request.Form["access2008_cookie_"+value];
    }

    private void outMP3Info()
    {
        string info = Request.Form["access2008_mp3_info"];

        if (info != null)
        {
            string[] infoArr = info.Split('|');
            if (infoArr.Length == 9)
            {
                Response.Write("<br />MP3文件信息:<br/>");
                Response.Write("版本:" + infoArr[0] + "<br/>");

                Response.Write("层:" + infoArr[1] + "<br/>");

                if (infoArr[2] == "0")
                    Response.Write("CRC校验:校验<br/>");
                else
                    Response.Write("CRC校验:不校验<br/>");

                Response.Write("位率:" + infoArr[3] + "Kbps<br/>");

                Response.Write("采样频率:" + infoArr[4] + "Hz<br/>");
                if (infoArr[5] == "0")
                    Response.Write("声道模式:立体声Stereo<br/>");
                else if (infoArr[5] == "1")
                    Response.Write("声道模式:Joint Stereo<br/>");
                else if (infoArr[5] == "2")
                    Response.Write("声道模式:双声道<br/>");
                else
                    Response.Write("声道模式:单声道<br/>");


                if (infoArr[6] == "0")
                    Response.Write("版权:不合法<br/>");
                else
                    Response.Write("版权:合法<br/>");

                if (infoArr[7] == "0")
                    Response.Write("原版标志:非原版<br/>");
                else
                    Response.Write("原版标志:原版<br/>");

                Response.Write("播放时长:"+(Convert.ToInt32(infoArr[8]))/1000)+"秒<br/>");
            }
		  
        }
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="url">保存路径,填写相对路径</param>
    /// <returns></returns>
    private void SaveImages(string url)
    {
        ///'遍历File表单元素
        HttpFileCollection files = HttpContext.Current.Request.Files;

        ///'检查文件扩展名字
        //HttpPostedFile postedFile = files[iFile];
        HttpPostedFile postedFile = Request.Files["Filedata"]; //得到要上传文件
        string fileName, fileExtension, filesize;
        fileName = System.IO.Path.GetFileName(postedFile.FileName.ToString()); //得到文件名
        filesize = System.IO.Path.GetFileName(postedFile.ContentLength.ToString()); //得到文件大小
        if (fileName != "")
        {
            fileExtension = System.IO.Path.GetExtension(fileName);//'获取扩展名


            //注意：可能要修改你的文件夹的匿名写入权限。
            postedFile.SaveAs(System.Web.HttpContext.Current.Request.MapPath(url) + fileName);
        }

        Response.Write(fileName + "上传成功");
        

    }

}
