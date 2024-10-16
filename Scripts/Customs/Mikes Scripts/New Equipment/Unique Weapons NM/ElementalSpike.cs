using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalSpike : ShortSpear
{
    [Constructable]
    public ElementalSpike()
    {
        Name = "Elemental Spike";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 20;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.WaterDissipation;
        WeaponAttributes.HitEnergyArea = 35;
        WeaponAttributes.HitMagicArrow = 25;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalSpike(Serial serial) : base(serial)
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
