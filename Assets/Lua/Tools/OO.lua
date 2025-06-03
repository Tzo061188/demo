--面向对象--
Object = {}

--封装--
function Object:new()
    local obj = {};
    setmetatable(obj,self);
    self.__index = self;
    return obj;
end

--继承--

function Object:extend(className)
    _G[className] = {};
    local obj  = _G[className];
    --设置base 指向它的父类
    obj.base = self;
    setmetatable(obj,self);
    self.__index = self;
    
end