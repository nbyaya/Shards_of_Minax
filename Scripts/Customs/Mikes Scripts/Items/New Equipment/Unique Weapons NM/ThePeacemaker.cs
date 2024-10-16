using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThePeacemaker : ShortSpear
{
    [Constructable]
    public ThePeacemaker()
    {
        Name = "The Peacemaker";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.BonusHits = 20;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.HitHarm = 45;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(2, SkillName.Healing, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThePeacemaker(Serial serial) : base(serial)
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
