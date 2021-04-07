namespace OurPresence.Modeller.Domain.Extensions
{
    public static class EnumerationExtensions
    {
        public static Enumeration AddItem(this Enumeration enumeration, string name)
        {
            int value;
            if (enumeration.Items.Count == 0)
                value = 0;
            else if (enumeration.Flag)
                value = 1 << (enumeration.Items.Count - 1);
            else
                value = enumeration.Items.Count;

            enumeration.Items.Add(new EnumerationItem(value, name));
            return enumeration;
        }
    }

}
