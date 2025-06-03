--前置文件
require("Tools/OO")
require("Tools/SplitTools")
require("Logic/Global_Define") --定义类

Util =  require("xlua.util")

Json = require("Tools/JsonUtility")

--开始
require("Logic/LoadData") --加载物品数据ItemData
require("Logic/PlayerData") -- 初始化  加载玩家数据

require("Logic/Item") --背包格子

require("Logic/MainPanel") -- 加载主面板
require("Logic/BagPanel")  -- 背包面板

require("Logic/UIPanel")  -- UI面板
require("Logic/TaskPanel")  -- 任务面板
require("Logic/MakePanel")  -- 制作面板
require("Logic/SettingPanel")  -- 设置面板

require("Logic/DialoguePanel")  -- 对话面板
require("Logic/StorePanel")  --商店面板

require("Logic/SceneLoadBar") --跳转场景的UI
SceneLoadBar:Init()


MainPanel:ShowMe();