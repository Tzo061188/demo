---@diagnostic disable: need-check-nil, undefined-global, inject-field




TaskPanel = {}

TaskPanel.panelObj = nil
TaskPanel.content = nil
TaskPanel.taskName = nil
TaskPanel.task_Reward = nil
TaskPanel.task_describe = nil
TaskPanel.Button_Finish = nil
TaskPanel.Button_Enter = nil

TaskPanel.currentTaskData = nil  --当前选中的任务数据

TaskPanel.task_List = nil  --C#的list
--应该有一个变量记录当前点击到的事件数据  里面  包含  ！！奖励的是否领取
TaskPanel.isGetReward = false


function TaskPanel:Init()
    if(self.panelObj == nil) then
        self.panelObj =  ResourcesManager.Instance:loadRes("Prefabs/UI/TaskPanel",typeof(GameObject))
        self.panelObj = GameObject.Instantiate(self.panelObj,UIPanel.panelObj.transform)
        self.content = self.panelObj.transform:Find("Scroll View/Viewport/Content").transform
        self.taskName = self.panelObj.transform:Find("Task_Name"):GetComponent(typeof(Text))
        self.task_describe = self.panelObj.transform:Find("Task_Describe"):GetComponent(typeof(Text))
        self.task_Reward = self.panelObj.transform:Find("Task_Reward"):GetComponent(typeof(Text))
        self.Button_Finish = self.panelObj.transform:Find("Button_Finish"):GetComponent(typeof(Button))
        self.Button_Enter = self.panelObj.transform:Find("Button_Enter"):GetComponent(typeof(Button))

        self.Button_Finish.onClick:AddListener(function ()
            --细分奖励类型
            if self.currentTaskData.taskID == 2  then
                self.isGetReward = true  --领过奖之后应该保存数据
                MainPanel.playerSaveData.Gold_number = 500
                MainPanel.Gold_number.text = 500;
                self.Button_Finish.gameObject:SetActive(false)  --隐藏
            end

        end)

        self.Button_Enter.onClick:AddListener(function ()
            UIPanel:HideMe()
            GameManager.Instance:LoadScene()
        end)

    end
end

function TaskPanel:ShowMe()
    
    self:Init()
    
    self:Update_TaskData()
    
    self.panelObj:SetActive(true)
end
function TaskPanel:HideMe()
    self.taskName.gameObject:SetActive(false)
    self.task_describe.gameObject:SetActive(false)
    self.task_Reward.gameObject:SetActive(false)
    self.Button_Finish.gameObject:SetActive(false)
    self.Button_Enter.gameObject:SetActive(false)
    self.panelObj:SetActive(false)
end

function TaskPanel:Update_TaskData()

    -- 清除任务
    if(self.content.childCount>0)then
        for  i = 0, self.content.childCount - 1 do       
            GameObject.Destroy(self.content:GetChild(i).gameObject);
        end
    end

    self.task_List = MainPanel.playerSaveData.task_List
    self:Init_TaskData()
end

function TaskPanel:Init_TaskData()
   

    for i = 0, self.task_List.Count-1 do

        --实例化 任务 
        local Task =  ResourcesManager.Instance:loadRes("Prefabs/UI/Task",typeof(GameObject))
        Task = GameObject.Instantiate(Task,self.content)

        -- 设置位置
        Task:GetComponent(typeof(RectTransform)).anchoredPosition = Vector2(0,90 - i*120)

        --获取控件
        local Task_name = Task.transform:Find("Name"):GetComponent(typeof(Text))
        local Task_IsDone = Task.transform:Find("IsDone"):GetComponent(typeof(Text))
        local Task_Button = Task:GetComponent(typeof(Button))

        -- 设置任务 的 控件显示的文字
        Task_name.text =  self.task_List[i].taskName

        if(self.task_List[i].isDone == 0)then
            Task_IsDone.text = "未完成"
        else
            Task_IsDone.text = "完成"
        end

        Task_Button.onClick:AddListener(function ()
            self:SetDescribe(self.task_List[i])
        end)
        
    end
end 

function TaskPanel:SetDescribe(data)

    self.currentTaskData = data

    if(data.isDone == 1 and self.isGetReward == false)then
        self.Button_Finish.gameObject:SetActive(true)
        
    else
        self.Button_Finish.gameObject:SetActive(false)
    end

    if(data.isDone == 0)then
        self.Button_Enter.gameObject:SetActive(true)
    else
        self.Button_Enter.gameObject:SetActive(false)
    end


    self.taskName.text = data.taskName
    self.task_describe.text = data.taskDescribe
    self.task_Reward.text = data.rewardDescribe

    self.taskName.gameObject:SetActive(true)
    self.task_describe.gameObject:SetActive(true)
    self.task_Reward.gameObject:SetActive(true)
end