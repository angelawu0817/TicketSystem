using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace TicketSystem
{
    class DBHelper
    {
        /**
         * 寫增加、刪除、修改通用的方法
         * 查詢(很多種)
         * 執行添加、刪除、修改的通用方法
         * <param name ="sql">SQL腳本</param>
         * <param name = "paras">參數</param>
         * */
    private static string ConnStr = "server=.;uid=sa;pwd=rebe1214;database=TicketSystemDB";

    //此為新增、刪除、修改用的方法
    public static int ExecuteNonQuery(string sql, params SqlParameter[] paras)      //形參
        {
            //創建數據庫連接對象, 不能把連線字串放外面(上面)是因為怕重覆執行方法時,
            //字串會變成相加, 而產生問題
            int result = -1;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();    //打開數據庫的連結
                                //創建執行腳本的對象
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);     //添加參數
                result = command.ExecuteNonQuery();
                //conn.Close();
            }
            return result;
        }

        //查詢用的方法, 執行返回第一行第一列
        public static object ExecuteScalar(string sql, params SqlParameter[] paras)      //形參
        {
            //創建數據庫連接對象
            object obj = null;
            //使用using超出{}範圍就會會自動被釋放, 不需另外寫conn.Close();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();    //打開數據庫的連結
                                //創建執行腳本的對象
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);     //添加參數
                obj = command.ExecuteScalar();       //執行腳本
                //conn.Close();                                            //關閉連結;很易忘記
            }   //自動釋放資源
            return obj;
        }

        /// <summary>
        /// 查詢用的方法, 執行返回游標方式的結果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] paras)      //形參
        {
            //連接式連接方式, 和數據庫隨時保持連接狀態
            SqlConnection conn = new SqlConnection(ConnStr);
            conn.Open();    //打開數據庫的連結
                            //創建執行腳本的對象
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddRange(paras);     //添加參數
            //使用CommandBehavior.CloseConnection可讓reader在關閉SqlDataReader時就會自動關閉
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);       //執行腳本
            return reader;
        }

        public static DataRow GetDataRow(string sql, params SqlParameter[] paras)
        {
            DataTable dt = null;
            //斷開式連接方式,不需要顯示寫open，不需要close
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);     //添加參數
                //創建數據適配器
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                dt = new DataTable();
                //填充
                adapter.Fill(dt);
            };
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }
        /// <summary>
        /// 執行返回臨時表DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, params SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            //斷開式連接方式,不需要顯示寫open，不需要close
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);     //添加參數
                //創建數據適配器
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                //填充
                adapter.Fill(dt);
            };
            return dt;
        }

        public static DataSet GetDataSet(string sql, params SqlParameter[] paras)
        {
            DataSet ds = new DataSet();
            //斷開式連接方式,不需要顯示寫open，不需要close
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(paras);     //添加參數
                //創建數據適配器
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                //填充
                adapter.Fill(ds);
            };
            return ds;
        }
    }
}
