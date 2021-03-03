using System.Text;

internal static class Extensions
{
    /// <summary>
    /// Appends an indent to the current line
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="value">Indentation level</param>
    /// <param name="spaces">Size of an indent (default: 4 spaces)</param>
    /// <returns></returns>
    internal static StringBuilder i(this StringBuilder sb, int value, int spaces = 4)
    {
        a(sb,new string(' ', value * spaces));
        return sb;
    }

    /// <summary>
    /// Appends the line to the current line of the string builder instance
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    internal static StringBuilder a(this StringBuilder sb, string line)
    {
        sb.Append(line);
        return sb;
    }

    /// <summary>
    /// Adds the line with a <see cref="Environment.NewLine"/> to the string builder instance
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="line">A string</param>
    /// <returns><see cref="StringBuilder"/></returns>
    internal static StringBuilder al(this StringBuilder sb, string line)
    {
        sb.AppendLine(line);
        return sb;
    }

    /// <summary>
    /// Add a blank line to the string builder instance
    /// </summary>
    /// <param name="sb"></param>
    /// <returns><see cref="StringBuilder"/></returns>
    internal static StringBuilder b(this StringBuilder sb)
    {
        sb.AppendLine();
        return sb;
    }
}
