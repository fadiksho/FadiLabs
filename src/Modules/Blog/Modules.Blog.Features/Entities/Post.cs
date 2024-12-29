using Modules.Shared.Integration.Domain;
using Modules.Shared.Integration.Domain.Contracts;
using System.Globalization;
using System.Text;

namespace Modules.Blog.Features.Entities;

public class Post : AuditableEntity, IEntity<Guid>, IOwnedBy
{
	public Guid Id { get; set; }
	public required string Title { get; set; }
	public required string Slug { get; set; }
	public string? Description { get; set; }
	public string? Body { get; set; }
	public DateTime? PublishedDate { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
	public bool IsPublished { get; set; }
	public List<Tag> Tags { get; set; } = [];
	public List<Comment> Comments { get; set; } = [];
	public Guid OwndedBy { get; set; }

	public static string CreateSlug(string title)
	{
		title = title?.ToLowerInvariant().Replace(
				" ", "-", StringComparison.OrdinalIgnoreCase) ?? string.Empty;
		title = RemoveDiacritics(title);
		title = RemoveReservedUrlCharacters(title);

		return title.ToLowerInvariant();
	}

	#region Post Entity Helpers
	private static string RemoveDiacritics(string text)
	{
		var normalizedString = text.Normalize(NormalizationForm.FormD);
		var stringBuilder = new StringBuilder();

		foreach (var c in normalizedString)
		{
			var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
			if (unicodeCategory != UnicodeCategory.NonSpacingMark)
			{
				stringBuilder.Append(c);
			}
		}

		return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
	}
	private static string RemoveReservedUrlCharacters(string text)
	{
		var reservedCharacters = new List<string> { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~", "`", "+" };

		foreach (var chr in reservedCharacters)
		{
			text = text.Replace(chr, string.Empty, StringComparison.OrdinalIgnoreCase);
		}

		return text;
	}
	#endregion
}
