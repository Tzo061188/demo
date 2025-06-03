---@diagnostic disable: undefined-global, empty-block, need-check-nil, param-type-mismatch

--读取json文件
local txt =  ResourcesManager.Instance:loadRes("Json/ItemData",typeof(TextAsset))
local Task_txt =  ResourcesManager.Instance:loadRes("Json/TaskData",typeof(TextAsset))
local Dialogue_txt =  ResourcesManager.Instance:loadRes("Json/DialogueData",typeof(TextAsset))
local StoreData_txt =  ResourcesManager.Instance:loadRes("Json/StoreData",typeof(TextAsset))
local StoreItemData_txt =  ResourcesManager.Instance:loadRes("Json/StoreItemData",typeof(TextAsset))
--将TextAsset.text 解析到lua的table中
local itemlist =  Json.decode(txt.text)
local tasklist =  Json.decode(Task_txt.text)
local dialoguelist = Json.decode(Dialogue_txt.text)

local StoreDatalist = Json.decode(StoreData_txt.text)
local StoreItemDatalist = Json.decode(StoreItemData_txt.text)
--全局物品数据
ItemData = {}

for _, value in pairs(itemlist) do
    ItemData[value.ID] = value;
end

--全局任务数据
TaskDataList = {}

for _, value in pairs(tasklist) do
    TaskDataList[value.TaskID] = value;
end

DialogueData = {}

for _, value in pairs(dialoguelist) do
    DialogueData[value.index] = value
end

--商店和商店物品数据
StoreData = {}

for _, value in pairs(StoreDatalist) do
    StoreData[value.Store_Type] = value
end

StoreItemData = {}

for _, value in pairs(StoreItemDatalist) do
    StoreItemData[value.Index] = value
end


