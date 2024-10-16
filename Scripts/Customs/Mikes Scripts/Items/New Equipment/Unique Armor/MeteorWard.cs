using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MeteorWard : Buckler
{
    [Constructable]
    public MeteorWard()
    {
        Name = "Meteor Ward";
        Hue = Utility.Random(500, 900);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.ResonanceFire = 25;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.DefendChance = 25;
        Attributes.SpellDamage = -10;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
        FireBonus = 20;
        ColdBonus = 10;
        PhysicalBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MeteorWard(Serial serial) : base(serial)
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
