--输入 a=io.read()
--输出 print();
--number转string tostring(number) 转换失败值为nil
--string转number tonumber(str) 转换失败值为nil

--require("fkk")  导入同路径下的fkk.lua文件内的内容 
--setmetatable(table,metatable) 返回table     设置元表 son 继承 parent 内 __index表格（或函数）的数据
--getmetatable(table)           返回metatable 获取元表 

--空表
son={}
--带__index 表格的表
parent={
    --__index是表格
    __index = {
        name = 1234
    }
}
--设置 parent 为 son 的 元表
setmetatable(son,parent)
--测试
print(son.name)
--result 1234

--空表
son={}
--带__index 表格的表
parent={
    --__index是方法
    __index = function()
        print("FKK")
    end
}
--设置 parent 为 son 的 元表
setmetatable(son,parent)
--测试
print(son.name)
--[[result
        FKK --没找到name对应的值，所以先执行__index方法
        nil --然后打印空值
]] 


