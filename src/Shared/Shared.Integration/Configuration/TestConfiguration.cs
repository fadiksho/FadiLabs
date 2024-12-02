namespace Shared.Integration.Configuration;

public class TestConfiguration
{
	public static readonly string SectionName = "TestConfiguration:TestSection";
	public string Value { get; set; } = default!;
}
