namespace MyVaccine.WebApi.Services;

// IGuidGenerator.cs
public interface IGuidGenerator
{
    Guid GetGuid();
}

public interface IGuidGeneratorScope : IGuidGenerator
{
}

public interface IGuidGeneratorTrasient : IGuidGenerator
{
}

public interface IGuidGeneratorSingleton : IGuidGenerator
{
}

public interface IGuidGeneratorDeep
{
    MyGuidDI GetGuidDeep();
}

// GuidServiceScope.cs
public class GuidServiceScope : IGuidGeneratorScope
{
    private Guid _guid;

    public GuidServiceScope()
    {
        _guid = Guid.NewGuid();
    }

    public Guid GetGuid()
    {
        return _guid;
    }
}

// GuidServiceTransient.cs
public class GuidServiceTransient : IGuidGeneratorTrasient
{
    private Guid _guid;

    public GuidServiceTransient()
    {
        _guid = Guid.NewGuid();
    }

    public Guid GetGuid()
    {
        return _guid;
    }
}

// GuidServiceSingleton.cs
public class GuidServiceSingleton : IGuidGeneratorSingleton
{
    private Guid _guid;

    public GuidServiceSingleton()
    {
        _guid = Guid.NewGuid();
    }

    public Guid GetGuid()
    {
        return _guid;
    }
}


public class GuidGeneratorDeep : IGuidGeneratorDeep
{
    private IGuidGeneratorTrasient _guidGeneratorTrasient;
    private IGuidGeneratorScope _guidGeneratorScope;
    private IGuidGeneratorSingleton _guidGeneratorSingleton;
    public GuidGeneratorDeep(IGuidGeneratorSingleton guidGeneratorSingleton,
        IGuidGeneratorScope guidGeneratorScope,
        IGuidGeneratorTrasient guidGeneratorTrasient
        )
    {
        _guidGeneratorScope = guidGeneratorScope;
        _guidGeneratorSingleton = guidGeneratorSingleton;
        _guidGeneratorTrasient = guidGeneratorTrasient;
    }

    public MyGuidDI GetGuidDeep()
    {
        var response = new MyGuidDI();
        response.TrasientGuid = _guidGeneratorTrasient.GetGuid();
        response.ScopeGuid = _guidGeneratorScope.GetGuid();
        response.SingletonGuid = _guidGeneratorSingleton.GetGuid();
        return response;
    }
}

public class MyGuidDI
{
    public Guid ControllerGuid { get; set; }
    public Guid TrasientGuid { get; set; }
    public Guid ScopeGuid { get; set; }
    public Guid SingletonGuid { get; set; }
}

