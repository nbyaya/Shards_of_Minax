using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheSoulDivider : WarAxe
{
    [Constructable]
    public TheSoulDivider()
    {
        Name = "The Soul Divider";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.CastRecovery = 1;
        Attributes.SpellDamage = 20;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitManaDrain = 50;
        WeaponAttributes.HitLeechMana = 25;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheSoulDivider(Serial serial) : base(serial)
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
