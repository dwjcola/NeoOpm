--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

--根管理器
manager.BaseManager=class()

--endregion
function manager.BaseManager:ctor()
    manager.ManagerControllor.GetInstance():Register(self)
end


function manager.BaseManager:Update(deltatime, unscaledDeltaTime)
    
end

function manager.BaseManager:FixedUpdate(fixedDeltaTime)
    
end

function manager.BaseManager:LateUpdate()
    
end


function manager.BaseManager:Release()
    manager.ManagerControllor.GetInstance():Deregister(self)
end

