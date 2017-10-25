using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class IDGenerator
{
    public static string IntToString(int value)
    {
        char[] baseChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x' };
        int i = 40;
        char[] result = new char[i];
        int targetBase = baseChars.Length;

        do
        {
            result[--i] = baseChars[value % targetBase];
            value = value / targetBase;
        }
        while (i > 0);
        
        return new string(result);
    } 
}