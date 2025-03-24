using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Notifications
{
    public enum ActionNotification
    {        
        AddAsync = 1,
        UpdateAsync = 2,
        DeleteAsync = 3,
        GetAllAsync = 4,
        GetByIdAsync = 5,

        GetAllWithFilterAsync = 6,
        GetWithFilterAsync = 7,

        CalculateTotalItemAmount=8
    }
}
