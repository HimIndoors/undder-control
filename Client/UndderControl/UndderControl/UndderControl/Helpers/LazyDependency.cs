using System;
using System.Collections.Generic;
using System.Text;

namespace UndderControl.Helpers
{
    public interface ILazyDependency<T>
    {
        T Value { get; }
    }

    public class LazyDependency<T> : Lazy<T>, ILazyDependency<T>
    {
        public LazyDependency(Func<T> valueFactory) 
            : base(valueFactory)
        { }
    }
}
