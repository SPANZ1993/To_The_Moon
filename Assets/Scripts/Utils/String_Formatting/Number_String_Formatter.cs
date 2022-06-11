using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Globalization;


public class Number_String_Formatter
{


    public static string bannerUIFormatThrustNumberText(float thrust, int decimals=0){
        return defaultNumberFormat(thrust, decimals:decimals);
    }

    public static string bannerUIFormatThrustNumberText(double thrust, int decimals=0){
        return defaultNumberFormat(thrust, decimals:decimals);
    }

    public static string bannerUIFormatCoinsNumberText(float coins, int decimals=0){
        return defaultNumberFormat(coins, decimals:decimals);
    }

    public static string bannerUIFormatCoinsNumberText(double coins, int decimals=0){
        return defaultNumberFormat(coins, decimals:decimals);
    }

    public static string bannerUIFormatGemsNumberText(float coins, int decimals=0){
        return defaultNumberFormat(coins, decimals:decimals);
    }

    public static string bannerUIFormatGemsNumberText(double coins, int decimals=0){
        return defaultNumberFormat(coins, decimals:decimals);
    }



    public static string rocketFlightFormatAltitudeNumberText(float rocketAltitude, int decimals=0){
        return defaultNumberFormat(rocketAltitude, decimals:decimals);
    }

    public static string rocketFlightFormatAltitudeNumberText(double rocketAltitude, int decimals=0){
        return defaultNumberFormat(rocketAltitude, decimals:decimals);
    }

    public static string rocketFlightFormatThrustNumberText(float rocketThrust, int decimals=0){
        return defaultNumberFormat(rocketThrust, decimals:decimals);
    }

    public static string rocketFlightFormatThrustNumberText(double rocketThrust, int decimals=0){
        return defaultNumberFormat(rocketThrust, decimals:decimals);
    }




    public static string mineGameFormatTimeNumberText(float time, int decimals=0){
        return defaultNumberFormat(time, decimals:decimals);
    }

    public static string mineGameFormatTimeNumberText(double time, int decimals=0){
        return defaultNumberFormat(time, decimals:decimals);
    }

    public static string mineGameFormatScoreNumberText(float score, int decimals=0){
        return defaultNumberFormat(score, decimals:decimals);
    }

    public static string mineGameFormatScoreNumberText(double score, int decimals=0){
        return defaultNumberFormat(score, decimals:decimals);
    }

    public static string mineGameFormatFloatingNumberText(float score, int decimals=0){
        return defaultNumberFormat(score, decimals:decimals);
    }

    public static string mineGameFormatFloatingNumberText(double score, int decimals=0){
        return defaultNumberFormat(score, decimals:decimals);
    }



