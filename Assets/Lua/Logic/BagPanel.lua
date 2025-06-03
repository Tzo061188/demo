---@diagnostic disable: unbalanced-assignments, undefined-global, need-check-nil

BagPanel = {}

BagPanel.panelObj = nil

--weapon describe --
BagPanel.item_Name,BagPanel.item_Describe,BagPanel.item_Attribute,BagPanel.atkValue,BagPanel.defValue,BagPanel.hpValue = nil
--fast Item  --

BagPanel.fastBag_Item_List = {
    BagPanel.item_Fast0,BagPanel.item_Fast1,BagPanel.item_Fast2,BagPanel.item_Fast3,BagPanel.item_Fast4,BagPanel.item_Fast5
}
--item --
BagPanel.bag_Item_List = {
    BagPanel.bag_Item0,BagPanel.bag_Item1,BagPanel.bag_Item2,BagPanel.bag_Item3,BagPanel.bag_Item4,BagPanel.bag_Item5,
    BagPanel.bag_Item6,BagPanel.bag_Item7,BagPanel.bag_Item8,BagPanel.bag_Item9,BagPanel.bag_Item10,BagPanel.bag_Item11,
    BagPanel.bag_Item12,BagPanel.bag_Item13,BagPanel.bag_Item14,BagPanel.bag_Item15,BagPanel.bag_Item16,BagPanel.bag_Item17,
    BagPanel.bag_Item18,BagPanel.bag_Item19,BagPanel.bag_Item20,BagPanel.bag_Item21,BagPanel.bag_Item22,BagPanel.bag_Item23
}
    
--Equip ---
BagPanel.weapon_Item,BagPanel.head_Item,BagPanel.armor_Item,BagPanel.decoration_Item,BagPanel.gem_Item = nil

--Player attribue ---
BagPanel.player_Atk,BagPanel.player_Def,BagPanel.player_HP = nil

--
BagPanel.drag_Image = nil 
BagPanel.item_data = nil
BagPanel.count = nil

