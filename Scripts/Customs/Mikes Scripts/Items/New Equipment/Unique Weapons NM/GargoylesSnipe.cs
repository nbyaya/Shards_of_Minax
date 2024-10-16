using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GargoylesSnipe : HeavyCrossbow
{
    [Constructable]
    public GargoylesSnipe()
    {
        Name = "Gargoyle's Snipe";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 15;
        Slayer = SlayerName.GargoylesFoe;
        WeaponAttributes.HitManaDrain = 30;
        WeaponAttributes.ResistFireBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GargoylesSnipe(Serial serial) : base(serial)
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
