﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Forms
{
    interface IUserInformer
    {
        void ShowUserMessage(string message);
        void HideUserMessage();
    }
}
