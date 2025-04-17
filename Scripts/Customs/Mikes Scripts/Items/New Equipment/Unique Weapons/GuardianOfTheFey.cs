using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GuardianOfTheFey : ResonantHarp
{
    [Constructable]
    public GuardianOfTheFey()
    {
        Name = "Guardian of the Fey";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.DefendChance = 20;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.ElementalBan;
        WeaponAttributes.HitMagicArrow = 40;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GuardianOfTheFey(Serial serial) : base(serial)
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
