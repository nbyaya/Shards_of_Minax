using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialCleaver : Bardiche
{
    [Constructable]
    public CelestialCleaver()
    {
        Name = "Celestial Cleaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(215, 250);
        Attributes.BonusMana = 40;
        Attributes.SpellDamage = 35;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitEnergyArea = 50;
        WeaponAttributes.ResistPoisonBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 15.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialCleaver(Serial serial) : base(serial)
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
