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

        //================第二分頁=================================

        //勾選卡片 塞進List中
        private void checkedListBoxBank_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedCards.Clear();
            selectedCards = checkedListBoxBank.CheckedItems.OfType<string>().ToList();
        }


        //點Type下拉選單(load TypeList)
        private void comboType_Click(object sender, EventArgs e)
        {
            comboType.Items.Clear();
            comboShop.Items.Clear();

            List<string> allTypes = clsUtility.GetAllShops().Select(t => t.Type).Distinct().ToList();
            comboType.Items.AddRange(allTypes.ToArray());
        }

        //選Type(load ShopList)
        private void comboType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            dataGridView1.DataSource = "";
            selectedType = comboType.Text;

            foreach (var shop in allShops)
            {
                if (shop.Type == selectedType)
                {
                    comboShop.Items.Add(shop.Name);
                }
            }
        }

        //選Shop
        private void comboShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedShop = comboShop.Text;
            btnSearch.Enabled = true;
        }

        private void comboShop_Click(object sender, EventArgs e)
        {
            if (comboType.Text == "")
            {
                foreach (var shop in allShops)
                {
                    comboShop.Items.Add(shop.Name);
                }
            }
        }

        //按Search: load BestCard & BestRate & BestCard Detail
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                labShowCard.Text = "";
                labShowRate.Text = "";
                dataGridView1.DataSource = "";

                btnSeeSelected.Enabled = true;
                btnSeeAll.Enabled = true;
                btnSeeSelected.BackColor = Color.SandyBrown;
                btnSeeAll.BackColor = Color.Transparent;

                //=====================================

                BestCardsDetails = clsUtility.FindBestCards(selectedCards, selectedShop);
                foreach (BestCard card in BestCardsDetails)
                {
                    labShowCard.Text = $"{card.Name.ToString()}";
                }
                double bestRate = clsUtility.getBestRate();
                labShowRate.Text = $"{bestRate * 100}%";

                SelectedCardsDetails = clsUtility.FindSelectedCards(selectedCards, selectedShop);
                dataGridView1.DataSource = SelectedCardsDetails;
            }
            catch(Exception ex)
            {
                MessageBox.Show("請選擇卡片");
            }
            
        }

        //Switch btn to Selected -> Show on Datagridview
        private void btnSeeSelected_Click(object sender, EventArgs e)
        {
            btnSeeAll.BackColor = Color.Transparent;
            btnSeeSelected.BackColor = Color.SandyBrown;
            dataGridView1.DataSource = SelectedCardsDetails.ToList();
        }

        //Switch btn to All -> Show on Datagridview
        private void btnSeeAll_Click(object sender, EventArgs e)
        {
            btnSeeSelected.BackColor = Color.Transparent;
            btnSeeAll.BackColor = Color.SandyBrown;
            switchToAll();
        }

        void switchToAll()
        {
            var q = from d in dbContext.Discounts
                    where d.Shop.name == selectedShop
                    orderby d.discount1 descending
                    select new { Card = d.Card.name, Bank = d.Card.bank, Discount = d.discount1, d.remark };
            dataGridView1.DataSource = q.ToList();
        }


    }
}
