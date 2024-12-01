namespace Shared.Integration.Utilities;
//public class PagedListConverter<T> : JsonConverter<PagedList<T>>
//{
//	public override PagedList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//	{
//		using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
//		{
//			JsonElement root = doc.RootElement;

//			int pageNumber = root.GetProperty("PageNumber").GetInt32();
//			int totalCount = root.GetProperty("TotalCount").GetInt32();
//			int totalPages = root.GetProperty("TotalPages").GetInt32();
//			List<T> items = JsonSerializer.Deserialize<List<T>>(root.GetProperty("Items").GetRawText(), options);

//			return new PagedList<T>(items, totalCount, pageNumber, totalPages);
//		}
//	}

//	public override void Write(Utf8JsonWriter writer, PagedList<T> value, JsonSerializerOptions options)
//	{
//		writer.WriteStartObject();
//		writer.WriteNumber("PageNumber", value.PageNumber);
//		writer.WriteNumber("TotalCount", value.TotalCount);
//		writer.WriteNumber("TotalPages", value.TotalPages);
//		writer.WritePropertyName("Items");
//		JsonSerializer.Serialize(writer, value.Items, options);
//		writer.WriteEndObject();
//	}
//}

