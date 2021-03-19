using OurPresence.Modeller.Domain;
using System;
using System.ComponentModel;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FieldBuilder<T>
    {
        public FieldBuilder(T builder, Field field)
        {
            Build = builder ;
            Instance = field ?? throw new ArgumentNullException(nameof(field));
        }

        public T Build { get; }

        public Field Instance { get; }

        public FieldBuilder<T> Default(string value)
        {
            Instance.Default = value;
            return this;
        }

        public FieldBuilder<T> DataType(DataTypes value)
        {
            Instance.DataType = value;
            return this;
        }

        public FieldBuilder<T> DataTypeTypeName(string value)
        {
            Instance.DataTypeTypeName = value;
            return this;
        }

        public FieldBuilder<T> BusinessKey(bool value)
        {
            Instance.BusinessKey = value;
            return this;
        }

        public FieldBuilder<T> Nullable(bool value)
        {
            Instance.Nullable = value;
            return this;
        }

        public FieldBuilder<T> Scale(int value)
        {
            Instance.Scale = value;
            return this;
        }

        public FieldBuilder<T> Precision(int value)
        {
            Instance.Precision = value;
            return this;
        }

        public FieldBuilder<T> MaxLength(int value)
        {
            if (Instance.MinLength.HasValue && value <= Instance.MinLength.Value)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"MaxLength must be greater than {Instance.MinLength.Value}");
            }

            Instance.MaxLength = value;
            return this;
        }

        public FieldBuilder<T> MinLength(int value)
        {
            if (Instance.MaxLength.HasValue && value >= Instance.MaxLength.Value)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"MinLength must be less than {Instance.MaxLength.Value}");
            }

            Instance.MinLength = value;
            return this;
        }
    }
}
