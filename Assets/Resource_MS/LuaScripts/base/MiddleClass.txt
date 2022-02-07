--
--	模拟实现类，类的继承
-- Q: 没有看到，从子类实例访问到父类实例中变量的情况
--
local middleclass = {
    _VERSION = "middleclass v4.1.1",
    _DESCRIPTION = "Object Orientation for Lua",
    _URL = "https://github.com/kikito/middleclass",
    _LICENSE = [[
    MIT LICENSE

    Copyright (c) 2011 Enrique García Cota

    Permission is hereby granted, free of charge, to any person obtaining a
    copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be included
    in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
    OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
    CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
    SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
  ]]
}

-- 类实例，
-- 自己的__instanceDict中找不到的时候
-- 去访问父类__instanceDict
local function _createIndexWrapper(aClass, f)
    if f == nil then
        return aClass.__instanceDict
    else
        -- 父类向子类propogate __index函数的时候，
        -- 自己的__index函数，包裹一层父类的__index函数
        return function(self, name)
            -- 先从自己的__instanceDict中去取
            local value = aClass.__instanceDict[name]

            if value ~= nil then
                -- 如果没取到，再从父类去取
                return value
            elseif type(f) == "function" then
                return (f(self, name))
            else
                return f[name]
            end
        end
    end
end

-- 在subclass()中有调用
-- 	* aClass是子类
--	* name, f是从父类的__instanceDict中获取
--	* 放入到子类的__instanceDict中
-- 在_declareInstanceMethod()中有调用
local function _propagateInstanceMethod(aClass, name, f)
    -- 特殊处理__index函数
    f = name == "__index" and _createIndexWrapper(aClass, f) or f

    -- 放入到子类的__instanceDict中
    aClass.__instanceDict[name] = f

    -- 遍历aClass的子类
    -- 如果子类__declaredMethods中没有声明，同名方法
    -- 则将方法传递到子类的__instanceDict中
    for subclass in pairs(aClass.subclasses) do
        if rawget(subclass.__declaredMethods, name) == nil then
            _propagateInstanceMethod(subclass, name, f)
        end
    end
end

-- 类的__newindex
-- 使用类来声明的函数/属性
-- 存放在__declaredMethods中
local function _declareInstanceMethod(aClass, name, f)
    aClass.__declaredMethods[name] = f

    -- 如果是要删除定义的函数
    -- 则重新使用父类的同名函数
    if f == nil and aClass.super then
        f = aClass.super.__instanceDict[name]
    end

    -- 还会保存到__instanceDict中
    _propagateInstanceMethod(aClass, name, f)
end

local function _tostring(self)
    return "class " .. self.name
end
local function _call(self, ...)
    local className = self.name;
    warning(className .. "()创建对象已经禁用，请用" .. className .. ":new()创建对象！")
    return self:new(...)
end

-- 创建新的类
local function _createClass(name, super)
    -- 会被当做类实例的metatable
    local dict = {}

    -- 定义了__index方法，并且__index是自己
    -- (dict要被当做类实例的metatable，并且从自己中进行取值)
    dict.__index = dict

    -- 创建新的类
    -- 类的结构
    ---@class MiddleClassInfo
    local aClass = {
        ---@type string
        name = name, -- 类名称
        ---@type MiddleClassInfo
        super = super, -- 父类
        static = {},				-- 存放是，类static变量/方法
        --		* 只能直接通过类来访问
        --		* 找不到时候，还会查找__instanceDict
        --		* 有父类super的时候，还会访问父类static
        __instanceDict = dict, -- 将被当做类实例的metatable
        __declaredMethods = {}, -- 使用类的__newindex, _declareInstanceMethod声明的方法
        -- (使用类声明方法/属性)
        -- * 设置其中的值
        --		* ClassA.staticVar1 = 123
        --		* function ClassA:func() end
        --			* 还会放到本类__instanceDict中
        --			* 如果子类__declaredMethods中没有创建同名函数
        --				* 重复此过程
        --		* Class.staticVar1 = nil，还会从父类super.__instanceDict中取值，并赋值
        --		*
        subclasses = setmetatable({}, { __mode = "k" })
    }

    -- ------------------------
    -- 判断是否有父亲
    -- 设置static的metatable
    -- ------------------------
    if super then
        -- 设置，访问static取不到的时候
        -- 从dict中取，
        -- 然后从super的static中取
        setmetatable(
                aClass.static,
                {
                    __index = function(_, k)
                        local result = rawget(dict, k)
                        if result == nil then
                            return super.static[k]
                        end
                        return result
                    end
                }
        )
    else
        -- 没有父类
        -- 访问static取不到的时候，
        -- 从dict中取
        setmetatable(
                aClass.static,
                {
                    __index = function(_, k)
                        return rawget(dict, k)
                    end
                }
        )
    end

    -- ------------------------
    -- 设置类的metatable
    -- ------------------------
    setmetatable(
            aClass,
            {
                __index = aClass.static, --
                __tostring = _tostring, -- 打印"class 类名称"
                __call = _call, -- 调用new方法
                __newindex = _declareInstanceMethod -- 定义的方法，都是实例方法
            }
    )

    return aClass
end

