using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MysticsGuardBuckler : Buckler
{
    [Constructable]
    public MysticsGuardBuckler()
    {
        Name = "Mystic's Guard Buckler";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 55);
        ArmorAttributes.SelfRepair = 10;
        Attributes.SpellChanneling = 1;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 50.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 30.0);
        PhysicalBonus = 10;
        ColdBonus = 10;
        FireBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MysticsGuardBuckler(Serial serial) : base(serial)
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
