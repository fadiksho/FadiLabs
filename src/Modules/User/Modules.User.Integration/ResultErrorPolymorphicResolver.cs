using Fadi.Result.Serialization;
using Modules.User.Integration.User.ResultErrors;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Modules.User.Integration;

public class ResultErrorPolymorphicResolver : IResultErrorPolymorphicResolver
{
	public static Type BaseType { get; } = typeof(string);

	public JsonDerivedType[] DerivedTypes =>
	[
		new(typeof(InvalidRoleAssignmentError), nameof(InvalidRoleAssignmentError)),
	];

	public void ResolveDerivedType(JsonTypeInfo jsonTypeInfo)
	{
		if (jsonTypeInfo.Type == IResultErrorPolymorphicResolver.BaseType)
		{
			jsonTypeInfo.PolymorphismOptions ??= new JsonPolymorphismOptions();
			jsonTypeInfo.PolymorphismOptions.UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor;

			foreach (var derived in DerivedTypes)
			{
				if (jsonTypeInfo.PolymorphismOptions.DerivedTypes.IndexOf(derived) < 0)
				{
					jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(derived);
				}
			}
		}
	}
}
