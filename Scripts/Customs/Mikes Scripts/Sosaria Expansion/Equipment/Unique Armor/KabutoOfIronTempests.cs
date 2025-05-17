using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KabutoOfIronTempests : PlateBattleKabuto
{
    [Constructable]
    public KabutoOfIronTempests()
    {
        Name = "Kabuto of Iron Tempests";
        Hue = Utility.Random(1150, 1200); // A dark metallic color
        BaseArmorRating = Utility.RandomMinMax(35, 90); // Stronger than normal plate armor

        // Special Attributes
        Attributes.BonusStr = 15; // Strength boost to enhance combat capabilities
        Attributes.BonusDex = 10; // Dexterity for better agility
        Attributes.DefendChance = 20; // Increased defense for better survivability

        // Skill bonuses that align with the battle and tempest theme
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0); // Sword fighting is key to the Kabuto’s legacy
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0); // Mastery of battle tactics
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0); // Parry for blocking incoming attacks

        // Resistance bonuses
        ColdBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 10; // Extra physical protection
        EnergyBonus = 10;

        // Unique features of the Kabuto of Iron Tempests
        Attributes.CastSpeed = 1; // Fast casting, great for spellcasters in battle
        Attributes.CastRecovery = 2; // Reduced recovery time for faster reaction

        // Self-repair to ensure the helmet stays durable during long battles
        ArmorAttributes.SelfRepair = 10;

        // Attach XmlLevelItem for unique item characteristics
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KabutoOfIronTempests(Serial serial) : base(serial)
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
