---@diagnostic disable: need-check-nil, undefined-global

SettingPanel = {}

SettingPanel.panelObj = nil
SettingPanel.soundSlider = nil
SettingPanel.BGMusicSlider = nil
SettingPanel.save_GameButton = nil
SettingPanel.saveQuit_GameButton = nil


SettingPanel.soundVolume = 0.5
SettingPanel.bgMusicVolume = 0.5
function SettingPanel:Init()
    if(self.panelObj == nil) then
        self.panelObj =  ResourcesManager.Instance:loadRes("Prefabs/UI/SettingPanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,UIPanel.panelObj.transform)
        self.soundSlider = self.panelObj.transform:Find("Sound_Slider"):GetComponent(typeof(Slider))
        self.BGMusicSlider = self.panelObj.transform:Find("BGMusic_Slider"):GetComponent(typeof(Slider))
        self.save_GameButton = self.panelObj.transform:Find("Save_Game"):GetComponent(typeof(Button))
        self.saveQuit_GameButton = self.panelObj.transform:Find("QuitSave_Game"):GetComponent(typeof(Button))

        self.soundSlider.value = PlayerData.soundVolume
        self.BGMusicSlider.value = PlayerData.bgMusicVolume

        --添加事件
        self.soundSlider.onValueChanged:AddListener(function (value)
            SettingPanel.Change_SoundVolume(value) 
        end)
        self.BGMusicSlider.onValueChanged:AddListener(function (value)
            SettingPanel.Change_BGMusicVolume(value)
        end)
        self.save_GameButton.onClick:AddListener(function ()
            SettingPanel:SaveGameButton()
        end)
        self.saveQuit_GameButton.onClick:AddListener(function ()
            SettingPanel:SaveQuitGameBUtton() 
        end)

    end

end

function SettingPanel:ShowMe()
    self:Init()
    self.panelObj:SetActive(true)
end
function SettingPanel:HideMe()

    self.panelObj:SetActive(false)
end

function SettingPanel.Change_SoundVolume(value)
    SettingPanel.soundVolume = value
    MusicManager.Instance:SetSoundEffectVolume(value)
end
function SettingPanel.Change_BGMusicVolume(value)
    SettingPanel.bgMusicVolume = value
    MusicManager.Instance:SetVolume(value)
end

function SettingPanel:SaveGameButton()
    
    --清除以前的data
    MainPanel.playerSaveData.itemData_List:Clear()

    --Bag
    for i = 1, #BagPanel.bag_Item_List do
        if(BagPanel.bag_Item_List[i].data ~= nil)then
            self.SaveData(BagPanel.bag_Item_List[i],i)
        end
    end

    for i = 1, #BagPanel.fastBag_Item_List do
        if(BagPanel.fastBag_Item_List[i].data ~= nil)then
            self.SaveData(BagPanel.fastBag_Item_List[i],i)
        end
    end

    if(BagPanel.weapon_Item.data ~= nil)then
        self.SaveData(BagPanel.weapon_Item,0)
    end
    if(BagPanel.head_Item.data ~= nil)then
        self.SaveData(BagPanel.head_Item,0)
    end
    if(BagPanel.armor_Item.data ~= nil)then
        self.SaveData(BagPanel.armor_Item,0)
    end
    if(BagPanel.decoration_Item.data ~= nil)then
        self.SaveData(BagPanel.decoration_Item,0)
    end
    if(BagPanel.gem_Item.data ~= nil)then
        self.SaveData(BagPanel.gem_Item,0)
    end

    MainPanel.playerSaveData.soundVolume = SettingPanel.soundVolume
    MainPanel.playerSaveData.bgMusicVolume = SettingPanel.bgMusicVolume

    --保存为json
    JsonManager.Instance:SaveData( MainPanel.playerSaveData,"PlayerData",JsonType.LitJson)

end
function SettingPanel.SaveQuitGameBUtton()
    SettingPanel:SaveGameButton()
    Application.Quit()
end

function SettingPanel.SaveData(item,pos)

    local count =  CS.System.Int32.Parse(item.item_Count.text)
    local playerItemData =  PlayerItemData(item.belongTo,item.data.ID,pos,count)

    MainPanel.playerSaveData.itemData_List:Add(playerItemData)
end



-- 清除list 更新List中数据
function SettingPanel:SaveItemData()
      --清除以前的data
    MainPanel.playerSaveData.itemData_List:Clear()

    --Bag
    for i = 1, #BagPanel.bag_Item_List do
        if(BagPanel.bag_Item_List[i].data ~= nil)then
            self.SaveData(BagPanel.bag_Item_List[i],i)
        end
    end

    for i = 1, #BagPanel.fastBag_Item_List do
        if(BagPanel.fastBag_Item_List[i].data ~= nil)then
            self.SaveData(BagPanel.fastBag_Item_List[i],i)
        end
    end

    if(BagPanel.weapon_Item.data ~= nil)then
        self.SaveData(BagPanel.weapon_Item,0)
    end
    if(BagPanel.head_Item.data ~= nil)then
        self.SaveData(BagPanel.head_Item,0)
    end
    if(BagPanel.armor_Item.data ~= nil)then
        self.SaveData(BagPanel.armor_Item,0)
    end
    if(BagPanel.decoration_Item.data ~= nil)then
        self.SaveData(BagPanel.decoration_Item,0)
    end
    if(BagPanel.gem_Item.data ~= nil)then
        self.SaveData(BagPanel.gem_Item,0)
    end

end