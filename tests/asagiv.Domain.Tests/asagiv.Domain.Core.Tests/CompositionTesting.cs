using asagiv.Domain.Core.DependencyInjection;
using asagiv.Domain.Core.Extensions;
using MediatR;
using Xunit;

namespace asagiv.Domain.core.tests;

#region Test Interfaces
public interface ITestInterface
{
    bool Foo { get; set; }
    string Bar { get; set; }
}
public interface ITestInterfaceAttributed : ITestInterface { }
public interface ITestInterfaceWithContract : ITestInterface { }
public interface ITestInterfaceWithMetadata : ITestInterface { }
#endregion

#region Test Classes
public abstract class TestClassBase()
{
    public bool Foo { get; set; }
    public string Bar { get; set; }
}
public class TestClass : TestClassBase, ITestInterface { }
[Export(typeof(ITestInterfaceAttributed))]
public class TestClassAttributed : TestClassBase, ITestInterfaceAttributed { }
[Export(typeof(ITestInterfaceWithContract), "Contract")]
public class TestClassWithContract : TestClassBase, ITestInterfaceWithContract { }
[Export(typeof(ITestInterfaceWithMetadata))]
[ExportMetadata("foo", "bar")]
public class TestClassWithMetadata : TestClassBase, ITestInterfaceWithMetadata { }
#endregion

#region Unit Tests
public class CompositionTesting
{
    [Fact]
    public void ComponentContainerBuild_Should_CreateSingleton()
    {
        // Arrange
        ComponentContainer.Container.Initialize(cb =>
        {
            cb.AddSingleton<ITestInterface, TestClass>();
        });

        // Act
        var instance1 = ComponentContainer.Container.Build<ITestInterface>();
        var instance2 = ComponentContainer.Container.Build<ITestInterface>();

        // Assert
        Assert.IsType<TestClass>(instance1);
        Assert.IsType<TestClass>(instance2);
        Assert.Equal(instance1, instance2);
    }

    [Fact]
    public void ComponentContainerBuild_Should_CreateTransient()
    {
        // Arrange
        ComponentContainer.Container.Initialize(cb =>
        {
            cb.AddTransient<ITestInterface, TestClass>();
        });

        // Act
        var instance1 = ComponentContainer.Container.Build<ITestInterface>();
        var instance2 = ComponentContainer.Container.Build<ITestInterface>();

        // Assert
        Assert.IsType<TestClass>(instance1);
        Assert.IsType<TestClass>(instance2);
        Assert.NotEqual(instance1, instance2);
    }

    [Fact]
    public void ComponentContainerBuildWithContract_Should_CreateSingleton()
    {
        // Arrange
        ComponentContainer.Container.Initialize(cb =>
        {
            cb.AddSingleton<ITestInterface, TestClass>("Contract");
        });

        // Act
        var instance1 = ComponentContainer.Container.Build<ITestInterface>("Contract");
        var instance2 = ComponentContainer.Container.Build<ITestInterface>("Contract");

        // Assert
        Assert.IsType<TestClass>(instance1);
        Assert.IsType<TestClass>(instance2);
        Assert.Equal(instance1, instance2);
    }

    [Fact]
    public void ComponentContainerBuildWithMetadata_Should_CreateSingleton()
    {
        // Arrange
        ComponentContainer.Container.Initialize(cb =>
        {
            cb.AddSingleton<ITestInterface, TestClass>(null, ("foo", "bar"), ("alpha", "beta"));
        });

        // Act
        var instance1 = ComponentContainer.Container.Build<ITestInterface>("foo", "bar");
        var instance2 = ComponentContainer.Container.Build<ITestInterface>("foo", "bar");

        // Assert
        Assert.IsType<TestClass>(instance1);
        Assert.IsType<TestClass>(instance2);
        Assert.Equal(instance1, instance2);
    }

    [Fact]
    public void ComponentContainerAttribue_Should_CreateTransient()
    {
        ComponentContainer.Container.Initialize();

        // Act
        var instance1 = ComponentContainer.Container.Build<ITestInterfaceAttributed>();
        var instance2 = ComponentContainer.Container.Build<ITestInterfaceAttributed>();

        // Assert
        Assert.IsType<TestClassAttributed>(instance1);
        Assert.IsType<TestClassAttributed>(instance2);
        Assert.NotEqual(instance1, instance2);
    }

    [Fact]
    public void ComponentContainerAttributeWithContract_Should_CreateTransient()
    {
        // Arrange
        ComponentContainer.Container.Initialize();

        // Act
        var instance1 = ComponentContainer.Container.Build<ITestInterfaceWithContract>("Contract");
        var instance2 = ComponentContainer.Container.Build<ITestInterfaceWithContract>("Contract");

        // Assert
        Assert.IsType<TestClassWithContract>(instance1);
        Assert.IsType<TestClassWithContract>(instance2);
        Assert.NotEqual(instance1, instance2);
    }

    [Fact]
    public void ComponentContainerAttributeWithMetadata_Should_CreateTransient()
    {
        // Arrange
        ComponentContainer.Container.Initialize();

        // Act
        var instance1 = ComponentContainer.Container.Build<ITestInterfaceWithMetadata>("foo", "bar");
        var instance2 = ComponentContainer.Container.Build<ITestInterfaceWithMetadata>("foo", "bar");

        // Assert
        Assert.IsType<TestClassWithMetadata>(instance1);
        Assert.IsType<TestClassWithMetadata>(instance2);
        Assert.NotEqual(instance1, instance2);
    }

    [Fact]
    public void AddMediatR_Should_CreateMediator()
    {
        // Arrange
        ComponentContainer.Container.Initialize(cb =>
        {
            cb.AddMediatR();
        });

        // Act
        var instance = ComponentContainer.Container.Build<IMediator>();

        // Assert
        Assert.IsType<Mediator>(instance);
    }
}
#endregion
