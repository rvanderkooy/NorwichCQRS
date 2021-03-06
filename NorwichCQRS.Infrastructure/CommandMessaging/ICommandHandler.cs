﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Infrastructure.CommandMessaging
{
    public interface ICommandHandler<TCommand>
    {
        void Process(TCommand command);
    }
}
