using OurPresence.Modeller.Interfaces;
using System;

namespace OurPresence.Modeller.Generator
{
    public class GeneratorVersion : IEquatable<Version>, IComparable, IComparable<Version>, IComparable<GeneratorVersion>, IGeneratorVersion
    {
        private int _preRelease;
        private const int Released = 100;
        private const int LessThan = -1;
        private const int GreaterThan = 1;
        private const int Equal = 0;
        private const int Alpha = 1;
        private const int Beta = 2;

        public GeneratorVersion()
            : this("")
        { }

        public GeneratorVersion(Version version)
            : this(version.ToString())
        { }

        public GeneratorVersion(string vers)
        {
            ForceVersion(vers);
        }

        public Version Version { get; set; } = new Version();

        private void ForceVersion(string vers)
        {
            if (string.IsNullOrWhiteSpace(vers))
            {
                Version = new Version();
                return;
            }

            var sep = vers.IndexOf("-", StringComparison.InvariantCulture);
            if (sep > -1)
            {
                if (vers.EndsWith("-alpha"))
                    IsAlphaRelease = true;
                else if (vers.EndsWith("-beta"))
                    IsBetaRelease = true;
                else
                {
                    throw new FormatException("Version was not the correct format. Use: major.minor.revision.build[-alpha|-beta]");
                }
                vers = vers.Remove(sep);
            }
            if (Version.TryParse(vers, out var result))
            {
                Version = result;
            }
            else
            {
                throw new FormatException("Version was not the correct format. Use: major.minor.revision.build[-alpha|-beta]");
            }
        }

        public bool IsRelease
        {
            get => _preRelease == Released || _preRelease == 0;
            set
            {
                if (value)
                    _preRelease = Released;
            }
        }
        public bool IsAlphaRelease
        {
            get => _preRelease == Alpha;
            set
            {
                if (value)
                    _preRelease = Alpha;
            }
        }

        public bool IsBetaRelease
        {
            get => _preRelease == Beta;
            set
            {
                if (value)
                    _preRelease = Beta;
            }
        }

        public static IGeneratorVersion Empty => new GeneratorVersion("0.0");

        public override string ToString()
        {
            var s = Version.ToString();
            if (IsAlphaRelease)
                s += "-alpha";
            else if (IsBetaRelease)
                s += "-beta";
            return s;
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                null => false,
                GeneratorVersion gv => CompareTo(gv) == Equal,
                Version v => CompareTo(v) == Equal,
                _ => false
            };
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public bool Equals(Version? other) => other is not null && CompareTo(new GeneratorVersion(other)) == Equal;

        private static int PreRelease(GeneratorVersion v) => v.IsAlphaRelease ? Alpha : v.IsBetaRelease ? Beta : Released;

        public static bool operator ==(GeneratorVersion v1, GeneratorVersion v2) => v1.Equals(v2);

        public static bool operator !=(GeneratorVersion v1, GeneratorVersion v2) => !v1.Equals(v2);

        public static bool operator >(GeneratorVersion v1, GeneratorVersion v2) => v1.CompareTo(v2) == GreaterThan;

        public static bool operator <(GeneratorVersion v1, GeneratorVersion v2) => v1.CompareTo(v2) == LessThan;

        public static bool operator >=(GeneratorVersion v1, GeneratorVersion v2)
        {
            return v1.Equals(v2) || v1 > v2;
        }

        public static bool operator <=(GeneratorVersion v1, GeneratorVersion v2)
        {
            return v1.Equals(v2) || v1 < v2;
        }

        public int CompareTo(object? obj)
        {
            return obj switch
            {
                Version v => CompareTo(v),
                GeneratorVersion gv => CompareTo(gv),
                string s => CompareTo(new GeneratorVersion(s)),
                _ => throw new InvalidCastException(
                    $"Unable to cast {obj.GetType().FullName} to a {typeof(GeneratorVersion).FullName}")
            };
        }

        public int CompareTo(Version? other)
        {
            var result = Version.CompareTo(other);
            return result == Equal && (IsAlphaRelease || IsBetaRelease) ? LessThan : result;
        }

        public int CompareTo(GeneratorVersion? other)
        {
            var result = Version.CompareTo(other?.Version);
            if (result != Equal) return result;

            var p1 = PreRelease(this);
            var p2 = PreRelease(other);
            return p1 == p2 ? Equal : p1 > p2 ? GreaterThan : LessThan;
        }
    }
}
