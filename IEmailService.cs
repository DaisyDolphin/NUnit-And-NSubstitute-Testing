﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuilding
{
    public interface IEmailService
    {
        void SendEmail(string emailAddress, string subject, string message);
    }
}
