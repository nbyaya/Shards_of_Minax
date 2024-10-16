using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArchonsMysticRobe : PlateChest
{
    [Constructable]
    public ArchonsMysticRobe()
    {
        Name = "Archon's Mystic Robe";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 75);
        AbsorptionAttributes.CastingFocus = 10;
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusInt = 25;
        Attributes.LowerManaCost = 20;
        Attributes.SpellDamage = 15;
        SkillBonuses.SetValues(0, SkillName.Magery, 50.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 50.0);
        SkillBonuses.SetValues(2, SkillName.Inscribe, 30.0);
        PhysicalBonus = 10;
        FireBonus = 20;
        ColdBonus = 20;
        EnergyBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArchonsMysticRobe(Serial serial) : base(serial)
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
