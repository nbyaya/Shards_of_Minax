using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ApollosSong : NinjaBow
{
    [Constructable]
    public ApollosSong()
    {
        Name = "Apollo's Song";
        Hue = Utility.Random(880, 2900);
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(75, 95);
        Attributes.BonusInt = 10;
        Attributes.NightSight = 1;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ApollosSong(Serial serial) : base(serial)
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
