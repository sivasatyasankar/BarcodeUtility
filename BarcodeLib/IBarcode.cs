using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeUtility
{
    interface IBarcode
    {
        string Encoded_Value
        {
            get;
        }

        string RawData
        {
            get;
        } 

        List<string> Errors
        {
            get;
        } 
    } 
} 
