using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChestOfThePaleLegion : BoneChest
{
    [Constructable]
    public ChestOfThePaleLegion()
    {
        Name = "Chest of the Pale Legion";
        Hue = 1109; // Ghostly or Bone-colored hue
        BaseArmorRating = Utility.RandomMinMax(40, 60);  // Base armor rating with a good balance

        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 25;
        Attributes.BonusHits = 50;
        Attributes.RegenHits = 2;
        Attributes.RegenStam = 2;
        Attributes.RegenMana = 3;

        Attributes.DefendChance = 10;
        Attributes.CastSpeed = 1;
        Attributes.LowerManaCost = 5;

        ArmorAttributes.SelfRepair = 5;
        ArmorAttributes.DurabilityBonus = 20;  // Enhanced durability, a feature of bone-based armor

        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 15.0);

        ColdBonus = 10;
        EnergyBonus = 5;
        PhysicalBonus = 10;
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChestOfThePaleLegion(Serial serial) : base(serial)
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
