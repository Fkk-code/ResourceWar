require("PropItem")

Equip ={
    equiptype = "Chest"
}
setmetatable(Equip,Prop)

function Equip : new (name,proptype,quality,description,price,sprite,equiptype)
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
    self.equiptype = equiptype
    return o
end