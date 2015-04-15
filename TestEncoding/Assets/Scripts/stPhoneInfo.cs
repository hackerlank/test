using Net;
using System;

public class stPhoneInfo
{
    public string cpu = string.Empty;
    public string opengl = string.Empty;
    public string os = string.Empty;
    public string phone_model = string.Empty;
    public string phone_uuid = string.Empty;
    public string pushid = string.Empty;
    public string ram = string.Empty;
    public string resolution = string.Empty;

    public void WriteCmd(UMessage dat)
    {
        dat.WriteString(this.phone_uuid, 100);
        dat.WriteString(this.pushid, 100);
        dat.WriteString(this.phone_model, 100);
        dat.WriteString(this.resolution, 100);
        dat.WriteString(this.opengl, 100);
        dat.WriteString(this.cpu, 100);
        dat.WriteString(this.ram, 100);
        dat.WriteString(this.os, 100);
    }

    public void WriteEmpty(UMessage dat)
    {
        dat.WriteString(string.Empty, 100);
        dat.WriteString(string.Empty, 100);
        dat.WriteString(string.Empty, 100);
        dat.WriteString(string.Empty, 100);
        dat.WriteString(string.Empty, 100);
        dat.WriteString(string.Empty, 100);
        dat.WriteString(string.Empty, 100);
        dat.WriteString(string.Empty, 100);
    }
}

