﻿using System;

namespace WebShop.QuickPay.Parameters
{
	public enum SortDirection
	{
		asc,
		desc
	}

	public struct SortingParameters
	{
		public SortingParameters(string sortBy, SortDirection sortDirection)
		{
			SortBy = sortBy;
			SortDirection = sortDirection;
		}

		public string SortBy { get; set; }

		public SortDirection SortDirection { get; set; }
	}
}