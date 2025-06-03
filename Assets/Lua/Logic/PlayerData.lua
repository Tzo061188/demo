---@diagnostic disable: undefined-global, need-check-nil

local function AddTask(TaskID)
    print("执行事件"..MainPanel.playerSaveData.task_List.Count)
  
    for i = 0, MainPanel.playerSaveData.task_List.Count - 1 do
        if(MainPanel.playerSaveData.task_List[i].taskID == TaskID)then
            --防止重复添加
            return;
        end
    end
    
    MainPanel.playerSaveData.task_List:Add(TaskData(
        TaskDataList[TaskID].TaskID,
        TaskDataList[TaskID].IsDone,
        TaskDataList[TaskID].IsAccept,
        TaskDataList[TaskID].TaskName,
        TaskDataList[TaskID].Describe,
        TaskDataList[TaskID].Reward_Describe,
        TaskDataList[TaskID].Target
    )) 

end


--玩家数据
PlayerData = {}


PlayerData.BagData = {}
PlayerData.FastBagData = {}
PlayerData.EquipData = {}

PlayerData.PlayerTaskData = nil --玩家的任务数据

PlayerData.soundVolume = nil
PlayerData.bgMusicVolume = nil

--加载玩家数据
function PlayerData:Init()  -- 后续通过json加载

    local playerData =  JsonManager.Instance:LoadDataInLua("PlayerData",JsonType.LitJson)
    --添加任务的事件  添加监听

    --local action = Util.cs_generator(CS.UnityEngine.Events.UnityAction(CS.System.Int32),AddTask);

    EventCenter.Instance:AddEventListener_InLua(AllEventName.AddTask,AddTask)
    
    if(playerData == nil)then
        table.insert(self.BagData,{ID = 0 ,pos = 1,Count = -1}) --装备不显示数量 -1 表示
        table.insert(self.BagData,{ID = 2 ,pos = 2,Count = -1}) --装备不显示数量 -1 表示
        table.insert(self.BagData,{ID = 4 ,pos = 3,Count = -1}) --装备不显示数量 -1 表示
        table.insert(self.BagData,{ID = 6 ,pos = 4,Count = -1}) --装备不显示数量 -1 表示
        table.insert(self.BagData,{ID = 8 , pos = 5,Count = -1}) --装备不显示数量 -1 表示
        table.insert(self.BagData,{ID = 10 , pos = 6,Count = 20}) --装备不显示数量 -1 表示
        table.insert(self.BagData,{ID = 11 , pos = 7,Count = 10}) --装备不显示数量 -1 表示
        table.insert(self.BagData,{ID = 1 , pos = 8,Count = -1}) --装备不显示数量 -1 表示
        
        table.insert(self.FastBagData,{ID = 11 , pos = 0,Count = 10}) --装备不显示数量 -1 表示
        
        
        table.insert(self.EquipData,{ID = 1 , belongTo = 3,Count = -1}) --装备不显示数量 -1 表示
        table.insert(self.EquipData,{ID = 3 , belongTo = 4,Count = -1}) --装备不显示数量 -1 表示
        self.bgMusicVolume = 0.5
        self.soundVolume =  0.5
        return
    end
    for i = 0, playerData.itemData_List.Count-1 do
        local playerItemData =  playerData.itemData_List[i]
        if(playerItemData.belongTo == 1)then
            table.insert(self.BagData,{
                ID = playerItemData.id,
                pos = playerItemData.pos,
                Count = playerItemData.count,
                belongTo = playerItemData.belongTo
            })
        elseif playerItemData.belongTo == 2 then
            table.insert(self.FastBagData,{
                ID = playerItemData.id,
                pos = playerItemData.pos,
                Count = playerItemData.count,
                belongTo = playerItemData.belongTo
            })
        else
            table.insert(self.EquipData,{
                ID = playerItemData.id,
                pos = playerItemData.pos,
                Count = playerItemData.count,
                belongTo = playerItemData.belongTo
            })
        end
                    
                    
    end 
    

    self.PlayerTaskData = playerData.task_List  -- 这里直接用的是C#的 list  

    self.bgMusicVolume = playerData.bgMusicVolume
    self.soundVolume =  playerData.soundVolume

                
end
            
-- 直接就 初始化
PlayerData:Init()

function PlayerData:UpdateData()

    self.BagData = {}
    self.FastBagData = {}
    self.EquipData = {}

    local playerData =  MainPanel.playerSaveData
    
    for i = 0, playerData.itemData_List.Count-1 do
        local playerItemData =  playerData.itemData_List[i]
        if(playerItemData.belongTo == 1)then
            table.insert(self.BagData,{
                ID = playerItemData.id,
                pos = playerItemData.pos,
                Count = playerItemData.count,
                belongTo = playerItemData.belongTo
            })
        elseif playerItemData.belongTo == 2 then
            table.insert(self.FastBagData,{
                ID = playerItemData.id,
                pos = playerItemData.pos,
                Count = playerItemData.count,
                belongTo = playerItemData.belongTo
            })
        else
            table.insert(self.EquipData,{
                ID = playerItemData.id,
                pos = playerItemData.pos,
                Count = playerItemData.count,
                belongTo = playerItemData.belongTo
            })
        end
                    
                    
    end

end


