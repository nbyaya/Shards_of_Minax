using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonsBreath : Scimitar
{
    [Constructable]
    public DragonsBreath()
    {
        Name = "Dragon's Breath";
        Hue = Utility.Random(100, 2250);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(90, 130);
		Attributes.SpellChanneling = 1;
        Attributes.BonusStr = 20;
        Attributes.SpellDamage = 10;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.ResistFireBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 30.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 55.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonsBreath(Serial serial) : base(serial)
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
