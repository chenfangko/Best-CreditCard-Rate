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
        CreditCardEntities dbContext = new CreditCardEntities();
        ClsUtility clsUtility = new ClsUtility();

        List<BestCard> SelectedCardsDetails;
        List<BestCard> BestCardsDetails;

        List<CardModel> allCards;
        List<ShopModel> allShops;
        List<DiscountModel> allDiscounts;

        List<string> selectedCards = new List<string>();
        string selectedShop;
        string selectedType;

        public Form1(ClsMember loggedInMember)
        {
            if(loggedInMember.Identity == "User")
            {
                btnUpdateInfo.Enabled = false;
                tabPage3.Hide();
                tabPage4.Hide();
            }
            else
            {
                btnUpdateInfo.Enabled = true;
                tabPage3.Show();
                tabPage4.Show();
            }
            InitializeComponent();
            allCards = clsUtility.GetAllCards();
            allShops = clsUtility.GetAllShops();
            allDiscounts = clsUtility.GetAllDiscounts();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            comboBank.Text = "";
            comboCard.Text = "";
            btnUpdateInfo.Enabled = false;

            labShowCard2.Text = "";
            labShowShop2.Text = "";
            txtShowRate2.Text = "";
            txtShowRemark2.Text = "";
            txtShowRate2.BackColor = Color.White;
            txtShowRemark2.BackColor = Color.White;
            btnUpdate.BackColor = Color.Transparent;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            checkedListBoxBank.Items.Clear();
            dataGridView2.DataSource = allDiscounts;
            btnUpdate.Enabled = false;

            foreach (var card in allCards)
            {
                checkedListBoxBank.Items.Add(card.Name);
            }
        }

        //==第一分頁＝＝＝＝＝＝＝＝＝＝＝＝＝

        //選擇銀行
        private void comboBank_Click(object sender, EventArgs e)
        {
            string[] allBanks;
            allBanks = allCards
                .Select(c => c.Bank)
                .ToArray();
            comboBank.Items.Clear();
            comboBank.Items.AddRange(allBanks); 
        }
        private void comboBank_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboCard.Items.Clear();
        }

        //選擇卡別
        private void comboCard_Click(object sender, EventArgs e)
        {
            string[] cards;
            if (comboBank.Text == "")
            { 
                cards = allCards
                .Select(c => c.Name)
                .ToArray();
                comboCard.Items.Clear();
                comboCard.Items.AddRange(cards);
            }
            else
            {
                cards = allCards
                .Where(c => c.Bank == comboBank.Text)
                .Select(c => c.Name)
                .ToArray();
                comboCard.Items.Clear();
                comboCard.Items.AddRange(cards);
            }



        }

        private void comboCard_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                btnUpdateInfo.Enabled = true;
                btnOpen.Enabled = true;

                string cardName = comboCard.Text;
                //load card general info
                string info = allCards
                   .Where(c => c.Name == cardName)
                   .Select(c => c.GeneralInfo)
                   .Single();
                txtShowInfo.Text = info;

                //load shop detail on datagridview
                List<ShopDetailModel> shopDetails = allDiscounts
                   .Where(d => d.Name == cardName)
                   .Select(d => new ShopDetailModel(d.Shop, d.Rate, d.Remark))
                   .ToList();
                dataGridView3.DataSource = shopDetails;

                //load picture
                if (comboCard.Text == "")
                {

                }
                else
                {
                    byte[] cardPhoto = allCards
                    .Where(c => c.Name == comboCard.Text)
                    .Select(c => c.CardPicture)
                    .Single();
                    pictureBox1.Image = ByteToImage(cardPhoto);
                }
            }
            catch { }
            
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            btnUploadPic.Enabled = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
            }
        }

        private void btnUploadPic_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                clsUtility.savePhoto(comboCard.Text, ms);
                MessageBox.Show("上傳成功");
            }
            catch { MessageBox.Show("上傳失敗"); }
            

        }

        private void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            string newInfo = txtShowInfo.Text;
            string nowCard = comboCard.Text;
            bool updateStatus = clsUtility.updateInfo(nowCard, newInfo);
            if (updateStatus == true)
            {
                MessageBox.Show("更新成功");
            }
            else
            {
                MessageBox.Show("更新失敗");
            }
        }

        
    }
}
