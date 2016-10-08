using System;


namespace EpicomTest.Exceptions
{
    public class OrderException: Exception
    {
        public OrderException(string message, Exception inner = null) : base(message, inner) { }
    }
}