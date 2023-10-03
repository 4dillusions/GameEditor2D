namespace GE2D.CoreTests.DependencyInjection;

public class BL1 : IBL1
{
    static int refCount;
    public int RefCounter1 => refCount;

    public BL1()
    {
        refCount++;
    }

    public static void Init()
    {
        refCount = 0;
    }
}
