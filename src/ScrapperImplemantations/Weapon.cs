using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSkinScrapper.ScrapperImplemantations
{
    public enum Weapon
    {
        //Pistols
        DesertEagle,
        R8Revolver,
        DualBerettas,
        FiveSeveN,
        Glock18,
        P2000,
        USPS,
        P250,
        CZ75Auto,
        Tec9,

        //Shotguns
        Mag7,
        Nova,
        SawedOff,
        XM1014,

        //SMGs
        PPBizon,
        MAC10,
        MP7,
        MP5SD,
        MP9,
        P90,
        UMP45,

        //Rifles
        AK47,
        AUG,
        FAMAS,
        GalilAR,
        M4A4,
        M4A1S,
        SG553,

        //LMGs
        M249,
        Negev,

        //SniperRifles
        AWP,
        G3SG1,
        SCAR20,
        SSG08,
    }

    public class WeaponStrings
    {
        public string[] weapons =
        {
            //Pistols
            "Desert Eagle",
            "R8 Revolver",
            "Dual Berettas",
            "Five-SeveN",
            "Glock-18",
            "P2000",
            "USP-S",
            "P250",
            "CZ75-Auto",
            "Tec-9",

            //Shotguns
            "Mag-7",
            "Nova",
            "Sawed-Off",
            "XM1014",

            //SMGs
            "PP-Bizon",
            "MAC-10",
            "MP7",
            "MP5-SD",
            "MP9",
            "P90",
            "UMP-45",

            //Rifles
            "AK-47",
            "AUG",
            "FAMAS",
            "Galil AR",
            "M4A4",
            "M4A1-S",
            "SG 553",

            //LMGs
            "M249",
            "Negev",

            //Sniper Rifles
            "AWP",
            "G3SG1",
            "SCAR-20",
            "SSG 08",
        };
    }
}
