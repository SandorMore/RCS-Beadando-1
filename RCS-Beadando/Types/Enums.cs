using System.ComponentModel.DataAnnotations;

namespace RCS_Beadando.Types
{
    public class Enums
    {
        // [Display(Name = ...)] is what dropdowns (Html.GetEnumSelectList) and our pages show instead of the raw name
        public enum Stakes
        {
            [Display(Name = "25/50")] _25_50,
            [Display(Name = "50/100")] _50_100,
            [Display(Name = "100/200")] _100_200,
            [Display(Name = "250/500")] _250_500,
            [Display(Name = "500/1000")] _500_1000,
            [Display(Name = "1000/2000")] _1000_2000,
            [Display(Name = "2500/5000")] _2500_5000,
            [Display(Name = "5000/10000")] _5000_10000
        }

        // Seats in order of action preflop (9-max table)
        public enum Position
        {
            UTG,
            [Display(Name = "UTG+1")] UTG1,
            [Display(Name = "UTG+2")] UTG2,
            [Display(Name = "Middle")] MP,
            [Display(Name = "Hijack")] HJ,
            [Display(Name = "Cutoff")] CO,
            [Display(Name = "Button")] BTN,
            [Display(Name = "Small blind")] SB,
            [Display(Name = "Big blind")] BB
        }

        public enum PlayStyle
        {
            Unknown,
            [Display(Name = "Tight-passive (rock)")] TightPassive,
            [Display(Name = "Tight-aggressive (TAG)")] TightAggressive,
            [Display(Name = "Loose-passive (calling station)")] LoosePassive,
            [Display(Name = "Loose-aggressive (LAG)")] LooseAggressive
        }
    }
}
