using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MacefightersGrips : RingmailGloves
{
    [Constructable]
    public MacefightersGrips()
    {
        Name = "Macefighter's Grips";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(55, 85);
        ArmorAttributes.DurabilityBonus = 80;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 25;
        Attributes.WeaponSpeed = 30;
        SkillBonuses.SetValues(0, SkillName.Macing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 35.0);
        PhysicalBonus = 10;
        FireBonus = 20;
        ColdBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MacefightersGrips(Serial serial) : base(serial)
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
