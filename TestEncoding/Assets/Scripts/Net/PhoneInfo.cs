using System;
using System.Collections.Generic;
using System.Text;
using Net;

public class stPhoneInfo
{
    public string phone_uuid = "";  // 机器唯一码
    public string pushid = "";      // 推送id
    public string phone_model = ""; // 机型
    public string resolution = "";  // 分辨率
    public string opengl = "";      // opengl
    public string cpu = "";
    public string ram = "";
    public string os = "";   // 操作系统

    public void WriteCmd(UMessage dat)
    {
        dat.WriteString(phone_uuid, 100);
        dat.WriteString(pushid, 100);
        dat.WriteString(phone_model, 100);
        dat.WriteString(resolution, 100);
        dat.WriteString(opengl, 100);
        dat.WriteString(cpu, 100);
        dat.WriteString(ram, 100);
        dat.WriteString(os, 100);
    }

    public void WriteEmpty(UMessage dat)
    {
        dat.WriteString("", 100);
        dat.WriteString("", 100);
        dat.WriteString("", 100);
        dat.WriteString("", 100);
        dat.WriteString("", 100);
        dat.WriteString("", 100);
        dat.WriteString("", 100);
        dat.WriteString("", 100);
    }
}
