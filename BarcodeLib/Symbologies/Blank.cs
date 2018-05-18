using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeUtility.Symbologies
{
    /// <summary>
    ///  Blank encoding template
    ///  Written by: Siva Annam
    /// </summary>
    class Blank: BarcodeCommon, IBarcode
    {
        
        #region IBarcode Members

        public string Encoded_Value
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
