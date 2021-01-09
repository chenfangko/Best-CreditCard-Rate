using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProgram
{
    class ClsMemberFactory
    {
        CreditCardEntities dbContext = new CreditCardEntities();

        internal bool check(string email, string password, string identity)
        {
            if ((email == "") || (password == "") || (identity == ""))
            {
                return false;
            }
            return true;
        }

        internal ClsMember login(string email, string password, string identity)
        {
            var q = from m in dbContext.Members
                    where m.email == email && m.password == password
                    select new { m.email, m.password, m.identity };

            if (q.Count() == 1)
            {
                return new ClsMember(q.Single().email, q.Single().password, q.Single().identity);
            }

            return null;
        }

        internal bool create(string email, string password, string identity)
        {
            var q = from m in dbContext.Members
                    where m.email == email
                    select new { m.email, m.password, m.identity };

            if (q.Count() != 0)
            {
                return false;
            }
            var member = new Member { email = email, password = password, identity = identity };
            dbContext.Members.Add(member);
            dbContext.SaveChanges();
            return true;

        }


        



        //ASP.NET MVC
        //public void executeSql(string sql)
        //{
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = @"Data Source=.;Initial Catalog=dbMembership;Integrated Security=True";
        //    con.Open();

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    cmd.CommandText = sql;
        //    cmd.ExecuteNonQuery();

        //    con.Close();
        //}
    }
}
