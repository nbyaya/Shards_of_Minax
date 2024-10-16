using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TempestsReaich : GnarledStaff
{
    [Constructable]
    public TempestsReaich()
    {
        Name = "Tempest's Reach";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusMana = 20;
        Attributes.RegenMana = 3;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitLightning = 55;
        WeaponAttributes.HitDispel = 25;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TempestsReaich(Serial serial) : base(serial)
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
