using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        private string _stringResult = "";
        private string _htmlResult = "";
        
        private readonly CultureInfo _cultureInfo = new("en-US");
        
        public string Print(Invoice invoice, Dictionary<string, Play> plays, PrintType printType)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            WriteLineToResult("h1", string.Format("Statement for {0}", invoice.Customer));

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayName];
                
                var amountInCents = GetAmountInCents(play.Type, perf.Audience);
                
                WriteLineToResult("p",String.Format(_cultureInfo, "  {0}: {1:C} ({2} seats)", play.Name, Convert.ToDecimal(amountInCents / 100), perf.Audience)); 
                
                totalAmount += amountInCents;
                volumeCredits += GetVolumeCredits(play.Type, perf.Audience);
            }
            
            WriteLineToResult("b",String.Format(_cultureInfo, "Amount owed is {0:C}", Convert.ToDecimal(totalAmount / 100)));
            WriteLineToResult("b", String.Format("You earned {0} credits", volumeCredits));

            switch (printType)
            {
                case PrintType.String:
                    return _stringResult;
                case PrintType.Html:
                    return _htmlResult;
                default:
                    throw new ArgumentOutOfRangeException(nameof(printType), printType, null);
            }
        }

        private void WriteLineToResult(string htmlTag, string line)
        {
            _stringResult += $"{line}\n";
            _htmlResult += $"<{htmlTag}>{line}</{htmlTag}>\n";
        }

        private int GetAmountInCents(PlayType playType, int performanceAudience)
        {
            var thisAmount = 0;
            switch (playType) 
            {
                case PlayType.Tragedy:
                    thisAmount = 40000;
                    if (performanceAudience > 30) {
                        thisAmount += 1000 * (performanceAudience - 30);
                    }
                    break;
                case PlayType.Comedy:
                    thisAmount = 30000;
                    if (performanceAudience > 20) {
                        thisAmount += 10000 + 500 * (performanceAudience - 20);
                    }
                    thisAmount += 300 * performanceAudience;
                    break;
                default:
                    throw new Exception("unknown type: " + playType);
            }

            return thisAmount;
        }

        private int GetVolumeCredits(PlayType playType, int performanceAudience)
        {
            var volumeCredits = 0;

            volumeCredits += Math.Max(performanceAudience - 30, 0);
            if (playType == PlayType.Comedy) 
                volumeCredits += (int)Math.Floor((decimal)performanceAudience / 5);

            return volumeCredits;
        }
    }
}
