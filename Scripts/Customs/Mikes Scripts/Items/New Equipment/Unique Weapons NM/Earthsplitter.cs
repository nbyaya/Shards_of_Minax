using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Earthsplitter : BattleAxe
{
    [Constructable]
    public Earthsplitter()
    {
        Name = "Earthsplitter";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.OrcSlaying;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.ResistPhysicalBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 15.0);
        SkillBonuses.SetValues(2, SkillName.Chivalry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Earthsplitter(Serial serial) : base(serial)
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
