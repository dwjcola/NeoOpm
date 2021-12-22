---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by liuyubao.
--- DateTime: 2021/12/17 16:18
---

local function getGlobalScheduler()
    return TimerScheduler
end

--[[
    @desc ScheduleEntry of BattleScheduler
]]
---@type table<string, number>
local kTaskStatus = {
    Running = 0,
    Stop = 1,
    Pause = 2,
    Unknow = 4,
}

---@class ScheduleEntry : SimpleClassUtil
local ScheduleEntry = SimpleClassUtil:class()

---@param scheduler BattleScheduler
function ScheduleEntry:initialize(scheduler)
    self._scheduler = scheduler
    self:reset()
end

function ScheduleEntry:reset()
    self._callback = nil
    self._interval = 0
    self._accuTime = 0
    self._triggerCallbackOnNextTick = false
    self._status = kTaskStatus.Stop
end

---@param callback fun(t:any, dt:number)
---@param interval number
---@param triggerImmediately boolean
function ScheduleEntry:start(
        callback,
        interval,
        triggerImmediately)
    self._callback = callback
    self._interval = interval
    self._accuTime = 0

    if triggerImmediately then
        self._triggerCallbackOnNextTick = true
    end
    self._status = kTaskStatus.Running
end

function ScheduleEntry:getScheduler()
    return self._scheduler
end

function ScheduleEntry:isDeleted()
    return self._status == kTaskStatus.Stop
end

