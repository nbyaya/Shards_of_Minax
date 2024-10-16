using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DeathwhispersCap : LeatherCap
{
    [Constructable]
    public DeathwhispersCap()
    {
        Name = "Deathwhisper's Cap";
        Hue = Utility.Random(222, 888);
        BaseArmorRating = Utility.RandomMinMax(25, 45);
        ArmorAttributes.SelfRepair = 15;
        Attributes.BonusInt = 25;
        Attributes.RegenMana = 8;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 45.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 35.0);
        PhysicalBonus = 12;
        ColdBonus = 18;
        FireBonus = 12;
        EnergyBonus = 22;
        PoisonBonus = 16;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DeathwhispersCap(Serial serial) : base(serial)
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
