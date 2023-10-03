namespace GE2D.CoreTests.DependencyInjection;

public class BL2 : IBL2
{
    static int refCount;
    public int RefCounter2 => refCount;

    public BL2()
    {
        refCount++;
    }

    public static void Init()
    {
        refCount = 0;
    }
}
