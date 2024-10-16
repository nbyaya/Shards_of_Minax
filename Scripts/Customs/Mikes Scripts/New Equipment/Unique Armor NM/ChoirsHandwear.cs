using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChoirsHandwear : StuddedGloves
{
    [Constructable]
    public ChoirsHandwear()
    {
        Name = "Choir's Handwear";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(30, 50);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.BonusHits = 20;
        Attributes.SpellDamage = 15;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 50.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 30.0);
        PhysicalBonus = 10;
        EnergyBonus = 12;
        FireBonus = 10;
        ColdBonus = 10;
        PoisonBonus = 18;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChoirsHandwear(Serial serial) : base(serial)
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
