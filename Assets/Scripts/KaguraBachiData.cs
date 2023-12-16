using UnityEngine;

public static class KaguraBachiData
{
    // atk based on weapon level
    private static readonly int maxHealth = 3;
    private static int health = 3;
    private static readonly int maxKi = 100;
    private static int ki = 0;
    private static readonly int[] atk = { 5, 10, 15 };
    private static readonly int[] kiRegeneratePerHit = { 5, 7, 10 };
    private static readonly int maxWeaponLevel = 3;
    private static int currentWeaponLevel = 1;
    private static int parasiteEssence = 0;
    private static int pureParasiteHeart = 0;

    // Attributes
    public static int MaxHealth
    {
        get { return maxHealth; }
    }
    public static int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, maxHealth); }
    }
    public static int MaxKi
    {
        get { return maxKi; }
    }
    public static int Ki
    {
        get { return ki; }
        set { ki = Mathf.Clamp(value, 0, maxKi); }
    }
    public static int Atk
    {
        get { return atk[currentWeaponLevel - 1]; }
    }
    public static int KiRegeneratePerHit
    {
        get { return kiRegeneratePerHit[currentWeaponLevel - 1]; }
    }
    public static int CurrentWeaponLevel
    {
        get { return currentWeaponLevel; }
        set { currentWeaponLevel = Mathf.Clamp(value, 1, maxWeaponLevel); }
    }
    public static int ParasiteEssence
    {
        get { return parasiteEssence; }
        set { parasiteEssence = Mathf.Max(0, value); }
    }
    public static int PureParasiteHeart
    {
        get { return pureParasiteHeart; }
        set { pureParasiteHeart = Mathf.Max(0, value); }
    }
}
