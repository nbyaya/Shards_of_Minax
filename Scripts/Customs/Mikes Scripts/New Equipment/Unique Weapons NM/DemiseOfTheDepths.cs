using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DemiseOfTheDepths : HammerPick
{
    [Constructable]
    public DemiseOfTheDepths()
    {
        Name = "Demise of the Depths";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 15;
        Attributes.RegenMana = 2;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.Ophidian;
        WeaponAttributes.HitPoisonArea = 25;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DemiseOfTheDepths(Serial serial) : base(serial)
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
