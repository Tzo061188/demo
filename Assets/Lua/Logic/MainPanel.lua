---@diagnostic disable: undefined-global, need-check-nil

MainPanel = {}

MainPanel.panelObj = nil

MainPanel.tired_Count = nil
MainPanel.hp_Count = nil
MainPanel.Gold_number = nil

MainPanel.itemBG_C = nil
MainPanel.itemBG_L = nil
MainPanel.itemBG_R = nil

MainPanel.LData = nil
MainPanel.CData = nil
MainPanel.RData = nil

MainPanel.playerSaveData = nil

--打开背包
local OpenBag = function(context) --有按下了Tab

    
    if(UIPanel:GetActive()) then    --是打开的
        if(UIPanel.toggle_Item.isOn == false) then --不在背包这一栏
            UIPanel.toggle_Item.isOn = true        -- 打开背包
            return
        end
        UIPanel:HideMe();        --在背包这一栏  隐藏

        Cursor.lockState = CS.UnityEngine.CursorLockMode.Locked;  --锁定鼠标
        Time.timeScale = 1       --恢复暂停
        GameManager.Instance.isPlayerCanMove = true -- 人物可以移动
        GameManager.Instance.isCameraCanMove = true --  相机可以移动
        
    else  -- 是关闭的
    
        Cursor.lockState = CS.UnityEngine.CursorLockMode.None;  --显示鼠标
    
        UIPanel:ShowMe()            --显示 UI
        UIPanel.toggle_Item.isOn = true   -- 设置bool值
        BagPanel:ShowMe()          --打开背包
        Time.timeScale = 0             --暂停  
        GameManager.Instance.isPlayerCanMove = false -- 人物不可以移动
        GameManager.Instance.isCameraCanMove = false --  相机不可以移动
    end
end
--打开设置
local OpenSetting =  function(context)

    if(UIPanel:GetActive()) then
        if(UIPanel.toggle_Setting.isOn == false) then --不在设置
            UIPanel.toggle_Setting.isOn = true  
            return
        end
        UIPanel:HideMe();
        Cursor.lockState = CS.UnityEngine.CursorLockMode.Locked;  --锁定鼠标
        Time.timeScale = 1       --恢复暂停
        GameManager.Instance.isPlayerCanMove = true -- 人物可以移动
        GameManager.Instance.isCameraCanMove = true --  相机可以移动
    else

        Cursor.lockState = CS.UnityEngine.CursorLockMode.None;

        UIPanel:ShowMe()
        UIPanel.toggle_Setting.isOn = true
        SettingPanel:ShowMe()
        Time.timeScale = 0             --暂停
        GameManager.Instance.isPlayerCanMove = false -- 人物不可以移动
        GameManager.Instance.isCameraCanMove = false --  相机不可以移动
    end

end
--打开对话
local OpenDialogue = function ()

    if(NpcDialogue.IsEnter == false)then
        return
    end
    
    if(DialoguePanel.panelObj == nil)then
        DialoguePanel:ShowMe();
        return
    end

    if(NpcDialogue.IsEnter==true and DialoguePanel.panelObj.activeSelf == false)then
        DialoguePanel:ShowMe();
    elseif(DialoguePanel.panelObj.activeSelf == true)then
        DialoguePanel:HideMe();
    end

end

--任务完成  
local function EnemyDie_Task(id)   
    for i = 0, MainPanel.playerSaveData.task_List.Count-1 do
 
        if(MainPanel.playerSaveData.task_List[i].Target == id)then 
            MainPanel.playerSaveData.task_List[i].isDone = 1
            print("任务完成")
        end
    end
 end

function MainPanel:Init()
    
    if(self.panelObj == nil) then
        MusicManager.Instance:SetSoundEffectVolume(PlayerData.soundVolume)
        MusicManager.Instance:SetVolume(PlayerData.bgMusicVolume)

        RoleInputSystem.Instance.roleInput.Role.Bag:started("+",OpenBag)
        print("---------")
        RoleInputSystem.Instance.roleInput.Role.Esc:started("+",OpenSetting)
        --进入法师的对话范围
        RoleInputSystem.Instance.roleInput.Role.Interactive:started("+",OpenDialogue)

        self.panelObj =  ResourcesManager.Instance:loadRes("Prefabs/UI/MainPanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,Canvas,false)


        self.tired_Count =  self.panelObj.transform:Find("Tired/Tired_Count"):GetComponent(typeof(RectTransform))
        self.hp_Count =  self.panelObj.transform:Find("HP/HP_Count"):GetComponent(typeof(Image))
        self.Gold_number = self.panelObj.transform:Find("Gold_number"):GetComponent(typeof(Text))

        self.itemBG_L =  self.panelObj.transform:Find("ItemBag/itemBG_L")
        self.itemBG_C =  self.panelObj.transform:Find("ItemBag/itemBG_C")
        self.itemBG_R =  self.panelObj.transform:Find("ItemBag/itemBG_R")

        self.playerSaveData =  PlayerDataList() --初始化保存数据
        self.playerSaveData.task_List = PlayerData.PlayerTaskData  --初始化任务数据
        
        MonoManager.Instance:AddUpdate(MainPanel.Tired)
        MonoManager.Instance:AddUpdate(MainPanel.HP)

        --添加任务完成事件
        EventCenter.Instance:AddEventListener_InLua(AllEventName.EnemyDie,EnemyDie_Task) 
    end
    
end 
function MainPanel:ShowMe()
    self:Init()

    self.Gold_number.text = self.playerSaveData.Gold_number;

    self.panelObj:SetActive(true)
end

function MainPanel:HideMe()
    self.panelObj:SetActive(false)
end