function BagPanel:Init()
    if(self.panelObj == nil) then
        self.panelObj =  ResourcesManager.Instance:loadRes("Prefabs/UI/BagPanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,UIPanel.panelObj.transform)
        --item  背包的初始化  --
        local H =  self.panelObj.transform:Find("Bag_Area/H_1")
        self:Init_Item(H,self.bag_Item_List,1,8,"Bag_Item",1)  

        H =  self.panelObj.transform:Find("Bag_Area/H_2")
        self:Init_Item(H,self.bag_Item_List,9,16,"Bag_Item",1)  

        H =  self.panelObj.transform:Find("Bag_Area/H_3")
        self:Init_Item(H,self.bag_Item_List,17,24,"Bag_Item",1)  

        --weapon describe  描述 的初始化 --
        self.item_Name = self.panelObj.transform:Find("Bag_Area/Describe_BG/Item_Name"):GetComponent(typeof(Text))
        self.item_Describe = self.panelObj.transform:Find("Bag_Area/Describe_BG/Item_Describe"):GetComponent(typeof(Text))
        
        self.item_Attribute  = self.panelObj.transform:Find("Bag_Area/Describe_BG/Item_Attribute")
        self.atkValue = self.item_Attribute.transform:Find("AtkValue"):GetComponent(typeof(Text))
        self.defValue = self.item_Attribute.transform:Find("DefValue"):GetComponent(typeof(Text))
        self.hpValue = self.item_Attribute.transform:Find("HPValue"):GetComponent(typeof(Text))
        --fast Item  快捷栏为的物品栏 --
        H =  self.panelObj.transform:Find("FastItem_Area")
        self:Init_Item(H,self.fastBag_Item_List,1,6,"Item_Fast",2)

        --Equip   装备武器的初始化---
        self.weapon_Item = self:Init_Equip_Item("Equip_Area/Weapon/Item_Weapon",3)
        self.head_Item = self:Init_Equip_Item("Equip_Area/Head/Item_Head",4)
        self.armor_Item = self:Init_Equip_Item("Equip_Area/Armor/Item_Armor",5)
        self.decoration_Item = self:Init_Equip_Item("Equip_Area/Decoration/Item_Decoration",6)
        self.gem_Item = self:Init_Equip_Item("Equip_Area/Gem/Item_Gem",7)

        --Player attribue 玩家数据显示描述的初始化 ---
        self.player_Atk = self.panelObj.transform:Find("Player_Attribute_Area/AtkValue"):GetComponent(typeof(Text))
        self.player_Def = self.panelObj.transform:Find("Player_Attribute_Area/DefValue"):GetComponent(typeof(Text))
        self.player_HP = self.panelObj.transform:Find("Player_Attribute_Area/HPValue"):GetComponent(typeof(Text))

        --
        self.drag_Image = self.panelObj.transform:Find("Drag_Image"):GetComponent(typeof(Image))
    end

    
end

function BagPanel:ShowMe()
    self:Init()
    self:Init_Data() --更新bag数据
    self.panelObj:SetActive(true)
end
function BagPanel:HideMe()

    self.panelObj:SetActive(false)
    --:每次关闭应该保存数据
    SettingPanel:SaveItemData() -- 保存一次List的数据 
    PlayerData:UpdateData()     -- 更新playerdata的数据  读取是通过playerdata的三个表  所以要重新构建
end

--获取一个空的物品栏
function BagPanel.Get_Empty_Inventory()
    for i = 1, #BagPanel.bag_Item_List do
        if(BagPanel.bag_Item_List[i].data == nil)then
            return BagPanel.bag_Item_List[i],i
        end
    end
end


 --- 更具数据设置 格子上的物品显示
function BagPanel:Init_Data()
    
    self.item_Name.gameObject:SetActive(false)
    self.item_Describe.gameObject:SetActive(false)
    self.item_Attribute.gameObject:SetActive(false)


    local Data = nil
    local pos = nil

    for i = 1,#PlayerData.BagData do   -- 初始化的Bag的
        local data =  PlayerData.BagData[i]
        Data =  ItemData[data.ID] 
        pos =  data.pos 
        self.bag_Item_List[pos]:Set(Data,data.Count)
    end



    for i = 1,#PlayerData.FastBagData do   -- 初始化的Bag的
        Data = ItemData[ PlayerData.FastBagData[i].ID ]
        pos =  PlayerData.FastBagData[i].pos
        self.fastBag_Item_List[pos]:Set(Data,PlayerData.FastBagData[i].Count)
    end

    for i = 1,#PlayerData.EquipData do
        local equip =  PlayerData.EquipData[i]
        Data = ItemData[ PlayerData.EquipData[i].ID ]
        if(equip.belongTo == 3)then   self.weapon_Item:Set(Data)
        elseif (equip.belongTo == 4) then  self.head_Item:Set(Data)
        elseif (equip.belongTo == 5) then  self.armor_Item:Set(Data)
        elseif (equip.belongTo == 6) then  self.decoration_Item:Set(Data)
        elseif (equip.belongTo == 7) then  self.gem_Item:Set(Data)

        end
    end
end

----  初始化 背包的格子 上的数据  关联控件
function BagPanel:Init_Item(parent,list,start_Num,End_num,FindStr,belongTo)  -- 能取到num  list的索引是从1开始的  
    for i = start_Num, End_num do

        list[i] = Item:new()

        local Item_transfrom = parent.transform:Find(FindStr..i-1)  
        local Item_Button = Item_transfrom:GetComponent(typeof(Button))
        local Item_Icon = Item_transfrom:Find("Item_Icon"):GetComponent(typeof(Image))
        local Item_Count = Item_transfrom:Find("Item_Count"):GetComponent(typeof(Text))
        
        list[i].item_transfrom = Item_transfrom
        list[i].item_Button = Item_Button
        list[i].item_Icon = Item_Icon
        list[i].item_Count = Item_Count
        list[i].belongTo = belongTo
        list[i]:Init()
    end
end

----  初始化 背包的格子 上的数据  关联控件
function BagPanel:Init_Equip_Item(FindStr,belongTo)  -- 能取到num  list的索引是从1开始的  
    
    local item = Item:new()
    
    local Item_transfrom = self.panelObj.transform:Find(FindStr)

    local Item_Button = Item_transfrom:GetComponent(typeof(Button))
    local Item_Icon = Item_transfrom:Find("Item_Icon"):GetComponent(typeof(Image))
    local Item_Count = Item_transfrom:Find("Item_Count"):GetComponent(typeof(Text))

    item.item_transfrom = Item_transfrom
    item.item_Button = Item_Button
    item.item_Icon = Item_Icon
    item.item_Count = Item_Count
    item.belongTo = belongTo
    item:Init()
    return item
end


---  通过名称 获取一个格子数据 
function BagPanel:GetItem(name)
    for i = 1, #self.bag_Item_List do
        if(self.bag_Item_List[i].item_transfrom.gameObject.name == name) then
            return self.bag_Item_List[i]
        end
    end 
    for i = 1, #self.fastBag_Item_List do
        if(self.fastBag_Item_List[i].item_transfrom.gameObject.name == name) then
            return self.fastBag_Item_List[i]
        end
    end
    if(self.head_Item.item_transfrom.gameObject.name == name)then
        return self.head_Item
    end
    if(self.weapon_Item.item_transfrom.gameObject.name == name)then
        return self.weapon_Item
    end
    if(self.armor_Item.item_transfrom.gameObject.name == name)then
        return self.armor_Item
    end
    if(self.decoration_Item.item_transfrom.gameObject.name == name)then
        return self.decoration_Item
    end
    if(self.gem_Item.item_transfrom.gameObject.name == name)then
        return self.gem_Item
    end
end