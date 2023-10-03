namespace GE2D.Core.DependencyInjection;

public enum DILifetimeScopes
{
    /// <summary> always create new object </summary>
    Transient,

    /// <summary> only one object </summary>
    Singleton,

    /// <summary> one object per thread </summary>
    Thread,
}
