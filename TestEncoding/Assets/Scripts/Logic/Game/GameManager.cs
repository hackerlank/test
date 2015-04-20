using UnityEngine;
using System.Collections;
using msg;

public class GameManager : LSingleton<GameManager>
{
    public EntitiesManager mEntitiesManager;

    public PlayerNetWork PNetWork;

    public MainPlayer MainPlayer = new MainPlayer();       //玩家自己

    public CharacterMainData MainPlayerData;

    public PlayerInfo PlayerInfo = new PlayerInfo();

    public MapData CurrentMapData = null;

    public ReadMapData MapDataReader;

    public UI_Fight FightUI;    //战斗UI，暂时在切换完场景后初始化，后面UI做成切换不销毁的对象

    public void Initialize()
    {
        MapDataReader = new ReadMapData();
        GameTime.Instance.Init();
        LoginModule.Instance.Init();


        PNetWork = new PlayerNetWork();
        PNetWork.Initialize();
        FightUI = new UI_Fight();
        //LSingleton<SkillManager>.Instance.Initialize();
    }
}
