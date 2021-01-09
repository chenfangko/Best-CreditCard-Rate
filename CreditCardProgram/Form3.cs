using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreditCardProgram
{
    public partial class Form1 : Form
    {

        //======================第三分頁=================================

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtShowRate2.Enabled = true;
            txtShowRemark2.Enabled = true;

            txtShowRate2.BackColor = Color.PeachPuff;
            txtShowRemark2.BackColor = Color.PeachPuff;
            btnUpdate.BackColor = Color.SandyBrown;
            btnUpdate.Enabled = true;

            int rowIndex = e.RowIndex;
            labShowCard2.Text = allDiscounts[rowIndex].Name;
            labShowShop2.Text = allDiscounts[rowIndex].Shop;
            txtShowRate2.Text = allDiscounts[rowIndex].Rate.ToString();
            txtShowRemark2.Text = allDiscounts[rowIndex].Remark;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string card = labShowCard2.Text;
            string shop = labShowShop2.Text;
            double newRate = double.Parse(txtShowRate2.Text);
            string newRemark = txtShowRemark2.Text;

            clsUtility.updateInfo(card, shop, newRate, newRemark);
            allDiscounts = clsUtility.GetAllDiscounts();
            dataGridView2.DataSource = allDiscounts;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtShowRate2.Clear();
            txtShowRemark2.Clear();
            labShowCard2.Text = "";
            labShowShop2.Text = "";
            txtShowRate2.BackColor = Color.White;
            txtShowRemark2.BackColor = Color.White;
            btnUpdate.BackColor = Color.Transparent;
            txtShowRate2.Enabled = false;
            txtShowRemark2.Enabled = false;
            btnUpdate.Enabled = false;
        }




        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

    }
}