-- aClass:_createClass创建出来的类对象
-- mixin: 一个table
-- 大概是，将mixin中的数据，拷贝到aClass中
-- * 将mixin中的数据，拷贝到aClass中
--		* 排除included, static
-- * 将mixin的static中的数据， 拷贝到aClass的static中
-- * 如果mixin中有included函数，调用
local function _includeMixin(aClass, mixin)
    assert(type(mixin) == "table", "mixin must be a table")

    -- 将mixin中的数据，拷贝到aClass中
    -- 排除掉名称"included", "static"
    for name, method in pairs(mixin) do
        if name ~= "included" and name ~= "static" then
            aClass[name] = method
        end
    end

    -- 将mixin的static中的数据，
    -- 拷贝到aClass的static中
    for name, method in pairs(mixin.static or {}) do
        aClass.static[name] = method
    end

    -- 如果mixin中有,叫"included“函数
    -- 调用此函数
    if type(mixin.included) == "function" then
        mixin:included(aClass)
    end
    return aClass
end

-- _createClass创建新类后，会调用_includeMixin，
-- 将此内容合并到新类中去
-- (也就是，一个类默认有的方法)
local DefaultMixin = {
    -- 下面的是，实例方法？
    __tostring = function(self)
        local className= "table"
        if self.class then
            className = self.class.name
        end
        local meta = getmetatable(self)
        setmetatable(self, nil)
        local str = string.format("instance %s of %s", tostring(self), className)
        setmetatable(self, meta)
        return str
    end,
    -- 初始化方法
    -- 分类类实例后调用
    initialize = function(self, ...)
    end,

    -- 是否是某类实例
    -- 调用.class的isSubclassOf
    isInstanceOf = function(self, aClass)
        return type(aClass) == "table" and type(self) == "table" and
                (self.class == aClass or
                        type(self.class) == "table" and type(self.class.isSubclassOf) == "function" and
                                self.class:isSubclassOf(aClass))
    end,
    -- static中，装的是类方法?
    -- 注意，都是使用:调用
    static = {
        -- 分配类实例
        -- 应该不直接使用，是通过new()来使用
        allocate = function(self)
            assert(type(self) == "table", "Make sure that you are using 'Class:allocate' instead of 'Class.allocate'")
            -- 类实例的结构
            -- 只有1个class变量
            -- __instanceDict：重定向类实例的取值
            --return setmetatable({ class = self }, self.__instanceDict)
            self.__instanceDict.class = self
            return setmetatable({}, self.__instanceDict)
        end,
        -- 创建实例
        -- :调用
        new = function(self, ...)
            assert(type(self) == "table", "Make sure that you are using 'Class:new' instead of 'Class.new'")
            local instance = self:allocate()
            -- 调用初始化函数
            instance:initialize(...)
            return instance
        end,

        ---把已经存在的Table变为一个类。主要用在代码生成中
        placement_new = function(self, existTable, ...)
            assert(type(self) == "table", "Make sure that you are using 'Class:placement_new' instead of 'Class.placement_new'")
            assert(existTable, "Make sure table exist!")
            local instance = setmetatable(existTable, self.__instanceDict)
            return instance
        end,

        -- 创有父亲的新类
        -- middleclass.class()中有调用
        -- self这里是super
        subclass = function(self, name)
            assert(type(self) == "table", "Make sure that you are using 'Class:subclass' instead of 'Class.subclass'")
            assert(type(name) == "string", "You must provide a name(string) for your class")

            -- 创建新的类
            local subclass = _createClass(name, self)

            -- 遍历super的__instanceDict
            for methodName, f in pairs(self.__instanceDict) do
                _propagateInstanceMethod(subclass, methodName, f)
            end

            -- 子类的initialize函数
            --- (会默认的调用父类的initialize)
            subclass.initialize = function(instance, ...)
                return self.initialize(instance, ...)
            end

            -- 将子类，记录在super的subclasses中
            self.subclasses[subclass] = true

            -- 回调父类，有子类被创建
            self:subclassed(subclass)

            return subclass
        end,
        -- 有创建新的子类，继承了自己回调
        -- (回调父类中)
        subclassed = function(self, other)
        end,
        isSubclassOf = function(self, other)
            return type(other) == "table" and type(self.super) == "table" and
                    (self.super == other or self.super:isSubclassOf(other))
        end,
        include = function(self, ...)
            assert(type(self) == "table", "Make sure you that you are using 'Class:include' instead of 'Class.include'")
            for _, mixin in ipairs({ ... }) do
                _includeMixin(self, mixin)
            end
            return self
        end
    }
}

-- name：字符串类型，新的类的名称
function middleclass.class(name, super)
    assert(type(name) == "string", "A name (string) is needed for the new class")
    return super and super:subclass(name) or _includeMixin(_createClass(name), DefaultMixin)
end

setmetatable(
        middleclass,
        { __call = function(_, ...)
            return middleclass.class(...)
        end }
)

---@class MiddleClass
---@field class MiddleClassInfo
local MiddleClass = {}
function MiddleClass:new(...)
end
function MiddleClass:placement_new(...)
end
function MiddleClass:release()
end
MiddleClass.static = {}

return middleclass
