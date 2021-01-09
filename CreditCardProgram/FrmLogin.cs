using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreditCardProgram
{
    public partial class FrmLogin : Form
    {
        ClsMember loggedInMember = null;
        public FrmLogin()
        {
            InitializeComponent();
            loggedInMember = null;
        }
        ClsMemberFactory memberFactory = new ClsMemberFactory();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(memberFactory.check(txtEmail.Text, txtPassword.Text, comboIdentity.Text) == false)
            {
                MessageBox.Show("欄位不可空白");
                return;
            }
            loggedInMember = memberFactory.login(txtEmail.Text, txtPassword.Text, comboIdentity.Text);
            if (loggedInMember == null)
            {
                MessageBox.Show("登入失敗");
            }
            else
            {
                MessageBox.Show("登入成功");
                Application.Run(new Form1(loggedInMember));
                this.Hide();
            }
        }

        private void btnBuildNew_Click(object sender, EventArgs e)
        {
            if(memberFactory.check(txtEmail.Text, txtPassword.Text, comboIdentity.Text) == false)
            {
                MessageBox.Show("欄位不可空白");
            }
            else if(memberFactory.create(txtEmail.Text, txtPassword.Text, comboIdentity.Text) == false)
            {
                MessageBox.Show("註冊失敗");
            }
            else
            {
                MessageBox.Show("註冊成功");
            }

        }


    }
}
