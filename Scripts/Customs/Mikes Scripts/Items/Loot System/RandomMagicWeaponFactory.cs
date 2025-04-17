using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;
using System.Collections.Generic;

public class RandomMagicWeaponFactory
{
    private static string[] prefixes = 
    {
        "Course's", "Class's"
    };

    private static string[] suffixes = 
    {
        "Horizon", "Zenith"
    };

    private static Random rand = new Random();

    private static Type[] weaponTypes = 
    {
		typeof(Broadsword), typeof(Cutlass), typeof(Katana), typeof(Longsword), typeof(Scimitar), typeof(VikingSword),
		typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(LargeBattleAxe),
		typeof(TwoHandedAxe), typeof(WarAxe), typeof(Club), typeof(HammerPick), typeof(Mace), typeof(Maul),
		typeof(WarHammer), typeof(WarMace), typeof(Bardiche), typeof(Halberd), typeof(Lance), typeof(Pike),
		typeof(ShortSpear), typeof(Spear), typeof(WarFork), typeof(CompositeBow), typeof(Crossbow),
		typeof(HeavyCrossbow), typeof(RepeatingCrossbow), typeof(Bow), typeof(Dagger), typeof(Kryss),
		typeof(SkinningKnife), typeof(ShortSpear), typeof(Spear), typeof(Pitchfork), typeof(BlackStaff),
		typeof(GnarledStaff), typeof(QuarterStaff), typeof(ShepherdsCrook), typeof(BladedStaff), typeof(Scythe),
		typeof(Scepter), typeof(MagicWand),
		typeof(AnimalClaws),
		typeof(WrestlingBelt),
		typeof(ArtificerWand),
		typeof(BashingShield),
		typeof(BeggersStick),
		typeof(BoltRod),
		typeof(CampingLanturn),
		typeof(CarpentersHammer),
		typeof(CooksCleaver),
		typeof(DetectivesBoneHarvester),
		typeof(DistractingHammer),
		typeof(ExplorersMachete),
		typeof(FireAlchemyBlaster),
		typeof(FishermansTrident),
		typeof(FletchersBow),
		typeof(FocusKryss),
		typeof(GearLauncher),
		typeof(GourmandsFork),
		typeof(HolyKnightSword),
		typeof(IllegalCrossbow),
		typeof(IntelligenceEvaluator),
		typeof(LoreSword),
		typeof(MageWand),
		typeof(MallKatana),
		typeof(MeatPicks),
		typeof(MeditationFans),
		typeof(MerchantsShotgun),
		typeof(MysticStaff),
		typeof(NecromancersStaff),
		typeof(NinjaBow),
		typeof(Nunchucks),
		typeof(PoisonBlade),
		typeof(RangersCrossbow),
		typeof(ResonantHarp),
		typeof(RevealingAxe),
		typeof(Scalpel),
		typeof(ScribeSword),
		typeof(SewingNeedle),
		typeof(ShadowSai),
		typeof(SilentBlade),
		typeof(SleepAid),
		typeof(SmithSmasher),
		typeof(SnoopersPaddle),
		typeof(SpellWeaversWand),
		typeof(SpiritScepter),
		typeof(TacticalMultitool),
		typeof(TenFootPole),
		typeof(VeterinaryLance),
		typeof(VivisectionKnife),
		typeof(WitchBurningTorch),
		typeof(AssassinSpike),
		typeof(Bokuto),
		typeof(BoneHarvester),
		typeof(BoneMachete),
		typeof(Boomerang),
		typeof(ButcherKnife),
		typeof(Cleaver),
		typeof(CrescentBlade),
		typeof(Cyclone),
		typeof(Daisho),
		typeof(DiamondMace),
		typeof(DiscMace),
		typeof(DoubleBladedStaff),
		typeof(ElvenCompositeLongbow),
		typeof(ElvenMachete),
		typeof(ElvenSpellblade),
		typeof(JukaBow),
		typeof(Kama),
		typeof(Lajatang),
		typeof(Leafblade),
		typeof(MagicalShortbow),
		typeof(Maul),
		typeof(NoDachi),
		typeof(Nunchaku),
		typeof(Pickaxe),
		typeof(Scythe),
		typeof(Tekagi),
		typeof(Tessen),
		typeof(Tetsubo),
		typeof(Wakizashi),
		typeof(WildStaff),
		typeof(Yumi)
    };

    private static SkillName[] allSkills = (SkillName[])Enum.GetValues(typeof(SkillName));

    public static BaseWeapon CreateRandomMagicWeapon()
    {
        // Pick a random weapon type and create an instance.
        Type selectedType = weaponTypes[rand.Next(weaponTypes.Length)];
        BaseWeapon weapon = (BaseWeapon)Activator.CreateInstance(selectedType);

        // Set a random ItemID using a temporary instance (so the correct graphic is used)
        weapon.ItemID = GetRandomItemID(selectedType);

        // Assign a random name.
        weapon.Name = GetRandomName();

        // Apply randomized attributes.
        InitializeWeaponAttributes(weapon);
        // SetWeaponSkill(weapon);
        AddRandomEffects(weapon);
        AddSkillBonuses(weapon);

        // Set a random hue and attach XmlSpawner properties.
        weapon.Hue = rand.Next(1, 3001);
        XmlAttach.AttachTo(weapon, new XmlSockets(rand.Next(0, 7)));

        return weapon;
    }