    // Robot Menu Stuff
    public static string robotMenuFormatPriceText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals:decimals);
    }

    public static string robotMenuFormatPriceText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals:decimals);
    }
    // End Robot Menu Stuff

    // Lab Menu Stuff
    public static string formatTimeForResearchDurationText(double secs){
        return defaultTimeNumberFormat(secs);
    }

    public static string formatTimeForResearchDurationText(float secs){
        return defaultTimeNumberFormat(secs);
    }

    public static string formatTimeForResearchTimeLeftText(double secs){
        return defaultTimeNumberFormat(secs);
    }

    public static string formatTimeForResearchTimeLeftText(float secs){
        return defaultTimeNumberFormat(secs);
    }

    public static string formatResearchPriceText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatResearchPriceText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }
    
    public static string formatResearchExpectedThrustText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatResearchExpectedThrustText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }
    // End Lab Menu Stuff

    // Bookshelf Menu Stuff
    public static string formatHighestAltitudeText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatHighestAltitudeText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMostCoinsText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMostCoinsText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }


    public static string formatMinecartCapacityText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMinecartCapacityText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMinecartRateText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMinecartRateText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMineshaftPerSwingText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMineshaftPerSwingText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMineshaftPerBlockText(double price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }

    public static string formatMineshaftPerBlockText(float price, int decimals=0){
        return defaultNumberFormat(price, decimals: decimals);
    }
    // End Bookshelf Menu Stuff


    // Speech Object Stuff
    public static string formatHeightForSpeechObject(double height, int decimals=0){
        return defaultNumberFormat(height, decimals:decimals);
    }
    
    public static string formatHeightForSpeechObject(float height, int decimals=0){
        return defaultNumberFormat(height, decimals:decimals);
    }
    // End Speech Object Stuff


    // Countdown Timer Stuff
    public static string formatTimeForCountdownTimer(double secs){
        return defaultTimeNumberFormat(secs);
    }

    public static string formatTimeForCountdownTimer(float secs){
        return defaultTimeNumberFormat(secs);
    }
    // End Countdown Timer Stuff



    private static float floorDivision(float dividend, float divisor, int decimals=0){

        int nthdecimal(float num, int n){
            return ((int)(num * Mathf.Pow(10f, n))/10);
        } 

        //return Mathf.Floor((dividend-(dividend%divisor))/divisor);
        //return (dividend-(dividend%divisor))/divisor;
        float floorDividendMantissa = dividend%divisor;
        float floorDividend = dividend-floorDividendMantissa;
        float floorResultMantissa = floorDividendMantissa/divisor;

        float floorRoundedMantissa = ((float)((int)(floorResultMantissa * Mathf.Pow(10f, decimals))))/Mathf.Pow(10f, decimals);

        float floorResult = (floorDividend/divisor) + floorResultMantissa;
        return floorResult;
    
        // //return floorResult;
        // if (decimals == 0 && false){
        //     Debug.Log("SWIFF -- RESULT: " + (floorDividend/divisor).ToString("0.00000") + " MANTISSA: " + floorResultMantissa.ToString("0.00000") + " AND: " + floorResult.ToString("0.00000") +  " ... " + Mathf.Floor(floorResult).ToString("0.00000"));
        //     //return Mathf.Truncate(floorResult);
        //     return 0.0f;
        // }
        // else{
        //     Debug.Log("SWIFF --  RESULT: " + (floorDividend/divisor).ToString("0.00000") + " MANTISSA: " + floorResultMantissa.ToString("0.00000") + " AND: " + floorResult.ToString("0.00000") + " ... " + (Mathf.Floor((floorResult * (10f*decimals))) / (10f*decimals)).ToString("0.00000"));
        //     //return Mathf.Floor((floorResult * (10f*decimals))) / (10f*decimals);
        //     Debug.Log("SWIFF... : " + (float)((int)((floorResult * Mathf.Pow(10f, decimals))/Mathf.Pow(10f, decimals))) + " --- " + floorRoundedMantissa.ToString("0.00000"));
        //     return ((float)(int)((floorResult * Mathf.Pow(10f, decimals))/Mathf.Pow(10f, decimals)));
        // }
    }

    private static double floorDivision(double dividend, double divisor, int decimals=0){
        int nthdecimal(float num, int n){
            return ((int)(num * Mathf.Pow(10f, n))/10);
        } 

        //return Mathf.Floor((dividend-(dividend%divisor))/divisor);
        //return (dividend-(dividend%divisor))/divisor;
        double floorDividendMantissa = dividend%divisor;
        double floorDividend = dividend-floorDividendMantissa;
        double floorResultMantissa = floorDividendMantissa/divisor;

        double floorRoundedMantissa = ((double)((int)(floorResultMantissa * Math.Pow(10.0, (double)decimals))))/Math.Pow(10.0, (double)decimals);

        double floorResult = (floorDividend/divisor) + floorResultMantissa;
        return floorResult;
    }

    // k
    // M
    // G
    // T



    private static string defaultNumberFormat(float num, int decimals=0){
        return defaultNumberFormat((double)num, decimals);
    }

    private static string defaultNumberFormat(int num, int decimals=0){
        return defaultNumberFormat((double) num, decimals);
    }


    // public static string defaultNumberFormat(float num, int decimals=0){

    //     if (num == 0){
    //         return "0";
    //     }

    //     int nthdecimal(float num, int n){
    //         // This still has some mild rounding errors maybe but it's close enough

    //         //?int x = Mathf.FloorToInt(num*Mathf.Pow(10f, (float)decimals));
    //         //?return x % 10; // This might work Too But Idk
            
    //         int precisionDec = 5;

    //         int ndec = precisionDec;
    //         for (int i=0; i<n; i++){
    //             ndec += 1;
    //         }


    //         StringBuilder formatter = new StringBuilder("{0:N"+ndec.ToString()+"}");

    //         string stmp = string.Format(formatter.ToString(), num);
    //         StringBuilder sbs = new StringBuilder();
    //         for(int i=0; i<stmp.Length; i++){
    //             if(stmp[i] != ','){
    //                 sbs.Append(stmp[i].ToString());
    //             }
    //         }
    //         string s;
    //         if (decimals != 0){
    //             s = sbs.ToString();
    //             return Int32.Parse(s[s.Length-(1+precisionDec)].ToString());
    //         }
    //         else{
    //             return Mathf.FloorToInt(num)%10;
    //         }
    //     }

    //     string fixRoundingOverflow(string finalOutput, float flooredNum){
    //         float outputFloat = float.Parse(finalOutput, CultureInfo.InvariantCulture.NumberFormat);

    //         if(outputFloat > flooredNum){
    //             List<char> numCharList = new List<char>();
    //             List<int> numCharIndices = new List<int>();
    //             bool foundOnly9AndDec = false;
    //             for (int i=finalOutput.Length-1; i >= 0; i--){
                    
    //                 if(finalOutput[i] != '9' && finalOutput[i] != '.'){
    //                     foundOnly9AndDec = true;
    //                 }
                    
    //                 if(foundOnly9AndDec==true && finalOutput[i]!='9' && finalOutput[i]!='.'){
    //                     numCharList.Add(finalOutput[i]);
    //                     numCharIndices.Add(i);
    //                 }
    //             }

    //             numCharList.Reverse();
    //             numCharIndices.Reverse();

    //             int origCharListLen = numCharList.ToArray().Length;
    //             float fixedRoundingErrorFloat = float.Parse(string.Concat(numCharList.ToArray()), CultureInfo.InvariantCulture.NumberFormat) - 1f;
    //             List<char> fixedRoundingErrorCharList = new List<char>(fixedRoundingErrorFloat.ToString("0").ToCharArray());
    //             int fixedCharListLen = fixedRoundingErrorCharList.ToArray().Length;
                
    //             StringBuilder finalOutputSb = new StringBuilder(finalOutput);

    //             // for(int i=0; i<origCharListLen; i++){
    //             //     if(i != 0 && origCharListLen != fixedCharListLen){
    //             //         finalOutputSb[i] = fixedRoundingErrorCharList[i];
    //             //     }
    //             // }

    //             if (origCharListLen == fixedCharListLen){
    //                 for(int i=0; i<origCharListLen; i++){
    //                     finalOutputSb[numCharIndices[i]] = fixedRoundingErrorCharList[i];
    //                 }
    //             }
    //             else if (origCharListLen == fixedCharListLen + 1){
    //                 for(int i=1; i<origCharListLen; i++){
    //                     finalOutputSb[numCharIndices[i]] = fixedRoundingErrorCharList[i-1];
    //                 }
    //             }
    //             else{
    //                 Debug.Log("WTF: " + string.Concat(numCharList.ToArray()) + " _ " + string.Concat(fixedRoundingErrorCharList.ToArray()));
    //             }


    //             finalOutput = finalOutputSb.ToString();
    //             if(origCharListLen != fixedCharListLen){
    //                 finalOutput = finalOutput.Remove(0, 1);
    //             }
    //         }

    //         return finalOutput;
    //     }

    //     float flooredNum = 0.0f;
    //     string s = "";
    //     string s_form = "0";
    //     string id = "";

    //    if (decimals >= 0){
    //         s_form += '.';
    //         for (int i=0; i<decimals; i++){
    //             s_form += '0';
    //         }
    //     }

    //     switch(num){
    //         case float n when (n <= 10000f):
    //         {
    //             flooredNum = Mathf.Floor(n);
    //             break;
    //         }
    //         case float n when (n >= 10000f && n < 10000000f):
    //         {
    //             flooredNum = floorDivision(n, 1000f, decimals:decimals);
    //             id = "k";
    //             break;
    //         }
    //         case float n when (n >= 10000000f && n < 10000000000f):
    //         {
    //             flooredNum = floorDivision(n, 1000000f, decimals:decimals);
    //             id = "M";
    //             break;
    //         }
    //         case float n when (n >= 10000000000f && n < 10000000000000f):
    //         {
    //             flooredNum = floorDivision(n, 1000000000f, decimals:decimals);
    //             id = "G";
    //             break;
    //         }
    //         case float n when (n >= 10000000000000f):{
    //             flooredNum = floorDivision(n, 1000000000000f, decimals:decimals);
    //             id = "T";
    //             break;
    //         }
    //     }

        

    //     s = flooredNum.ToString(s_form);
        

    //     int nthDec = nthdecimal(flooredNum, decimals);
    //     int sLen = s.Length;
        

    //     StringBuilder sb = new StringBuilder(s);
    //     char nthDecChar = nthDec.ToString().ToCharArray()[0];
    //     sb[sLen-1] = nthDecChar;

    //     while(sb[sb.Length-1] == '0' && sb.ToString().Contains('.')){
    //         sb.Length -= 1;
    //     }
    //     if(sb[sb.Length-1] == '.'){
    //         sb.Length -= 1;
    //     }


    //     Debug.Log("FLOORED NUM: " + flooredNum + " AND STRING VERSION " + s + " WITH FORM " + s_form + " DECIMAL " + decimals + " IS " + nthDec + " ROUNDING ERROR: " + fixRoundingOverflow(sb.ToString(), flooredNum));


    //     return fixRoundingOverflow(sb.ToString(), flooredNum) + id;
    // }








    private static string defaultNumberFormat(double num, int decimals=0){

        if (num == 0.0){
            return "0";
        }

        string trimDecimals(string s, int decimals){
            int decInd = -1;
            int firstTrailingZeroInd = -1;
            for(int i = 0; i<s.Length; i++){
                if(s[i]=='.'){
                    decInd = i;
                }
            }

            bool seenOnlyZerosOrDec = true;
            for(int i = s.Length-1; i>=0; i--){
                if(s[i]=='0' && seenOnlyZerosOrDec){
                    firstTrailingZeroInd = i;
                }
                else{
                    seenOnlyZerosOrDec = false;
                }
            }

            StringBuilder sb = new StringBuilder(s);

            int origSbLen = sb.Length;
            for(int i = origSbLen-1; i>=0; i--){
                if((firstTrailingZeroInd!=-1 && i>=firstTrailingZeroInd && decInd!=-1) || (decInd!=-1 && i>decInd+decimals)){
                    sb.Length -= 1;
                }
            }
            if (sb.ToString()[sb.Length-1] == '.'){
                sb.Length -= 1;
            }

            return sb.ToString();
        }



        //return defaultNumberFormat(num, decimals:decimals);
    
        double flooredNum = 0.0;
        string s = "";
        string id = "";

        switch(num){
            case double n when (n <= 10000.0):
            {
                //flooredNum = Math.Floor(n);
                flooredNum = floorDivision(n, 1.0, decimals:decimals);
                break;
            }
            case double n when (n >= 10000.0 && n < 10000000.0):
            {
                flooredNum = floorDivision(n, 1000.0, decimals:decimals);
                id = "k";
                break;
            }
            case double n when (n >= 10000000.0 && n < 10000000000.0):
            {
                flooredNum = floorDivision(n, 1000000.0, decimals:decimals);
                id = "M";
                break;
            }
            case double n when (n >= 10000000000.0 && n < 10000000000000.0):
            {
                flooredNum = floorDivision(n, 1000000000.0, decimals:decimals);
                id = "G";
                break;
            }
            case double n when (n >= 10000000000000.0):{
                flooredNum = floorDivision(n, 1000000000000.0, decimals:decimals);
                id = "T";
                break;
            }
        }

        s = flooredNum.ToString();
        
        return trimDecimals(s, decimals) + id;
    }

    // Time Stuff
    private static double[] ConvertSectoDay(double n)
        {
            double day = n / (24.0 * 3600.0);
        
            n = n % (24.0 * 3600.0);
            double hour = n / 3600.0;
        
            n %= 3600.0;
            double minutes = n / 60.0 ;
        
            n %= 60.0;
            double seconds = n;


            if(day < 1.0){
                day = 0.0;
                if(hour < 1.0){
                    hour = 0.0;
                    if(minutes < 1.0){
                        minutes = 0;
                        if(seconds < 1.0){
                            seconds = 1.0;
                        }
                    }
                }

            }
            
            return new double[] {Math.Floor(day), Math.Floor(hour), Math.Floor(minutes), Math.Floor(seconds)};
    }

    private static string defaultTimeNumberFormat(double n){
        if(n < 1){
            return "S: 0";
        }
        double[] timeArr = ConvertSectoDay(n);
        string displayString = "";
        if(timeArr[0] != 0.0){
            displayString += "D: " + timeArr[0].ToString("0");
        }
        if(timeArr[1] != 0.0){
            if(displayString != ""){
                displayString += "  ";
            }
            displayString += "H: " + timeArr[1].ToString("0");
        }
        if(timeArr[2] != 0.0){
            if(displayString != ""){
                displayString += "  ";
            }
            displayString += "M: " + timeArr[2].ToString("0");
        }
        if(timeArr[3] != 0.0){
            if(displayString != ""){
                displayString += "  ";
            }
            displayString += "S: " + timeArr[3].ToString("0");
        }
        return displayString;
    }

    private static string defaultTimeNumberFormat(float n){
        return defaultTimeNumberFormat((double)n);
    }
    // End Time Stuff



}
