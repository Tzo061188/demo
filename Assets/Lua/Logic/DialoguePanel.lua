---@diagnostic disable: undefined-global, need-check-nil


DialoguePanel = {}

DialoguePanel.panelObj = nil
DialoguePanel.dialogue_Text = nil
DialoguePanel.Tips_Text = nil

DialoguePanel.currentIndex = nil
DialoguePanel.currentState = nil
DialoguePanel.dialogue_iFinish = false  -- 应该根据NPC的id 或者 名字 获取他的对话数据  用变量记录

local Next_Dialogue = function (context)

    if(DialoguePanel.dialogue_iFinish ==  false)then
        
        DialoguePanel.currentIndex = DialoguePanel.currentIndex + 1

        if(DialoguePanel.currentIndex > 3)then
            DialoguePanel.currentIndex = 3
            DialoguePanel:HideMe()
            return
        end
        DialoguePanel.dialogue_Text.text = DialogueData[DialoguePanel.currentIndex].content

        if(DialogueData[DialoguePanel.currentIndex].isTask == 1)then
            EventCenter.Instance:EventTrigger_InLua(AllEventName.AddTask,DialogueData[DialoguePanel.currentIndex].TaskID)
        end

    elseif (DialoguePanel.dialogue_iFinish ==  true) then

        DialoguePanel.currentIndex = 4

        print("打开商店")
        DialoguePanel:HideMe()

        StorePanel:ShowMe()
        
    end
        
end

--关注空格事件
function DialoguePanel:Init()


    if(self.panelObj == nil)then
        
        self.panelObj = ResourcesManager.Instance:loadRes("Prefabs/UI/DialoguePanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,Canvas,false)
        self.dialogue_Text =  self.panelObj.transform:Find("Dialogue_BG/Text"):GetComponent(typeof(Text));
        self.Tips_Text =  self.panelObj.transform:Find("Dialogue_BG/Tips_Text"):GetComponent(typeof(Text));
        self.currentIndex = 1
        self.currentState = 0
    end
    
    RoleInputSystem.Instance.roleInput.Role.Sprint:started("+",Next_Dialogue)
end


function DialoguePanel:ShowMe()
    self:Init()

    GameManager.Instance.isPlayerCanMove = false  -- 关闭玩家移动
    GameManager.Instance.isCameraCanMove = false  -- 关闭摄像机移动
    Cursor.lockState = CS.UnityEngine.CursorLockMode.Locked;  --锁定鼠标


    local currentValue =  DialogueData[self.currentIndex]

    if(self.dialogue_iFinish == true)then
        currentValue = DialogueData[4]
    end
   
    if(MainPanel.playerSaveData.task_List.Count>0)then
        if(MainPanel.playerSaveData.task_List[0].isDone == 1 and self.dialogue_iFinish == false)then
    
            currentValue = DialogueData[4]
    
            self.dialogue_iFinish = true
        end
    end

    self.dialogue_Text.text = currentValue.content

    self.panelObj:SetActive(true)
end


function DialoguePanel:HideMe()

    GameManager.Instance.isPlayerCanMove = true  -- 打开玩家移动
    GameManager.Instance.isCameraCanMove = true  -- 打开摄像机移动
    Cursor.lockState = CS.UnityEngine.CursorLockMode.Locked;  --锁定鼠标


    RoleInputSystem.Instance.roleInput.Role.Sprint:started("-",Next_Dialogue)
    self.panelObj:SetActive(false)
end



function DialoguePanel:InitDialogue(BelongTo,index)
    
end