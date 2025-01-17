﻿namespace Modules.Shared.Integration.Extensions;

public static class TypeUtilities
{
	public static string GetGenericTypeName(this Type type)
	{
		if (type.IsGenericType)
		{
			string genericTypes = string.Join(",", type.GetGenericArguments().Select(GetGenericTypeName).ToArray());
			return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
		}

		return type.Name;
	}
}
