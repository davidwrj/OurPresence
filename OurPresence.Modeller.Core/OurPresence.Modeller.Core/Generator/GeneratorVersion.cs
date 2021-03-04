using OurPresence.Modeller.Interfaces;
using System;

namespace OurPresence.Modeller.Generator
{
    public class GeneratorVersion : IEquatable<Version>, IComparable, IComparable<Version>, IComparable<GeneratorVersion>, IGeneratorVersion
    {
        private int _preRelease;
        private const int _released = 100;
        private const int _lessThan = -1;
        private const int _greaterThan = 1;
        private const int _equal = 0;
        private const int _alpha = 1;
        private const int _beta = 2;

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

            var sep = vers.IndexOf("-");
            if (sep > -1)
            {
                if (vers.EndsWith("-alpha"))
                    IsAlphaRelease = true;
                else if (vers.EndsWith("-beta"))
                    IsBetaRelease = true;
                else
                {
                    throw new FormatException("Version was not the correct format. Use: major.minor.revision.build[-aplha|-beta]");
                }
                vers = vers.Remove(sep);
            }
            if (Version.TryParse(vers, out var result))
            {
                Version = result;
            }
            else
            {
                throw new FormatException("Version was not the correct format. Use: major.minor.revision.build[-aplha|-beta]");
            }
        }

        public bool IsRelease
        {
            get => _preRelease == _released || _preRelease == 0;
            set
            {
                if (value)
                    _preRelease = _released;
            }
        }
        public bool IsAlphaRelease
        {
            get => _preRelease == _alpha;
            set
            {
                if (value)
                    _preRelease = _alpha;
            }
        }

        public bool IsBetaRelease
        {
            get => _preRelease == _beta;
            set
            {
                if (value)
                    _preRelease = _beta;
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
                GeneratorVersion gv => CompareTo(gv) == _equal,
                Version v => CompareTo(v) == _equal,
                _ => false
            };
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public bool Equals(Version? other) => other is not null && CompareTo(new GeneratorVersion(other)) == _equal;

        private static int PreRelease(GeneratorVersion v) => v.IsAlphaRelease ? _alpha : v.IsBetaRelease ? _beta : _released;

        public static bool operator ==(GeneratorVersion v1, GeneratorVersion v2) => v1.Equals(v2);

        public static bool operator !=(GeneratorVersion v1, GeneratorVersion v2) => !v1.Equals(v2);

        public static bool operator >(GeneratorVersion v1, GeneratorVersion v2) => v1.CompareTo(v2) == _greaterThan;

        public static bool operator <(GeneratorVersion v1, GeneratorVersion v2) => v1.CompareTo(v2) == _lessThan;

        public static bool operator >=(GeneratorVersion v1, GeneratorVersion v2)
        {
            return v1.Equals(v2) || v1 > v2;
        }

        public static bool operator <=(GeneratorVersion v1, GeneratorVersion v2)
        {
            return v1.Equals(v2) || v1 < v2;
        }

        public int CompareTo(object obj)
        {
            if (obj is Version v)
                return CompareTo(v);
            else if (obj is GeneratorVersion gv)
                return CompareTo(gv);
            else if (obj is string s)
                return CompareTo(new GeneratorVersion(s));
            else
                throw new InvalidCastException($"Unable to cast {obj.GetType().FullName} to a {typeof(GeneratorVersion).FullName}");
        }

        public int CompareTo(Version? other)
        {
            var result = Version.CompareTo(other);
            return result == _equal && (IsAlphaRelease || IsBetaRelease) ? _lessThan : result;
        }

        public int CompareTo(GeneratorVersion? other)
        {
            var result = Version.CompareTo(other?.Version);
            if (result != _equal) return result;

            var p1 = PreRelease(this);
            var p2 = PreRelease(other);
            return p1 == p2 ? _equal : p1 > p2 ? _greaterThan : _lessThan;
        }
    }
}
