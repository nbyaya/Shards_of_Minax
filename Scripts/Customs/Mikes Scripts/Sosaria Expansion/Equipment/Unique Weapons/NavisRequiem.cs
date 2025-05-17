using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NavisRequiem : Crossbow
{
    [Constructable]
    public NavisRequiem()
    {
        Name = "Navi’s Requiem";
        Hue = Utility.Random(1200, 1250);  // A deep oceanic blue, representing the mystery of the sea
        MinDamage = Utility.RandomMinMax(15, 40);
        MaxDamage = Utility.RandomMinMax(45, 70);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;
        
        // Slayer effect – Navi's Requiem is especially powerful against Sea Creatures and water-based enemies
        Slayer = SlayerName.ElementalBan;
        
        // Weapon attributes - providing a mystical and elemental advantage, ideal for the oceanic explorer
        WeaponAttributes.HitEnergyArea = 30;  // The crossbow releases an energy blast that affects multiple enemies
        WeaponAttributes.HitDispel = 20;      // Dispel any magical energies or buffs on targets hit
        WeaponAttributes.HitColdArea = 10;    // Icy projectiles chill enemies caught in the blast
        WeaponAttributes.BattleLust = 10;

        // Skill bonuses that reflect Navi’s maritime expertise and spiritual connection to the sea
        SkillBonuses.SetValues(0, SkillName.Archery, 15.0);    // Boosting archery to enhance crossbow use
        SkillBonuses.SetValues(1, SkillName.Meditation, 10.0);  // Reflecting Navi’s deep, spiritual connection to the sea
        SkillBonuses.SetValues(2, SkillName.Fishing, 10.0);     // Navi is a master of the tides, the ocean's bounty

        // Aesthetic / Thematic effect: This weapon has a slight glow that pulses like ocean currents in the dark
        Attributes.NightSight = 1;  // Allowing the wielder to see in the darkness of deep sea environments

        // Bonus that ties in with the unique narrative of Navi’s ties to the seas and hidden mystical forces
        Attributes.ReflectPhysical = 10;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NavisRequiem(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
