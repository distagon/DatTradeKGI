using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeBot
{
    public partial class StockEditor : Form
    {
        public StockEditor(DataTable table)
        {
            InitializeComponent();

            dgv_StockEditor.DataSource = table;

        }

        private void StockEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
