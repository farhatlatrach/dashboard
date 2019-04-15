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
        public double  Open { get; set; }
        public double PreviousClose { get; set; }
        public double Last { get; set; }
        public string Currency { get; set; }
    }
    public class Future : Security
    {
        public double ContractSize { get; set; }
    }
    public class Forex
    {
        private Forex() { }
        private readonly string ccy1_;
        private readonly string ccy2_;

        public Forex(string ccy1, string ccy2)
        {
            ccy1_ = ccy1;
            ccy2_ = ccy2;
        }
        public string Name() { return ccy1_ + ccy2_; }
        public string Inverse() { return ccy2_ + ccy1_; }
        public double Open { get; set; }
        public double PreviousClose { get; set; }
        public double Last { get; set; }
    }
    public class Position 
    {
        public Security Underlying { get; set; }
        public double SoldQuantity { get; set; }
        public double SoldAveragePrice { get; set; }
        public double BoughtQuantity { get; set; }
        public double BoughtAveragePrice { get; set; }
        public double BeginOfDayQuantity { get; set; }
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
                    PreviousClose = 4.4,
                    Last = 3.9,
                    Sector = "Europe"
                },
                BoughtAveragePrice = 3.9,
                BoughtQuantity = 45677,
                SoldAveragePrice = 3.8,
                SoldQuantity = 290008,
                BeginOfDayQuantity = 3400
            };
            Position pos2 = new Position()
            {
                Underlying = new Security()
                {
                    Name = "HSBC2",
                    PreviousClose = 40.4,
                    Last = 63.9,
                    Sector = "Europe"
                },
                BoughtAveragePrice = 39.9,
                BoughtQuantity = 90077,
                SoldAveragePrice = 37.8,
                SoldQuantity = 94448,
                BeginOfDayQuantity = 3800
            };
            Position pos3 = new Position()
            {
                Underlying = new Security()
                {
                    Name = "BNP",
                    PreviousClose = 24.4,
                    Last = 23.9,
                    Sector = "Europe"
                },
                
                BoughtAveragePrice = 23.9,
                BoughtQuantity = 80077,
                SoldAveragePrice = 23.8,
                SoldQuantity = 690008,
                BeginOfDayQuantity = 2400
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
