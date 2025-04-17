using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VoidsEmbrace : SpiritScepter
{
    [Constructable]
    public VoidsEmbrace()
    {
        Name = "Void's Embrace";
        Hue = Utility.Random(5, 15);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(90, 140);
        Attributes.SpellDamage = 20;
        Attributes.BonusMana = 20;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitLeechMana = 50;
        WeaponAttributes.HitCurse = 40;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VoidsEmbrace(Serial serial) : base(serial)
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
