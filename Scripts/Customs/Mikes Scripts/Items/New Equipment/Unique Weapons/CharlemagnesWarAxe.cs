using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CharlemagnesWarAxe : WarAxe
{
    [Constructable]
    public CharlemagnesWarAxe()
    {
        Name = "Charlemagne's WarAxe";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 120);
        Attributes.AttackChance = 10;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.BattleLust = 25;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 70.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CharlemagnesWarAxe(Serial serial) : base(serial)
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
