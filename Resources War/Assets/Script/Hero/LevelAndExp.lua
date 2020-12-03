--[[
    升级上限10级
]]
--佣兵升级所需经验
NeedExp={
    400,--2
    880,--3
    1440,--4
    2080,--5
    2800,--6
    3600,--7
    4480,--8
    5440,--9
    6480--10
    }
--匿名方法
--获取升级所需经验
GetExp = function (level)
    --如果为空值则不能升级
    if(not NeedExp[level]) then
        return 99999999
    end
    return NeedExp[level]
end

--升级
UpLevel = function (level,exp)
    --经验为空则直接跳出
    if not exp then
        return level
    end
    --经验满足升级
    while exp >= GetExp(level) do
        --扣除经验
        exp = exp - GetExp(level)
        --提升等级
        level=level+1
        --等级到达上限则停止
        if not NeedExp[level] then
            break
        end
    end
    --返回升级完后的等级与剩余经验
    return level,exp
end

--方法测试
--print(GetExp(1))
--print(UpLevel(1,100000000))

--备用升级经验表
NeedExp222={
    400,--2
    880,--3
    1440,--4
    2080,--5
    2800,--6
    3600,--7
    4480,--8
    5440,--9
    6480,--10
    7600,--11
    8800,--12
    10080,--13
    11440,--14
    12880,--15
    14400,--16
    16000,--17
    17680,--18
    19440,--19
    21280,--20
    23200,--21
    25200,--22
    27280,--23
    29440,--24
    31680,--25
    34000,--26
    38880,--27
    41440,--28
    44300,--29
    47400--30
    }