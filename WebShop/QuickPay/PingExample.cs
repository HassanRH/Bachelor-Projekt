﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using WebShop.QuickPay.Models;

namespace WebShop.QuickPay
{
	// Please note:
	// This is only an example implementation, do not use this for production; this class is not updated as new endpoints are created!!!
	// models are created by inserting a json to this page http://json2csharp.com/#

	[Obsolete("This is an example class, please do not use in production")]
	public class PingExample : QuickPayRestClient
	{
		public PingExample(string username, string password) : base(username, password)
		{
		}

		public PingExample(string apikey) : base(apikey)
		{
		}

		public PingResponse Ping()
		{
			return CallEndpoint<PingResponse>("ping");
		}

		public PingResponse PingPost()
		{
			Action<RestRequest> prepareRequest = (RestRequest request) => {
				request.Method = Method.POST;
			};
			return CallEndpoint<PingResponse>("ping", prepareRequest);
		}

		// please note:
		// this is not an example of how to do async in .Net!!!
		public async Task<PingResponse> PingAsync()
		{
			return await CallEndpointAsync<PingResponse>("ping");
		}

		public Dictionary<string, string> PingDictionary()
		{
			return CallEndpoint<Dictionary<string, string>>("ping");
		}
	}
}