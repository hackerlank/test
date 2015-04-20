using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net;

enum enumProfession
{
	PROFESSION_NONE	= 0,	//无业
	PROFESSION_1	= 1,	//侠客
	PROFESSION_2	= 2,	//侠女
	PROFESSION_3	= 3,	//箭侠
	PROFESSION_4	= 4,	//箭灵
	PROFESSION_5	= 5,	//天师
	PROFESSION_6	= 6,	//美女
	PROFESSION_7	= 7,	//法师
	PROFESSION_8	= 8		//仙女
};

public enum enumRoleState
{
    eRoleState_NULL,                   // 角色空标志 
    eRoleState_DONE,                   // 角色已有标志
};

public class Role
{
    public string name;        // 角色名称
    public ushort type;        // 职业类型    
    public ushort conntryId;   // 国家ID    
	public string countryName; /// 国家名称
	public ushort level;
    public enumRoleState state;
   
    public Role()
    {
        init();
    }

    public void init()
    {
        name = "10086";
        type = (ushort)enumProfession.PROFESSION_NONE;
        conntryId = 255;
        countryName = "宋国";
        level = 0;

        state = enumRoleState.eRoleState_NULL;
    }
}
