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
        private Security() { }
        private string name_;
        public Security(string name) { name_ = name; }
        public string Name
        {
            get
            {
                return name_;
            }
        }
        public string Sector { get; set; }
        public string Ticker { get; set; }
        public string Country { get; set; }

        public double  Open { get; set; }
        public double PreviousClose { get; set; }
        public double Last { get; set; }
        public string Currency { get; set; }
        public string SecurityType { get; set; }
        public long ID { get; set; }
        
    }
    public class Future : Security
    {
       
        public Future(string name):base(name)
        {
        }
        public double ContractSize { get; set; }
        public bool isFuture = true;
    }
    public class Equity : Security
    {
        public Equity(string name) : base(name)
        {
        }
        
    }
    public class Forex : Security
    {
       
        private readonly string ccy1_;
        private readonly string ccy2_;

        public Forex(string ccy1, string ccy2) : base(ccy1 + "/" + ccy2)
        {
            ccy1_ = ccy1;
            ccy2_ = ccy2;
        }
       
        public Forex Inverse()
        {
            Forex inv = new Forex(ccy2_, ccy1_);
            inv.Last = 1 / this.Last;
            inv.Open = 1 / this.Open;
            inv.PreviousClose = 1 / this.PreviousClose;
            
            return inv;
        }
      
    }
    public class Position 
    {
        public string Underlying { get; set; } //unique name of the security
        public string UnderlyingType { get; set; } //unique name of the security
        public string UnderlyingTicker { get; set; } //unique name of the security
        public double SoldQuantity { get; set; }
        public double BODPnL { get; set; }

        public double Quantity { get; set; }
        public double SoldAveragePrice { get; set; }
        public double BoughtQuantity { get; set; }
        public double BoughtAveragePrice { get; set; }
        public double BeginOfDayQuantity { get; set; }
        public double RealizedPnL { get; set; }
        public string PortfolioName { get; set; }
        public string Ticker { get; set; }
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
        private Dictionary<string, Portfolio> portfolios_ = new Dictionary<string, Portfolio>();

        private Dictionary<string, Security> securities_ = new Dictionary<string, Security>();
        public IReadOnlyDictionary<string, Security> Securities
        {
            get
            {
                return securities_;

            }

        }

        //return read only collection so modification is not allowed 
        public IReadOnlyDictionary<string, Portfolio> Portfolios
        {
            get
            {
                return portfolios_;

            }

        }

        private Model()
        {
            LoadModel();

        }
        static Model()
        {
            

        }

         public  List<String> getSecurities()
        {
            // for each 
            List<string> myTickers = new List<string>();

            foreach (var ptflIter in Portfolios)
            {
                foreach (var posIter in ptflIter.Value.Positions) {
                    myTickers.Add(posIter.Value.Underlying);
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

        //public bool LoadFromBooksFile(System.IO.StreamReader reader)  
        //{
        //    bool at_header = true;
        //    portfolios_.Clear();
        //    securities_.Clear();
        //    while (!reader.EndOfStream)
        //    {
        //        var line = reader.ReadLine();
        //        if (at_header)
        //        {
        //            at_header=false;
        //            continue;//first line is the header
        //        }
               
        //        var values = line.Split('|');
        //        /*
        //     Date|Book|Security|Delta|TdyPnL|BODPnL|TdingPnL|DivPnL|LastPx|PrevCLS
        //     |Position|BODPos|Bought|BuyPx|Sold|SellPx|Multiplier|SecType|Curncy|YTDPnL|MTDPnL|WTDPnL    
        //     */
        //        bool is_new_folio = false;
        //Portfolio the_portfolio= new Portfolio();
        //    if(false == portfolios_.TryGetValue(values[1],out the_portfolio))
        //    {
        //            the_portfolio = new Portfolio(){ Name = values[1]};
        //             is_new_folio = true;

        //        }
        //        Position pos = new Position()
        //        {
  
        //                Underlying = values[2]
        //            ,
        //            BODPnL = Convert.ToDouble(values[5]),
        //            BoughtAveragePrice = Convert.ToDouble(values[13]),
        //            BoughtQuantity = Convert.ToDouble(values[12]),
        //            SoldAveragePrice = Convert.ToDouble(values[15]),
        //            SoldQuantity = Convert.ToDouble(values[14]),
        //            BeginOfDayQuantity = Convert.ToDouble(values[11])
        //        };
        //        the_portfolio.Positions.Add(pos.Underlying.Name, pos);
        //      if (is_new_folio)
        //            Portfolios.Add(the_portfolio.Name, the_portfolio);
        //    }
        //    return true;
        //}
        private void LoadModel()
        {/*
            Portfolios.Clear();
            return;
            // testings..

            Portfolio ptf0 = new Portfolio() { Name = "European0" };
            Portfolio ptf1 = new Portfolio() { Name = "European1" };
            Portfolio ptf2 = new Portfolio() { Name = "European2" };
            Portfolio ptf3 = new Portfolio() { Name = "European3" };

            Position pos1 = new Position()
            {
                Underlying = new Security()
                {
                    Name = "IBM US EQUITY",
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
                    Name = "MSFT US EQUITY",
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
                    Name = "ESM9 INDEX",
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
           */ 
          
        }
    }
}
