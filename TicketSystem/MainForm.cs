using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //引用命名空間

namespace TicketSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        public void Search()
        {
            //datagridview listview
            //在datagridview上按右鍵選編輯資料行, 新增編輯每一個欄位的中文名後,
            //更改DataPropertyName的名稱與Name相同, 才會真正和查詢的資料表名稱對應
            //更改datagridview的屬性AllowUserToAddRows改為Flase,查詢結果就不會多一行
            //取得用戶輸入的條件
            string start = this.cboStartStation.Text;
            string end = this.cboEndStation.Text;
            string date = this.dtpDate.Text;
            string sql = @"select TrainId,TrainNo,RunTime,StartStation,EndStation,Price,TicketNumber 
                              from Ticket where  convert(varchar,RunTime,111) = convert(varchar,@date,111)";

            ///*下面這段可以用另一種方式取代,如下式
            //if (start != "")
            //{
            //    sql = sql + " and StartStation = @StartStation";
            //}
            //if (end != "")
            //{
            //    sql = sql + " and EndStation = @EndStation";
            //}
            //*/
            //如下sql寫法取代上面if做法  條件?true的返回值:false的返回值
            sql += start !="" ? " and StartStation = @StartStation " : "";
            sql += end != "" ? " and EndStation = @EndStation " : "";
            SqlParameter[] paras =
            {
                new SqlParameter("@date",Convert.ToDateTime(date)),
                new SqlParameter("@StartStation",start),
                new SqlParameter("@EndStation",end)
            };
            this.dgvTickets.DataSource = DBHelper.GetDataTable(sql, paras);
        }

        //購買的按鍵是使用工具列的contextMenuStrip加入到MainForm畫面中, 
        //MainForm的屬性contextMenuStrip要選此加入工具列的名稱contextMenuStrip1
        //點了"購買"會開啟OrderForm
        private void tsmiBuy_Click(object sender, EventArgs e)
        {
            DataGridViewRow row =  this.dgvTickets.SelectedRows[0]; //選中第一個列
            //string trainId = row.Cells["TrainId"].Value.ToString();  //取得列上面的值;此教學直接以列來查
            OrderForm of = new OrderForm(row);
            of.updateTicket += UpdateTicket;    //為updateTicket委託事件進行實例化,降低視窗之間的耦合數, 此列可寫可不寫
            of.Show();
        }

        private void UpdateTicket()     //此方法名稱是配合第69列實例化名稱
        {
            Search();       //為了在購票後重新載入一次主畫面的grid
        }
        /// <summary>
        /// 鼠標點擊事件, 分左鍵和右鍵, 此例使用右鍵, 選中grid某一筆按滑鼠右鍵時會出現"購買"的選項
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTickets_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)    //有選中會>0, 沒選中會是-1
                {
                    //取消原來選中的數據
                    this.dgvTickets.ClearSelection();   //清除選中
                    this.dgvTickets.Rows[e.RowIndex].Selected = true; //手動選中
                }
            }
        }
    }
}
