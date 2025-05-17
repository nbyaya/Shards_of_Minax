using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KnightsBreathDoublet : Doublet
{
    [Constructable]
    public KnightsBreathDoublet()
    {
        Name = "Knight's Breath Doublet";
        Hue = Utility.Random(1157, 1158); // Subtle dark armor hues (grey or dark blue)

        // Set attributes and bonuses
        Attributes.BonusStr = 15;
        Attributes.BonusHits = 30;
        Attributes.BonusStam = 20;
        Attributes.BonusMana = 10;
        Attributes.DefendChance = 10;
        Attributes.WeaponDamage = 15;
        Attributes.AttackChance = 10;

        // Resistances (The Knight's Breath Doublet is robust and battle-hardened)
        Resistances.Physical = 20;
        Resistances.Fire = 10;
        Resistances.Cold = 5;
        Resistances.Poison = 15;
        Resistances.Energy = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Knightly tactics
        SkillBonuses.SetValues(1, SkillName.Archery, 10.0);  // Strong in ranged combat
        SkillBonuses.SetValues(2, SkillName.Fencing, 10.0);  // Good with swords and similar weapons
        SkillBonuses.SetValues(3, SkillName.MagicResist, 5.0);  // Resilience to magic attacks

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KnightsBreathDoublet(Serial serial) : base(serial)
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
