﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace forum.Utility
{
    public class JWTTokenOptions
    {

        public string Audience
        {
            get;
            set;
        }
        public string SecurityKey
        {
            get;
            set;
        }
        public string Issuer
        {
            get;
            set;
        }
 
    }
}
