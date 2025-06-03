---@diagnostic disable: undefined-global, need-check-nil

UIPanel = {}

UIPanel.panelObj = nil

UIPanel.toggle_Item = nil
UIPanel.toggle_Make = nil
UIPanel.toggle_Task = nil
UIPanel.toggle_Setting = nil

function UIPanel:OpenPanel(panel,isON)
    
    if isON == true then

        panel:ShowMe()
    else 
        
        if(panel.panelObj ~= nil)then
            panel:HideMe()
        end
        
    end

end

function UIPanel:Init()
    if(self.panelObj == nil) then

        self.panelObj =  ResourcesManager.Instance:loadRes("Prefabs/UI/UIPanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,Canvas,false)

        self.toggle_Item = self.panelObj.transform:Find("Tatle_BG/Toggle_Item"):GetComponent(typeof(Toggle))
        self.toggle_Make = self.panelObj.transform:Find("Tatle_BG/Toggle_Make"):GetComponent(typeof(Toggle))
        self.toggle_Task = self.panelObj.transform:Find("Tatle_BG/Toggle_Task"):GetComponent(typeof(Toggle))
        self.toggle_Setting = self.panelObj.transform:Find("Tatle_BG/Toggle_Setting"):GetComponent(typeof(Toggle))

        self.toggle_Item.onValueChanged:AddListener(function(isON)
            self:OpenPanel(BagPanel,isON)
        end)
        self.toggle_Make.onValueChanged:AddListener(function(isON)
            self:OpenPanel(MakePanel,isON)
        end)
        self.toggle_Task.onValueChanged:AddListener(function(isON)
            self:OpenPanel(TaskPanel,isON)
        end)
        self.toggle_Setting.onValueChanged:AddListener(function(isON)
            self:OpenPanel(SettingPanel,isON)
        end)

    end

end

function UIPanel:ShowMe()
    self:Init()
    self.panelObj:SetActive(true)
end

function UIPanel:HideMe()
    GameManager.Instance.isCameraCanMove = true
    GameManager.Instance.isPlayerCanMove = true
    Cursor.lockState = CS.UnityEngine.CursorLockMode.Locked;
    Time.timeScale = 1;
    self.panelObj:SetActive(false)
end

function UIPanel:GetActive()
    if(self.panelObj == nil) then
        return false
    end

    return self.panelObj.activeSelf
end


