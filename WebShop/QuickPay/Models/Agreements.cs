﻿using System;
using System.Collections.Generic;

namespace WebShop.QuickPay.Models
{
	[Obsolete("This is an example class, please do not use in production")]
	public class AclPermission
	{
		public string resource { get; set; }
		public bool get { get; set; }
		public bool post { get; set; }
		public bool put { get; set; }
		public bool delete { get; set; }
		public bool patch { get; set; }
	}

	[Obsolete("This is an example class, please do not use in production")]
	public class User
	{
		public int id { get; set; }
		public object email { get; set; }
		public bool system_user { get; set; }
		public string name { get; set; }
	}

	[Obsolete("This is an example class, please do not use in production")]
	public class AgreementAccount
	{
		public int id { get; set; }
		public string type { get; set; }
		public string name { get; set; }
	}

	[Obsolete("This is an example class, please do not use in production")]
	public class Agreement
	{
		public int id { get; set; }
		public bool owner { get; set; }
		public string api_key { get; set; }
		public string description { get; set; }
		public List<AclPermission> acl_permissions { get; set; }
		public bool accepted { get; set; }
		public bool locked { get; set; }
		public bool support { get; set; }
		public string service { get; set; }
		public User user { get; set; }
		public AgreementAccount account { get; set; }
		public string created_at { get; set; }
	}
}