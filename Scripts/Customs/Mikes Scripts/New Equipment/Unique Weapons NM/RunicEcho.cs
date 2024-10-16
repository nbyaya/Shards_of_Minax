using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RunicEcho : WarAxe
{
    [Constructable]
    public RunicEcho()
    {
        Name = "Runic Echo";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 25;
        Attributes.BonusHits = 15;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.ResistPhysicalBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RunicEcho(Serial serial) : base(serial)
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
