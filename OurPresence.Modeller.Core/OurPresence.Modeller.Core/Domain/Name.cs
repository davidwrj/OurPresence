using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OurPresence.Modeller.Domain
{
    [DefaultProperty(nameof(Value))]
    public class Name : IEquatable<Name>
    {
        public string Overridden { get; private set; } = string.Empty;

#pragma warning disable CS8618 // Non-nullable field is uninitialized is not true, SetName will set them.
        public Name(string value)
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
        {
            SetName(value);
        }

        public void SetOverride(string name)
        {
            Overridden = name;
        }

        public bool IsOverridden() => !string.IsNullOrWhiteSpace(Overridden);

        public void SetName(string name)
        {
            string? ovr = null;

            var value = string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
            if (value.Length == 0)
            {
                Plural = new Names(string.Empty, string.Empty, string.Empty, string.Empty);
                Singular = new Names(string.Empty, string.Empty, string.Empty, string.Empty);
                return;
            }
            else
            {
                if (value.EndsWith("]", StringComparison.InvariantCultureIgnoreCase))
                {
                    var match = value.LastIndexOf("[", StringComparison.InvariantCultureIgnoreCase);
                    if (match > -1)
                    {
                        ovr = value.Substring(match + 1, value.Length - match - 2);
                        value = value.Substring(0, match);
                    }
                }
            }

            var t = value.Humanize().Transform(To.TitleCase);
            var s = t.Singularize(false);
            var p = t.Pluralize(false);

            var ds = s.Dehumanize();
            var dp = p.Dehumanize();

            Plural = new Names(dp, dp.Camelize(), dp.Pascalize(), p);
            Singular = new Names(ds, ds.Camelize(), ds.Pascalize(), s);

            if (ovr != null)
            {
                SetOverride(ovr);
            }
        }

        public bool Equals(Name? other) => !(other is null) && Value == other.Value;

        [JsonIgnore]
        public Names Singular { get; private set; }

        [JsonIgnore]
        public Names Plural { get; private set; }

        public string Value => IsOverridden() ? Overridden : Singular.Value;

        public override string ToString() => Value;

        public override int GetHashCode() => -568181920 + EqualityComparer<string>.Default.GetHashCode(Value);

        public static bool operator ==(Name name1, Name name2) => !(name2 is null) && EqualityComparer<Name>.Default.Equals(name1, name2);

        public static bool operator !=(Name name1, Name name2) => !(name1 == name2);

        public override bool Equals(object? obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) || (obj is Name other
                                                                             ? other.Value == Value : obj.ToString() == Value));
        }
    }
}