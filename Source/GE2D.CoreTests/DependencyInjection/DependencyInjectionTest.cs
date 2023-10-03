using GE2D.Core.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GE2D.CoreTests.DependencyInjection;

/// <summary>
/// Don't trust 3rd party packages!
/// Test it for good working and write implementation for its documentation
/// </summary>
[TestClass]
public class DependencyInjectionTest
{
    IDIManager CreateDIEnvironment()
    {
        BL1.Init();
        BL2.Init();

        return new DIManager();
    }

    [TestMethod]
    public void InstanceLifecycle()
    {
        IDIManager di = CreateDIEnvironment();
        di.Init
        (
            () =>
            {
                di.Bind<IBL1, BL1>(DILifetimeScopes.Singleton);
                di.Bind<IBL2, BL2>(DILifetimeScopes.Transient);
            }
        );

        di.GetDependency<IBL1>();
        var bl11 = di.GetDependency<IBL1>(); //get same object
        di.GetDependency<IBL2>();
        var bl22 = di.GetDependency<IBL2>(); //generate new BL2

        Assert.IsTrue(bl11.RefCounter1 == 1, "DependencyInjectionTests.InstanceLifecycle - BL1 singleton, be only one");
        Assert.IsTrue(bl22.RefCounter2 == 2, "DependencyInjectionTests.InstanceLifecycle - BL2 created twice");
    }

    [TestMethod]
    public void InstanceInject()
    {
        string injectedName = "Prototype object";

        IDIManager di = CreateDIEnvironment();
        di.Init
        (
            () =>
            {
                di.Bind<IBL1, BL1>(DILifetimeScopes.Transient);
                di.Bind<IBL2, BL2>(DILifetimeScopes.Singleton);
                di.Bind<Injected, Injected>(DILifetimeScopes.Transient, new Injected() { Name = injectedName });
            }
        );

        di.GetDependency<IBL1>(); //generate new BL1
        var bl2 = di.GetDependency<IBL2>(); //generate BL2 singleton
        var bl11 = di.GetDependency<IBL1>(); //generate new BL1

        Assert.IsTrue(bl11.RefCounter1 == 2, "DependencyInjectionTests.InstanceInject - creating two instances BL1");
        Assert.IsTrue(bl2.RefCounter2 == 1, "DependencyInjectionTests.InstanceInject - BL1 is singleton, will be only one");

        var injected1 = di.GetDependency<Injected>();
        var injected2 = di.GetDependency<Injected>();
        Assert.IsFalse(injected1.Equals(injected2), "DependencyInjectionTests.InstanceInject - create Injected 1-2 transiens obj failed");
        Assert.IsFalse(ReferenceEquals(injected1, injected2), "DependencyInjectionTests.InstanceInject - the Injected type is not transient");

        IDIManager di2 = CreateDIEnvironment();
        di2.Init
        (
            () =>
            {
                di2.Bind<Injected, Injected>(DILifetimeScopes.Singleton, new Injected() { Name = injectedName });
            }
        );

        var injected11 = di2.GetDependency<Injected>();
        var injected22 = di2.GetDependency<Injected>();
        Assert.IsTrue(injected11.Equals(injected22), "DependencyInjectionTests.InstanceInject - Injected 1-2 is must singleton");
        Assert.IsTrue(ReferenceEquals(injected11, injected22), "DependencyInjectionTests.InstanceInject - Injected type is not singleton");
    }
}
