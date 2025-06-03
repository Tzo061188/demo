---@diagnostic disable: undefined-global, need-check-nil

StorePanel = {}

StorePanel.panelObj = nil
StorePanel.type_Content = nil
StorePanel.item_content = nil
StorePanel.Group = nil
StorePanel.button_Close = nil

StorePanel.firstStore = nil

function StorePanel:Init()
    if(self.panelObj == nil)then
        self.panelObj = ResourcesManager.Instance:loadRes("Prefabs/UI/StorePanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,Canvas,false)

        self.type_Content = self.panelObj.transform:Find("Store_BG/Store_Type/Viewport/Content")
        self.Group = self.type_Content:GetComponent(typeof(UI.ToggleGroup))


        self.item_content = self.panelObj.transform:Find("Store_BG/Store_Item/Viewport/Content")

        self.button_Close = self.panelObj.transform:Find("Store_BG/Button_Close"):GetComponent(typeof(Button))

        self.button_Close.onClick:AddListener(function ()
            self:HideMe()
        end)

        self:InitStore()
    end
end


function StorePanel:ShowMe()
    self:Init()

    GameManager.Instance.isPlayerCanMove = false  -- 关闭玩家移动
    GameManager.Instance.isCameraCanMove = false  -- 关闭摄像机移动

    Cursor.lockState = CS.UnityEngine.CursorLockMode.None;  --显示鼠标

    -- 进来就显示
    self.firstStore.isOn = true

    self.panelObj:SetActive(true)
end



function StorePanel:HideMe()

    Cursor.lockState = CS.UnityEngine.CursorLockMode.Locked;  --锁定鼠标
    GameManager.Instance.isPlayerCanMove = true  -- 打开玩家移动
    GameManager.Instance.isCameraCanMove = true  -- 打开摄像机移动
    self.panelObj:SetActive(false)

end

function StorePanel:InitStore()

    for i = 1, #StoreData do

        local Store_Toggle_button = ResourcesManager.Instance:loadRes("Prefabs/UI/Store_Toggle",typeof(GameObject))
        Store_Toggle_button = GameObject.Instantiate(Store_Toggle_button,self.type_Content,false)
        -- 设置位置
        Store_Toggle_button:GetComponent(typeof(RectTransform)).anchoredPosition = Vector2(0,-90 - (i-1)*70)
        --获取控件
        Store_Toggle_button:GetComponent(typeof(Toggle)).group = self.Group

        Store_Toggle_button:GetComponent(typeof(Toggle)).onValueChanged:AddListener(function (value)

            StorePanel.Create_StoreItem(value,StoreData[i].Store_Type)

        end)

        Store_Toggle_button.transform:Find("Label"):GetComponent(typeof(Text)).text = StoreData[i].Store_Name
        
        if(i == 1)then
            self.firstStore =  Store_Toggle_button:GetComponent(typeof(Toggle))
            Store_Toggle_button:GetComponent(typeof(Toggle)).isOn = true
        end

    end
end


function StorePanel.Create_StoreItem(value,Store_Type)

    if(StorePanel.item_content.childCount > 0)then  --删除

        for i = 0, StorePanel.item_content.childCount - 1  do

            GameObject.Destroy(StorePanel.item_content:GetChild(i).gameObject)
        end
    end

    if(value == true)then

        for i = 1, #StoreItemData do
            if(StoreItemData[i].BelongTo_Store == Store_Type)then -- 匹配
            
                local store_Item = ResourcesManager.Instance:loadRes("Prefabs/UI/Store_Item",typeof(GameObject))
                store_Item = GameObject.Instantiate(store_Item,StorePanel.item_content,false)
                -- 设置位置
                local pos = nil;

                ---因为写在了一起  所以要减去7 减去前面的索引 防止 尾随  后面配置要注意！！！！
                if (Store_Type == 1) then
                    pos = Vector2(150 + (i-1)%4 * 170 , -55 - math.floor((i-1)/4) * 165)
                elseif(Store_Type == 2)then
                    pos = Vector2(150 + (i-1-7)%4 * 170 , -55 - math.floor((i-1-7)/4) * 165)
                end
                        ---后面配置要注意！！！！
                store_Item:GetComponent(typeof(RectTransform)).anchoredPosition = pos

                --获取对应Id的物品数据
                local itemData =  ItemData[StoreItemData[i].ID]
                
                --加载图集
                local IconData =  string.split(itemData.Icon,"/")
                local Atlas =  ResourcesManager.Instance:loadRes("Atlas/"..IconData[1],typeof(SpriteAtlas))

                --设置
                store_Item:GetComponent(typeof(Button)).onClick:AddListener(function ()
                    StorePanel.BuyItem(itemData,StoreItemData[i].Need_Gold)
                end)
                
                store_Item.transform:Find("Item_Icon"):GetComponent(typeof(Image)).sprite = Atlas:GetSprite(IconData[2])
                store_Item.transform:Find("Item_Count"):GetComponent(typeof(Text)).text = StoreItemData[i].Need_Gold
            
            end
        end

    end

end

function StorePanel.BuyItem(itemData,Need_Gold)
    if(MainPanel.playerSaveData.Gold_number >= Need_Gold)then
        --可以购买
        MainPanel.playerSaveData.Gold_number = MainPanel.playerSaveData.Gold_number - Need_Gold
        MainPanel.Gold_number.text = MainPanel.playerSaveData.Gold_number;


        --获取一个空各自的位置
        local _,pos =  BagPanel.Get_Empty_Inventory()

        MainPanel.playerSaveData.itemData_List:Add(PlayerItemData(1,itemData.ID,pos,1))
        
        PlayerData:UpdateData()     -- 更新playerdata的数据  读取是通过playerdata的三个表  所以要重新构建

    end
end