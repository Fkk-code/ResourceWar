--[[
    4英雄
    2棋子属性
    7基础属性
]]
--英雄职业枚举
HeroJobEnum={}
--英雄职业基础数值
HeroBaseStateByJobEnum={}
--英雄基础属性枚举
HeroBaseStateEnum={   
    "Hp",
    "Mp",
    "Atk",
    "Def",
    "MAtk",
    "MDef",
    "Speed"}
--添加职业数据
--金刚狼1
HeroJobEnum[1]="Barbarian"
HeroBaseStateByJobEnum["Barbarian"]={
        --棋子属性
        maxDistance =5,--最大移动格子
        maxAttack = 1,--最大攻击范围
        --初始属性
        BaseHp = 20,
        BaseMp = 20,
        BaseAtk = 10,
        BaseDef = 10,
        BaseMAtk = 10,
        BaseMDef = 10,
        BaseSpeed=10,
        --Grow up ==> GU
        --成长属性
        GUHp = 5,
        GUMp = 5,
        GUAtk = 5,
        GUDef = 5,
        GUMAtk = 5,
        GUMDef = 10,
        GUSpeed = 10}
--射手2
HeroJobEnum[2]="Archer"
HeroBaseStateByJobEnum["Archer"]={
    --棋子属性
    maxDistance =5,--最大移动格子
    maxAttack = 5,--最大攻击范围
    --初始属性
    BaseHp = 20,
    BaseMp = 20,
    BaseAtk = 10,
    BaseDef = 10,
    BaseMAtk = 10,
    BaseMDef = 10,
    BaseSpeed=10,
    --Grow up ==> GU
    --成长属性
    GUHp = 5,
    GUMp = 5,
    GUAtk = 5,
    GUDef = 5,
    GUMAtk = 5,
    GUMDef = 10,
    GUSpeed = 10}
--骑士3
HeroJobEnum[3]="Knight"
HeroBaseStateByJobEnum["Knight"]={
        --棋子属性
        maxDistance =5,--最大移动格子
        maxAttack = 1,--最大攻击范围
        --初始属性
        BaseHp = 20,
        BaseMp = 20,
        BaseAtk = 10,
        BaseDef = 10,
        BaseMAtk = 10,
        BaseMDef = 10,
        BaseSpeed=10,
        --Grow up ==> GU
        --成长属性
        GUHp = 5,
        GUMp = 5,
        GUAtk = 5,
        GUDef = 5,
        GUMAtk = 5,
        GUMDef = 10,
        GUSpeed = 10}
--巫师4
HeroJobEnum[4]="Wizard"
HeroBaseStateByJobEnum["Wizard"]={
        --棋子属性
        maxDistance =5,--最大移动格子
        maxAttack = 1,--最大攻击范围
        --初始属性
        BaseHp = 20,
        BaseMp = 20,
        BaseAtk = 10,
        BaseDef = 10,
        BaseMAtk = 10,
        BaseMDef = 10,
        BaseSpeed=10,
        --Grow up ==> GU
        --成长属性
        GUHp = 5,
        GUMp = 5,
        GUAtk = 5,
        GUDef = 5,
        GUMAtk = 5,
        GUMDef = 10,
        GUSpeed = 10}
--[[
    方法
]]
--获取当前状态的值
function GetState(JobNum,Level,StateNum)
    --获得职业
    local Job = HeroJobEnum[JobNum]
    --获得属性词条
    local State = HeroBaseStateEnum[StateNum]
    --获取基础值
    local BaseState = HeroBaseStateByJobEnum[Job]["Base" .. State]
    --获取成长值
    local GUState = HeroBaseStateByJobEnum[Job]["GU" .. State]
    return BaseState + GUState * Level
end
--获取属性的个数
function GetStateNum()
    local count = 0
    for i in  ipairs(HeroBaseStateEnum) do
        count = count + 1
    end
    return count
end
--[[
    方法测试
]]
--print(GetState(1,1,1))
--print(GetState(1,1,2))
--print(GetAllState(1,1))
