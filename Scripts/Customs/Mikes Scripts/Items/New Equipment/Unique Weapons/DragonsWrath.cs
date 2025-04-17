using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonsWrath : FireAlchemyBlaster
{
    [Constructable]
    public DragonsWrath()
    {
        Name = "Dragon's Wrath";
        Hue = Utility.Random(700, 2900);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.AttackChance = 20;
        Attributes.BonusHits = 30;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.BalronDamnation;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.HitLeechHits = 30;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 40.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonsWrath(Serial serial) : base(serial)
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