---@param dtInS number @ ms
function ScheduleEntry:tick(dtInS)
    -- XXX `_isDeleted' is already checked in scheduler.
    -- if self._isDeleted then return end
    if self._status ~= kTaskStatus.Running then
        return
    end
    local interval = self._interval
    if interval == nil or interval <= 0 then
        self._callback(self, dtInS)
    else
        if self._triggerCallbackOnNextTick then
            self._callback(self, dtInS)
            self._triggerCallbackOnNextTick = nil
            self._accuTime = 0
        else
            -- self._accuTime: accumulated time
            dtInS = (self._accuTime or 0) + dtInS
            while dtInS >= interval and self._status == kTaskStatus.Running do
                self._callback(self, interval)
                dtInS = dtInS - interval
            end
        end
        self._accuTime = dtInS
    end
end

-- function ScheduleEntry:stop()
--     self._status = kTaskStatus.Stop;
-- end

function ScheduleEntry:pause()
    -- body
    self._status = kTaskStatus.Pause
end

function ScheduleEntry:resume()
    -- body
    self._status = kTaskStatus.Running
end

function ScheduleEntry:isPause()
    -- body
    return (self._status == kTaskStatus.Pause)
end

function ScheduleEntry:release()
    if self._scheduler then
        self._scheduler:_backScheduleObj(self)
    else
        self:reset()
    end
end

--------------------------------------------------------------------------------

---@class BattleSchedulerMode
local BattleSchedulerMode = {
    kNormal = 1,
    kMaxDt = 2,
}

--[[
    @desc The scheduler object.
]]
---@class BattleScheduler : MiddleClass
---@field _pool List @{ScheduleEntry}
local BattleScheduler = SimpleClassUtil:class()

---@param poolSize number @default 1
---@param mode BattleSchedulerMode
function BattleScheduler:initialize(poolSize, mode)
    ---@type table<number, ScheduleEntry> | table
    self._tasks = {}
    ---@type number
    self._timeScale = 1
    poolSize = poolSize or 1
    self._pool = {}
    self._rmPool = {} --删除列表
    for i=1, poolSize do
        self._pool[i] = ScheduleEntry:new(self)
    end
    self._logicTime = 0   -- 最大当前消耗，用于pvp网络慢，掉帧计时器停住
    self._consumeTime = 0 -- 必须消耗，用于pvp时渲染追帧时，计时器消耗
    self._timeMode = BattleSchedulerMode.kNormal

end

---@param mode BattleSchedulerMode
function BattleScheduler:setSchedulerMode(mode)
    if mode then
        self._timeMode = mode
    else
        self._timeMode = BattleSchedulerMode.kNormal
    end
end

---@return ScheduleEntry
function BattleScheduler:_getScheduleObj()
    ---@type ScheduleEntry
    local schedule
    if #self._pool>0 then
        schedule = self._pool[#self._pool]
        self._pool[#self._pool] = nil
    else
        schedule = ScheduleEntry:new(self)
    end
    return schedule
end

---@param task ScheduleEntry
function BattleScheduler:_backScheduleObj(task)
    task:reset()
    self._rmPool[#self._rmPool+1] = task
end

function BattleScheduler:start()
    if self.tickEntry == nil then
        self.tickEntry = getGlobalScheduler():schedulePerFrame(
                function()
                    self:_tick(TimerScheduler.deltaTime)
                end)
    end
end

function BattleScheduler:stop()
    if self.tickEntry ~= nil then
        getGlobalScheduler():removeSchedule(self.tickEntry)
        self.tickEntry = nil
        self._tasks = {}
        self._timeScale = 1;
    end
end

function BattleScheduler:pause()
    self._isPaused = true
end

function BattleScheduler:resume()
    self._isPaused = false
end

function BattleScheduler:isRunning()
    return self.tickEntry ~= nil
end

--[[
    @desc Schedule a function every interval time.
    @param callback The function to be scheduled.
    @param interval The interval time in seconds. The default value is 0.
    @param triggerImmediately Whether schedule the callback immediately. The
        default value false.
]]

---@overload fun(cb:fun(t:ScheduleEntry, dt:number), interval:number)
---@overload fun(cb:fun(t:ScheduleEntry, dt:number))
---@param callback fun(t:ScheduleEntry, dt:number)
---@param interval number
---@param triggerImmediately boolean
function BattleScheduler:schedule(callback, interval, triggerImmediately)
    assert(callback ~= nil, "callback required but got nil!")
    local task = self:_getScheduleObj()--ScheduleEntry:new(self, callback, interval, triggerImmediately)
    task:start(callback, interval, triggerImmediately)
    -- self._tasks[task] = task
    self._tasks[#self._tasks + 1] = task
    return task
end

---@param duration number
---@param updateFunc fun(dt:number)
---@param endFunc fun()
function BattleScheduler:runUpdateTask(duration, updateFunc, endFunc)
    if duration <=0 then
        assert(false, "arguments error duration <= 0  duration = " .. duration)
    end
    assert(updateFunc ~= nil, "arguments error updateFunc == nil")
    local step = 1 / duration
    local p = 0

    ---@param task ScheduleEntry
    ---@param dt number
    local task = self:schedule(function(task, dt)
        p = p + dt * step
        local ended = false
        if p >= 1 then
            p = 1
            ended = true
        end
        updateFunc(p)
        if ended then
            if endFunc ~= nil then
                endFunc()
            end
            self:unschedule(task)
        end
    end, 0)

    -- start
    updateFunc(0)

    return task
end

---@param task ScheduleEntry
function BattleScheduler:unschedule(task)
    if task and task:getScheduler() == self then
        self:_backScheduleObj(task)
    end
end

function BattleScheduler:unscheduleAll()
    --@type ScheduleEntry
    local task
    for i = 1, #self._tasks do
        task = self._tasks[i]
        task:release()
    end
    self._tasks = {}
end

function BattleScheduler:doRmPoolToPool()
    for i=#self._rmPool, 1, -1 do
        self._pool[#self._pool + 1] = self._rmPool[i]
        self._rmPool[i] = nil
    end
end

function BattleScheduler:addLogicTime(dt)
    self._consumeTime = self._consumeTime+self._logicTime
    self._logicTime = dt
end

---@param dt number
function BattleScheduler:_tick(dt) -- pvp模式下，如果网络卡顿，时间逻辑显示对不上
    if self._isPaused then
        return
    end

    ---@type number
    dt = dt * self._timeScale

    if self._tasks == nil or next(self._tasks) == nil then
        return
    end

    if self._timeMode == BattleSchedulerMode.kMaxDt then
        if self._logicTime > 0 then
            dt = math.min(self._logicTime, dt)
            self._logicTime = self._logicTime - dt
        end
        dt = dt+self._consumeTime
        self._consumeTime = 0
    end

    -- 先检查无效计时器
    local tasks = self._tasks
    local count = #tasks
    local fidx = 1
    for i = 1, count do
        local task = tasks[i]
        if task:isDeleted() then
            tasks[i] = nil
        else
            if i ~= fidx then
                tasks[fidx] = task
                tasks[i] = nil
            end
            fidx = fidx + 1
        end
    end

    -- 将回收的计时器加回缓存池
    self:doRmPoolToPool()

    local imax = #tasks
    ---@type ScheduleEntry
    local task
    for i = 1, imax do
        -- 在此期间新加的task索引必大于imax，它们将在下一帧开始执行
        task = tasks[i]
        -- 该task可能被其它(已经执行的)task停止，故须在tick前再次检测其是否有效
        if task ~= nil and not task:isDeleted() then
            task:tick(dt)
        end
    end
end

---@param timeScale number
function BattleScheduler:setTimeScale(timeScale)
    self._timeScale = timeScale;
end

function BattleScheduler:getTimeScale()
    return self._timeScale;
end

---@param seconds number
---@param func fun()
function BattleScheduler:callWithDelay(seconds, func)
    assert(seconds ~= nil, "arguments error seconds == nil");

    local cdtime = seconds;
    ---@param task ScheduleEntry
    ---@param dt number
    local task = self:schedule(function(task, dt)
        cdtime = cdtime - dt
        if cdtime <= 0 then
            cdtime = nil
            xpcall(
                    function()
                        func()
                    end,
                    function(msg)
                        error(msg)
                    end
            )
            self:unschedule(task)
        end
    end)

    return task;
end

return { ScheduleEntry, BattleScheduler,BattleSchedulerMode }