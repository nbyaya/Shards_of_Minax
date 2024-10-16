using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThePeacekeeper : Club
{
    [Constructable]
    public ThePeacekeeper()
    {
        Name = "The Peacekeeper";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 30;
        Attributes.CastRecovery = 1;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.Silver;
        WeaponAttributes.HitDispel = 40;
        WeaponAttributes.HitFatigue = 35;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 25.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThePeacekeeper(Serial serial) : base(serial)
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
