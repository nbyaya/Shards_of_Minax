using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonsBreathWarAxe : WarAxe
{
    [Constructable]
    public DragonsBreathWarAxe()
    {
        Name = "Dragon's Breath WarAxe";
        Hue = Utility.Random(500, 2550);
        MinDamage = Utility.RandomMinMax(60, 100);
        MaxDamage = Utility.RandomMinMax(110, 170);
        Attributes.BonusStr = 25;
        Attributes.WeaponSpeed = 7;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitFireArea = 50;
        WeaponAttributes.BattleLust = 40;
        SkillBonuses.SetValues(0, SkillName.Tactics, 30.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonsBreathWarAxe(Serial serial) : base(serial)
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
