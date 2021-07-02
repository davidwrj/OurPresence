using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class EntityFile : IGenerator
    {
        private readonly Module _module;

        public EntityFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Common");
            sb.Al("{");
            sb.I(1).Al("public abstract class Entity<TId>");
            sb.I(1).Al("{");
            sb.I(2).Al("public virtual TId Id { get; protected set; }");
            sb.B();
            sb.I(2).Al("protected Entity()");
            sb.I(2).Al("{");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected Entity(TId id)");
            sb.I(2).Al("{");
            sb.I(3).Al("Id = id;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public override bool Equals(object obj)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (!(obj is Entity<TId> other))");
            sb.I(3).Al("{");
            sb.I(4).Al("return false;");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("if (ReferenceEquals(this, other))");
            sb.I(3).Al("{");
            sb.I(4).Al("return true;");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("return ValueObject.GetUnproxiedType(this) != ValueObject.GetUnproxiedType(other)");
            sb.I(4).Al("? false");
            sb.I(4).Al(": Id.Equals(default(TId)) || other.Id.Equals(default(TId)) ? false : Id.Equals(other.Id);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator ==(Entity<TId> a, Entity<TId> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return a is null && b is null ? true : a is null || b is null ? false : a.Equals(b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator !=(Entity<TId> a, Entity<TId> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return !(a == b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public override int GetHashCode()");
            sb.I(2).Al("{");
            sb.I(3).Al("return (ValueObject.GetUnproxiedType(this).ToString() + Id).GetHashCode();");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("public abstract class Entity : Entity<long>");
            sb.I(1).Al("{");
            sb.I(2).Al("protected Entity()");
            sb.I(2).Al("{");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected Entity(long id)");
            sb.I(3).Al(": base(id)");
            sb.I(2).Al("{");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("Entity.cs", sb.ToString());
        }
    }
}
