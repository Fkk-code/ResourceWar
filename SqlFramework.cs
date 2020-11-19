using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Json;
using LitJson;

public class SqlFramework
{
    #region 单例 Singleton
    private static SqlFramework instance;
    public static SqlFramework Instance {
        get {
            if(instance == null) {
                instance = new SqlFramework();
            }
            return instance;
        }
    }
    private SqlFramework() {
    }
    #endregion

    public static string DB_PATH {
        get {
#if UNITY_EDITOR
            return $"{Application.streamingAssetsPath}/MyDB.sqlite";
#endif
        }
    }
    public enum Table {
        USER_INFO
    }
    private static string GetTableName(Table table) {
        string res = "";
        switch(table) {
            case Table.USER_INFO:
                res = "UserInfo";
                break;
            default:
                break;
        }
        return res;
    }

    /// <summary>
    /// 只执行语句，无返回值
    /// </summary>
    /// <param name="sqlString">操作语句</param>
    public static void Execute(string sqlString) {
        using(SqliteConnection con = new SqliteConnection(DB_PATH)) {
            con.Open();
            using(SqliteCommand com = con.CreateCommand()) {
                com.CommandText = sqlString;
                com.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// 执行返回1行1列结果
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns></returns>
    public static object ExecuteForSingle(string sqlString) {
        using(SqliteConnection con = new SqliteConnection(DB_PATH)) {
            con.Open();
            using(SqliteCommand com = con.CreateCommand()) {
                com.CommandText = sqlString;
                return com.ExecuteScalar();
            }
        }
    }

    /// <summary>
    /// 执行返回json形式结果
    /// </summary>
    /// <param name="sqlString"></param>
    /// <returns>结果的json字符串</returns>
    public static string ExecuteForAll(string sqlString) {
        string res = "";
        using(SqliteConnection con = new SqliteConnection(DB_PATH)) {
            con.Open();
            using(SqliteCommand com = con.CreateCommand()) {
                com.CommandText = sqlString;
                // 将结果转成json返回
                SqliteDataReader reader = com.ExecuteReader();
                JsonArray arr = new JsonArray();
                while(reader.Read()) {
                    JsonObject obj = new JsonObject();
                    for(int i = 0; i < reader.FieldCount; i++) {
                        // 将每个字段用key-value形式存入
                        obj.Add(reader.GetName(i), reader.GetValue(i).ToString());
                    }
                    arr.Add(obj);
                }
                res = arr.ToString();
            }
        }
        return res;
    }
}
