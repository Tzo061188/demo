---@diagnostic disable: undefined-global, need-check-nil, missing-parameter, inject-field, undefined-field

Object:extend("Item")

Item.item_transfrom = nil
Item.item_Button = nil
Item.item_Icon = nil
Item.item_Count = nil
-- 1 Bag  2 Fast  3 weapon 4 head  5 armor  6 decoration  7 gem
Item.belongTo = nil    

Item.data = nil

local eventSystem = EventSystems.EventSystem.current
local graphicRaycaster = Canvas:GetComponent(typeof(UI.GraphicRaycaster))


function Item:Init()
    local eventTrigger =  self.item_transfrom.gameObject:AddComponent(typeof(EventSystems.EventTrigger))

    local DeginDrag = EventSystems.EventTrigger.Entry()
    DeginDrag.eventID = EventSystems.EventTriggerType.BeginDrag
    DeginDrag.callback:AddListener(function ()
        self:OnBeginDrag()
    end)

    local Drag = EventSystems.EventTrigger.Entry()
    Drag.eventID = EventSystems.EventTriggerType.Drag
    Drag.callback:AddListener(function ()
        self:OnDrag()
    end)

    local EndDrag = EventSystems.EventTrigger.Entry()
    EndDrag.eventID = EventSystems.EventTriggerType.EndDrag
    EndDrag.callback:AddListener(function ()
        self:OnEndDrag()
    end)

    eventTrigger.triggers:Add(DeginDrag)
    eventTrigger.triggers:Add(Drag)
    eventTrigger.triggers:Add(EndDrag)


    self.item_Button.onClick:AddListener(function ()
        self:OnClick()
    end
    )
end


local mouse_Pos = Vector2(0,0)
--开始拖拽  
function Item:OnBeginDrag()

    if(self.data == nil)then
        return
    end
    self.item_Count.gameObject:SetActive(false)
    self.item_Icon.gameObject:SetActive(false)

    
    BagPanel.drag_Image.gameObject:SetActive(true) 
    BagPanel.drag_Image.sprite = self.item_Icon.sprite
    --找到控件
    
    BagPanel.item_data = self.data --拿到数据
    BagPanel.count =  self.item_Count.text     -- 拿到物品个数
    
    mouse_Pos:Set(Input.mousePosition.x,Input.mousePosition.y)
    local _,Pos =  RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect,mouse_Pos);
    BagPanel.drag_Image.rectTransform.anchoredPosition = Pos
    
end
--拖拽
function Item:OnDrag()

        

   -- BagPanel.drag_Image.rectTransform.anchoredPosition = CS.Lua_Attribute.GetPos(CanvasRect)
   --input.mousePosition 是vector3 而这个方法要vector2
    mouse_Pos:Set(Input.mousePosition.x,Input.mousePosition.y)
    local _,Pos =  RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect,mouse_Pos);
    BagPanel.drag_Image.rectTransform.anchoredPosition = Pos
