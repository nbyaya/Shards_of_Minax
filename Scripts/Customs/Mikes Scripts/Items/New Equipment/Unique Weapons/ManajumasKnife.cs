using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ManajumasKnife : Dagger
{
    [Constructable]
    public ManajumasKnife()
    {
        Name = "Manajuma's Knife";
        Hue = Utility.Random(650, 2900);
        MinDamage = Utility.RandomMinMax(10, 50);
        MaxDamage = Utility.RandomMinMax(50, 90);
        Attributes.BonusInt = 15;
        Attributes.SpellDamage = 5;
        Slayer = SlayerName.Ophidian;
        WeaponAttributes.HitLeechMana = 25;
        WeaponAttributes.HitPoisonArea = 20;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ManajumasKnife(Serial serial) : base(serial)
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
