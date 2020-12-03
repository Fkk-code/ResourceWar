--[[
    道具类
]]
Prop = 
{
    --道具编号
    id = 1,
    --道具名称
    name = "药瓶",
    --道具类型
    proptype = "消耗品",
    --道具等级
    quality =1,
    --道具描述
    description = "恢复20点生命值",
    --道具价值
    price = 100,
    --道具图标路径
    sprite = "药瓶"
}
--[[
    构造函数
]]
function Prop : new (name,proptype,quality,description,price,sprite)
    o = {}
    --self={}
    setmetatable(o,self)
    self.__index = self
    self.name = name
    self.proptype = proptype
    self.quality = quality
    self.description = description
    self.price = price
    self.sprite = sprite
    return o
end
--[[
    成员方法
]]
--设置道具品质
function Prop : setquality(newQuality)
    self.quality = newQuality
end