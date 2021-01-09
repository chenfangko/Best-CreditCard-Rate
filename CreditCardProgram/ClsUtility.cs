using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardProgram
{
    public class ClsUtility
    {
        CreditCardEntities dbContext = new CreditCardEntities();
        List<string> cardList = new List<string>();
        double maxRate;


        //Best Cards
        public List<BestCard> FindBestCards(List<string> cards, string shop)
        {
            var q = from d in dbContext.Discounts
                    join s in dbContext.Shops on d.shopId equals s.id
                    join c in dbContext.Cards on d.cardId equals c.id
                    where cards.Contains(c.name) && s.name == shop
                    select new { c.name, c.bank, discount = d.discount1, d.remark };

            List<BestCard> bestCards = q.ToList().Select(qcard => new BestCard(
                qcard.name,
                qcard.bank,
                qcard.discount,
                qcard.remark
            )).ToList();

            maxRate = bestCards.Max(selected => selected.Discount);
            return bestCards.Where(selected => selected.Discount == maxRate).ToList();
        }
         //Best Rate
        public double getBestRate()
        {
            return maxRate;
        }

        //Selected Cards
        public List<BestCard> FindSelectedCards(List<string> cards, string shop)
        {
            var q = from d in dbContext.Discounts
                    join s in dbContext.Shops on d.shopId equals s.id
                    join c in dbContext.Cards on d.cardId equals c.id
                    where cards.Contains(c.name) && s.name == shop
                    select new { c.name, c.bank, discount = d.discount1, d.remark };

            List<BestCard> selectedCards = q.ToList().Select(qcard => new BestCard(
                qcard.name,
                qcard.bank,
                qcard.discount,
                qcard.remark
            )).ToList();

            return selectedCards;
        }

        //Get All Cards
        public List<CardModel> GetAllCards()
        {
            var q = from c in dbContext.Cards
                    select new { c.name, c.bank, c.picture, c.generalInfo };
            List<CardModel> allCards = q.ToList().Select(qcard => new CardModel(
               qcard.name,
               qcard.bank,
               qcard.picture,
               qcard.generalInfo
           )).ToList();
            return allCards;

        }

        //Get All Shop
        public List<ShopModel> GetAllShops()
        {
            var q = from s in dbContext.Shops
                    select new { s.type, s.name };

            List<ShopModel> allShops = q.ToList().Select(qshop => new ShopModel(
                qshop.type,
                qshop.name
            )).ToList();
            return allShops;
        }


        //Get All Discounts
        public List<DiscountModel> GetAllDiscounts()
        {
            var q = from d in dbContext.Discounts
                    join s in dbContext.Shops on d.shopId equals s.id
                    join c in dbContext.Cards on d.cardId equals c.id

                    select new
                    {
                        c.name,
                        c.bank,
                        s.type,
                        Shop = s.name,
                        d.discount1,
                        d.remark,
                        d.id
                    };

            List<DiscountModel> allDiscounts = q.ToList().Select(x => new DiscountModel(
                x.name,
                x.bank,
                x.type,
                x.Shop,
                x.discount1,
                x.remark,
                x.id
            )).ToList();
            return allDiscounts;
        }


        //UPdate
        public bool updateInfo(string card, string newinfo)
        {
            try
            {
                var q = from c in dbContext.Cards
                        where (c.name == card)
                        select c;
                foreach (var c in q)
                {
                    c.generalInfo = newinfo;
                }
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool updateInfo(string card, string shop, double newrate, string newremark)
        {
            try
            {
                var q = from d in dbContext.Discounts
                        where (d.Card.name == card) && (d.Shop.name == shop)
                        select d;
                foreach (var d in q)
                {
                    d.discount1 = newrate;
                    d.remark = newremark;
                }
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public void savePhoto(string card, MemoryStream ms)
        {
            var q = from c in dbContext.Cards
                    where c.name == card
                    select c;
            
            byte[] bytes = ms.GetBuffer();
            foreach (var c in q)
            {
                c.picture = bytes;
            }
            dbContext.SaveChanges();
        }

        //Open File
        public void saveFile()
        {
            
        }

        

    }


    //Structures

    public struct CardModel
    {
        public string Name { get; set; }
        public string Bank { get; set; }
        public byte[] CardPicture { get; set; }
        public string GeneralInfo { get; set; }

        public CardModel(string name, string bank, byte[] pic, string info)
        {
            this.Name = name;
            this.Bank = bank;
            this.CardPicture = pic;
            this.GeneralInfo = info;
        }
    }


    public struct ShopModel
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public ShopModel(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }

    public struct ShopDetailModel
    {
        public string Shop { get; set; }
        public double Rate { get; set; }
        public string Remark { get; set; }

        public ShopDetailModel(string name, double rate, string remark)
        {
            this.Shop = name;
            this.Rate = rate;
            this.Remark = remark;
        } 
    }


    public struct DiscountModel
    {
        public string Name { get; set; }
        public string Bank { get; set; }
        public string ShopType { get; set; }
        public string Shop { get; set; }
        public double Rate { get; set; }
        public string Remark { get; set; }
        public int DiscountId { get; set; }

        public DiscountModel(string name, string bank, string type, string shop, double rate, string remark, int id)
        {
            this.Name = name;
            this.Bank = bank;
            this.ShopType = type;
            this.Shop = shop;
            this.Rate = rate;
            this.Remark = remark;
            this.DiscountId = id;
        }

    }

    public struct BestCard
    {
        public string Name { get; set; }
        public string Bank { get; set; }
        public double Discount { get; set; }
        public string Remark { get; set; }

        public BestCard(string name, string bank, double discount, string remark)
        {
            this.Name = name;
            this.Bank = bank;
            this.Discount = discount;
            this.Remark = remark;
        }
    };

}
