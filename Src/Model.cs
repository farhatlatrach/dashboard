﻿using System;
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
        public string Country { get; set; }

        public double  Open { get; set; }
        public double PreviousClose { get; set; }
        public double Last { get; set; }
        public string Currency { get; set; }
        public long ID { get; set; }
        
    }
    public class Future : Security
    {
        public double ContractSize { get; set; }
        public bool isFuture = true;
    }
    public class Equity : Security
    {
        public bool isFuture = false;
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
    public class Trade
    {
        public double UnitPrice { get; set; }

        // - sign for sell / + sign for Buy
        public double Quantity { get; set; }
        public string PortfolioName { get; set; }
        public string SecurityName { get; set; }
        public string TraderName { get; set; } = "Mahmoud Elarbi";
    }
    public class Portfolio
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
    public sealed class Model
    {
        public Dictionary<string, Portfolio> Portfolios { get; set; } = new Dictionary<string, Portfolio>();

        public Dictionary<long, Security> Securities { get; set; } = new Dictionary<long, Security>();

        private Model()
        {
            LoadModel();

        }
        static Model()
        {
            

        }

        public List<String> getSecurities()
        {
            // for each 
            List<string> myTickers = new List<string>();

            foreach (var ptflIter in Portfolios)
            {
                foreach (var posIter in ptflIter.Value.Positions) {
                    myTickers.Add(posIter.Value.Underlying.Name);
                }
            }

            return myTickers;
        }
        

        private static readonly Model instance = new Model();
        public static Model Instance
        {
            get
            {
                return instance;
            }
        }

        private void LoadModel()
        {
            Portfolios.Clear();

            // testings..

            Portfolio ptf0 = new Portfolio() { Name = "European0" };
            Portfolio ptf1 = new Portfolio() { Name = "European1" };
            Portfolio ptf2 = new Portfolio() { Name = "European2" };
            Portfolio ptf3 = new Portfolio() { Name = "European3" };

            Position pos1 = new Position()
            {
                Underlying = new Security()
                {
                    Name = "HSBC LN EQUITY",
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
                    Name = "VOD LN EQUITY",
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
                    Name = "BNP FP EQUITY",
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

            Portfolios.Add(ptf0.Name, ptf0);
            Portfolios.Add(ptf1.Name, ptf1);
            Portfolios.Add(ptf2.Name, ptf2);
            Portfolios.Add(ptf3.Name, ptf3);
            
          
        }
    }
}
