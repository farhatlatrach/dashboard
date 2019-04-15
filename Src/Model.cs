using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace Dashboard
{
     public class Security
    {
        public string Name{ get; set; }
        public string Sector { get; set; }
        public string Ticker { get; set; }
        public double  Close { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }
    public class Position 
    {
        public Security Underlying { get; set; }
        public double SellQuantity { get; set; }
        public double SellAveragePrice { get; set; }
        public double BuyQuantity { get; set; }
        public double BuyAveragePrice { get; set; }
        public string PortfolioName { get; set; }
    }
    class Portfolio
    {
        private Dictionary<string, Position> positions_ = new Dictionary<string, Position>();
        public string Name { get; set; }
        public Dictionary<string, Position>  Positions
        {
            get
            {
                return positions_;
            }
            set
            {
             positions_ = value;   
            }
        }
    }
    class Model
    {
        private Dictionary<string, Portfolio> portfolios_ = new Dictionary<string, Portfolio>();
        public Dictionary<string, Portfolio> Portfolios
        {
            get
            {
                return portfolios_;
            }
            set
            {
                portfolios_ = value;
            }
        }
        
        public void LoadModel()
        {
            portfolios_.Clear();

            // testings..

            Portfolio ptf0 = new Portfolio() { Name = "European0" };
            Portfolio ptf1 = new Portfolio() { Name = "European1" };
            Portfolio ptf2 = new Portfolio() { Name = "European2" };
            Portfolio ptf3 = new Portfolio() { Name = "European3" };

            Position pos1 = new Position()
            {
                Underlying = new Security()
                {
                    Name = "HSBC",
                    Ask = 4.4,
                    Bid = 3.9,
                    Sector = "Europe"
                },
                BuyAveragePrice = 3.9,
                BuyQuantity = 45677,
                SellAveragePrice = 3.8,
                SellQuantity = 290008
            };
            Position pos2 = new Position()
            {
                Underlying = new Security()
                {
                    Name = "HSBC2",
                    Ask = 40.4,
                    Bid = 63.9,
                    Sector = "Europe"
                },
                BuyAveragePrice = 39.9,
                BuyQuantity = 90077,
                SellAveragePrice = 37.8,
                SellQuantity = 94448
            };
            Position pos3 = new Position()
            {
                Underlying = new Security()
                {
                    Name = "BNP",
                    Ask = 24.4,
                    Bid = 23.9,
                    Sector = "Europe"
                },
                
                BuyAveragePrice = 23.9,
                BuyQuantity = 80077,
                SellAveragePrice = 23.8,
                SellQuantity = 690008
            };
        

            ptf0.Positions.Add(pos1.Underlying.Name,pos1);
            ptf0.Positions.Add(pos2.Underlying.Name, pos2);
            ptf0.Positions.Add(pos3.Underlying.Name, pos3);
            ptf2.Positions.Add(pos2.Underlying.Name, pos2);
            ptf2.Positions.Add(pos3.Underlying.Name, pos3);
            ptf1.Positions.Add(pos1.Underlying.Name, pos1);
            ptf1.Positions.Add(pos3.Underlying.Name, pos3);
            ptf3.Positions.Add(pos1.Underlying.Name, pos1);
            ptf3.Positions.Add(pos2.Underlying.Name, pos2);
            ptf3.Positions.Add(pos3.Underlying.Name, pos3);

            portfolios_.Add(ptf0.Name, ptf0);
            portfolios_.Add(ptf1.Name, ptf1);
            portfolios_.Add(ptf2.Name, ptf2);
            portfolios_.Add(ptf3.Name, ptf3);
            
          
        }
    }
}
