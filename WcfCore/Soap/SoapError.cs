﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WcfCore.Soap
{
    public class SoapError
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }
    }
}
