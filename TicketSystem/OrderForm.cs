using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;  //增加命名空間

namespace TicketSystem
{
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
        }
        private DataGridViewRow Row;
        //委託+事件:可以將方法做為參數進行傳遞
        //定義一個委託事件,未做實例化, 為了要使用MainForm的Search()方法
        //其委託事件的實例化寫在MainForm的第69列
        public event Action updateTicket; 
        public OrderForm(DataGridViewRow row)
        {
            InitializeComponent();
            this.Row = row;   //擴大作用域範圍
        }
        /// <summary>
        /// 窗體加載事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderForm_Load(object sender, EventArgs e)
        {
            this.lblTrainNo.Text = Row.Cells["TrainNo"].Value.ToString();
            this.lblRunTime.Text = Convert.ToDateTime(Row.Cells["RunTime"].Value).ToString("yyyy/MM/dd HH:mm");
            this.lblStartStation.Text = Row.Cells["Startstation"].Value.ToString();
            this.lblEndStation.Text = Row.Cells["EndStation"].Value.ToString();
        }

        private void btnBuyTicket_Click(object sender, EventArgs e)
        {
            string name = this.txtName.Text.Trim();
            string phone = this.txtPhone.Text.Trim();
            string id = this.txtID.Text.Trim();
            string trainId = Row.Cells["TrainId"].Value.ToString();
            //編寫插入資料表的sql
            string sql =@"INSERT INTO Record(Name,CardID,TelePhone,TrainId)
                               VALUES(@Name, @CardID, @TelePhone, @TrainId)";
            //定義傳入參數
            SqlParameter[] paras =
            {
                new SqlParameter("@Name",name),
                new SqlParameter("@CardID", id),
                new SqlParameter("@TelePhone",phone),
                new SqlParameter("@TrainId",trainId)
            };
            //執行sql和傳入參數, 共傳回成功與否, 1為成功, -1為失敗
            int result = DBHelper.ExecuteNonQuery(sql, paras);
            if (result > 0)
            {
                MessageBox.Show("購票成功");
                //根據車票數量將ticket資料表的車票張數-1,要寫事件
                //教師建議寫觸發器trigger處理:為了解決數據的完整性
                //此if判斷是為了MainForm的第69列如果沒有寫時, 才不會報錯
                if (updateTicket != null)
                    updateTicket();
                this.Close();
            }
            else
            {
                MessageBox.Show("購票失敗");
            }
        }
    }
}
