using System;

namespace theme_park_console
{
    class AttractionException : Exception
    {
        public AttractionException(string message) 
            : base(message) { }
    }
}
