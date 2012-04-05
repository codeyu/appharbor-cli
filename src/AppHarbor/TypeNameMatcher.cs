﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AppHarbor
{
	public class TypeNameMatcher<T>
	{
		private readonly IEnumerable<Type> _candidateTypes;

		public TypeNameMatcher(IEnumerable<Type> candidateTypes)
		{
			if (candidateTypes.Any(x => !typeof(T).IsAssignableFrom(x)))
			{
				throw new ArgumentException(string.Format("{0} must be assignable from all injected types", typeof(T).FullName), "candidateTypes");
			}
			_candidateTypes = candidateTypes;
		}

		public Type GetMatchedType(string commandName, string scope)
		{
			try
			{
				return _candidateTypes.Single(x => x.Name.ToLower().StartsWith(commandName.ToLower()));
			}
			catch (InvalidOperationException)
			{
				throw new ArgumentException("No candidate type matches", "commandName");
			}
		}
	}
}