    private static string GetRandomName()
    {
        return prefixes[rand.Next(prefixes.Length)] + " " + suffixes[rand.Next(suffixes.Length)];
    }

    private static void InitializeWeaponAttributes(BaseWeapon weapon)
    {
        weapon.Attributes.AttackChance = rand.Next(10, 50);
        weapon.Attributes.DefendChance = rand.Next(10, 50);
        weapon.Speed = (float)Math.Round(rand.NextDouble() * 9.9 + 0.1, 1);
        weapon.Attributes.SpellChanneling = 1;

        double tierChance = rand.NextDouble();
        if (tierChance < 0.05)
        {
            weapon.MinDamage = rand.Next(1, 80);
            weapon.MaxDamage = rand.Next(80, 120);
        }
        else if (tierChance < 0.2)
        {
            weapon.MinDamage = rand.Next(1, 70);
            weapon.MaxDamage = rand.Next(70, 100);
        }
        else if (tierChance < 0.5)
        {
            weapon.MinDamage = rand.Next(1, 50);
            weapon.MaxDamage = rand.Next(50, 75);
        }
        else
        {
            weapon.MinDamage = rand.Next(1, 30);
            weapon.MaxDamage = rand.Next(30, 50);
        }
    }

    private static void SetWeaponSkill(BaseWeapon weapon)
    {
        if (weapon is BaseRanged)
        {
            weapon.Skill = SkillName.Archery;
        }
        else if (weapon is BaseBashing || weapon is BaseStaff)
        {
            weapon.Skill = SkillName.Macing;
        }
        else if (weapon is BaseAxe || weapon is BasePoleArm)
        {
            weapon.Skill = SkillName.Swords;
        }
        else if (weapon is BaseSword)
        {
            weapon.Skill = SkillName.Swords;
        }
        else if (weapon is BaseKnife)
        {
            weapon.Skill = SkillName.Fencing;
        }
        else
        {
            // Default to one of Swords, Macing, or Fencing.
            SkillName[] skills = { SkillName.Swords, SkillName.Macing, SkillName.Fencing };
            weapon.Skill = skills[rand.Next(skills.Length)];
        }
    }

    private static void AddRandomEffects(BaseWeapon weapon)
    {
        List<Action> effects = new List<Action>
        {
            () => weapon.WeaponAttributes.HitLightning = rand.Next(1, 51),
            () => weapon.WeaponAttributes.HitFireball = rand.Next(1, 41),
            () => weapon.WeaponAttributes.HitHarm = rand.Next(1, 31),
            () => weapon.WeaponAttributes.HitMagicArrow = rand.Next(1, 21),
            () => weapon.WeaponAttributes.HitDispel = rand.Next(1, 21),
            () => weapon.WeaponAttributes.HitColdArea = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitFireArea = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitPoisonArea = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitEnergyArea = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitPhysicalArea = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitLeechHits = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitLeechMana = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitLeechStam = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitLowerAttack = rand.Next(1, 26),
            () => weapon.WeaponAttributes.HitLowerDefend = rand.Next(1, 26),
            () => weapon.WeaponAttributes.ResistPhysicalBonus = rand.Next(1, 16),
            () => weapon.WeaponAttributes.ResistFireBonus = rand.Next(1, 16),
            () => weapon.WeaponAttributes.ResistColdBonus = rand.Next(1, 16),
            () => weapon.WeaponAttributes.ResistPoisonBonus = rand.Next(1, 16),
            () => weapon.WeaponAttributes.ResistEnergyBonus = rand.Next(1, 16),
            () => weapon.WeaponAttributes.UseBestSkill = 1,
            () => weapon.WeaponAttributes.MageWeapon = rand.Next(-10, 11),
            () => weapon.WeaponAttributes.DurabilityBonus = rand.Next(1, 51)
        };

        int numEffects = rand.Next(1, 6);
        for (int i = 0; i < numEffects; i++)
        {
            if (effects.Count > 0)
            {
                int index = rand.Next(effects.Count);
                effects[index]();
                effects.RemoveAt(index);
            }
        }
    }

    private static void AddSkillBonuses(BaseWeapon weapon)
    {
        int numBonuses = GetWeightedRandomBonuses();
        List<SkillName> availableSkills = new List<SkillName>(allSkills);

        for (int i = 0; i < numBonuses; i++)
        {
            if (availableSkills.Count > 0)
            {
                int index = rand.Next(availableSkills.Count);
                SkillName skill = availableSkills[index];
                int bonus = rand.Next(1, 50);
                weapon.SkillBonuses.SetValues(i, skill, bonus);
                availableSkills.RemoveAt(index);
            }
        }
    }

    private static int GetWeightedRandomBonuses()
    {
        int roll = rand.Next(100);
        if (roll < 60)
            return 1;
        else if (roll < 85)
            return 2;
        else if (roll < 95)
            return 3;
        else if (roll < 99)
            return 4;
        else
            return 5;
    }

    private static int GetRandomItemID(Type weaponType)
    {
        BaseWeapon tempWeapon = (BaseWeapon)Activator.CreateInstance(weaponType);
        int itemID = tempWeapon.ItemID;
        tempWeapon.Delete();
        return itemID;
    }
}
