﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace WebShop.QuickPay.Models
{
	[Obsolete("This is an example class, please do not use in production")]
	public class LinkVariable
	{
	}

	[Obsolete("This is an example class, please do not use in production")]
	public class Changes
	{
		public List<string> link_token { get; set; }
		public List<string> link_url { get; set; }
		// for now we can't deserialize properties with dots https://github.com/restsharp/RestSharp/issues/714
		public List<int?> link_agreement_id { get; set; }
		public List<string> link_language { get; set; }
		public List<int?> link_amount { get; set; }
		public List<string> link_callbackurl { get; set; }
		public List<string> link_payment_methods { get; set; }
		public List<bool?> link_autofee { get; set; }
		public List<bool?> link_autocapture { get; set; }
		public List<string> link_version { get; set; }
		public List<LinkVariable> link_variables { get; set; }
	}

	[Obsolete("This is an example class, please do not use in production")]
	public class Extra
	{
		public string path { get; set; }
		public string user { get; set; }
	}

	[Obsolete("This is an example class, please do not use in production")]
	public class Activity
	{
		public string id { get; set; }
		public string target_type { get; set; }
		public int target_id { get; set; }
		public string action { get; set; }
		public Changes changes { get; set; }
		public int user_id { get; set; }
		public int account_id { get; set; }
		public Extra extra { get; set; }
		public bool support { get; set; }
		public string created_at { get; set; }
	}
}