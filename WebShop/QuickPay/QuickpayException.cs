﻿using System;
using System.Net;

namespace WebShop.QuickPay
{
    /// <summary>
    ///     Quick pay exception
    /// </summary>
    public class QuickpayException : Exception
    {
        internal QuickpayException(string message, WebException innerException) : base(message, innerException)
        {
        }

        internal QuickpayException(string message) : base(message)
        {
        }
    }
}