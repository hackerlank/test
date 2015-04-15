using msg;
using System;

public class GameManager : LSingleton<GameManager>
{
    public MapData CurrentMapData;
    public UI_Fight FightUI;
    public MainPlayer MainPlayer;
    public CharacterMainData MainPlayerData;
    public ReadMapData MapDataReader;
    public EntitiesManager mEntitiesManager;
    public PlayerInfo PlayerInfo = new PlayerInfo();
    public PlayerNetWork PNetWork;

    public void Initialize()
    {
        this.MapDataReader = new ReadMapData();
        GameTime.Instance.Init();
        LoginModule.Instance.Init();
        ConfigManager.Instance.Initialize();
        mEntitiesManager = new EntitiesManager();
        mEntitiesManager.Initialize();
        PNetWork = new PlayerNetWork();
        PNetWork.Initialize();
        FightUI = new UI_Fight();
    }
}

