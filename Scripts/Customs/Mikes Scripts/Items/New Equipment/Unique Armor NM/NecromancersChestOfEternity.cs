using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersChestOfEternity : PlateChest
{
    [Constructable]
    public NecromancersChestOfEternity()
    {
        Name = "Necromancer's Chest of Eternity";
        Hue = Utility.Random(666, 999);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusInt = 30;
        Attributes.RegenMana = 10;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 50.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 40.0);
        PhysicalBonus = 20;
        ColdBonus = 15;
        FireBonus = 10;
        EnergyBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersChestOfEternity(Serial serial) : base(serial)
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
