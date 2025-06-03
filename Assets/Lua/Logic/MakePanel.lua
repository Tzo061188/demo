---@diagnostic disable: need-check-nil, undefined-global
MakePanel = {}

MakePanel.panelObj = nil


function MakePanel:Init()
    if(self.panelObj == nil) then
        self.panelObj =  ResourcesManager.Instance:loadRes("Prefabs/UI/MakePanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,UIPanel.panelObj.transform)  
    end

end

function MakePanel:ShowMe()
    self:Init()
    self.panelObj:SetActive(true)
end
function MakePanel:HideMe()
    self.panelObj:SetActive(false)
end