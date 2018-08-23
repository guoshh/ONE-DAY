using Sap.Data.Hana;
using System;
using System.Data;
using System.Data.Odbc;

namespace TencentAPI.Utils
{
    public class HanaHelper
    {
        public DataTable GetTable(string TableName,string TableColumns)
        {
            String sql =string.Format("SELECT {0} FROM  SAPPRD.{1}", TableColumns, TableName);
            //sql = string.Format("SELECT top 19 * FROM  SAPPRD.VBAK", TableColumns, TableName);
            return getDataTableBySql(sql);
        }
        public DataTable getDataTableBySql(String sql)
        {
            try
            {
                //ODBCNAME
                DataSet ds = new DataSet();
                OdbcCommand command = new OdbcCommand(sql);  //command  对象
                String connstring = "dsn=HANAODBC32;uid=SHENGHAN;pwd=Force12345";  //ODBC连接字符串
                using (OdbcConnection conn = new OdbcConnection(connstring))  //创建connection连接对象
                {
                    command.Connection = conn;
                    conn.Open();  //打开链接
                    OdbcDataAdapter adapter = new OdbcDataAdapter(command);  //实例化dataadapter
                    DataTable employeeTable = new DataTable("employees");
                    adapter.Fill(employeeTable);  //填充查询结果
                    conn.Close();
                    return employeeTable;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}