---@diagnostic disable: undefined-global, need-check-nil


SceneLoadBar = {}

SceneLoadBar.BG = nil
SceneLoadBar.Text = nil
SceneLoadBar.a = nil
SceneLoadBar.color = nil


function SceneLoadBar:Init()
    if(self.BG == nil)then
        self.BG =  Canvas:Find("SceneLoadBar"):GetComponent(typeof(Image))
        self.Text =  Canvas:Find("SceneLoadBar/Text")
        self.a = 0
        self.color =  Color(self.BG.color.r,self.BG.color.g,self.BG.color.b,self.a)
    end
end


local function Show()
    if(SceneLoadBar.a < 1)then
        SceneLoadBar.a = SceneLoadBar.a + Time.deltaTime
    end

    if(SceneLoadBar.a >= 1)then
        SceneLoadBar.a = 1
        SceneLoadBar.BG.color = SceneLoadBar.color
        SceneLoadBar.Text.gameObject:SetActive(true)
        MonoManager.Instance:RemoveUpdate(Show)
        return
    end
    SceneLoadBar.BG.color = SceneLoadBar.color
end

local function Hide()
    if(SceneLoadBar.a>=1)then
        SceneLoadBar.a = SceneLoadBar.a - Time.deltaTime
    end

    if(SceneLoadBar.a <= 0)then
        SceneLoadBar.a = 0
        SceneLoadBar.BG.color = SceneLoadBar.color
        SceneLoadBar.Text.gameObject:SetActive(false)
        MonoManager.Instance:RemoveUpdate(Hide)
        return
    end
    SceneLoadBar.BG.color = SceneLoadBar.color

end



function SceneLoadBar.ShowMe()
    SceneLoadBar.a = 0
    MonoManager.Instance:AddUpdate(Show)
end

function SceneLoadBar.HideMe()
    SceneLoadBar.a = 1
    MonoManager.Instance:AddUpdate(Hide)
end