end
--结束拖拽
function Item:OnEndDrag()

    if(self.data == nil)then
        return
    end
    print(self.data.Name)

    BagPanel.drag_Image.gameObject:SetActive(false) -- 关闭拖拽的图片

    mouse_Pos:Set(Input.mousePosition.x,Input.mousePosition.y)

    --当前的鼠标位置
    local PointDataEvent = EventSystems.PointerEventData(eventSystem)
    PointDataEvent.position = mouse_Pos

    --射线检测
    local ResultList = List(EventSystems.RaycastResult)()
    graphicRaycaster:Raycast(PointDataEvent,ResultList)
    
    
    --置换位置
    local IsReplace = false
    for i = 0, ResultList.Count-1 do
        local result = ResultList[i]
        if(result.gameObject:TryGetComponent(typeof(Button)))then

            print("Button")

            local item = BagPanel:GetItem(result.gameObject.name)  -- 得到item 格子类
            if(item.data == nil)then --这个格子就是空的
                --背包
                if( item.belongTo == 1 ) then  
                    self:ReplaceData(item)
                    return  
                end
                --消耗品
                if( (item.belongTo == 1 or item.belongTo == 2) and BagPanel.item_data.Type == 6) then
                    self:ReplaceData(item)
                    return  
                end
                --武器
                if(item.belongTo == 3 and BagPanel.item_data.Type == 1) then
                    self:ReplaceData(item)
                    return  
                end
                --头盔
                if(item.belongTo == 4 and BagPanel.item_data.Type == 2) then
                    self:ReplaceData(item)
                    return  
                end
                --宝石
                if(item.belongTo == 7 and BagPanel.item_data.Type == 4) then
                    self:ReplaceData(item)
                    return  
                end
                --护甲
                if(item.belongTo == 5 and BagPanel.item_data.Type == 3) then
                    self:ReplaceData(item)
                    return  
                end
                 --饰品
                if(item.belongTo == 6 and BagPanel.item_data.Type == 5) then
                    self:ReplaceData(item)
                    return  
                end

            else -- 背包格子上有数据替换 
                --背包
                if( item.belongTo == 1) then   --和背包替换物品

                    if(item.data.Type ~=  BagPanel.item_data.Type)then  -- 物品类型不一致
                        
                        if(self.belongTo ~= 1)then --拿起物品 不在背包内

                            --无效置换
                            --for 循环走完了 也没替换成功 证明没找到格子或者失败
                            self.item_Icon.gameObject:SetActive(true)
    
                            if(self.data.Type == 6) then 
                                self.item_Count.gameObject:SetActive(true)
                            else
                                self.item_Count.gameObject:SetActive(false) --装备不显示数量
                            end
                            return

                        else  --拿起物品 在背包内
                            local data = item.data
                            local count =  item.item_Count.text
                            item:Set(BagPanel.item_data,BagPanel.count)
                            self:Set(data,count)
                            return  
                        end

                    else -- 物品类型一致

                        local data = item.data
                        local count =  item.item_Count.text
                        item:Set(BagPanel.item_data,BagPanel.count)
                        self:Set(data,count)
                        return  
                    end

                end
                if(item.data.Type == BagPanel.item_data.Type) then
                    local data = item.data
                    local count =  item.item_Count.text
                    item:Set(BagPanel.item_data,BagPanel.count)
                    self:Set(data,count)
                    return  
                end
            end

        end
    end
    --无效置换
   
    --for 循环走完了 也没替换成功 证明没找到格子或者失败
    self.item_Icon.gameObject:SetActive(true)

    if(self.data.Type == 6) then 
        self.item_Count.gameObject:SetActive(true)
    else
        self.item_Count.gameObject:SetActive(false) --装备不显示数量
    end
    
end



function Item:Set(data,count)

    self.data =  data 

    local IconData =  string.split(self.data.Icon,"/")
    local Atlas =  ResourcesManager.Instance:loadRes("Atlas/"..IconData[1],typeof(SpriteAtlas))
    self.item_Icon.sprite = Atlas:GetSprite(IconData[2])
    if(self.data.Type == 6) then 
        self.item_Count.text =  count
        self.item_Count.gameObject:SetActive(true)
    else
        self.item_Count.gameObject:SetActive(false) --装备不显示数量
    end
    self.item_Icon.gameObject:SetActive(true)

end


function Item:OnClick()
    if(self.data == nil)then
        BagPanel.item_Name.gameObject:SetActive(false)
        BagPanel.item_Describe.gameObject:SetActive(false)
        BagPanel.item_Attribute.gameObject:SetActive(false)
        return
    end

    BagPanel.item_Name.text = self.data.Name
    BagPanel.item_Describe.text = self.data.Describe
    BagPanel.item_Name.gameObject:SetActive(true)
    BagPanel.item_Describe.gameObject:SetActive(true)


    if(self.data.Type == 6) then --消耗品不显示
        BagPanel.item_Attribute.gameObject:SetActive(false)
        return
    else
        BagPanel.atkValue.text = self.data.AtkValue
        BagPanel.defValue.text = self.data.DefValue
        BagPanel.hpValue.text = self.data.HPValue
        BagPanel.item_Attribute.gameObject:SetActive(true)
    end


end

--单方替换数据
function Item:ReplaceData(item)
    item:Set(BagPanel.item_data,BagPanel.count)
    BagPanel.item_data = nil
    BagPanel.count = nil
    self.data = nil
    self.item_Count.gameObject:SetActive(false)
    self.item_Icon.gameObject:SetActive(false)
end