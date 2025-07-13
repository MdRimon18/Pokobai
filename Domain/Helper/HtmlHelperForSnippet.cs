using HtmlAgilityPack;
using System.Text.RegularExpressions;

public class HtmlHelperForSnippet
{
    public static string GetFirst150Characters(string htmlContent, int maxLength = 150)
    {
        if (string.IsNullOrWhiteSpace(htmlContent))
            return string.Empty;

        // Load the HTML content
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Extract plain text
        string plainText = doc.DocumentNode.InnerText;

        // Trim to the desired length
        if (plainText.Length > maxLength)
            plainText = plainText.Substring(0, maxLength);

        return plainText.Trim() + "...";
    }
}
