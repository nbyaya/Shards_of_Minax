using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class IronHeartplate : PlateChest
{
    [Constructable]
    public IronHeartplate()
    {
        Name = "The Iron Heartplate";
        Hue = 1152; // A deep, dark iron color
        BaseArmorRating = Utility.RandomMinMax(50, 75); // High armor rating for a plate chest

        Attributes.BonusStr = 15; // Strength bonus to reflect the plate's durability
        Attributes.BonusHits = 25; // Adds more health as it is a robust armor
        Attributes.DefendChance = 10; // Slight chance to defend against attacks
        Attributes.Luck = 50; // Lucky to survive in such heavy armor

        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Useful for someone who fights with tactics
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0); // Swords are often paired with plate armor
        SkillBonuses.SetValues(2, SkillName.Macing, 10.0); // The heavy weight of the plate makes it suited for maces too

        ColdBonus = 10;
        FireBonus = 15; // Protects against both cold and fire, symbolizing resilience

        ArmorAttributes.SelfRepair = 5; // Repairs itself slowly over time, a mark of the armor's quality

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attaching the XML level item to this armor

    }

    public IronHeartplate(Serial serial) : base(serial)
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
