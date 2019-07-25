using Fusillade;
using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControl.Services
{
    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
