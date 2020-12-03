require("PropItem")
require("EquipItem")
--道具大全
PropTable=
{
    --[[
        消耗品1-10
    ]]
    --[1] = Prop:new ("回复剂","Consumable",1,"回复25生命值",10,"Sprites/Hi-Potion"),
    --[2] = Prop:new ("大回复剂","Consumable",1,"回复50生命值",30,"Sprites/Potion"),
    --[3] = Prop:new ("乙醚","Consumable",1,"回复75魔力值",50,"Sprites/Ether"),
    --[4] = Prop:new ("少女之吻","Consumable",1,"回复75魔力值，75生命值",100,"Sprites/Maiden_Kiss"),
    --[[
        装备10-100
    ]]
    --[10] = Prop:new ("麻布长袍","Equipment",1,"增加15物理防御，22魔法防御",50,"Sprites/Hempen_Robe","Chest"),
    --[11] = Prop:new ("丝袍","Equipment",1,"增加15物理防御，28魔法防御",100,"Sprites/Silken_Robe","Chest"),
    --[12] = Prop:new ("魔道士长袍","Equipment",1,"增加15物理防御，35魔法防御",200,"Sprites/Magus_Robe","Chest"),
    --[13] = Prop:new ("变色龙袍","Equipment",1,"增加22物理防御，15魔法防御",50,"Sprites/Magus_Robe","Chest"),
    --[14] = Prop:new ("火群之衣","Equipment",1,"增加28物理防御，15魔法防御",100,"Sprites/Blaze_Robe","Chest"),
    --[15] = Prop:new ("雪花之衣","Equipment",1,"增加35物理防御，15魔法防御",200,"Sprites/Flurry_Robe","Chest"),
    --[[
        材料100-110
    ]]
    [101] = Prop:new ("兔毛","Collection",1,"可以出售换取金钱",3,"Sprites/Collection/兔毛"),
    [102] = Prop:new ("松子果","Collection",1,"可以出售换取金钱",6,"Sprites/Collection/松子果"),
    [103] = Prop:new ("狗牙","Collection",1,"可以出售换取金钱",10,"Sprites/Collection/小狗牙"),
    [104] = Prop:new ("蜗牛壳","Collection",1,"可以出售换取金钱",20,"Sprites/Collection/蜗牛壳"),
    [105] = Prop:new ("猴皮","Collection",1,"可以出售换取金钱",20,"Sprites/Collection/猴皮"),
    [106] = Prop:new ("泥块","Collection",1,"可以出售换取金钱",20,"Sprites/Collection/泥块")
}
--道具编号
PropId=
{
    ["回复剂"]=1
}
--通过 道具名称 查询 道具编号
function GetIdByPropName(name)
    return PropId[name]
end
--通过 道具编号 查询 道具信息
function GetPropByPropId(id)
    return PropTable[id]
end
--实例化测试
--newProp = Prop:new (1,"血瓶","消耗品",1,"恢复20点生命值",100,"血瓶")
--结果输出测试
--  获取道具编号
--print(GetIdByPropName("血瓶"))
--  调用道具的成员方法
print(GetPropByPropId(10).price)

